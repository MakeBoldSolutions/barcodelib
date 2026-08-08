using Barcode.Web.Extensions;
using Barcode.Web.Models;
using BarcodeLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Barcode.Web.Controllers;

/// <summary>
/// Interactive barcode playground: pick a symbology and options, preview the result, and
/// download it as an image or JSON -- the web equivalent of the old BarcodeStandardExample
/// WinForms demo, built directly on BarcodeStandard (no intermediate wrapper library).
/// </summary>
public class BarCodeController : Controller
{
    [HttpGet]
    public IActionResult Index(BarcodeViewModel? model)
    {
        model ??= new BarcodeViewModel();
        TryEncode(model);
        return View(model);
    }

    [HttpGet]
    public IActionResult Image(BarcodeViewModel model)
    {
        try
        {
            var b = CreateBarcode(model);
            return new ImageResult { Image = b.EncodedImage, ImageFormat = ImageFormat.Png };
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    public IActionResult Svg(BarcodeViewModel model)
    {
        try
        {
            var b = CreateBarcode(model);
            return Content(b.GetSvg(), "image/svg+xml");
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    public IActionResult Download(BarcodeViewModel model, SaveTypes format = SaveTypes.PNG)
    {
        try
        {
            var b = CreateBarcode(model);
            var bytes = b.GetImageData(format);
            var (contentType, ext) = format switch
            {
                SaveTypes.BMP => ("image/bmp", "bmp"),
                SaveTypes.GIF => ("image/gif", "gif"),
                SaveTypes.JPG => ("image/jpeg", "jpg"),
                SaveTypes.TIFF => ("image/tiff", "tiff"),
                _ => ("image/png", "png"),
            };
            return File(bytes, contentType, $"barcode.{ext}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult ExportJson(BarcodeViewModel model)
    {
        try
        {
            var b = CreateBarcode(model);
            var bytes = Encoding.UTF8.GetBytes(b.ToJSON());
            return File(bytes, "application/json", "barcode.json");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult ImportJson(IFormFile? jsonFile)
    {
        var model = new BarcodeViewModel();

        if (jsonFile is { Length: > 0 })
        {
            try
            {
                using var stream = jsonFile.OpenReadStream();
                var saveData = BarcodeLib.Barcode.FromJSON(stream);

                model.BarValue = saveData.RawData;
                model.EncodedType = Enum.TryParse<TYPE>(saveData.Type, out var type) ? type : TYPE.CODE93;
                model.IncludeLabel = saveData.IncludeLabel;
                model.ForeColor = ColorTranslator.FromHtml(saveData.Forecolor);
                model.BackColor = ColorTranslator.FromHtml(saveData.Backcolor);
                model.Width = saveData.ImageWidth;
                model.Height = saveData.ImageHeight;
                model.RotateFlip = saveData.RotateFlipType;
                model.LabelPosition = (LabelPositions)saveData.LabelPosition;
                model.Alignment = (AlignmentPositions)saveData.Alignment;
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Could not import JSON: " + ex.Message;
            }
        }

        TryEncode(model);
        return View("Index", model);
    }

    private static BarcodeLib.Barcode CreateBarcode(BarcodeViewModel model)
    {
        var b = new BarcodeLib.Barcode
        {
            Alignment = model.Alignment,
            BarWidth = model.BarWidth,
            AspectRatio = model.AspectRatio,
            IncludeLabel = model.IncludeLabel,
            RotateFlipType = model.RotateFlip,
            AlternateLabel = model.AlternateLabel,
            LabelPosition = model.LabelPosition,
            EnforceGS1QuietZone = model.EnforceGS1QuietZone,
        };
        b.Encode(model.EncodedType, (model.BarValue ?? string.Empty).Trim(), model.ForeColor, model.BackColor, model.Width, model.Height);
        return b;
    }

    private static void TryEncode(BarcodeViewModel model)
    {
        model.ErrorMessage = null;
        model.EncodedValueText = null;

        try
        {
            var b = CreateBarcode(model);
            model.EncodingTimeMs = b.EncodingTime;
            if (!model.IsMatrix)
            {
                model.EncodedValueText = b.EncodedValue;
            }
        }
        catch (Exception ex)
        {
            model.ErrorMessage = ex.Message;
        }
    }
}
