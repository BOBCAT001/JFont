using JFont.Models;
using SkiaSharp;

namespace JFont.Services;

sealed class ImageExporter(CardRenderer renderer)
{
    private const float Scale = 2f; // ~144 DPI for letter-size output

    public void Export(string filePath, FontInfo font, CardOptions options)
    {
        int w = (int)(options.PageWidth * Scale);
        int h = (int)(options.PageHeight * Scale);

        using var bitmap = new SKBitmap(w, h);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);
        canvas.Scale(Scale);
        renderer.Draw(canvas, options.PageWidth, options.PageHeight, font, options);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(filePath);
        data.SaveTo(stream);
    }
}
