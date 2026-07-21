using JFont.Models;
using SkiaSharp;

namespace JFont.Services;

sealed class PdfExporter(CardRenderer renderer)
{
    public void Export(string filePath, FontInfo font, CardOptions options)
    {
        var metadata = new SKDocumentPdfMetadata
        {
            Title = $"{font.FamilyName} Font Card",
            Creator = "JFont",
        };

        using var stream = new SKFileWStream(filePath);
        using var document = SKDocument.CreatePdf(stream, metadata);
        var canvas = document.BeginPage(options.PageWidth, options.PageHeight);
        renderer.Draw(canvas, options.PageWidth, options.PageHeight, font, options);
        document.EndPage();
        document.Close();
    }
}
