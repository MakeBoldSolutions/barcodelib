using Barcode.Web.Extensions;
using Barcode.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Barcode.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var symbologies = Enum.GetNames(typeof(BarcodeLib.TYPE))
            .Where(name => name != nameof(BarcodeLib.TYPE.UNSPECIFIED))
            .OrderBy(name => name)
            .ToList();

        var model = new LibraryShowcaseVM
        {
            SymbologyCount = symbologies.Count,
            Symbologies = symbologies,
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public ActionResult GetFile()
    {
        // No need to dispose the stream, MVC does it for you
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "image.png");
        FileStream stream = new FileStream(path, FileMode.Open);
        FileStreamResult result = new FileStreamResult(stream, "image/png");
        result.FileDownloadName = "image.png";
        return result;
    }

    public ActionResult MyImage()
    {
        var barCode = new BarcodeLib.Barcode { EncodedType = BarcodeLib.TYPE.CODE93 };
        barCode.Encode(BarcodeLib.TYPE.CODE93, "controlorigins.com", 290, 120);
        return new ImageResult
        {
            Image = barCode.EncodedImage,
            ImageFormat = ImageFormat.Png
        };
    }

    public ContentResult MySvg()
    {
        var barCode = new BarcodeLib.Barcode { EncodedType = BarcodeLib.TYPE.CODE93 };
        barCode.Encode(BarcodeLib.TYPE.CODE93, "controlorigins.com", 290, 120);
        return Content(barCode.GetSvg(), "image/svg+xml");
    }

    public ActionResult MyQr()
    {
        var qr = new BarcodeLib.Barcode { EncodedType = BarcodeLib.TYPE.QRCODE };
        var image = qr.Encode(BarcodeLib.TYPE.QRCODE, "https://github.com/markhazleton/barcodelib", 240, 240);
        return new ImageResult
        {
            Image = image,
            ImageFormat = ImageFormat.Png
        };
    }

    public ContentResult MyQrSvg()
    {
        var qr = new BarcodeLib.Barcode { EncodedType = BarcodeLib.TYPE.QRCODE };
        qr.Encode(BarcodeLib.TYPE.QRCODE, "https://github.com/markhazleton/barcodelib", 240, 240);
        return Content(qr.GetSvg(), "image/svg+xml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
