// QR Code generator library
// Ported to C# from the Java implementation:
// https://github.com/nayuki/QR-Code-generator (java/src/main/java/io/nayuki/qrcodegen/BitBuffer.java)
//
// Copyright (c) Project Nayuki. (MIT License)
// https://www.nayuki.io/page/qr-code-generator-library
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// - The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
// - The Software is provided "as is", without warranty of any kind, express or
//   implied, including but not limited to the warranties of merchantability,
//   fitness for a particular purpose and noninfringement. In no event shall the
//   authors or copyright holders be liable for any claim, damages or other
//   liability, whether in an action of contract, tort or otherwise, arising from,
//   out of or in connection with the Software or the use or other dealings in the
//   Software.

using System;
using System.Collections.Generic;

namespace BarcodeLib.Symbologies.QrEncoding
{
    /// <summary>
    /// An appendable sequence of bits (0s and 1s). Mainly used by <see cref="QrSegment"/>.
    /// </summary>
    internal sealed class BitBuffer
    {
        private List<bool> data;

        public BitBuffer()
        {
            data = new List<bool>();
        }

        public int BitLength => data.Count;

        public int GetBit(int index)
        {
            if (index < 0 || index >= data.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return data[index] ? 1 : 0;
        }

        /// <summary>
        /// Appends the specified number of low-order bits of the specified value to this
        /// buffer. Requires 0 &lt;= len &lt;= 31 and 0 &lt;= val &lt; 2^len.
        /// </summary>
        public void AppendBits(int val, int len)
        {
            if (len < 0 || len > 31 || (val >>> len) != 0)
                throw new ArgumentException("Value out of range");
            for (int i = len - 1; i >= 0; i--)
                data.Add(QrCode.GetBit(val, i));
        }

        public void AppendData(BitBuffer bb)
        {
            if (bb == null)
                throw new ArgumentNullException(nameof(bb));
            data.AddRange(bb.data);
        }

        public BitBuffer Clone()
        {
            var result = new BitBuffer();
            result.data = new List<bool>(data);
            return result;
        }
    }
}
