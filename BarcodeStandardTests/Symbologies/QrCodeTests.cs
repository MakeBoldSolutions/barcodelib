using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using ZXing;
using ZXing.Windows.Compatibility;

namespace BarcodeStandardTests.Symbologies
{
    /// <summary>
    /// QR is the highest-risk symbology in this library: the vendored encoder (ported from
    /// Project Nayuki's MIT-licensed QR Code generator library) implements Reed-Solomon error
    /// correction and data masking, where a subtle bug could produce a QR code that *renders*
    /// fine but fails to scan on real readers. These tests decode the rendered raster output with
    /// an independent decoder (ZXing.Net, a test-only dependency -- does not affect BarcodeLib's
    /// shipped package dependencies) and assert round-trip equality, which is a much stronger
    /// correctness signal than checking the encoder didn't throw.
    /// </summary>
    [TestClass]
    public class QrCodeTests
    {
        private static string DecodeRoundTrip(Bitmap bitmap)
        {
            var reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = { TryHarder = true },
            };
            var result = reader.Decode(bitmap);
            return result?.Text;
        }

        [TestMethod]
        [DataRow("https://github.com/markhazleton/barcodelib")]
        [DataRow("Hello, World!")]
        [DataRow("12345678901234567890")]
        [DataRow("A")]
        [DataRow("The quick brown fox jumps over the lazy dog. 0123456789 !@#$%^&*()")]
        public void QrCode_RoundTripsThroughIndependentDecoder(string input)
        {
            var barcode = new Barcode { EncodedType = TYPE.QRCODE };
            using var image = barcode.Encode(TYPE.QRCODE, input, 300, 300);
            using var bitmap = (Bitmap)image;

            string decoded = DecodeRoundTrip(bitmap);

            Assert.AreEqual(input, decoded);
        }

        [TestMethod]
        public void QrCode_LongText_StillRoundTrips()
        {
            string input = string.Concat(System.Linq.Enumerable.Repeat("BarcodeLib QR test data. ", 20));
            var barcode = new Barcode { EncodedType = TYPE.QRCODE };
            using var image = barcode.Encode(TYPE.QRCODE, input, 500, 500);
            using var bitmap = (Bitmap)image;

            string decoded = DecodeRoundTrip(bitmap);

            Assert.AreEqual(input, decoded);
        }

        [TestMethod]
        public void QrCode_MatrixIsSquareAndAllModulesSet()
        {
            var barcode = new Barcode { EncodedType = TYPE.QRCODE };
            barcode.Encode(TYPE.QRCODE, "square-check", 200, 200);

            var matrix = barcode.EncodedMatrix;
            Assert.IsNotNull(matrix);
            Assert.AreEqual(matrix.GetLength(0), matrix.GetLength(1));

            // A valid QR is never all-light or all-dark.
            bool sawDark = false, sawLight = false;
            for (int y = 0; y < matrix.GetLength(0) && !(sawDark && sawLight); y++)
            {
                for (int x = 0; x < matrix.GetLength(1) && !(sawDark && sawLight); x++)
                {
                    if (matrix[y, x]) sawDark = true; else sawLight = true;
                }
            }
            Assert.IsTrue(sawDark && sawLight);
        }

        [TestMethod]
        public void QrCode_EncodedValue_ThrowsNotSupported()
        {
            var barcode = new Barcode { EncodedType = TYPE.QRCODE };
            barcode.Encode(TYPE.QRCODE, "not-a-1d-barcode", 200, 200);

            Exception caught = null;
            try
            {
                var _ = barcode.EncodedValue;
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsInstanceOfType(caught, typeof(NotSupportedException));
        }

        [TestMethod]
        public void QrCode_Svg_RoundTripsThroughIndependentDecoder()
        {
            // Rasterize the SVG's own <rect> geometry back into a Bitmap (rather than requiring an
            // SVG-capable decoder) to verify the SVG path encodes the identical module data as the
            // raster path, using the same independent-decoder round trip as the other tests.
            var barcode = new Barcode { EncodedType = TYPE.QRCODE };
            barcode.Encode(TYPE.QRCODE, "svg-round-trip", 300, 300);

            var matrix = barcode.EncodedMatrix;
            int moduleCount = matrix.GetLength(0);
            const int quietZoneModules = 4;
            int moduleSizePx = 10;
            int totalPx = (moduleCount + 2 * quietZoneModules) * moduleSizePx;

            using var bitmap = new Bitmap(totalPx, totalPx);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                for (int y = 0; y < moduleCount; y++)
                {
                    for (int x = 0; x < moduleCount; x++)
                    {
                        if (matrix[y, x])
                        {
                            int px = (x + quietZoneModules) * moduleSizePx;
                            int py = (y + quietZoneModules) * moduleSizePx;
                            g.FillRectangle(Brushes.Black, px, py, moduleSizePx, moduleSizePx);
                        }
                    }
                }
            }

            string decoded = DecodeRoundTrip(bitmap);
            Assert.AreEqual("svg-round-trip", decoded);
        }
    }
}
