using System.Collections.Generic;

namespace BarcodeLib
{
    /// <summary>
    /// Barcode interface for 2D/matrix symbologies (e.g. QR Code), whose encoded data is a 2D
    /// grid of modules rather than a 1D sequence of bar/space widths. Kept separate from
    /// <see cref="IBarcode"/> (whose <c>Encoded_Value</c> is a '0'/'1' string) since a bit matrix
    /// cannot be represented that way.
    /// </summary>
    internal interface IMatrixBarcode
    {
        bool[,] Encoded_Matrix
        {
            get;
        }

        string RawData
        {
            get;
        }

        List<string> Errors
        {
            get;
        }
    }
}
