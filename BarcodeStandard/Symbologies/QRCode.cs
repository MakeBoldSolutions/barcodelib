using BarcodeLib.Symbologies.QrEncoding;
using System;
using System.Collections.Generic;

namespace BarcodeLib.Symbologies
{
    /// <summary>
    ///  QR Code encoding (thin adapter over the vendored QrEncoding encoder).
    /// </summary>
    class QRCode : IMatrixBarcode
    {
        private readonly List<string> _errors = new List<string>();
        private bool[,] _matrix;

        public QRCode(string input)
        {
            RawData = input;
        }

        public string RawData { get; }

        public List<string> Errors => _errors;

        public bool[,] Encoded_Matrix => _matrix ??= Encode();

        private bool[,] Encode()
        {
            try
            {
                var qr = QrCode.EncodeText(RawData, QrCode.Ecc.Medium);
                var matrix = new bool[qr.size, qr.size];
                for (int y = 0; y < qr.size; y++)
                {
                    for (int x = 0; x < qr.size; x++)
                    {
                        matrix[y, x] = qr.GetModule(x, y);
                    }
                }
                return matrix;
            }
            catch (Exception ex)
            {
                var message = "EQR-1: " + ex.Message;
                _errors.Add(message);
                throw new Exception(message, ex);
            }
        }
    }
}
