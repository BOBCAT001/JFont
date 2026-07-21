using JFont.Models;
using SkiaSharp;

namespace JFont.Services;

sealed class CardRenderer
{
    private static readonly SKColor BgColor = SKColors.White;
    private static readonly SKColor HeaderBgColor = new(0xF5, 0xF5, 0xF5);
    private static readonly SKColor TextColor = new(0x1A, 0x1A, 0x1A);
    private static readonly SKColor MutedColor = new(0x66, 0x66, 0x66);
    private static readonly SKColor DividerColor = new(0xCC, 0xCC, 0xCC);
    private static readonly SKColor LabelColor = new(0x99, 0x99, 0x99);
    private static readonly SKColor CellBorderColor = new(0xE0, 0xE0, 0xE0);
    private static readonly SKColor CellMissingColor = new(0xF2, 0xF2, 0xF2);

    public void Draw(SKCanvas canvas, float width, float height, FontInfo font, CardOptions options)
    {
        canvas.Clear(BgColor);

        float x = options.Margin;
        float cw = width - 2 * options.Margin;
        float y = options.Margin;

        y = DrawHeader(canvas, x, y, cw, font);
        y += 10f;
        y = DrawStyleVariants(canvas, x, y, cw, font, options);
        y += 10f;
        y = DrawSizeSamples(canvas, x, y, cw, font, options);
        y += 10f;
        DrawCharacterMap(canvas, x, y, cw, height - options.Margin - y, font);
    }

    private float DrawHeader(SKCanvas canvas, float x, float y, float width, FontInfo font)
    {
        const float h = 80f;

        using var bgPaint = new SKPaint { Color = HeaderBgColor };
        canvas.DrawRect(SKRect.Create(x - 2, y - 2, width + 4, h + 4), bgPaint);

        using var nameTypeface = SKTypeface.FromFamilyName(font.FamilyName, SKFontStyle.Normal);
        using var nameFont = new SKFont(nameTypeface, 46f);
        using var namePaint = new SKPaint { Color = TextColor, IsAntialias = true };
        canvas.DrawText(font.FamilyName, x, y + 50f, SKTextAlign.Left, nameFont, namePaint);

        var styles = new List<string> { "Regular" };
        if (font.HasBold) styles.Add("Bold");
        if (font.HasItalic) styles.Add("Italic");
        if (font.HasBoldItalic) styles.Add("Bold Italic");

        using var uiTypeface = UiTypeface();
        using var metaFont = new SKFont(uiTypeface, 9f);
        using var metaPaint = new SKPaint { Color = MutedColor, IsAntialias = true };
        canvas.DrawText(string.Join("  ·  ", styles), x, y + 68f, SKTextAlign.Left, metaFont, metaPaint);

        float bottom = y + h;
        Divider(canvas, x, bottom, width);
        return bottom + 6f;
    }

    private float DrawStyleVariants(SKCanvas canvas, float x, float y, float width, FontInfo font, CardOptions options)
    {
        y = SectionLabel(canvas, x, y, "STYLE VARIATIONS");

        using var uiTypeface = UiTypeface();

        var variants = new (SKFontStyle style, string label, bool available)[]
        {
            (SKFontStyle.Normal,    "Regular",     true),
            (SKFontStyle.Bold,      "Bold",        font.HasBold),
            (SKFontStyle.Italic,    "Italic",      font.HasItalic),
            (SKFontStyle.BoldItalic,"Bold Italic", font.HasBoldItalic),
        };

        foreach (var (style, label, available) in variants)
        {
            if (!available) continue;

            using var typeface = SKTypeface.FromFamilyName(font.FamilyName, style);
            using var sampleFont = new SKFont(typeface, 14f);
            using var samplePaint = new SKPaint { Color = TextColor, IsAntialias = true };
            using var labelFont = new SKFont(uiTypeface, 8f);
            using var labelPaint = new SKPaint { Color = MutedColor, IsAntialias = true };

            string prefix = label + "  ";
            float prefixW = labelFont.MeasureText(prefix);
            float baseline = y + 14f;

            canvas.DrawText(prefix, x, baseline, SKTextAlign.Left, labelFont, labelPaint);
            canvas.DrawText(Truncate(options.SampleText, sampleFont, width - prefixW), x + prefixW, baseline, SKTextAlign.Left, sampleFont, samplePaint);

            y += 20f;
        }

        y += 4f;
        Divider(canvas, x, y, width);
        return y + 4f;
    }

    private float DrawSizeSamples(SKCanvas canvas, float x, float y, float width, FontInfo font, CardOptions options)
    {
        y = SectionLabel(canvas, x, y, "SIZE SAMPLES");

        using var typeface = SKTypeface.FromFamilyName(font.FamilyName, SKFontStyle.Normal);
        using var uiTypeface = UiTypeface();

        foreach (int size in options.Sizes)
        {
            using var sampleFont = new SKFont(typeface, size);
            using var samplePaint = new SKPaint { Color = TextColor, IsAntialias = true };
            using var sizeFont = new SKFont(uiTypeface, 7f);
            using var sizePaint = new SKPaint { Color = MutedColor, IsAntialias = true };

            float rowH = size + 6f;
            float baseline = y + rowH * 0.82f;

            canvas.DrawText($"{size}pt", x, baseline, SKTextAlign.Left, sizeFont, sizePaint);
            canvas.DrawText(Truncate(options.SampleText, sampleFont, width - 28f), x + 28f, baseline, SKTextAlign.Left, sampleFont, samplePaint);

            y += rowH;
        }

        y += 4f;
        Divider(canvas, x, y, width);
        return y + 4f;
    }

    private void DrawCharacterMap(SKCanvas canvas, float x, float y, float width, float availableHeight, FontInfo font)
    {
        y = SectionLabel(canvas, x, y, "CHARACTER MAP");

        using var typeface = SKTypeface.FromFamilyName(font.FamilyName, SKFontStyle.Normal);
        using var uiTypeface = UiTypeface();

        const float cellW = 28f;
        const float cellH = 34f;
        int cols = (int)(width / cellW);
        int maxRows = (int)((availableHeight - 14f) / cellH);

        using var glyphFont = new SKFont(typeface, 15f);
        using var glyphPaint = new SKPaint { Color = TextColor, IsAntialias = true };
        using var codeFont = new SKFont(uiTypeface, 5.5f);
        using var codePaint = new SKPaint { Color = MutedColor, IsAntialias = true };
        using var borderPaint = new SKPaint { Color = CellBorderColor, StrokeWidth = 0.5f, Style = SKPaintStyle.Stroke };
        using var missingPaint = new SKPaint { Color = CellMissingColor };

        int col = 0, row = 0;

        for (int cp = 0x20; cp <= 0xFF; cp++)
        {
            if (cp > 0x7E && cp < 0xA0) continue;
            if (row >= maxRows) break;

            float cx = x + col * cellW;
            float cy = y + row * cellH;
            var cell = SKRect.Create(cx, cy, cellW - 1, cellH - 4);

            string ch = char.ConvertFromUtf32(cp);
            bool hasGlyph = typeface is not null && typeface.GetGlyphs(ch).FirstOrDefault() != 0;

            if (!hasGlyph)
                canvas.DrawRect(cell, missingPaint);
            canvas.DrawRect(cell, borderPaint);

            if (hasGlyph)
            {
                float glyphW = glyphFont.MeasureText(ch);
                canvas.DrawText(ch, cx + (cellW - glyphW) / 2f, cy + cellH * 0.60f, SKTextAlign.Left, glyphFont, glyphPaint);
            }

            string code = cp.ToString("X4");
            float codeW = codeFont.MeasureText(code);
            canvas.DrawText(code, cx + (cellW - codeW) / 2f, cy + cellH - 6f, SKTextAlign.Left, codeFont, codePaint);

            col++;
            if (col >= cols) { col = 0; row++; }
        }
    }

    private float SectionLabel(SKCanvas canvas, float x, float y, string label)
    {
        using var typeface = UiTypeface();
        using var font = new SKFont(typeface, 7.5f);
        using var paint = new SKPaint { Color = LabelColor, IsAntialias = true };
        canvas.DrawText(label, x, y + 8f, SKTextAlign.Left, font, paint);
        return y + 12f;
    }

    private void Divider(SKCanvas canvas, float x, float y, float width)
    {
        using var paint = new SKPaint { Color = DividerColor, StrokeWidth = 0.5f, Style = SKPaintStyle.Stroke };
        canvas.DrawLine(x, y, x + width, y, paint);
    }

    private static SKTypeface UiTypeface() =>
        SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal)
        ?? SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)!;

    private static string Truncate(string text, SKFont font, float maxWidth)
    {
        if (font.MeasureText(text) <= maxWidth)
            return text;

        for (int i = text.Length - 1; i > 0; i--)
        {
            string candidate = text[..i] + "…";
            if (font.MeasureText(candidate) <= maxWidth)
                return candidate;
        }
        return "…";
    }
}
