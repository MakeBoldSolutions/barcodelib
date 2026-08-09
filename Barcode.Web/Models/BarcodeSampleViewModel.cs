using BarcodeLib;
using Microsoft.AspNetCore.Routing;
using System.Drawing;

namespace Barcode.Web.Models;

public class BarcodeSampleViewModel
{
    public string TargetUrl { get; set; } = string.Empty;
    public List<BarcodeSampleItem> Samples { get; set; } = new();
    public List<BarcodeSampleItem> FailedSamples { get; set; } = new();

    public int ValidCount => Samples.Count;
    public int AttemptedCount => Samples.Count + FailedSamples.Count;
}

public class BarcodeSampleItem
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TYPE EncodedType { get; set; }
    public string Payload { get; set; } = string.Empty;
    public string RenderAction { get; set; } = "Svg";
    public int Width { get; set; }
    public int Height { get; set; }
    public string ForeColor { get; set; } = "#000000";
    public string BackColor { get; set; } = "#FFFFFF";
    public bool IncludeLabel { get; set; }
    public bool EnforceGS1QuietZone { get; set; } = true;
    public AlignmentPositions Alignment { get; set; } = AlignmentPositions.CENTER;
    public LabelPositions LabelPosition { get; set; } = LabelPositions.BOTTOMCENTER;
    public RotateFlipType RotateFlip { get; set; } = RotateFlipType.RotateNoneFlipNone;
    public int? BarWidth { get; set; }
    public double? AspectRatio { get; set; }
    public string? ErrorMessage { get; set; }

    public RouteValueDictionary RouteValues => new(new
    {
        BarValue = Payload,
        AlternateLabel = "Make Bold Solutions",
        EncodedType,
        Alignment,
        LabelPosition,
        RotateFlip,
        Width,
        Height,
        BarWidth,
        AspectRatio,
        IncludeLabel,
        EnforceGS1QuietZone,
        ForeColor,
        BackColor,
    });
}
