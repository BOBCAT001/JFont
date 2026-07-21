namespace JFont.Models;

sealed class FontInfo
{
    public required string FamilyName { get; init; }
    public bool HasBold { get; init; }
    public bool HasItalic { get; init; }
    public bool HasBoldItalic { get; init; }
    public bool HasLatinSupport { get; init; }

    public override string ToString() => FamilyName;
}
