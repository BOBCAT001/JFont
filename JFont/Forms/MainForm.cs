using JFont.Models;
using JFont.Services;
using SkiaSharp;

namespace JFont.Forms;

sealed class MainForm : Form
{
    private readonly FontService _fontService = new();
    private readonly CardRenderer _renderer = new();
    private readonly CardOptions _options = new();
    private List<FontInfo> _allFonts = [];

    private TextBox _searchBox = null!;
    private ListBox _fontList = null!;
    private PictureBox _preview = null!;
    private Button _exportPdfBtn = null!;
    private Button _exportPngBtn = null!;
    private Button _printBtn = null!;
    private Label _statusLabel = null!;

    public MainForm()
    {
        Text = "JFont — Font Card Generator";
        Size = new Size(1100, 850);
        MinimumSize = new Size(800, 600);
        BuildLayout();
        LoadFonts();
    }

    private void BuildLayout()
    {
        // Bottom toolbar — add before Fill so docking works correctly
        var bottomPanel = new Panel { Height = 50, Dock = DockStyle.Bottom };
        _exportPdfBtn = new Button { Text = "Export PDF", Size = new Size(110, 32), Location = new Point(8, 9) };
        _exportPngBtn = new Button { Text = "Export PNG", Size = new Size(110, 32), Location = new Point(126, 9) };
        _printBtn     = new Button { Text = "Print…",     Size = new Size(110, 32), Location = new Point(244, 9) };
        _statusLabel  = new Label  { Location = new Point(370, 16), Size = new Size(500, 18), ForeColor = Color.Gray };
        bottomPanel.Controls.AddRange([_exportPdfBtn, _exportPngBtn, _printBtn, _statusLabel]);
        Controls.Add(bottomPanel);

        // Top search bar
        var topPanel = new Panel { Height = 40, Dock = DockStyle.Top, Padding = new Padding(6) };

        var helpLink = new LinkLabel { Text = "Help && Docs", Dock = DockStyle.Right, Width = 87, TextAlign = ContentAlignment.MiddleCenter };
        helpLink.LinkClicked += (_, _) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://www.jlion.com/tools/jfont") { UseShellExecute = true });

        _searchBox = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Filter fonts…" };

        // Right-docked controls must be added before Fill so docking resolves correctly
        topPanel.Controls.Add(helpLink);
        topPanel.Controls.Add(_searchBox);
        Controls.Add(topPanel);

        // Main split: font list | card preview
        var split = new SplitContainer { Dock = DockStyle.Fill };
        // MinSize properties re-validate the stored SplitterDistance (default 50) via an internal
        // callback when the handle is created, so they must be set after SplitterDistance is
        // established. BeginInvoke from Load defers past WM_SIZE so Width is real.
        Load += (_, _) => BeginInvoke(() =>
        {
            split.SplitterDistance = 230;
            split.Panel1MinSize = 150;
            split.Panel2MinSize = 300;
        });

        _fontList = new ListBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f) };
        split.Panel1.Controls.Add(_fontList);

        _preview = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(0xDD, 0xDD, 0xDD),
        };
        split.Panel2.Controls.Add(_preview);

        Controls.Add(split);

        _searchBox.TextChanged += (_, _) => FilterFonts(_searchBox.Text);
        _fontList.SelectedIndexChanged += (_, _) => UpdatePreview();
        _exportPdfBtn.Click += (_, _) => ExportPdf();
        _exportPngBtn.Click += (_, _) => ExportPng();
        _printBtn.Click     += (_, _) => PrintCard();
    }

    private void LoadFonts()
    {
        _allFonts = [.. _fontService.GetInstalledFonts()];
        FilterFonts("");
    }

    private void FilterFonts(string filter)
    {
        _fontList.BeginUpdate();
        _fontList.Items.Clear();

        var source = string.IsNullOrWhiteSpace(filter)
            ? _allFonts
            : _allFonts.Where(f => f.FamilyName.Contains(filter, StringComparison.OrdinalIgnoreCase));

        foreach (var f in source)
            _fontList.Items.Add(f);

        _fontList.EndUpdate();

        if (_fontList.Items.Count > 0)
            _fontList.SelectedIndex = 0;
    }

    private void UpdatePreview()
    {
        if (_fontList.SelectedItem is not FontInfo font) return;

        if (!font.HasLatinSupport)
        {
            MessageBox.Show(
                $"\"{font.FamilyName}\" does not appear to be a Latin font.\n\n" +
                "Non-Latin font support (CJK, Hebrew, Arabic, etc.) is planned for a future update.",
                "Unsupported Font",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var old = _preview.Image;
        _preview.Image = RenderToBitmap(font, scale: 1f);
        old?.Dispose();

        _statusLabel.Text = font.FamilyName;
    }

    private Bitmap RenderToBitmap(FontInfo font, float scale)
    {
        int w = (int)(_options.PageWidth * scale);
        int h = (int)(_options.PageHeight * scale);

        var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        var data = bmp.LockBits(new Rectangle(0, 0, w, h),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

        var info = new SKImageInfo(w, h, SKColorType.Bgra8888, SKAlphaType.Premul);
        using (var surface = SKSurface.Create(info, data.Scan0, data.Stride))
        {
            var canvas = surface.Canvas;
            if (scale != 1f) canvas.Scale(scale);
            _renderer.Draw(canvas, _options.PageWidth, _options.PageHeight, font, _options);
            canvas.Flush();
        }

        bmp.UnlockBits(data);
        return bmp;
    }

    private void PrintCard()
    {
        if (_fontList.SelectedItem is not FontInfo font) return;

        using var doc = new System.Drawing.Printing.PrintDocument();
        doc.PrintPage += (_, e) =>
        {
            float scale = e.Graphics!.DpiX / 72f;
            using var bmp = RenderToBitmap(font, scale);
            e.Graphics.DrawImage(bmp, e.PageBounds);
        };

        using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                doc.Print();
                _statusLabel.Text = $"Sent to printer: {doc.PrinterSettings.PrinterName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ExportPdf()
    {
        if (_fontList.SelectedItem is not FontInfo font) return;

        using var dlg = new SaveFileDialog
        {
            Filter = "PDF Files|*.pdf",
            FileName = $"{font.FamilyName} Font Card.pdf",
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            new PdfExporter(_renderer).Export(dlg.FileName, font, _options);
            _statusLabel.Text = $"Saved: {Path.GetFileName(dlg.FileName)}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ExportPng()
    {
        if (_fontList.SelectedItem is not FontInfo font) return;

        using var dlg = new SaveFileDialog
        {
            Filter = "PNG Files|*.png",
            FileName = $"{font.FamilyName} Font Card.png",
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            new ImageExporter(_renderer).Export(dlg.FileName, font, _options);
            _statusLabel.Text = $"Saved: {Path.GetFileName(dlg.FileName)}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _preview.Image?.Dispose();
        base.OnFormClosing(e);
    }
}
