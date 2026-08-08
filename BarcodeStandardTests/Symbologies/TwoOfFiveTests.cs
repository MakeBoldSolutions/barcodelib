using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeStandardTests.Symbologies
{
    /// <summary>
    /// Covers the Standard2of5 (Standard2of5, Standard2of5_Mod10, Industrial2of5,
    /// Industrial2of5_Mod10 -- all four TYPE values route to the same Standard2of5 class) and
    /// Interleaved2of5 (Interleaved2of5, Interleaved2of5_Mod10) symbologies, neither of which had
    /// any dedicated/assertive test coverage before this file.
    /// </summary>
    [TestClass]
    public class TwoOfFiveTests
    {
        // Standard 2-of-5 symbol table (digit 0-9), copied from
        // BarcodeStandard/Symbologies/Standard2of5.cs for independent assembly of expected output.
        private static readonly string[] S25Code =
        {
            "10101110111010", "11101010101110", "10111010101110", "11101110101010", "10101110101110",
            "11101011101010", "10111011101010", "10101011101110", "11101010111010", "10111010111010"
        };

        [TestMethod]
        public void Standard2of5_NoCheckDigit_EncodesDigitsBetweenStartAndStopBars()
        {
            var barcode = new Barcode { EncodedType = TYPE.Standard2of5 };
            var actual = barcode.GenerateBarcode("07");
            var expected = "11011010" + S25Code[0] + S25Code[7] + "1101011";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Standard2of5Mod10_AppendsHandVerifiedCheckDigit()
        {
            // "07": weights from the right are 3,1 -> sum = 7*3 + 0*1 = 21, checksum = (10 - 21%10) % 10 = 9
            var barcode = new Barcode { EncodedType = TYPE.Standard2of5_Mod10 };
            var actual = barcode.GenerateBarcode("07");
            var expected = "11011010" + S25Code[0] + S25Code[7] + S25Code[9] + "1101011";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Industrial2of5_UsesSameTableAsStandard2of5()
        {
            // Industrial2of5 shares Standard2of5's implementation class and symbol table exactly.
            var industrial = new Barcode { EncodedType = TYPE.Industrial2of5 };
            var standard = new Barcode { EncodedType = TYPE.Standard2of5 };

            Assert.AreEqual(standard.GenerateBarcode("07"), industrial.GenerateBarcode("07"));
        }

        [TestMethod]
        public void Industrial2of5Mod10_DoesNotActuallyAppendACheckDigit()
        {
            // Standard2of5's checksum branch only fires when EncodedType == TYPE.Standard2of5_Mod10
            // specifically ("_encodedType == TYPE.Standard2of5_Mod10 ? ... : string.Empty"), so
            // TYPE.Industrial2of5_Mod10 -- despite its name -- never gets a check digit appended.
            // This test locks in that surprising current behavior rather than assuming the name
            // implies checksum support.
            var industrialMod10 = new Barcode { EncodedType = TYPE.Industrial2of5_Mod10 };
            var plainStandard = new Barcode { EncodedType = TYPE.Standard2of5 };

            Assert.AreEqual(plainStandard.GenerateBarcode("07"), industrialMod10.GenerateBarcode("07"));
        }

        [TestMethod]
        public void Standard2of5_NonNumericData_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.Standard2of5 };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("1A");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "ES25-1");
        }

        [TestMethod]
        public void Interleaved2of5_EvenLengthInput_EncodesWithoutError()
        {
            var barcode = new Barcode { EncodedType = TYPE.Interleaved2of5 };
            var actual = barcode.GenerateBarcode("1234");

            // Start "1010" + one interleaved bar/space block per digit pair + end "1101".
            Assert.IsTrue(actual.StartsWith("1010"));
            Assert.IsTrue(actual.EndsWith("1101"));
            Assert.IsTrue(actual.Length > "1010".Length + "1101".Length);
        }

        [TestMethod]
        public void Interleaved2of5_OddLengthInput_Throws()
        {
            // Interleaved 2 of 5 requires digit pairs; an odd-length input with no check digit
            // requested cannot be paired up.
            var barcode = new Barcode { EncodedType = TYPE.Interleaved2of5 };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("123");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EI25-1");
        }

        [TestMethod]
        public void Interleaved2of5Mod10_RequiresOddLengthInput_SinceCheckDigitMakesItEven()
        {
            // Interleaved2of5_Mod10 expects an odd-length input (the appended check digit brings
            // the total to an even, pairable length).
            var barcode = new Barcode { EncodedType = TYPE.Interleaved2of5_Mod10 };

            // "1234" (even) + check digit -> odd total length -> should throw per the length guard.
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("1234");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EI25-1");

            // "123" (odd) is the length the guard expects for a Mod10 variant.
            var actual = barcode.GenerateBarcode("123");
            Assert.IsTrue(actual.StartsWith("1010"));
            Assert.IsTrue(actual.EndsWith("1101"));
        }
    }
}
