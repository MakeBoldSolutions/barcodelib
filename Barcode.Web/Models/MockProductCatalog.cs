namespace Barcode.Web.Models;

public static class MockProductCatalog
{
    public const string ProductFamilySlug = "bold-coffee";
    public const string ProductFamilyName = "Bold Coffee";
    public const string BoldCoffeeSlug = "bold-dark-roast-coffee";
    public const string BoldCoffeeGtin = "00012345678905";
    public const string BoldCoffeeUpc = "012345678905";
    public const string BoldCoffeeLot = "2027-C";

    public static string BoldCoffeeDigitalLinkUrl => CreateDigitalLinkUrl(BoldCoffeeGtin, BoldCoffeeLot);

    public static IReadOnlyList<MockProduct> Products { get; } =
    [
        new MockProduct
        {
            Slug = BoldCoffeeSlug,
            Name = "Bold Dark Roast Coffee",
            Brand = ProductFamilyName,
            Description = "A full-bodied connected-product mock for demonstrating UPC, QR Code, and GS1 Digital Link-style identity flows.",
            NetWeight = "12 oz whole bean coffee",
            Gtin = BoldCoffeeGtin,
            Upc = BoldCoffeeUpc,
            Price = "$12.99",
            Source = "Colombia, Guatemala, and Brazil",
            Region = "Latin America blend",
            Process = "Washed and natural process blend",
            Roast = "Dark roast",
            FlavorNotes = "Cocoa, toasted walnut, and brown sugar",
            Ingredients = "Arabica coffee",
            Allergens = "No major allergens declared",
            Brewing = "Use 2 tablespoons of coffee per 6 fl oz of filtered water.",
            Recycling = "Recycle paper bag where facilities exist. Remove valve before recycling.",
            SupportEmail = "support@makeboldsolutions.com",
            Lots =
            [
                Lot("2027-A", "Active", new DateOnly(2028, 1, 15), "Current production lot for normal sale."),
                Lot("2027-B", "Markdown", new DateOnly(2027, 12, 1), "Near-date demonstration lot used for markdown scenarios."),
                Lot(BoldCoffeeLot, "Active", new DateOnly(2027, 12, 15), "The lot encoded in the homepage QR Code demonstration.")
            ]
        },
        new MockProduct
        {
            Slug = "bold-ethiopia-yirgacheffe",
            Name = "Bold Ethiopia Yirgacheffe",
            Brand = ProductFamilyName,
            Description = "A bright single-origin mock coffee page showing how origin, process, and lot information can travel behind one QR code.",
            NetWeight = "10 oz whole bean coffee",
            Gtin = "00012345678912",
            Upc = "012345678912",
            Price = "$15.49",
            Source = "Ethiopia",
            Region = "Yirgacheffe, Gedeo Zone",
            Process = "Washed",
            Roast = "Light roast",
            FlavorNotes = "Jasmine, lemon zest, and black tea",
            Ingredients = "Arabica coffee",
            Allergens = "No major allergens declared",
            Brewing = "Best as pour-over at a 1:16 coffee-to-water ratio.",
            Recycling = "Recycle paper bag where facilities exist. Remove valve before recycling.",
            SupportEmail = "support@makeboldsolutions.com",
            Lots =
            [
                Lot("2027-E1", "Active", new DateOnly(2028, 2, 10), "Fresh crop demonstration lot."),
                Lot("2027-E2", "Active", new DateOnly(2028, 3, 4), "Alternate roast date for traceability comparison.")
            ]
        },
        new MockProduct
        {
            Slug = "bold-guatemala-antigua",
            Name = "Bold Guatemala Antigua",
            Brand = ProductFamilyName,
            Description = "A balanced single-origin mock with farm-region details, roast metadata, and lot-specific freshness dates.",
            NetWeight = "12 oz whole bean coffee",
            Gtin = "00012345678929",
            Upc = "012345678929",
            Price = "$14.25",
            Source = "Guatemala",
            Region = "Antigua Valley",
            Process = "Washed",
            Roast = "Medium roast",
            FlavorNotes = "Milk chocolate, red apple, and baking spice",
            Ingredients = "Arabica coffee",
            Allergens = "No major allergens declared",
            Brewing = "Works well for drip, pour-over, and press pot.",
            Recycling = "Recycle paper bag where facilities exist. Remove valve before recycling.",
            SupportEmail = "support@makeboldsolutions.com",
            Lots =
            [
                Lot("2027-G1", "Active", new DateOnly(2028, 1, 28), "Primary retail lot."),
                Lot("2027-G2", "Active", new DateOnly(2028, 2, 18), "Second roast batch from the same source.")
            ]
        },
        new MockProduct
        {
            Slug = "bold-sumatra-mandheling",
            Name = "Bold Sumatra Mandheling",
            Brand = ProductFamilyName,
            Description = "An earthy Indonesian mock product used to show how processing method and origin can become scan-accessible data.",
            NetWeight = "12 oz whole bean coffee",
            Gtin = "00012345678936",
            Upc = "012345678936",
            Price = "$14.99",
            Source = "Indonesia",
            Region = "North Sumatra",
            Process = "Wet-hulled",
            Roast = "Medium-dark roast",
            FlavorNotes = "Cedar, molasses, and dark chocolate",
            Ingredients = "Arabica coffee",
            Allergens = "No major allergens declared",
            Brewing = "Use a slightly coarser grind for French press or immersion brewing.",
            Recycling = "Recycle paper bag where facilities exist. Remove valve before recycling.",
            SupportEmail = "support@makeboldsolutions.com",
            Lots =
            [
                Lot("2027-S1", "Active", new DateOnly(2028, 2, 5), "Origin-specific lot for Indonesian source testing."),
                Lot("2027-S2", "Hold", new DateOnly(2028, 2, 22), "Mock quality-hold state for resolver demonstrations.")
            ]
        },
        new MockProduct
        {
            Slug = "bold-colombia-decaf",
            Name = "Bold Colombia Decaf",
            Brand = ProductFamilyName,
            Description = "A decaf mock coffee page with source and process fields that mirror the regular product records.",
            NetWeight = "12 oz ground coffee",
            Gtin = "00012345678943",
            Upc = "012345678943",
            Price = "$13.75",
            Source = "Colombia",
            Region = "Huila",
            Process = "Sugarcane ethyl acetate decaf",
            Roast = "Medium roast",
            FlavorNotes = "Caramel, almond, and orange peel",
            Ingredients = "Decaffeinated Arabica coffee",
            Allergens = "No major allergens declared",
            Brewing = "Use for drip brewing or espresso-style decaf testing.",
            Recycling = "Recycle paper bag where facilities exist. Remove valve before recycling.",
            SupportEmail = "support@makeboldsolutions.com",
            Lots =
            [
                Lot("2027-D1", "Active", new DateOnly(2028, 1, 20), "Decaf production lot."),
                Lot("2027-D2", "Active", new DateOnly(2028, 2, 12), "Second decaf batch for source comparison.")
            ]
        }
    ];

    public static ProductFamilyViewModel Family => new()
    {
        Slug = ProductFamilySlug,
        Name = ProductFamilyName,
        Description = "A mock connected-product family showing how one brand can publish multiple coffees, source details, lots, and QR-accessible product records.",
        Products = Products,
        Sources = Products.Select(product => product.Source).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(source => source).ToList(),
        RoastProfiles = Products.Select(product => product.Roast).Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
    };

    public static string CreateDigitalLinkUrl(MockProduct product)
    {
        return CreateDigitalLinkUrl(product.Gtin, product.PrimaryLot?.Code);
    }

    public static string CreateDigitalLinkUrl(MockProduct product, string? lot)
    {
        return CreateDigitalLinkUrl(product.Gtin, lot);
    }

    public static string CreateDigitalLinkUrl(string gtin, string? lot = null)
    {
        var path = $"{SiteBranding.SiteUrl}/01/{gtin}";
        return string.IsNullOrWhiteSpace(lot) ? path : $"{path}/10/{lot}";
    }

    public static MockProduct? FindBySlug(string slug)
    {
        return Products.FirstOrDefault(product => string.Equals(product.Slug, slug, StringComparison.OrdinalIgnoreCase));
    }

    public static MockProduct? FindByGtin(string gtin)
    {
        return Products.FirstOrDefault(product => string.Equals(product.Gtin, gtin, StringComparison.OrdinalIgnoreCase));
    }

    private static MockProductLot Lot(string code, string status, DateOnly bestBefore, string message)
    {
        return new MockProductLot
        {
            Code = code,
            Status = status,
            BestBefore = bestBefore,
            Message = message
        };
    }
}

public class ProductFamilyViewModel
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<MockProduct> Products { get; set; } = [];
    public IReadOnlyList<string> Sources { get; set; } = [];
    public IReadOnlyList<string> RoastProfiles { get; set; } = [];
}

public class MockProduct
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NetWeight { get; set; } = string.Empty;
    public string Gtin { get; set; } = string.Empty;
    public string Upc { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Process { get; set; } = string.Empty;
    public string Roast { get; set; } = string.Empty;
    public string FlavorNotes { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Allergens { get; set; } = string.Empty;
    public string Brewing { get; set; } = string.Empty;
    public string Recycling { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public IReadOnlyList<MockProductLot> Lots { get; set; } = [];
    public MockProductLot? PrimaryLot => Lots.FirstOrDefault();
}

public class MockProductLot
{
    public string Code { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly BestBefore { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ProductPageViewModel
{
    public MockProduct Product { get; set; } = new();
    public MockProductLot? SelectedLot { get; set; }
    public bool OpenedFromDigitalLink { get; set; }
    public string DigitalLinkUrl => MockProductCatalog.CreateDigitalLinkUrl(Product, SelectedLot?.Code ?? Product.PrimaryLot?.Code);
}
