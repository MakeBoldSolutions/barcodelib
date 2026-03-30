using BarcodeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BarcodeStandard;
using System.Drawing;

namespace BarcodeStandardTests.Symbologies
{
    [TestClass]
    public class SymbologySmokeTests
    {
        private static Exception CaptureException(Action action)
        {
            try
            {
                action();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        [TestMethod]
        public void GenerateBarcode_BlankData_Throws()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.CODE128,
            };

            var ex = CaptureException(() => barcode.GenerateBarcode("   "));
            Assert.IsNotNull(ex);
            StringAssert.Contains(ex.Message, "EENCODE-1");
        }

        [TestMethod]
        public void GenerateBarcode_UnspecifiedType_Throws()
        {
            var barcode = new Barcode
            {
                EncodedType = TYPE.UNSPECIFIED,
            };

            var ex = CaptureException(() => barcode.GenerateBarcode("12345"));
            Assert.IsNotNull(ex);
            StringAssert.Contains(ex.Message, "EENCODE-2");
        }

        [TestMethod]
        public void GenerateBarcode_ManySymbologies_HasAtLeastOneValidSample()
        {
            var samplesByType = new Dictionary<TYPE, string[]>
            {
                [TYPE.UPCA] = new[] { "03600029145", "12345678901" },
                [TYPE.UCC12] = new[] { "03600029145", "12345678901" },
                [TYPE.EAN13] = new[] { "590123412345", "490123456789" },
                [TYPE.UCC13] = new[] { "590123412345", "490123456789" },
                [TYPE.Interleaved2of5] = new[] { "1234", "001122" },
                [TYPE.Interleaved2of5_Mod10] = new[] { "12345", "00123" },
                [TYPE.Standard2of5] = new[] { "12345", "987654" },
                [TYPE.Standard2of5_Mod10] = new[] { "12345", "987654" },
                [TYPE.Industrial2of5] = new[] { "12345", "987654" },
                [TYPE.Industrial2of5_Mod10] = new[] { "12345", "987654" },
                [TYPE.CODE39] = new[] { "ABC123", "HELLO-1" },
                [TYPE.CODE39Extended] = new[] { "abc123", "hello!" },
                [TYPE.CODE39_Mod43] = new[] { "ABC123", "TEST43" },
                [TYPE.LOGMARS] = new[] { "LOGMARS", "ABC123" },
                [TYPE.Codabar] = new[] { "A12345A", "B1234B" },
                [TYPE.PostNet] = new[] { "12345", "90210" },
                [TYPE.BOOKLAND] = new[] { "978030640615", "978123456789" },
                [TYPE.ISBN] = new[] { "978030640615", "978123456789" },
                [TYPE.JAN13] = new[] { "490123456789", "491234567890" },
                [TYPE.UPC_SUPPLEMENTAL_2DIGIT] = new[] { "12", "05" },
                [TYPE.MSI_Mod10] = new[] { "12345", "98765" },
                [TYPE.MSI_2Mod10] = new[] { "12345", "98765" },
                [TYPE.MSI_Mod11] = new[] { "12345", "98765" },
                [TYPE.MSI_Mod11_Mod10] = new[] { "12345", "98765" },
                [TYPE.Modified_Plessey] = new[] { "12345", "98765" },
                [TYPE.UPC_SUPPLEMENTAL_5DIGIT] = new[] { "51234", "12345" },
                [TYPE.UPCE] = new[] { "04210005", "01234565", "123456" },
                [TYPE.EAN8] = new[] { "5512345", "1234567" },
                [TYPE.CODE11] = new[] { "123-45", "12345" },
                [TYPE.USD8] = new[] { "123-45", "12345" },
                [TYPE.CODE128] = new[] { "hello", "123456" },
                [TYPE.CODE128A] = new[] { "ABC123", "HELLO" },
                [TYPE.CODE128B] = new[] { "hello", "AbC123" },
                [TYPE.CODE128C] = new[] { "123456", "001122" },
                [TYPE.ITF14] = new[] { "1234567890123", "0001234567890" },
                [TYPE.CODE93] = new[] { "CODE93", "ABC123" },
                [TYPE.TELEPEN] = new[] { "TELEPEN", "12345" },
                [TYPE.FIM] = new[] { "A", "B" },
                [TYPE.PHARMACODE] = new[] { "12345", "123" },
            };

            var barcode = new Barcode();

            foreach (var kvp in samplesByType)
            {
                barcode.EncodedType = kvp.Key;

                var success = false;
                foreach (var candidate in kvp.Value)
                {
                    try
                    {
                        var encoded = barcode.GenerateBarcode(candidate);
                        if (!string.IsNullOrWhiteSpace(encoded))
                        {
                            success = true;
                            break;
                        }
                    }
                    catch
                    {
                        // Try next candidate for this symbology.
                    }
                }

                Assert.IsTrue(success, $"No valid sample found for {kvp.Key}.");
            }
        }

        [TestMethod]
        public void SaveData_CanRoundTripProperties_AndDispose()
        {
            var model = new SaveData
            {
                Type = "CODE128",
                RawData = "abc123",
                EncodedValue = "101010",
                EncodingTime = 1.23,
                IncludeLabel = true,
                Forecolor = "Black",
                Backcolor = "White",
                CountryAssigningManufacturingCode = "N/A",
                ImageWidth = 320,
                ImageHeight = 120,
                Image = "base64",
                RotateFlipType = RotateFlipType.Rotate90FlipNone,
                LabelPosition = 1,
                Alignment = 2,
                LabelFont = "Arial,10",
                ImageFormat = "Png"
            };

            Assert.AreEqual("CODE128", model.Type);
            Assert.AreEqual("abc123", model.RawData);
            Assert.AreEqual("101010", model.EncodedValue);
            Assert.AreEqual(1.23, model.EncodingTime);
            Assert.IsTrue(model.IncludeLabel);
            Assert.AreEqual("Black", model.Forecolor);
            Assert.AreEqual("White", model.Backcolor);
            Assert.AreEqual("N/A", model.CountryAssigningManufacturingCode);
            Assert.AreEqual(320, model.ImageWidth);
            Assert.AreEqual(120, model.ImageHeight);
            Assert.AreEqual("base64", model.Image);
            Assert.AreEqual(RotateFlipType.Rotate90FlipNone, model.RotateFlipType);
            Assert.AreEqual(1, model.LabelPosition);
            Assert.AreEqual(2, model.Alignment);
            Assert.AreEqual("Arial,10", model.LabelFont);
            Assert.AreEqual("Png", model.ImageFormat);

            model.Dispose();
        }
    }
}
