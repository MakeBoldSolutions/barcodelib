using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

namespace BarcodeLib
{
    /// <summary>
    /// Renders a <see cref="Barcode"/>'s encoded value as an SVG document, as an alternative to
    /// the raster (<see cref="System.Drawing.Bitmap"/>) rendering in <see cref="Barcode"/>. Reuses
    /// the same GS1 quiet-zone-aware geometry (<see cref="Barcode.CalculateModuleGeometry"/>) as
    /// raster rendering so bar positions agree between the two output formats.
    /// </summary>
    internal static class SvgRenderer
    {
        private const string SvgNs = "http://www.w3.org/2000/svg";

        internal static string Render(Barcode barcode)
        {
            bool isMatrix = Barcode.IsMatrixSymbology(barcode.EncodedType);
            if (isMatrix ? barcode.EncodedMatrix == null : string.IsNullOrEmpty(barcode.EncodedValue))
                throw new Exception("EGENERATE_SVG-1: Must be encoded first.");

            int width = barcode.Width;
            int height = barcode.Height;

            XElement root = new XElement(XName.Get("svg", SvgNs),
                new XAttribute("width", width),
                new XAttribute("height", height),
                new XAttribute("viewBox", FormattableString.Invariant($"0 0 {width} {height}")));

            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", 0),
                new XAttribute("y", 0),
                new XAttribute("width", width),
                new XAttribute("height", height),
                new XAttribute("fill", ToHex(barcode.BackColor))));

            if (isMatrix)
            {
                RenderMatrix(barcode, root, width, height);
            }
            else if (barcode.EncodedType == TYPE.ITF14)
            {
                RenderItf14(barcode, root, width, height);
            }
            else
            {
                RenderLinear(barcode, root, width, height);
            }

            return new XDocument(root).ToString();
        }

        private static void RenderMatrix(Barcode barcode, XElement root, int width, int height)
        {
            const int quietZoneModules = 4;
            bool[,] matrix = barcode.EncodedMatrix;
            int moduleCount = matrix.GetLength(0);

            var geometry = barcode.CalculateMatrixGeometry(width, height, moduleCount, quietZoneModules);
            string fill = ToHex(barcode.ForeColor);

            for (int y = 0; y < moduleCount; y++)
            {
                for (int x = 0; x < moduleCount; x++)
                {
                    if (!matrix[y, x])
                        continue;

                    int px = geometry.offsetX + ((x + quietZoneModules) * geometry.moduleSizePx);
                    int py = geometry.offsetY + ((y + quietZoneModules) * geometry.moduleSizePx);

                    root.Add(new XElement(XName.Get("rect", SvgNs),
                        new XAttribute("x", px),
                        new XAttribute("y", py),
                        new XAttribute("width", geometry.moduleSizePx),
                        new XAttribute("height", geometry.moduleSizePx),
                        new XAttribute("fill", fill)));
                }
            }
        }

        private static void RenderLinear(Barcode barcode, XElement root, int width, int height)
        {
            string encodedValue = barcode.EncodedValue;
            var geometry = barcode.CalculateModuleGeometry(width, encodedValue.Length);
            int barWidth = geometry.barWidth;
            int shiftAdjustment = geometry.shiftAdjustment;

            if (barWidth <= 0)
                throw new Exception("EGENERATE_SVG-2: Image size specified not large enough to draw image. (Bar size determined to be less than 1 pixel)");

            int topLabelAdjustment = 0;
            int barsHeight = height;

            if (barcode.IncludeLabel)
            {
                if ((barcode.LabelPosition & (LabelPositions.TOPCENTER | LabelPositions.TOPLEFT | LabelPositions.TOPRIGHT)) > 0)
                    topLabelAdjustment = barcode.LabelFont.Height;

                barsHeight -= barcode.LabelFont.Height;
            }

            string fill = ToHex(barcode.ForeColor);
            bool isPostNet = barcode.EncodedType == TYPE.PostNet;

            int pos = 0;
            while (pos < encodedValue.Length)
            {
                int runStart = pos;
                char runChar = encodedValue[pos];
                while (pos < encodedValue.Length && encodedValue[pos] == runChar)
                    pos++;
                int runLength = pos - runStart;

                bool drawBar = isPostNet || runChar == '1';
                if (drawBar)
                {
                    int x = (runStart * barWidth) + shiftAdjustment;
                    int rectWidth = runLength * barWidth;

                    int y = topLabelAdjustment;
                    int rectHeight = barsHeight;

                    if (isPostNet)
                    {
                        // PostNet draws short dashes for '0' (bottom-aligned half height) and full
                        // height bars for '1', matching the raster PostNet special-case.
                        if (runChar == '0')
                        {
                            rectHeight = barsHeight / 2;
                            y = topLabelAdjustment + (barsHeight - rectHeight);
                        }
                    }

                    root.Add(new XElement(XName.Get("rect", SvgNs),
                        new XAttribute("x", x),
                        new XAttribute("y", y),
                        new XAttribute("width", rectWidth),
                        new XAttribute("height", rectHeight),
                        new XAttribute("fill", fill)));
                }
            }

            if (barcode.IncludeLabel)
            {
                RenderLabel(barcode, root, width, height, barWidth, shiftAdjustment);
            }
        }

        private static void RenderItf14(Barcode barcode, XElement root, int width, int height)
        {
            string encodedValue = barcode.EncodedValue;

            int ilHeight = height;
            if (barcode.IncludeLabel)
                ilHeight -= barcode.LabelFont.Height;

            int bearerWidth = (int)(width / 12.05);
            int quietZone = Convert.ToInt32(width * 0.05);
            int barWidth = (width - (bearerWidth * 2) - (quietZone * 2)) / encodedValue.Length;
            int shiftAdjustment = ((width - (bearerWidth * 2) - (quietZone * 2)) % encodedValue.Length) / 2;

            if (barWidth <= 0 || quietZone <= 0)
                throw new Exception("EGENERATE_SVG-3: Image size specified not large enough to draw image. (Bar size determined to be less than 1 pixel or quiet zone determined to be less than 1 pixel)");

            string fill = ToHex(barcode.ForeColor);

            int pos = 0;
            while (pos < encodedValue.Length)
            {
                if (encodedValue[pos] == '1')
                {
                    int x = (pos * barWidth) + shiftAdjustment + bearerWidth + quietZone;
                    root.Add(new XElement(XName.Get("rect", SvgNs),
                        new XAttribute("x", x),
                        new XAttribute("y", 0),
                        new XAttribute("width", barWidth),
                        new XAttribute("height", height),
                        new XAttribute("fill", fill)));
                }
                pos++;
            }

            // Bearer bars (box) around the symbol.
            double bearerStrokeWidth = ilHeight / 8.0;
            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", ToInvariant(bearerStrokeWidth / 2)),
                new XAttribute("y", ToInvariant(bearerStrokeWidth / 2)),
                new XAttribute("width", ToInvariant(width - bearerStrokeWidth)),
                new XAttribute("height", ToInvariant(ilHeight - bearerStrokeWidth)),
                new XAttribute("fill", "none"),
                new XAttribute("stroke", fill),
                new XAttribute("stroke-width", ToInvariant(bearerStrokeWidth))));

            if (barcode.IncludeLabel)
            {
                string text = barcode.AlternateLabel ?? barcode.RawData;
                root.Add(new XElement(XName.Get("text", SvgNs),
                    new XAttribute("x", width / 2),
                    new XAttribute("y", height - 2),
                    new XAttribute("text-anchor", "middle"),
                    new XAttribute("font-family", FontFamilyName(barcode)),
                    new XAttribute("font-size", ToInvariant(barcode.LabelFont.Size)),
                    new XAttribute("fill", fill),
                    text));
            }
        }

        private static void RenderLabel(Barcode barcode, XElement root, int width, int height, int barWidth, int shiftAdjustment)
        {
            bool standardized = (barcode.AlternateLabel == null || barcode.RawData.StartsWith(barcode.AlternateLabel)) && barcode.StandardizeLabel;

            if (standardized && barcode.EncodedType == TYPE.UPCA)
            {
                RenderStandardizedUpcaLabel(barcode, root, width, height, barWidth, shiftAdjustment);
                return;
            }

            if (standardized && barcode.EncodedType == TYPE.EAN13)
            {
                RenderStandardizedEan13Label(barcode, root, width, height, barWidth, shiftAdjustment);
                return;
            }

            RenderGenericLabel(barcode, root, width, height);
        }

        private static void RenderGenericLabel(Barcode barcode, XElement root, int width, int height)
        {
            string text = barcode.AlternateLabel ?? barcode.RawData;
            var font = barcode.LabelFont;

            int labelX;
            int labelY;
            string anchor;

            switch (barcode.LabelPosition)
            {
                case LabelPositions.BOTTOMLEFT:
                    labelX = 0; labelY = height - font.Height; anchor = "start";
                    break;
                case LabelPositions.BOTTOMRIGHT:
                    labelX = width; labelY = height - font.Height; anchor = "end";
                    break;
                case LabelPositions.TOPCENTER:
                    labelX = width / 2; labelY = 0; anchor = "middle";
                    break;
                case LabelPositions.TOPLEFT:
                    labelX = 0; labelY = 0; anchor = "start";
                    break;
                case LabelPositions.TOPRIGHT:
                    labelX = width; labelY = 0; anchor = "end";
                    break;
                case LabelPositions.BOTTOMCENTER:
                default:
                    labelX = width / 2; labelY = height - font.Height; anchor = "middle";
                    break;
            }

            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", 0),
                new XAttribute("y", labelY),
                new XAttribute("width", width),
                new XAttribute("height", font.Height),
                new XAttribute("fill", ToHex(barcode.BackColor))));

            root.Add(new XElement(XName.Get("text", SvgNs),
                new XAttribute("x", labelX),
                new XAttribute("y", labelY + font.Height * 0.8),
                new XAttribute("text-anchor", anchor),
                new XAttribute("font-family", FontFamilyName(barcode)),
                new XAttribute("font-size", font.Size),
                new XAttribute("fill", ToHex(barcode.ForeColor)),
                text));
        }

        private static void RenderStandardizedUpcaLabel(Barcode barcode, XElement root, int width, int height, int barWidth, int shiftAdjustment)
        {
            string defTxt = barcode.RawData;
            int halfBarWidth = (int)(barWidth * 0.5);

            // The available width for the digit label is the space actually consumed by bars
            // (module count * GS1-aware bar width), not the full canvas width, since the reserved
            // quiet zone on either side must stay clear of label text too.
            int labelWidth = (int)(barWidth * barcode.EncodedValue.Length * 0.9f);
            int fontSize = Labels.getFontsize(barcode, labelWidth, height, defTxt);
            double smallFontSize = fontSize * 0.5;

            int labelY = height - fontSize;

            float s1 = shiftAdjustment - barWidth;
            float s2 = s1 + (barWidth * 12);
            float w2 = barWidth * 34;
            float s3 = s2 + w2 + (barWidth * 5);
            float w3 = barWidth * 34;
            float s4 = s3 + w3 + (barWidth * 8) - halfBarWidth;

            string fill = ToHex(barcode.ForeColor);
            string fontFamily = FontFamilyName(barcode);

            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", ToInvariant(s2)), new XAttribute("y", labelY),
                new XAttribute("width", ToInvariant(w2)), new XAttribute("height", fontSize),
                new XAttribute("fill", ToHex(barcode.BackColor))));
            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", ToInvariant(s3)), new XAttribute("y", labelY),
                new XAttribute("width", ToInvariant(w3)), new XAttribute("height", fontSize),
                new XAttribute("fill", ToHex(barcode.BackColor))));

            root.Add(TextElement(fontFamily, smallFontSize, fill, s1, height - 1, "start", defTxt.Substring(0, 1)));
            root.Add(TextElement(fontFamily, fontSize, fill, s2 - barWidth, labelY + fontSize * 0.8, "start", defTxt.Substring(1, 5)));
            root.Add(TextElement(fontFamily, fontSize, fill, s3 - barWidth, labelY + fontSize * 0.8, "start", defTxt.Substring(6, 5)));
            root.Add(TextElement(fontFamily, smallFontSize, fill, s4, height - 1, "start", defTxt.Substring(11)));
        }

        private static void RenderStandardizedEan13Label(Barcode barcode, XElement root, int width, int height, int barWidth, int shiftAdjustment)
        {
            string defTxt = barcode.RawData;

            int labelWidth = (int)(barWidth * barcode.EncodedValue.Length * 0.9f);
            int fontSize = Labels.getFontsize(barcode, labelWidth, height, defTxt);
            double smallFontSize = fontSize * 0.5;

            int labelY = height - fontSize;

            float s1 = shiftAdjustment - barWidth;
            float s2 = s1 + (barWidth * 4);
            float w2 = barWidth * 42;
            float s3 = s2 + w2 + (barWidth * 5);
            float w3 = barWidth * 42;

            string fill = ToHex(barcode.ForeColor);
            string fontFamily = FontFamilyName(barcode);

            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", ToInvariant(s2)), new XAttribute("y", labelY),
                new XAttribute("width", ToInvariant(w2)), new XAttribute("height", fontSize),
                new XAttribute("fill", ToHex(barcode.BackColor))));
            root.Add(new XElement(XName.Get("rect", SvgNs),
                new XAttribute("x", ToInvariant(s3)), new XAttribute("y", labelY),
                new XAttribute("width", ToInvariant(w3)), new XAttribute("height", fontSize),
                new XAttribute("fill", ToHex(barcode.BackColor))));

            root.Add(TextElement(fontFamily, smallFontSize, fill, s1, height - 1, "start", defTxt.Substring(0, 1)));
            root.Add(TextElement(fontFamily, fontSize, fill, s2, labelY + fontSize * 0.8, "start", defTxt.Substring(1, 6)));
            root.Add(TextElement(fontFamily, fontSize, fill, s3 - barWidth, labelY + fontSize * 0.8, "start", defTxt.Substring(7)));
        }

        private static XElement TextElement(string fontFamily, double fontSize, string fill, double x, double y, string anchor, string text)
        {
            return new XElement(XName.Get("text", SvgNs),
                new XAttribute("x", ToInvariant(x)),
                new XAttribute("y", ToInvariant(y)),
                new XAttribute("text-anchor", anchor),
                new XAttribute("font-family", fontFamily),
                new XAttribute("font-size", ToInvariant(fontSize)),
                new XAttribute("fill", fill),
                text);
        }

        private static string FontFamilyName(Barcode barcode)
        {
            return barcode.LabelFont != null ? barcode.LabelFont.FontFamily.Name : "Arial";
        }

        private static string ToHex(Color color)
        {
            return FormattableString.Invariant($"#{color.R:X2}{color.G:X2}{color.B:X2}");
        }

        private static string ToInvariant(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
