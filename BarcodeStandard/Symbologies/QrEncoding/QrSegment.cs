// QR Code generator library
// Ported to C# from the Java implementation:
// https://github.com/nayuki/QR-Code-generator (java/src/main/java/io/nayuki/qrcodegen/QrSegment.java)
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
//
// Note: porting omits QrSegmentAdvanced (kanji mode / optimal segment mixing),
// which is out of scope for this library's QR support.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BarcodeLib.Symbologies.QrEncoding
{
    /// <summary>
    /// A segment of character/binary/control data in a QR Code symbol.
    /// </summary>
    internal sealed class QrSegment
    {
        /*---- Static factory functions (mid level) ----*/

        public static QrSegment MakeBytes(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            var bb = new BitBuffer();
            foreach (byte b in data)
                bb.AppendBits(b, 8);
            return new QrSegment(Mode.Byte, data.Length, bb);
        }

        public static QrSegment MakeNumeric(string digits)
        {
            if (digits == null)
                throw new ArgumentNullException(nameof(digits));
            if (!IsNumeric(digits))
                throw new ArgumentException("String contains non-numeric characters");

            var bb = new BitBuffer();
            for (int i = 0; i < digits.Length;)
            {
                int n = Math.Min(digits.Length - i, 3);
                bb.AppendBits(int.Parse(digits.Substring(i, n)), n * 3 + 1);
                i += n;
            }
            return new QrSegment(Mode.Numeric, digits.Length, bb);
        }

        public static QrSegment MakeAlphanumeric(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (!IsAlphanumeric(text))
                throw new ArgumentException("String contains unencodable characters in alphanumeric mode");

            var bb = new BitBuffer();
            int i;
            for (i = 0; i <= text.Length - 2; i += 2)
            {
                int temp = AlphanumericCharset.IndexOf(text[i]) * 45;
                temp += AlphanumericCharset.IndexOf(text[i + 1]);
                bb.AppendBits(temp, 11);
            }
            if (i < text.Length)
                bb.AppendBits(AlphanumericCharset.IndexOf(text[i]), 6);
            return new QrSegment(Mode.Alphanumeric, text.Length, bb);
        }

        public static List<QrSegment> MakeSegments(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var result = new List<QrSegment>();
            if (text.Length == 0)
            {
                // Leave result empty
            }
            else if (IsNumeric(text))
                result.Add(MakeNumeric(text));
            else if (IsAlphanumeric(text))
                result.Add(MakeAlphanumeric(text));
            else
                result.Add(MakeBytes(Encoding.UTF8.GetBytes(text)));
            return result;
        }

        public static bool IsNumeric(string text)
        {
            return NumericRegex.IsMatch(text);
        }

        public static bool IsAlphanumeric(string text)
        {
            return AlphanumericRegex.IsMatch(text);
        }

        /*---- Instance fields ----*/

        public readonly Mode mode;
        public readonly int numChars;
        internal readonly BitBuffer data;

        /*---- Constructor (low level) ----*/

        public QrSegment(Mode md, int numCh, BitBuffer data)
        {
            mode = md ?? throw new ArgumentNullException(nameof(md));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (numCh < 0)
                throw new ArgumentException("Invalid value");
            numChars = numCh;
            this.data = data.Clone();
        }

        public BitBuffer GetData()
        {
            return data.Clone();
        }

        // Calculates the number of bits needed to encode the given segments at the given version.
        // Returns a non-negative number if successful, or -1 if a segment has too many characters
        // to fit its length field, or the total bits exceeds int.MaxValue.
        internal static int GetTotalBits(List<QrSegment> segs, int version)
        {
            if (segs == null)
                throw new ArgumentNullException(nameof(segs));
            long result = 0;
            foreach (var seg in segs)
            {
                int ccbits = seg.mode.NumCharCountBits(version);
                if (seg.numChars >= (1 << ccbits))
                    return -1;
                result += 4L + ccbits + seg.data.BitLength;
                if (result > int.MaxValue)
                    return -1;
            }
            return (int)result;
        }

        /*---- Constants ----*/

        private static readonly Regex NumericRegex = new Regex("^[0-9]*$", RegexOptions.Compiled);
        private static readonly Regex AlphanumericRegex = new Regex("^[A-Z0-9 $%*+./:-]*$", RegexOptions.Compiled);

        internal const string AlphanumericCharset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

        /*---- Mode (ported from Java's nested enum with fields) ----*/

        public sealed class Mode
        {
            public static readonly Mode Numeric = new Mode(0x1, 10, 12, 14);
            public static readonly Mode Alphanumeric = new Mode(0x2, 9, 11, 13);
            public static readonly Mode Byte = new Mode(0x4, 8, 16, 16);
            public static readonly Mode Kanji = new Mode(0x8, 8, 10, 12);
            public static readonly Mode Eci = new Mode(0x7, 0, 0, 0);

            internal readonly int modeBits;
            private readonly int[] numBitsCharCount;

            private Mode(int mode, params int[] ccbits)
            {
                modeBits = mode;
                numBitsCharCount = ccbits;
            }

            internal int NumCharCountBits(int ver)
            {
                return numBitsCharCount[(ver + 7) / 17];
            }
        }
    }
}
