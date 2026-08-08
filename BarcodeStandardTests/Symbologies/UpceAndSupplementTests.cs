using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeStandardTests.Symbologies
{
    /// <summary>
    /// Covers UPC-E (zero-suppressed UPC-A) and the UPC 2-digit/5-digit supplemental add-on
    /// symbologies, none of which had dedicated/assertive test coverage before this file. Expected
    /// values are assembled from the same EAN L-code/G-code tables verified earlier for UPC-A/EAN-13
    /// (BarcodeStandard/Symbologies/UPCA.cs), which UPC-E and the supplements also use.
    /// </summary>
    [TestClass]
    public class UpceAndSupplementTests
    {
        // Standard EAN "L-code" (odd parity) and "G-code" (even parity) tables for digits 0-9.
        private static readonly string[] EanCodeA =
        {
            "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011"
        };
        private static readonly string[] EanCodeB =
        {
            "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111"
        };

        // UPC-E parity-selection table for number-system 0, indexed by check digit.
        private static readonly string[] UpcECode0 =
        {
            "bbbaaa", "bbabaa", "bbaaba", "bbaaab", "babbaa", "baabba", "baaabb", "bababa", "babaab", "baabab"
        };

        [TestMethod]
        public void Upce_EightDigitInput_MatchesHandAssembledPattern()
        {
            // numberSystem=0 (Raw_Data[0]), checkDigit=Raw_Data[7]='5' -> pattern UpcECode0[5]="baabba".
            // The encoder reads its six body digits from Raw_Data[0..5] (i.e. INCLUDING the leading
            // number-system digit), not from Raw_Data[1..6] -- Raw_Data[6] is never consulted. This
            // test locks in that current (if surprising) indexing behavior.
            var barcode = new Barcode { EncodedType = TYPE.UPCE };
            var actual = barcode.GenerateBarcode("01234565");

            var expected = "101"
                + EanCodeB[0] // b, Raw_Data[0] = '0'
                + EanCodeA[1] // a, Raw_Data[1] = '1'
                + EanCodeA[2] // a, Raw_Data[2] = '2'
                + EanCodeB[3] // b, Raw_Data[3] = '3'
                + EanCodeB[4] // b, Raw_Data[4] = '4'
                + EanCodeA[5] // a, Raw_Data[5] = '5'
                + "01010"
                + "1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Upce_InvalidLength_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.UPCE };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("1234567"); // 7 digits: not 6, 8, or 12
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EUPCE-1");
        }

        [TestMethod]
        public void Upce_InvalidNumberSystem_Throws()
        {
            // Only number system 0 or 1 is valid (first digit).
            var barcode = new Barcode { EncodedType = TYPE.UPCE };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("21234565");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EUPCE-3");
        }

        [TestMethod]
        public void UpcSupplement2_UsesModuloFourParitySelection()
        {
            // Parity pattern index = int("21") % 4 = 21 % 4 = 1 -> UPC_SUPP_2[1] = "ab"
            var barcode = new Barcode { EncodedType = TYPE.UPC_SUPPLEMENTAL_2DIGIT };
            var actual = barcode.GenerateBarcode("21");

            var expected = "1011" + EanCodeA[2] + "01" + EanCodeB[1];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpcSupplement2_InvalidLength_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.UPC_SUPPLEMENTAL_2DIGIT };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("1");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EUPC-SUP2-1");
        }

        [TestMethod]
        public void UpcSupplement5_AppliesHandVerifiedChecksumParity()
        {
            // "12345": odd positions (0,2,4) = 1,3,5 * 3 = 27; even positions (1,3) = 2,4 * 9 = 54
            // total = 27 + 54 = 81, checksum = 81 % 10 = 1 -> UPC_SUPP_5[1] = "babaa"
            var barcode = new Barcode { EncodedType = TYPE.UPC_SUPPLEMENTAL_5DIGIT };
            var actual = barcode.GenerateBarcode("12345");

            var expected = "1011" + EanCodeB[1]
                + "01" + EanCodeA[2]
                + "01" + EanCodeB[3]
                + "01" + EanCodeA[4]
                + "01" + EanCodeA[5];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpcSupplement5_InvalidLength_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.UPC_SUPPLEMENTAL_5DIGIT };
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
            StringAssert.Contains(caught.Message, "EUPC-SUP5-1");
        }
    }
}
