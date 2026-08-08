// QR Code generator library
// Ported to C# from the Java implementation:
// https://github.com/nayuki/QR-Code-generator (java/src/main/java/io/nayuki/qrcodegen/QrCode.java)
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
using System.Linq;

namespace BarcodeLib.Symbologies.QrEncoding
{
    /// <summary>
    /// A QR Code symbol, which is a type of two-dimension barcode. Invented by Denso Wave and
    /// described in the ISO/IEC 18004 standard. Instances of this class represent an immutable
    /// square grid of dark and light cells, covering QR Code Model 2, all versions 1 to 40, all
    /// 4 error correction levels, and the numeric/alphanumeric/byte character encoding modes
    /// (kanji mode and the "optimal segment mixing" advanced API are not ported).
    /// </summary>
    internal sealed class QrCode
    {
        /*---- Static factory functions (high level) ----*/

        public static QrCode EncodeText(string text, Ecc ecl)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            List<QrSegment> segs = QrSegment.MakeSegments(text);
            return EncodeSegments(segs, ecl);
        }

        public static QrCode EncodeBinary(byte[] data, Ecc ecl)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            QrSegment seg = QrSegment.MakeBytes(data);
            return EncodeSegments(new List<QrSegment> { seg }, ecl);
        }

        /*---- Static factory functions (mid level) ----*/

        public static QrCode EncodeSegments(List<QrSegment> segs, Ecc ecl)
        {
            return EncodeSegments(segs, ecl, MinVersion, MaxVersion, -1, true);
        }

        public static QrCode EncodeSegments(List<QrSegment> segs, Ecc ecl, int minVersion, int maxVersion, int mask, bool boostEcl)
        {
            if (segs == null)
                throw new ArgumentNullException(nameof(segs));
            if (!(MinVersion <= minVersion && minVersion <= maxVersion && maxVersion <= MaxVersion) || mask < -1 || mask > 7)
                throw new ArgumentException("Invalid value");

            // Find the minimal version number to use
            int version, dataUsedBits;
            for (version = minVersion; ; version++)
            {
                int dataCapacityBits = GetNumDataCodewords(version, ecl) * 8;
                dataUsedBits = QrSegment.GetTotalBits(segs, version);
                if (dataUsedBits != -1 && dataUsedBits <= dataCapacityBits)
                    break;
                if (version >= maxVersion)
                {
                    string msg = "Segment too long";
                    if (dataUsedBits != -1)
                        msg = $"Data length = {dataUsedBits} bits, Max capacity = {dataCapacityBits} bits";
                    throw new DataTooLongException(msg);
                }
            }

            // Increase the error correction level while the data still fits in the current version number
            foreach (Ecc newEcl in new[] { Ecc.Low, Ecc.Medium, Ecc.Quartile, Ecc.High })
            {
                if (boostEcl && dataUsedBits <= GetNumDataCodewords(version, newEcl) * 8)
                    ecl = newEcl;
            }

            // Concatenate all segments to create the data bit string
            var bb = new BitBuffer();
            foreach (var seg in segs)
            {
                bb.AppendBits(seg.mode.modeBits, 4);
                bb.AppendBits(seg.numChars, seg.mode.NumCharCountBits(version));
                bb.AppendData(seg.data);
            }

            // Add terminator and pad up to a byte if applicable
            int dataCapacityBits2 = GetNumDataCodewords(version, ecl) * 8;
            bb.AppendBits(0, Math.Min(4, dataCapacityBits2 - bb.BitLength));
            bb.AppendBits(0, (8 - bb.BitLength % 8) % 8);

            // Pad with alternating bytes until data capacity is reached
            for (int padByte = 0xEC; bb.BitLength < dataCapacityBits2; padByte ^= 0xEC ^ 0x11)
                bb.AppendBits(padByte, 8);

            // Pack bits into bytes in big endian
            byte[] dataCodewords = new byte[bb.BitLength / 8];
            for (int i = 0; i < bb.BitLength; i++)
                dataCodewords[i >>> 3] |= (byte)(bb.GetBit(i) << (7 - (i & 7)));

            return new QrCode(version, ecl, dataCodewords, mask);
        }

        /*---- Instance fields ----*/

        public readonly int version;
        public readonly int size;
        public readonly Ecc errorCorrectionLevel;
        public readonly int mask;

        private bool[][] modules;
        private bool[][] isFunction;

        /*---- Constructor (low level) ----*/

        public QrCode(int ver, Ecc ecl, byte[] dataCodewords, int msk)
        {
            if (ver < MinVersion || ver > MaxVersion)
                throw new ArgumentException("Version value out of range");
            if (msk < -1 || msk > 7)
                throw new ArgumentException("Mask value out of range");
            version = ver;
            size = ver * 4 + 17;
            errorCorrectionLevel = ecl;
            if (dataCodewords == null)
                throw new ArgumentNullException(nameof(dataCodewords));
            modules = CreateGrid(size);
            isFunction = CreateGrid(size);

            DrawFunctionPatterns();
            byte[] allCodewords = AddEccAndInterleave(dataCodewords);
            DrawCodewords(allCodewords);

            if (msk == -1)
            {
                int minPenalty = int.MaxValue;
                for (int i = 0; i < 8; i++)
                {
                    ApplyMask(i);
                    DrawFormatBits(i);
                    int penalty = GetPenaltyScore();
                    if (penalty < minPenalty)
                    {
                        msk = i;
                        minPenalty = penalty;
                    }
                    ApplyMask(i);
                }
            }
            mask = msk;
            ApplyMask(msk);
            DrawFormatBits(msk);

            isFunction = null;
        }

        private static bool[][] CreateGrid(int size)
        {
            var grid = new bool[size][];
            for (int i = 0; i < size; i++)
                grid[i] = new bool[size];
            return grid;
        }

        /*---- Public instance methods ----*/

        public bool GetModule(int x, int y)
        {
            return 0 <= x && x < size && 0 <= y && y < size && modules[y][x];
        }

        /*---- Private helper methods for constructor: Drawing function modules ----*/

        private void DrawFunctionPatterns()
        {
            for (int i = 0; i < size; i++)
            {
                SetFunctionModule(6, i, i % 2 == 0);
                SetFunctionModule(i, 6, i % 2 == 0);
            }

            DrawFinderPattern(3, 3);
            DrawFinderPattern(size - 4, 3);
            DrawFinderPattern(3, size - 4);

            int[] alignPatPos = GetAlignmentPatternPositions();
            int numAlign = alignPatPos.Length;
            for (int i = 0; i < numAlign; i++)
            {
                for (int j = 0; j < numAlign; j++)
                {
                    if (!(i == 0 && j == 0 || i == 0 && j == numAlign - 1 || i == numAlign - 1 && j == 0))
                        DrawAlignmentPattern(alignPatPos[i], alignPatPos[j]);
                }
            }

            DrawFormatBits(0);
            DrawVersion();
        }

        private void DrawFormatBits(int msk)
        {
            int data = FormatBitsFor(errorCorrectionLevel) << 3 | msk;
            int rem = data;
            for (int i = 0; i < 10; i++)
                rem = (rem << 1) ^ ((rem >>> 9) * 0x537);
            int bits = (data << 10 | rem) ^ 0x5412;

            for (int i = 0; i <= 5; i++)
                SetFunctionModule(8, i, GetBit(bits, i));
            SetFunctionModule(8, 7, GetBit(bits, 6));
            SetFunctionModule(8, 8, GetBit(bits, 7));
            SetFunctionModule(7, 8, GetBit(bits, 8));
            for (int i = 9; i < 15; i++)
                SetFunctionModule(14 - i, 8, GetBit(bits, i));

            for (int i = 0; i < 8; i++)
                SetFunctionModule(size - 1 - i, 8, GetBit(bits, i));
            for (int i = 8; i < 15; i++)
                SetFunctionModule(8, size - 15 + i, GetBit(bits, i));
            SetFunctionModule(8, size - 8, true);
        }

        private void DrawVersion()
        {
            if (version < 7)
                return;

            int rem = version;
            for (int i = 0; i < 12; i++)
                rem = (rem << 1) ^ ((rem >>> 11) * 0x1F25);
            int bits = version << 12 | rem;

            for (int i = 0; i < 18; i++)
            {
                bool bit = GetBit(bits, i);
                int a = size - 11 + i % 3;
                int b = i / 3;
                SetFunctionModule(a, b, bit);
                SetFunctionModule(b, a, bit);
            }
        }

        private void DrawFinderPattern(int x, int y)
        {
            for (int dy = -4; dy <= 4; dy++)
            {
                for (int dx = -4; dx <= 4; dx++)
                {
                    int dist = Math.Max(Math.Abs(dx), Math.Abs(dy));
                    int xx = x + dx, yy = y + dy;
                    if (0 <= xx && xx < size && 0 <= yy && yy < size)
                        SetFunctionModule(xx, yy, dist != 2 && dist != 4);
                }
            }
        }

        private void DrawAlignmentPattern(int x, int y)
        {
            for (int dy = -2; dy <= 2; dy++)
            {
                for (int dx = -2; dx <= 2; dx++)
                    SetFunctionModule(x + dx, y + dy, Math.Max(Math.Abs(dx), Math.Abs(dy)) != 1);
            }
        }

        private void SetFunctionModule(int x, int y, bool isDark)
        {
            modules[y][x] = isDark;
            isFunction[y][x] = true;
        }

        /*---- Private helper methods for constructor: Codewords and masking ----*/

        private byte[] AddEccAndInterleave(byte[] data)
        {
            if (data.Length != GetNumDataCodewords(version, errorCorrectionLevel))
                throw new ArgumentException();

            int numBlocks = NumErrorCorrectionBlocks[(int)errorCorrectionLevel][version];
            int blockEccLen = EccCodewordsPerBlock[(int)errorCorrectionLevel][version];
            int rawCodewords = GetNumRawDataModules(version) / 8;
            int numShortBlocks = numBlocks - rawCodewords % numBlocks;
            int shortBlockLen = rawCodewords / numBlocks;

            var blocks = new byte[numBlocks][];
            byte[] rsDiv = ReedSolomonComputeDivisor(blockEccLen);
            for (int i = 0, k = 0; i < numBlocks; i++)
            {
                int datLen = shortBlockLen - blockEccLen + (i < numShortBlocks ? 0 : 1);
                byte[] dat = CopyOfRange(data, k, k + datLen);
                k += dat.Length;
                byte[] block = CopyOf(dat, shortBlockLen + 1);
                byte[] ecc = ReedSolomonComputeRemainder(dat, rsDiv);
                Array.Copy(ecc, 0, block, block.Length - blockEccLen, ecc.Length);
                blocks[i] = block;
            }

            var result = new byte[rawCodewords];
            for (int i = 0, k = 0; i < blocks[0].Length; i++)
            {
                for (int j = 0; j < blocks.Length; j++)
                {
                    if (i != shortBlockLen - blockEccLen || j >= numShortBlocks)
                    {
                        result[k] = blocks[j][i];
                        k++;
                    }
                }
            }
            return result;
        }

        private void DrawCodewords(byte[] data)
        {
            if (data.Length != GetNumRawDataModules(version) / 8)
                throw new ArgumentException();

            int i = 0;
            for (int right = size - 1; right >= 1; right -= 2)
            {
                if (right == 6)
                    right = 5;
                for (int vert = 0; vert < size; vert++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int x = right - j;
                        bool upward = ((right + 1) & 2) == 0;
                        int y = upward ? size - 1 - vert : vert;
                        if (!isFunction[y][x] && i < data.Length * 8)
                        {
                            modules[y][x] = GetBit(data[i >>> 3], 7 - (i & 7));
                            i++;
                        }
                    }
                }
            }
        }

        private void ApplyMask(int msk)
        {
            if (msk < 0 || msk > 7)
                throw new ArgumentException("Mask value out of range");
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool invert;
                    switch (msk)
                    {
                        case 0: invert = (x + y) % 2 == 0; break;
                        case 1: invert = y % 2 == 0; break;
                        case 2: invert = x % 3 == 0; break;
                        case 3: invert = (x + y) % 3 == 0; break;
                        case 4: invert = (x / 3 + y / 2) % 2 == 0; break;
                        case 5: invert = x * y % 2 + x * y % 3 == 0; break;
                        case 6: invert = (x * y % 2 + x * y % 3) % 2 == 0; break;
                        case 7: invert = ((x + y) % 2 + x * y % 3) % 2 == 0; break;
                        default: throw new InvalidOperationException();
                    }
                    modules[y][x] ^= invert & !isFunction[y][x];
                }
            }
        }

        private int GetPenaltyScore()
        {
            int result = 0;

            for (int y = 0; y < size; y++)
            {
                bool runColor = false;
                int runX = 0;
                int[] runHistory = new int[7];
                for (int x = 0; x < size; x++)
                {
                    if (modules[y][x] == runColor)
                    {
                        runX++;
                        if (runX == 5)
                            result += PenaltyN1;
                        else if (runX > 5)
                            result++;
                    }
                    else
                    {
                        FinderPenaltyAddHistory(runX, runHistory);
                        if (!runColor)
                            result += FinderPenaltyCountPatterns(runHistory) * PenaltyN3;
                        runColor = modules[y][x];
                        runX = 1;
                    }
                }
                result += FinderPenaltyTerminateAndCount(runColor, runX, runHistory) * PenaltyN3;
            }
            for (int x = 0; x < size; x++)
            {
                bool runColor = false;
                int runY = 0;
                int[] runHistory = new int[7];
                for (int y = 0; y < size; y++)
                {
                    if (modules[y][x] == runColor)
                    {
                        runY++;
                        if (runY == 5)
                            result += PenaltyN1;
                        else if (runY > 5)
                            result++;
                    }
                    else
                    {
                        FinderPenaltyAddHistory(runY, runHistory);
                        if (!runColor)
                            result += FinderPenaltyCountPatterns(runHistory) * PenaltyN3;
                        runColor = modules[y][x];
                        runY = 1;
                    }
                }
                result += FinderPenaltyTerminateAndCount(runColor, runY, runHistory) * PenaltyN3;
            }

            for (int y = 0; y < size - 1; y++)
            {
                for (int x = 0; x < size - 1; x++)
                {
                    bool color = modules[y][x];
                    if (color == modules[y][x + 1] &&
                        color == modules[y + 1][x] &&
                        color == modules[y + 1][x + 1])
                        result += PenaltyN2;
                }
            }

            int dark = 0;
            foreach (var row in modules)
            {
                foreach (var color in row)
                {
                    if (color)
                        dark++;
                }
            }
            int total = size * size;
            int k = (Math.Abs(dark * 20 - total * 10) + total - 1) / total - 1;
            result += k * PenaltyN4;
            return result;
        }

        /*---- Private helper functions ----*/

        private int[] GetAlignmentPatternPositions()
        {
            if (version == 1)
                return Array.Empty<int>();
            else
            {
                int numAlign = version / 7 + 2;
                int step = (version * 8 + numAlign * 3 + 5) / (numAlign * 4 - 4) * 2;
                int[] result = new int[numAlign];
                result[0] = 6;
                for (int i = result.Length - 1, pos = size - 7; i >= 1; i--, pos -= step)
                    result[i] = pos;
                return result;
            }
        }

        private static int GetNumRawDataModules(int ver)
        {
            if (ver < MinVersion || ver > MaxVersion)
                throw new ArgumentException("Version number out of range");

            int size = ver * 4 + 17;
            int result = size * size;
            result -= 8 * 8 * 3;
            result -= 15 * 2 + 1;
            result -= (size - 16) * 2;
            if (ver >= 2)
            {
                int numAlign = ver / 7 + 2;
                result -= (numAlign - 1) * (numAlign - 1) * 25;
                result -= (numAlign - 2) * 2 * 20;
                if (ver >= 7)
                    result -= 6 * 3 * 2;
            }
            return result;
        }

        private static byte[] ReedSolomonComputeDivisor(int degree)
        {
            if (degree < 1 || degree > 255)
                throw new ArgumentException("Degree out of range");
            byte[] result = new byte[degree];
            result[degree - 1] = 1;

            int root = 1;
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    result[j] = (byte)ReedSolomonMultiply(result[j], root);
                    if (j + 1 < result.Length)
                        result[j] ^= result[j + 1];
                }
                root = ReedSolomonMultiply(root, 0x02);
            }
            return result;
        }

        private static byte[] ReedSolomonComputeRemainder(byte[] data, byte[] divisor)
        {
            byte[] result = new byte[divisor.Length];
            foreach (byte b in data)
            {
                int factor = b ^ result[0];
                Array.Copy(result, 1, result, 0, result.Length - 1);
                result[result.Length - 1] = 0;
                for (int i = 0; i < result.Length; i++)
                    result[i] ^= (byte)ReedSolomonMultiply(divisor[i], factor);
            }
            return result;
        }

        private static int ReedSolomonMultiply(int x, int y)
        {
            int z = 0;
            for (int i = 7; i >= 0; i--)
            {
                z = (z << 1) ^ ((z >>> 7) * 0x11D);
                z ^= ((y >>> i) & 1) * x;
            }
            return z;
        }

        internal static int GetNumDataCodewords(int ver, Ecc ecl)
        {
            return GetNumRawDataModules(ver) / 8
                - EccCodewordsPerBlock[(int)ecl][ver]
                * NumErrorCorrectionBlocks[(int)ecl][ver];
        }

        private int FinderPenaltyCountPatterns(int[] runHistory)
        {
            int n = runHistory[1];
            bool core = n > 0 && runHistory[2] == n && runHistory[3] == n * 3 && runHistory[4] == n && runHistory[5] == n;
            return (core && runHistory[0] >= n * 4 && runHistory[6] >= n ? 1 : 0)
                 + (core && runHistory[6] >= n * 4 && runHistory[0] >= n ? 1 : 0);
        }

        private int FinderPenaltyTerminateAndCount(bool currentRunColor, int currentRunLength, int[] runHistory)
        {
            if (currentRunColor)
            {
                FinderPenaltyAddHistory(currentRunLength, runHistory);
                currentRunLength = 0;
            }
            currentRunLength += size;
            FinderPenaltyAddHistory(currentRunLength, runHistory);
            return FinderPenaltyCountPatterns(runHistory);
        }

        private void FinderPenaltyAddHistory(int currentRunLength, int[] runHistory)
        {
            if (runHistory[0] == 0)
                currentRunLength += size;
            Array.Copy(runHistory, 0, runHistory, 1, runHistory.Length - 1);
            runHistory[0] = currentRunLength;
        }

        internal static bool GetBit(int x, int i)
        {
            return ((x >>> i) & 1) != 0;
        }

        private static byte[] CopyOfRange(byte[] src, int from, int to)
        {
            byte[] result = new byte[to - from];
            Array.Copy(src, from, result, 0, to - from);
            return result;
        }

        private static byte[] CopyOf(byte[] src, int newLength)
        {
            byte[] result = new byte[newLength];
            Array.Copy(src, result, Math.Min(src.Length, newLength));
            return result;
        }

        /*---- Constants and tables ----*/

        public const int MinVersion = 1;
        public const int MaxVersion = 40;

        private const int PenaltyN1 = 3;
        private const int PenaltyN2 = 3;
        private const int PenaltyN3 = 40;
        private const int PenaltyN4 = 10;

        // Indexed by [ecl ordinal][version]. Index 0 (version) is unused padding.
        private static readonly int[][] EccCodewordsPerBlock =
        {
            new[] {-1,  7, 10, 15, 20, 26, 18, 20, 24, 30, 18, 20, 24, 26, 30, 22, 24, 28, 30, 28, 28, 28, 28, 30, 30, 26, 28, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30}, // Low
            new[] {-1, 10, 16, 26, 18, 24, 16, 18, 22, 22, 26, 30, 22, 22, 24, 24, 28, 28, 26, 26, 26, 26, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28}, // Medium
            new[] {-1, 13, 22, 18, 26, 18, 24, 18, 22, 20, 24, 28, 26, 24, 20, 30, 24, 28, 28, 26, 30, 28, 30, 30, 30, 30, 28, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30}, // Quartile
            new[] {-1, 17, 28, 22, 16, 22, 28, 26, 26, 24, 28, 24, 28, 22, 24, 24, 30, 28, 28, 26, 28, 30, 24, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30}, // High
        };

        private static readonly int[][] NumErrorCorrectionBlocks =
        {
            new[] {-1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 4,  4,  4,  4,  4,  6,  6,  6,  6,  7,  8,  8,  9,  9, 10, 12, 12, 12, 13, 14, 15, 16, 17, 18, 19, 19, 20, 21, 22, 24, 25}, // Low
            new[] {-1, 1, 1, 1, 2, 2, 4, 4, 4, 5, 5,  5,  8,  9,  9, 10, 10, 11, 13, 14, 16, 17, 17, 18, 20, 21, 23, 25, 26, 28, 29, 31, 33, 35, 37, 38, 40, 43, 45, 47, 49}, // Medium
            new[] {-1, 1, 1, 2, 2, 4, 4, 6, 6, 8, 8,  8, 10, 12, 16, 12, 17, 16, 18, 21, 20, 23, 23, 25, 27, 29, 34, 34, 35, 38, 40, 43, 45, 48, 51, 53, 56, 59, 62, 65, 68}, // Quartile
            new[] {-1, 1, 1, 2, 4, 4, 4, 5, 6, 8, 8, 11, 11, 16, 16, 18, 16, 19, 21, 25, 25, 25, 34, 30, 32, 35, 37, 40, 42, 45, 48, 51, 54, 57, 60, 63, 66, 70, 74, 77, 81}, // High
        };

        // Maps an Ecc value to its uint2 format-bits value (NOT the same as declaration/ordinal
        // order, which is what indexes the tables above) -- mirrors the Java enum's separate
        // ordinal() vs formatBits field.
        private static int FormatBitsFor(Ecc ecl)
        {
            switch (ecl)
            {
                case Ecc.Low: return 1;
                case Ecc.Medium: return 0;
                case Ecc.Quartile: return 3;
                case Ecc.High: return 2;
                default: throw new ArgumentOutOfRangeException(nameof(ecl));
            }
        }

        /*---- Public helper enumeration ----*/

        /// <summary>
        /// The error correction level in a QR Code symbol. Declared in ascending order of error
        /// protection (Low tolerates ~7% erroneous codewords, High ~30%) so that the numeric enum
        /// value can be used directly to index the per-version lookup tables above.
        /// </summary>
        public enum Ecc
        {
            Low,
            Medium,
            Quartile,
            High,
        }
    }
}
