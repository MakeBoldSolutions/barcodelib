using Barcode.Web.Models;
using Cec.Barcode.Extensions;
using Cec.Barcode.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

namespace Barcode.Web.Controllers;

public class HomeController : BaseController
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
        return View();
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
        var barCode = new BarCodeModel();
        return new ImageResult
        {
            Image = barCode.BarcodeImage,
            ImageFormat = ImageFormat.Png
        };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
