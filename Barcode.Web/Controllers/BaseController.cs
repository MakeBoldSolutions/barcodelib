namespace Barcode.Web.Controllers;
using Cec.Barcode.Repositories;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    private static readonly BarCodeMock _barCodeMock = new BarCodeMock();

    public IBarCodeDB BarCodeDb => _barCodeMock;
}