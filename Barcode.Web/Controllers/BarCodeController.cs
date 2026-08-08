using Barcode.Web.Models;
using Cec.Barcode.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Barcode.Web.Controllers;

public class BarCodeController : BaseController
{
    public BarCodeVM BarCodeMain
    {
        get
        {
            return new BarCodeVM()
            {
                BarCodeList = BarCodeDb.ListAll()
            };
        }
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View(BarCodeMain);
    }

    [HttpGet]
    public ActionResult Details(int id = 1)
    {
        return View(BarCodeMain.BarCodeList.Where(w => w.Id == id).FirstOrDefault());
    }

    [HttpGet]
    public ActionResult Edit(int id = 1)
    {
        return View(BarCodeMain.BarCodeList.Where(w => w.Id == id).FirstOrDefault());
    }

    [HttpPost]
    public ActionResult Edit(BarCodeModel postBarcode, int id = 1)
    {
        BarCodeDb.Update(postBarcode);
        return View(BarCodeDb.Get(id));
    }
}
