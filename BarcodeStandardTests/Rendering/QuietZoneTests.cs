using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace BarcodeStandardTests.Rendering
{
    /// <summary>
    /// Verifies that EnforceGS1QuietZone (default true) actually reserves real background pixel
    /// margin on rendered images, by sampling pixels rather than just checking the render doesn't
    /// throw. GS1's General Specifications minimum quiet zone for UPC-A is 9 modules; with
    /// EnforceGS1QuietZone = false the pre-3.2.0 geometry (no reserved quiet zone) is reproduced.
    /// </summary>
    [TestClass]
    public class QuietZoneTests
    {
        private const int UpcAQuietZoneModules = 9;
        private const int ImageWidth = 300;
        private const int ImageHeight = 150;

        private static bool ColumnBandIsAllBackground(Bitmap bitmap, int startX, int columnCount, Color backColor)
        {
            // Compare via ToArgb(), not Color's == operator: GetPixel() returns Colors built from
            // a raw ARGB value, which do not compare equal to a "known color" constant like
            // Color.White via == even when the RGB bytes are identical (Color's equality also
            // considers internal known-color state, not just the packed ARGB value).
            int backArgb = backColor.ToArgb();
            int y = bitmap.Height / 2;
            for (int x = startX; x < startX + columnCount; x++)
            {
                if (bitmap.GetPixel(x, y).ToArgb() != backArgb)
                    return false;
            }
            return true;
        }

        [TestMethod]
        public void Upca_WithGs1QuietZoneEnforced_ReservesBackgroundMarginOnBothEdges()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.UPCA,
                IncludeLabel = false,
                EnforceGS1QuietZone = true,
            };

            using var image = barcode.Encode(TYPE.UPCA, "036000291452", ImageWidth, ImageHeight);
            using var bitmap = (Bitmap)image;

            // barWidth = Width / (moduleCount + 2*quietZoneModules), quietZonePx = quietZoneModules * barWidth
            int moduleCount = barcode.EncodedValue.Length;
            int barWidth = ImageWidth / (moduleCount + (2 * UpcAQuietZoneModules));
            int quietZonePx = UpcAQuietZoneModules * barWidth;

            Assert.IsTrue(quietZonePx > 0, "Test setup produced a zero-pixel quiet zone; increase ImageWidth.");
            Assert.IsTrue(ColumnBandIsAllBackground(bitmap, 0, quietZonePx, Color.White),
                "Expected the left quiet zone to be pure background.");
            Assert.IsTrue(ColumnBandIsAllBackground(bitmap, ImageWidth - quietZonePx, quietZonePx, Color.White),
                "Expected the right quiet zone to be pure background.");
        }

        [TestMethod]
        public void Upca_WithGs1QuietZoneDisabled_DoesNotReserveFullMargin()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.UPCA,
                IncludeLabel = false,
                EnforceGS1QuietZone = false,
            };

            using var image = barcode.Encode(TYPE.UPCA, "036000291452", ImageWidth, ImageHeight);
            using var bitmap = (Bitmap)image;

            // Same pixel band GS1 would require (9 modules at the *GS1-enforced* bar width) should
            // NOT be fully background once quiet-zone reservation is turned off, since UPC-A's
            // guard pattern starts drawing bars immediately and alignment centering alone isn't
            // wide enough to keep this whole band clear.
            int moduleCount = barcode.EncodedValue.Length;
            int enforcedBarWidth = ImageWidth / (moduleCount + (2 * UpcAQuietZoneModules));
            int comparisonBandPx = UpcAQuietZoneModules * enforcedBarWidth;

            Assert.IsFalse(ColumnBandIsAllBackground(bitmap, 0, comparisonBandPx, Color.White),
                "Expected the legacy (no quiet zone) geometry to paint into the GS1 quiet-zone band.");
        }

        [TestMethod]
        public void EnforceGS1QuietZone_DefaultsToTrue()
        {
            var barcode = new Barcode();
            Assert.IsTrue(barcode.EnforceGS1QuietZone);
        }
    }
}
