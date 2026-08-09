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
    public IActionResult Samples()
    {
        const string targetUrl = Barcode.Web.SiteBranding.SiteUrl;
        var model = new BarcodeSampleViewModel { TargetUrl = targetUrl };

        foreach (var sample in BuildSampleCandidates(targetUrl))
        {
            var validationError = ValidateSample(sample);
            if (validationError is null)
            {
                model.Samples.Add(sample);
                continue;
            }

            sample.ErrorMessage = validationError;
            model.FailedSamples.Add(sample);
        }

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

    private static BarcodeViewModel ToBarcodeViewModel(BarcodeSampleItem sample)
    {
        return new BarcodeViewModel
        {
            BarValue = sample.Payload,
            AlternateLabel = "Make Bold Solutions",
            EncodedType = sample.EncodedType,
            Alignment = sample.Alignment,
            LabelPosition = sample.LabelPosition,
            RotateFlip = sample.RotateFlip,
            Width = sample.Width,
            Height = sample.Height,
            BarWidth = sample.BarWidth,
            AspectRatio = sample.AspectRatio,
            IncludeLabel = sample.IncludeLabel,
            EnforceGS1QuietZone = sample.EnforceGS1QuietZone,
            ForeColor = ColorTranslator.FromHtml(sample.ForeColor),
            BackColor = ColorTranslator.FromHtml(sample.BackColor),
        };
    }

    private static string? ValidateSample(BarcodeSampleItem sample)
    {
        try
        {
            using var barcode = CreateBarcode(ToBarcodeViewModel(sample));
            var svg = barcode.GetSvg();
            return string.IsNullOrWhiteSpace(svg) ? "SVG renderer returned an empty document." : null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static IEnumerable<BarcodeSampleItem> BuildSampleCandidates(string targetUrl)
    {
        yield return Qr("QR Standard", "Clean black modules on white with a generous square canvas.", targetUrl, 220, 220, "#1E1E1E", "#FFFFFF");
        yield return Qr("QR Brand Rust", "Make Bold rust modules on a warm light surface.", targetUrl, 240, 240, "#982407", "#F8F6F2");
        yield return Qr("QR Ember", "Accent-color QR for print tests where contrast remains high.", targetUrl, 240, 240, "#653106", "#FDF1E6");
        yield return Qr("QR Ink On Cream", "Soft page-toned background while keeping dark module contrast.", targetUrl, 260, 260, "#1E1E1E", "#F8F6F2");
        yield return Qr("QR Compact", "Smaller footprint for badges, signs, and compact collateral.", targetUrl, 180, 180, "#1E1E1E", "#FFFFFF");
        yield return Qr("QR Large Display", "Large-format symbol intended for distance scanning.", targetUrl, 340, 340, "#1E1E1E", "#FFFFFF");
        yield return Qr("QR Blue Utility", "Cool utility palette for operational layouts.", targetUrl, 260, 260, "#2F5A8F", "#E2E9F2");
        yield return Qr("QR Green Confirmation", "Positive-state colorway with dark green modules.", targetUrl, 260, 260, "#2F6F4C", "#E4EFE7");
        yield return Qr("QR Tall Poster", "Extra canvas height for layouts that reserve room around the code.", targetUrl, 260, 320, "#1E1E1E", "#FFFFFF");
        yield return Qr("QR Wide Label", "Wider canvas for shelf tags or horizontal labels.", targetUrl, 340, 240, "#1E1E1E", "#FFFFFF");

        yield return Linear("Code 128 SVG", "General-purpose 1D barcode that can encode the full URL.", targetUrl, TYPE.CODE128, 520, 150, "#1E1E1E", "#FFFFFF", true);
        yield return Linear("Code 128 Brand", "Code 128 with Make Bold brand colors and bottom label.", targetUrl, TYPE.CODE128, 560, 150, "#982407", "#F8F6F2", true);
        yield return Linear("Code 128 No Label", "Machine-focused Code 128 without human-readable label text.", targetUrl, TYPE.CODE128, 500, 110, "#1E1E1E", "#FFFFFF", false);
        yield return Linear("Code 128 Top Label", "Code 128 with the label placed above the bars.", targetUrl, TYPE.CODE128B, 560, 160, "#1E1E1E", "#FFFFFF", true, LabelPositions.TOPCENTER);
        yield return Linear("Code 128 Left Aligned", "Left-aligned bars inside a wider label area.", targetUrl, TYPE.CODE128, 640, 150, "#1E1E1E", "#FFFFFF", true, LabelPositions.BOTTOMLEFT, AlignmentPositions.LEFT);
        yield return Linear("Code 128 Right Aligned", "Right-aligned bars inside a wider label area.", targetUrl, TYPE.CODE128, 640, 150, "#1E1E1E", "#FFFFFF", true, LabelPositions.BOTTOMRIGHT, AlignmentPositions.RIGHT);
        yield return Linear("Code 128 Rotated", "Rotated Code 128 for vertical layouts and edge labels.", targetUrl, TYPE.CODE128, 620, 180, "#1E1E1E", "#FFFFFF", true, LabelPositions.BOTTOMCENTER, AlignmentPositions.CENTER, RotateFlipType.Rotate90FlipNone);
        yield return Linear("Code 128 Tall Label", "Taller Code 128 treatment for labels that need more vertical whitespace.", targetUrl, TYPE.CODE128B, 640, 220, "#2F5A8F", "#E2E9F2", true);
        yield return Linear("Code 128 Rust Wide", "Wider Code 128 treatment for packaging mockups and signs.", targetUrl, TYPE.CODE128B, 760, 170, "#6C1804", "#F8F6F2", true);
        yield return Linear("Telepen", "Telepen candidate for ASCII payload comparison.", targetUrl, TYPE.TELEPEN, 620, 150, "#1E1E1E", "#FFFFFF", true);
    }

    private static BarcodeSampleItem Qr(string name, string description, string payload, int width, int height, string foreColor, string backColor)
    {
        return new BarcodeSampleItem
        {
            Name = name,
            Description = description,
            EncodedType = TYPE.QRCODE,
            Payload = payload,
            RenderAction = "Svg",
            Width = width,
            Height = height,
            ForeColor = foreColor,
            BackColor = backColor,
            IncludeLabel = false,
        };
    }

    private static BarcodeSampleItem Linear(
        string name,
        string description,
        string payload,
        TYPE type,
        int width,
        int height,
        string foreColor,
        string backColor,
        bool includeLabel,
        LabelPositions labelPosition = LabelPositions.BOTTOMCENTER,
        AlignmentPositions alignment = AlignmentPositions.CENTER,
        RotateFlipType rotateFlip = RotateFlipType.RotateNoneFlipNone)
    {
        return new BarcodeSampleItem
        {
            Name = name,
            Description = description,
            EncodedType = type,
            Payload = payload,
            RenderAction = "Svg",
            Width = width,
            Height = height,
            ForeColor = foreColor,
            BackColor = backColor,
            IncludeLabel = includeLabel,
            LabelPosition = labelPosition,
            Alignment = alignment,
            RotateFlip = rotateFlip,
        };
    }
}
