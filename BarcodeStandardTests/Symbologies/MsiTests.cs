using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeStandardTests.Symbologies
{
    /// <summary>
    /// MSI has four distinct checksum algorithms (Mod10, 2Mod10, Mod11, Mod11+Mod10) behind a
    /// single symbology class. These tests hand-verify each checksum calculation against the
    /// published MSI/Plessey algorithm, then confirm the correct checksum digit(s) were encoded
    /// at the correct position in the bar pattern using MSI's own symbol table (12-bar-widths per
    /// digit, "110" start, "1001" stop).
    /// </summary>
    [TestClass]
    public class MsiTests
    {
        // MSI symbol table (digit 0-9 -> 12-char bar/space pattern), copied from
        // BarcodeStandard/Symbologies/MSI.cs for independent assembly of expected output.
        private static readonly string[] MsiCode =
        {
            "100100100100", "100100100110", "100100110100", "100100110110", "100110100100",
            "100110100110", "100110110100", "100110110110", "110100100100", "110100100110"
        };

        private const string Start = "110";
        private const string Stop = "1001";

        private static string Digits(string digits)
        {
            var result = string.Empty;
            foreach (var c in digits)
                result += MsiCode[c - '0'];
            return result;
        }

        [TestMethod]
        public void MsiMod10_AppendsHandVerifiedCheckDigit()
        {
            // "1234": odds="24"*2=48 (evensum=1+3=4), oddsum=4+8=12, mod=(12+4)%10=6, checksum=10-6=4
            var barcode = new Barcode { EncodedType = TYPE.MSI_Mod10 };
            var actual = barcode.GenerateBarcode("1234");
            var expected = Start + Digits("12344") + Stop;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MsiMod11_AppendsHandVerifiedCheckDigit()
        {
            // "1234": weights from the right are 2,3,4,5 -> sum = 4*2+3*3+2*4+1*5 = 8+9+8+5 = 30
            // mod = 30 % 11 = 8, checksum = 11 - 8 = 3
            var barcode = new Barcode { EncodedType = TYPE.MSI_Mod11 };
            var actual = barcode.GenerateBarcode("1234");
            var expected = Start + Digits("12343") + Stop;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Msi2Mod10_AppendsTwoHandVerifiedCheckDigits()
        {
            // First digit is the same Mod10 check digit as above: "12344".
            // Second digit re-runs the odds/evens algorithm over "12344":
            // odds="134"*2=268 (evensum=2+4=6), oddsum=2+6+8=16, sum=22, checksum=10-(22%10)=8
            var barcode = new Barcode { EncodedType = TYPE.MSI_2Mod10 };
            var actual = barcode.GenerateBarcode("1234");
            var expected = Start + Digits("123448") + Stop;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MsiMod11Mod10_ChecksumOfTenAppendsTwoDigits()
        {
            // First digit is the Mod11 check digit from above: "12343".
            // Second-stage checksum re-runs the odds/evens algorithm over "12343":
            // odds="133"*2=266 (evensum=2+4=6), oddsum=2+6+6=14, sum=20, checksum=10-(20%10)=10.
            // Note: the source computes "10 - (sum % 10)" WITHOUT a "mod==0 -> 0" guard here
            // (unlike the Mod10/Mod11 stages), so a sum that's an exact multiple of 10 produces a
            // literal checksum value of 10, whose ToString() is "10" -- TWO extra characters get
            // appended ('1' then '0'), not a single check digit. This test locks in that
            // surprising-but-current behavior rather than the "always one extra digit" assumption.
            var barcode = new Barcode { EncodedType = TYPE.MSI_Mod11_Mod10 };
            var actual = barcode.GenerateBarcode("1234");
            var expected = Start + Digits("1234310") + Stop;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Msi_NonNumericData_Throws()
        {
            var barcode = new Barcode { EncodedType = TYPE.MSI_Mod10 };
            Exception caught = null;
            try
            {
                barcode.GenerateBarcode("12A4");
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.IsNotNull(caught);
            StringAssert.Contains(caught.Message, "EMSI-1");
        }
    }
}
