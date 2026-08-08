namespace Barcode.Web.Extensions;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// from https://stackoverflow.com/questions/186062/can-an-asp-net-mvc-controller-return-an-image
public class ImageResult : ActionResult
{
    public Image Image { get; set; } = null!;
    public ImageFormat ImageFormat { get; set; } = null!;

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        if (Image == null)
        {
            throw new ArgumentNullException(nameof(Image));
        }
        if (ImageFormat == null)
        {
            throw new ArgumentNullException(nameof(ImageFormat));
        }

        ImageCodecInfo? encoder = GetEncoder(ImageFormat);
        if (encoder == null)
        {
            throw new InvalidOperationException($"No image encoder was found for the format '{ImageFormat}'.");
        }

        context.HttpContext.Response.ContentType = encoder.MimeType ?? "application/octet-stream";

        using var memoryStream = new MemoryStream();
        Image.Save(memoryStream, encoder, null);
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(context.HttpContext.Response.Body);
    }

    private static ImageCodecInfo? GetEncoder(ImageFormat imageFormat)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        return codecs.FirstOrDefault(codec => codec.FormatID == imageFormat.Guid);
    }
}
