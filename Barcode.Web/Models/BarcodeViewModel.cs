using BarcodeLib;
using System.Drawing;

namespace Barcode.Web.Models;

/// <summary>
/// Playground form state for the interactive barcode demo. Bound directly from
/// BarcodeLib's own types (TYPE, AlignmentPositions, LabelPositions, RotateFlipType) --
/// no separate/duplicated enum set.
/// </summary>
public class BarcodeViewModel
{
    public string BarValue { get; set; } = "controlorigins.com";
    public string? AlternateLabel { get; set; } = "Control Origins";
    public TYPE EncodedType { get; set; } = TYPE.CODE93;
    public AlignmentPositions Alignment { get; set; } = AlignmentPositions.CENTER;
    public LabelPositions LabelPosition { get; set; } = LabelPositions.BOTTOMCENTER;
    public RotateFlipType RotateFlip { get; set; } = RotateFlipType.RotateNoneFlipNone;
    public int Width { get; set; } = 290;
    public int Height { get; set; } = 120;
    public int? BarWidth { get; set; }
    public double? AspectRatio { get; set; }
    public bool IncludeLabel { get; set; } = true;
    public bool EnforceGS1QuietZone { get; set; } = true;
    public Color ForeColor { get; set; } = Color.Black;
    public Color BackColor { get; set; } = Color.White;

    /// <summary>Populated by the controller after a successful encode, for display only.</summary>
    public string? EncodedValueText { get; set; }
    public double EncodingTimeMs { get; set; }
    public string? ErrorMessage { get; set; }

    public bool IsMatrix => EncodedType == TYPE.QRCODE;
}
