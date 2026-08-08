using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeStandardTests.Symbologies
{
    /// <summary>
    /// Codabar is a pure table lookup (16 characters -> fixed 9/10/12-bar patterns, start/stop
    /// characters A-D) with no checksum, so its expected output can be assembled directly and
    /// exactly from the class's own symbol table plus the documented "0" inter-character space and
    /// start/stop-char-stripping-from-label behavior.
    /// </summary>
    [TestClass]
    public class CodabarTests
    {
        // Codabar symbol table, copied from BarcodeStandard/Symbologies/Codabar.cs for independent
        // assembly of expected output.
        private static readonly System.Collections.Generic.Dictionary<char, string> CodabarCode = new()
        {
            ['0'] = "101010011",
            ['1'] = "101011001",
            ['2'] = "101001011",
            ['3'] = "110010101",
            ['4'] = "101101001",
            ['5'] = "110101001",
            ['6'] = "100101011",
            ['7'] = "100101101",
            ['8'] = "100110101",
            ['9'] = "110100101",
            ['-'] = "101001101",
            ['$'] = "101100101",
            [':'] = "1101011011",
            ['/'] = "1101101011",
            ['.'] = "1101101101",
            ['+'] = "101100110011",
            ['A'] = "1011001001",
            ['B'] = "1010010011",
            ['C'] = "1001001011",
            ['D'] = "1010011001",
        };

        private static string Expected(string rawData)
        {
            var result = string.Empty;
            foreach (var c in rawData)
                result += CodabarCode[c] + "0";
            return result.Remove(result.Length - 1);
        }

        [TestMethod]
        public void Codabar_EncodesEachCharacter_WithInterCharacterSpace()
        {
            var barcode = new Barcode { EncodedType = TYPE.Codabar };
            var actual = barcode.GenerateBarcode("A123D");

            Assert.AreEqual(Expected("A123D"), actual);
        }

        [TestMethod]
        public void Codabar_StripsStartStopCharacters_FromRawDataAfterEncoding()
        {
            // The symbology-level encoder mutates its own RawData to drop the leading/trailing
            // start/stop characters (for downstream label rendering), but that only propagates
            // back to Barcode.RawData via the full Encode() pipeline -- GenerateBarcode() alone
            // does not copy it back, since Barcode.RawData is a separate field set directly from
            // the caller-supplied string. Use the public Encode(TYPE, string) overload to observe
            // the propagated, stripped value.
            var barcode = new Barcode();
            barcode.Encode(TYPE.Codabar, "A123D");

            Assert.AreEqual("123", barcode.RawData);
        }

        [TestMethod]
        public void Codabar_LowercaseStartStopCharacters_AreAccepted()
        {
            var upper = new Barcode { EncodedType = TYPE.Codabar };
            var lower = new Barcode { EncodedType = TYPE.Codabar };

            Assert.AreEqual(upper.GenerateBarcode("A123D"), lower.GenerateBarcode("a123d"));
        }

        [TestMethod]
        public void Codabar_InvalidStartCharacter_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.Codabar };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("1231D");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "ECODABAR-2");
        }

        [TestMethod]
        public void Codabar_InvalidStopCharacter_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.Codabar };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("A1231");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "ECODABAR-3");
        }

        [TestMethod]
        public void Codabar_TooShort_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.Codabar };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("A");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "ECODABAR-1");
        }
    }
}
