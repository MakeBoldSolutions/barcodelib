using System.Collections.Generic;

namespace BarcodeLib
{
    /// <summary>
    /// Minimum quiet-zone widths, expressed in symbol modules (X-dimensions), per the GS1
    /// General Specifications. Rendering reserves this much clear space on each side of the
    /// bars so scanners have the margin they expect. Where GS1 specifies asymmetric margins
    /// (e.g. EAN-13's 11-module left / 7-module right split), the larger of the two sides is
    /// used here so a single symmetric margin always meets or exceeds the GS1 minimum on both
    /// sides. Symbologies GS1 does not specifically regulate fall back to a conservative,
    /// commonly used floor of 10 modules.
    /// </summary>
    internal static class Gs1QuietZone
    {
        private const int DefaultQuietZoneModules = 10;

        private static readonly Dictionary<TYPE, int> QuietZoneModulesByType = new Dictionary<TYPE, int>
        {
            // UPC-A: GS1 General Specifications minimum quiet zone is 9 modules.
            [TYPE.UPCA] = 9,
            [TYPE.UCC12] = 9,

            // UPC-E: same 9-module minimum as UPC-A.
            [TYPE.UPCE] = 9,

            // UPC supplemental add-ons render alongside a UPC-A/EAN-13 symbol; 9 modules keeps
            // them consistent with the primary symbol's own quiet zone.
            [TYPE.UPC_SUPPLEMENTAL_2DIGIT] = 9,
            [TYPE.UPC_SUPPLEMENTAL_5DIGIT] = 9,

            // EAN-13: GS1 specifies 11 modules left / 7 modules right; use the larger (11).
            [TYPE.EAN13] = 11,
            [TYPE.UCC13] = 11,
            [TYPE.JAN13] = 11,
            [TYPE.BOOKLAND] = 11,
            [TYPE.ISBN] = 11,

            // EAN-8: GS1 minimum quiet zone is 7 modules on each side.
            [TYPE.EAN8] = 7,

            // GS1-128 (Code 128 used as a GS1 symbology): GS1 minimum quiet zone is 10 modules.
            [TYPE.CODE128] = 10,
            [TYPE.CODE128A] = 10,
            [TYPE.CODE128B] = 10,
            [TYPE.CODE128C] = 10,

            // ITF-14 has its own dedicated bearer-bar-aware rendering path (see
            // Barcode.Generate_Image()) and is not routed through the shared quiet-zone helper,
            // but its entry is included here for completeness/documentation purposes.
            [TYPE.ITF14] = 10,
        };

        /// <summary>
        /// Gets the minimum GS1 quiet zone, in symbol modules, for the given barcode type. Types
        /// with no specific GS1 requirement return a conservative default floor.
        /// </summary>
        internal static int GetQuietZoneModules(TYPE type)
        {
            return QuietZoneModulesByType.TryGetValue(type, out var modules) ? modules : DefaultQuietZoneModules;
        }
    }
}
