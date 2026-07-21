namespace JFont.Models;

sealed class CardOptions
{
    public string SampleText { get; init; } = "The quick brown fox jumps over the lazy dog";
    public int[] Sizes { get; init; } = [12, 18, 24, 36, 48, 72];
    public float PageWidth { get; init; } = 612f;   // points — US Letter
    public float PageHeight { get; init; } = 792f;
    public float Margin { get; init; } = 36f;        // 0.5 inch
}
