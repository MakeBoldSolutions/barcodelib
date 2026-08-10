using Barcode.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Barcode.Web.Controllers;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View(MockProductCatalog.Family);
    }

    public IActionResult Family()
    {
        return View("Index", MockProductCatalog.Family);
    }

    public IActionResult Details(string slug)
    {
        var product = MockProductCatalog.FindBySlug(slug);
        if (product is null)
        {
            return NotFound();
        }

        return View(new ProductPageViewModel { Product = product });
    }

    public IActionResult DigitalLink(string gtin, string? lot)
    {
        var product = MockProductCatalog.FindByGtin(gtin);
        if (product is null)
        {
            return NotFound();
        }

        var selectedLot = product.Lots.FirstOrDefault(candidate =>
            string.Equals(candidate.Code, lot, StringComparison.OrdinalIgnoreCase));

        return View("Details", new ProductPageViewModel
        {
            Product = product,
            SelectedLot = selectedLot,
            OpenedFromDigitalLink = true,
        });
    }
}
