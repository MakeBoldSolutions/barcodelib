using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeStandardTests.Symbologies
{
    [TestClass]
    public class FocusedSymbologyTests
    {
        [TestMethod]
        public void Upca_NormalizesCheckDigit_AndSetsCountryCode()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.UPCA,
            };

            var encodedFrom11Digits = barcode.GenerateBarcode("03600029145");
            var encodedFrom12Digits = barcode.GenerateBarcode("036000291452");

            Assert.AreEqual("036000291452", barcode.RawData);
            Assert.AreEqual(encodedFrom12Digits, encodedFrom11Digits);
        }

        [TestMethod]
        public void Ean13_NormalizesCheckDigit_AndSetsCountryCode()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.EAN13,
            };

            var encodedFrom12Digits = barcode.GenerateBarcode("590123412345");
            var encodedFrom13Digits = barcode.GenerateBarcode("5901234123457");

            Assert.AreEqual("5901234123457", barcode.RawData);
            Assert.AreEqual(encodedFrom13Digits, encodedFrom12Digits);
        }

        [TestMethod]
        [DataRow(TYPE.UCC12, TYPE.UPCA, "03600029145")]
        [DataRow(TYPE.UCC13, TYPE.EAN13, "590123412345")]
        [DataRow(TYPE.BOOKLAND, TYPE.ISBN, "978030640615")]
        [DataRow(TYPE.USD8, TYPE.CODE11, "123-45")]
        [DataRow(TYPE.LOGMARS, TYPE.CODE39, "ABC123")]
        public void AliasSymbologies_ProduceSameEncoding(TYPE leftType, TYPE rightType, string data)
        {
            var left = new Barcode { EncodedType = leftType };
            var right = new Barcode { EncodedType = rightType };

            var leftEncoded = left.GenerateBarcode(data);
            var rightEncoded = right.GenerateBarcode(data);

            Assert.AreEqual(rightEncoded, leftEncoded);
        }

        [TestMethod]
        [DataRow("A", "10100000100000101")]
        [DataRow("B", "10001010001010001")]
        [DataRow("C", "10100010001000101")]
        [DataRow("D", "10101000100010101")]
        public void Fim_UsesDocumentedEncodingPatterns(string input, string expected)
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.FIM,
            };

            var actual = barcode.GenerateBarcode(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Pharmacode_MinimumValue_HasExpectedEncoding()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.PHARMACODE,
            };

            var actual = barcode.GenerateBarcode("3");

            Assert.AreEqual("1001", actual);
        }

        [TestMethod]
        public void Code39Extended_SupportsCharacters_ThatRegularCode39Rejects()
        {
            var regular = new Barcode
            {
                EncodedType = TYPE.CODE39,
            };
            var extended = new Barcode
            {
                EncodedType = TYPE.CODE39Extended,
            };

            Exception regularFailure = null;
            try
            {
                regular.GenerateBarcode("hello!");
            }
            catch (Exception ex)
            {
                regularFailure = ex;
            }

            var extendedEncoding = extended.GenerateBarcode("hello!");

            Assert.IsNotNull(regularFailure);
            StringAssert.Contains(regularFailure.Message, "EC39-1");
            Assert.IsFalse(string.IsNullOrWhiteSpace(extendedEncoding));
        }
    }
}