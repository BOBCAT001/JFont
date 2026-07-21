using JFont.Models;
using SkiaSharp;

namespace JFont.Services;

sealed class FontService
{
    public IReadOnlyList<FontInfo> GetInstalledFonts()
    {
        var mgr = SKFontManager.Default;
        return [.. mgr.GetFontFamilies()
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .Select(n => CreateFontInfo(mgr, n))];
    }

    private static FontInfo CreateFontInfo(SKFontManager mgr, string familyName)
    {
        using var regular = mgr.MatchFamily(familyName, SKFontStyle.Normal);
        using var bold = mgr.MatchFamily(familyName, SKFontStyle.Bold);
        using var italic = mgr.MatchFamily(familyName, SKFontStyle.Italic);
        using var boldItalic = mgr.MatchFamily(familyName, SKFontStyle.BoldItalic);

        bool Matches(SKTypeface? tf) =>
            tf is not null && string.Equals(tf.FamilyName, familyName, StringComparison.OrdinalIgnoreCase);

        return new FontInfo
        {
            FamilyName = familyName,
            HasBold = Matches(bold),
            HasItalic = Matches(italic),
            HasBoldItalic = Matches(boldItalic),
            HasLatinSupport = HasLatinSupport(regular),
        };
    }

    private static bool HasLatinSupport(SKTypeface? typeface)
    {
        if (typeface is null) return false;
        int covered = 0;
        for (char c = 'A'; c <= 'Z'; c++)
            if (typeface.GetGlyphs(c.ToString()).FirstOrDefault() != 0)
                covered++;
        return covered >= 20;
    }
}
