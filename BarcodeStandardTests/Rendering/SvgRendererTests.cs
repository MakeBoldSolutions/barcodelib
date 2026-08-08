using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;

namespace BarcodeStandardTests.Rendering
{
    /// <summary>
    /// Verifies Barcode.GetSvg() produces well-formed, geometrically correct SVG. Geometry
    /// assertions use EnforceGS1QuietZone = false where exact bar positions are checked, since
    /// that setting reproduces the well-known, easily-independently-derived pre-3.2.0 formula
    /// (barWidth = Width / moduleCount, center shiftAdjustment = (Width % moduleCount) / 2) without
    /// needing access to the internal GS1 quiet-zone module table across the test assembly
    /// boundary. Quiet-zone-enabled tests instead assert the qualitative property that a real
    /// margin exists before the first bar, which is what Barcode.EnforceGS1QuietZone actually
    /// promises.
    /// </summary>
    [TestClass]
    public class SvgRendererTests
    {
        private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

        [TestMethod]
        public void GetSvg_ProducesWellFormedXml_WithSvgRootAndMatchingDimensions()
        {
            var barcode = new Barcode { EncodedType = TYPE.CODE39 };
            barcode.Encode(TYPE.CODE39, "ABC", 300, 100);

            var doc = XDocument.Parse(barcode.GetSvg());

            Assert.AreEqual(Svg + "svg", doc.Root.Name);
            Assert.AreEqual("300", (string)doc.Root.Attribute("width"));
            Assert.AreEqual("100", (string)doc.Root.Attribute("height"));
        }

        [TestMethod]
        public void GetSvg_BeforeEncoding_Throws()
        {
            var barcode = new Barcode();
            Exception caught = null;
            try
            {
                barcode.GetSvg();
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
        }

        [TestMethod]
        public void GetSvg_WithoutGs1QuietZone_RectPositionsMatchLegacyFormula()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.CODE39,
                EnforceGS1QuietZone = false,
            };
            barcode.Encode(TYPE.CODE39, "A", 300, 100);

            string encodedValue = barcode.EncodedValue;
            int barWidth = barcode.Width / encodedValue.Length;
            int shiftAdjustment = (barcode.Width % encodedValue.Length) / 2; // CENTER is the default alignment

            var expectedRects = ExpectedBarRects(encodedValue, barWidth, shiftAdjustment);

            var doc = XDocument.Parse(barcode.GetSvg());
            var actualRects = doc.Root.Elements(Svg + "rect")
                .Where(r => (string)r.Attribute("fill") == "#000000")
                .Select(r => ((int)r.Attribute("x"), (int)r.Attribute("width")))
                .ToList();

            CollectionAssert.AreEqual(expectedRects, actualRects);
        }

        private static System.Collections.Generic.List<(int x, int width)> ExpectedBarRects(string encodedValue, int barWidth, int shiftAdjustment)
        {
            var rects = new System.Collections.Generic.List<(int, int)>();
            int pos = 0;
            while (pos < encodedValue.Length)
            {
                int runStart = pos;
                char runChar = encodedValue[pos];
                while (pos < encodedValue.Length && encodedValue[pos] == runChar)
                    pos++;

                if (runChar == '1')
                {
                    int x = (runStart * barWidth) + shiftAdjustment;
                    int width = (pos - runStart) * barWidth;
                    rects.Add((x, width));
                }
            }
            return rects;
        }

        [TestMethod]
        public void GetSvg_WithGs1QuietZoneEnforced_LeavesMarginBeforeFirstBar()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.CODE128,
                EnforceGS1QuietZone = true,
            };
            barcode.Encode(TYPE.CODE128, "SVG-TEST", 300, 100);

            var doc = XDocument.Parse(barcode.GetSvg());
            var firstBar = doc.Root.Elements(Svg + "rect")
                .First(r => (string)r.Attribute("fill") == "#000000");

            Assert.IsTrue((int)firstBar.Attribute("x") > 0,
                "Expected a reserved quiet-zone margin before the first bar.");
        }

        [TestMethod]
        public void GetSvg_IncludeLabelOnGenericSymbology_EmitsTextElementWithRawData()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.CODE128,
                IncludeLabel = true,
            };
            barcode.Encode(TYPE.CODE128, "LABELTEST", 300, 100);

            var doc = XDocument.Parse(barcode.GetSvg());
            var textElements = doc.Root.Elements(Svg + "text").ToList();

            Assert.AreEqual(1, textElements.Count);
            Assert.AreEqual("LABELTEST", textElements[0].Value);
        }

        [TestMethod]
        public void GetSvg_UpcaStandardizedLabel_EmitsFourTextSegments()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.UPCA,
                IncludeLabel = true,
            };
            barcode.Encode(TYPE.UPCA, "036000291452", 300, 150);

            var doc = XDocument.Parse(barcode.GetSvg());
            var textElements = doc.Root.Elements(Svg + "text").ToList();

            // Standardized UPC-A label: leading digit, two 5-digit groups, trailing digit.
            Assert.AreEqual(4, textElements.Count);
            Assert.AreEqual("0" + "36000" + "29145" + "2", string.Concat(textElements.Select(t => t.Value)));
        }

        [TestMethod]
        public void GetSvg_Ean13StandardizedLabel_EmitsThreeTextSegments()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.EAN13,
                IncludeLabel = true,
            };
            barcode.Encode(TYPE.EAN13, "5901234123457", 300, 150);

            var doc = XDocument.Parse(barcode.GetSvg());
            var textElements = doc.Root.Elements(Svg + "text").ToList();

            Assert.AreEqual(3, textElements.Count);
            Assert.AreEqual("5901234123457", string.Concat(textElements.Select(t => t.Value)));
        }

        [TestMethod]
        public void GetSvg_Itf14_IncludesBearerBarAndCenteredLabel()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.ITF14,
                IncludeLabel = true,
            };
            barcode.Encode(TYPE.ITF14, "15400141288763", 300, 150);

            var doc = XDocument.Parse(barcode.GetSvg());

            Assert.IsTrue(doc.Root.Elements(Svg + "rect").Any(r => (string)r.Attribute("stroke") == "#000000"),
                "Expected a bearer-bar rectangle with a stroke.");

            var textElements = doc.Root.Elements(Svg + "text").ToList();
            Assert.AreEqual(1, textElements.Count);
            Assert.AreEqual("15400141288763", textElements[0].Value);
            Assert.AreEqual("middle", (string)textElements[0].Attribute("text-anchor"));
        }

        [TestMethod]
        public void GetSvg_PostNet_DrawsHalfHeightDashesForZeroBitsAndFullHeightForOneBits()
        {
            // PostNet's own symbol table (e.g. digit 0 = "11000") can contain consecutive
            // identical bits, which the run-length rect emission merges into a single wider
            // rect -- so rect count isn't 1:1 with EncodedValue.Length. Assert the two heights
            // that actually matter (full height for '1' runs, half height bottom-aligned for '0'
            // runs) are both present and correctly proportioned instead.
            var barcode = new Barcode
            {
                EncodedType = TYPE.PostNet,
                IncludeLabel = false,
            };
            barcode.Encode(TYPE.PostNet, "554419712", 300, 60);

            var doc = XDocument.Parse(barcode.GetSvg());
            var rects = doc.Root.Elements(Svg + "rect")
                .Where(r => (string)r.Attribute("fill") == "#000000")
                .ToList();

            Assert.IsTrue(rects.Any(r => (int)r.Attribute("height") == barcode.Height),
                "Expected at least one full-height bar for a '1' run.");

            var shortRect = rects.FirstOrDefault(r => (int)r.Attribute("height") < barcode.Height);
            Assert.IsNotNull(shortRect, "Expected at least one half-height dash for a '0' run.");
            Assert.AreEqual(barcode.Height / 2, (int)shortRect.Attribute("height"));

            // Half-height dashes are bottom-aligned (y = half the height), not top-aligned.
            Assert.AreEqual(barcode.Height / 2, (int)shortRect.Attribute("y"));
        }
    }
}
