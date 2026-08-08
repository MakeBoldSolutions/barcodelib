using System.Drawing.Imaging;

namespace Cec.Barcode.Models
{
    using BarcodeLib;
    using System;
    using System.Drawing;
    using System.IO;
    public class BarCodeModel
    {
        public BarCodeModel()
        {
            Id = 1;
            Width = 290;
            Height = 120;
            AlignmentPosition = Cec.Barcode.Extensions.AlignmentPositions.CENTER;
            EncodedType = Cec.Barcode.Extensions.TYPE.CODE93;
            BarWidth = null;
            BarHeight = 250;
            AspectRatio = null;
            IncludeLabel = true;
            RotateFlip = RotateFlipType.Rotate180FlipXY;
            AlternateLabel = "Control Origins";
            LabelPosition = Cec.Barcode.Extensions.LabelPositions.BOTTOMCENTER;
            BarValue = "controlorigins.com";
            BackColor = Color.White;
            ForeColor = Color.Black;
        }

        public BarCodeModel(string Value)
        {
            Id = 1;
            Width = 290;
            Height = 120;
            AlignmentPosition = Cec.Barcode.Extensions.AlignmentPositions.CENTER;
            EncodedType = Cec.Barcode.Extensions.TYPE.CODE93;
            BarWidth = null;
            BarHeight = 250;
            AspectRatio = null;
            IncludeLabel = true;
            RotateFlip = RotateFlipType.Rotate180FlipXY;
            AlternateLabel = Value;
            LabelPosition = Cec.Barcode.Extensions.LabelPositions.BOTTOMCENTER;
            BarValue = Value;
            BackColor = Color.White;
            ForeColor = Color.Black;
        }



        public int Id { get; set; }
        public Cec.Barcode.Extensions.AlignmentPositions AlignmentPosition { get; set; }
        public string AlternateLabel { get; set; }
        public double? AspectRatio { get; set; }
        public Color BackColor { get; set; }
        public int BarHeight { get; set; }
        public string BarValue { get; set; }
        public int? BarWidth { get; set; }
        public string EncodedText { get; set; }
        public Cec.Barcode.Extensions.TYPE EncodedType { get; set; }
        public string EncodedTypeText { get; set; }
        public string EncodingTime { get; set; }
        public Color ForeColor { get; set; }
        public int Height { get; set; }
        public bool IncludeLabel { get; set; }
        public Cec.Barcode.Extensions.LabelPositions LabelPosition { get; set; }
        public RotateFlipType RotateFlip { get; set; }
        public int Width { get; set; }


        public Byte[] BarcodeByteArray
        {
            get
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    BarcodeImage.Save(mStream, ImageFormat.Png);
                    return mStream.ToArray();
                }
            }
        }

        public Image BarcodeImage
        {
            get
            {
                Barcode b = new Barcode();
                try
                {
                    if (EncodedType != Cec.Barcode.Extensions.TYPE.UNSPECIFIED)
                    {
                        b.EncodedType = (TYPE)EncodedType;
                        b.Alignment = (AlignmentPositions)this.AlignmentPosition;
                        b.BarWidth = this.BarWidth;
                        b.AspectRatio = this.AspectRatio;
                        b.IncludeLabel = this.IncludeLabel;
                        b.RotateFlipType = this.RotateFlip;
                        b.AlternateLabel = this.AlternateLabel;
                        b.LabelPosition = (LabelPositions)this.LabelPosition;
                    }//if
                }//try
                catch
                {
                    // do Something!
                }//catch
                return b.Encode(b.EncodedType, this.BarValue.Trim(), this.ForeColor, this.BackColor, this.Width, this.Height);
            }
        }

        public string BarcodeSvg
        {
            get
            {
                Barcode b = new Barcode();
                try
                {
                    if (EncodedType != Cec.Barcode.Extensions.TYPE.UNSPECIFIED)
                    {
                        b.EncodedType = (TYPE)EncodedType;
                        b.Alignment = (AlignmentPositions)this.AlignmentPosition;
                        b.BarWidth = this.BarWidth;
                        b.AspectRatio = this.AspectRatio;
                        b.IncludeLabel = this.IncludeLabel;
                        b.RotateFlipType = this.RotateFlip;
                        b.AlternateLabel = this.AlternateLabel;
                        b.LabelPosition = (LabelPositions)this.LabelPosition;
                    }//if
                }//try
                catch
                {
                    // do Something!
                }//catch
                b.Encode(b.EncodedType, this.BarValue.Trim(), this.ForeColor, this.BackColor, this.Width, this.Height);
                return b.GetSvg();
            }
        }
    }
}
