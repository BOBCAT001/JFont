<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ssMain = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tsMain = New System.Windows.Forms.ToolStrip
        Me.cmdPrint = New System.Windows.Forms.ToolStripButton
        Me.cmdSave = New System.Windows.Forms.ToolStripButton
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel
        Me.txtDisplayText = New System.Windows.Forms.ToolStripTextBox
        Me.splitMain = New System.Windows.Forms.SplitContainer
        Me.lvwFonts = New System.Windows.Forms.ListView
        Me.chName = New System.Windows.Forms.ColumnHeader
        Me.chSample = New System.Windows.Forms.ColumnHeader
        Me.tcPreview = New System.Windows.Forms.TabControl
        Me.tpSizes = New System.Windows.Forms.TabPage
        Me.rtbPreview = New RichTextExLib.RichTextBoxEx
        Me.tpChars = New System.Windows.Forms.TabPage
        Me.rtbChars = New RichTextExLib.RichTextBoxEx
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ssMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.splitMain.Panel1.SuspendLayout()
        Me.splitMain.Panel2.SuspendLayout()
        Me.splitMain.SuspendLayout()
        Me.tcPreview.SuspendLayout()
        Me.tpSizes.SuspendLayout()
        Me.tpChars.SuspendLayout()
        Me.SuspendLayout()
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.ssMain.Location = New System.Drawing.Point(0, 457)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(636, 22)
        Me.ssMain.TabIndex = 0
        Me.ssMain.Text = "(c) 2007 jlion.com"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(98, 17)
        Me.ToolStripStatusLabel1.Text = "(c) 2007 Joe Lynds"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Underline)
        Me.ToolStripStatusLabel2.ForeColor = System.Drawing.Color.MidnightBlue
        Me.ToolStripStatusLabel2.IsLink = True
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(109, 17)
        Me.ToolStripStatusLabel2.Text = "http://www.jlion.com"
        '
        'tsMain
        '
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdPrint, Me.cmdSave, Me.ToolStripLabel2, Me.txtDisplayText})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(636, 25)
        Me.tsMain.TabIndex = 2
        Me.tsMain.Text = "ToolStrip1"
        '
        'cmdPrint
        '
        Me.cmdPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdPrint.Image = CType(resources.GetObject("cmdPrint.Image"), System.Drawing.Image)
        Me.cmdPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(23, 22)
        Me.cmdPrint.Text = "Print"
        '
        'cmdSave
        '
        Me.cmdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(23, 22)
        Me.cmdSave.Text = "Save As RTF"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(70, 22)
        Me.ToolStripLabel2.Text = "Display Text:"
        '
        'txtDisplayText
        '
        Me.txtDisplayText.Name = "txtDisplayText"
        Me.txtDisplayText.Size = New System.Drawing.Size(300, 25)
        Me.txtDisplayText.Text = "The quick brown dog jumped over the lazy fox"
        '
        'splitMain
        '
        Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitMain.Location = New System.Drawing.Point(0, 25)
        Me.splitMain.Name = "splitMain"
        Me.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitMain.Panel1
        '
        Me.splitMain.Panel1.Controls.Add(Me.lvwFonts)
        '
        'splitMain.Panel2
        '
        Me.splitMain.Panel2.Controls.Add(Me.tcPreview)
        Me.splitMain.Size = New System.Drawing.Size(636, 432)
        Me.splitMain.SplitterDistance = 216
        Me.splitMain.TabIndex = 5
        '
        'lvwFonts
        '
        Me.lvwFonts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName, Me.chSample})
        Me.lvwFonts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwFonts.FullRowSelect = True
        Me.lvwFonts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvwFonts.Location = New System.Drawing.Point(0, 0)
        Me.lvwFonts.MultiSelect = False
        Me.lvwFonts.Name = "lvwFonts"
        Me.lvwFonts.ShowGroups = False
        Me.lvwFonts.ShowItemToolTips = True
        Me.lvwFonts.Size = New System.Drawing.Size(636, 216)
        Me.lvwFonts.TabIndex = 4
        Me.lvwFonts.UseCompatibleStateImageBehavior = False
        Me.lvwFonts.View = System.Windows.Forms.View.Details
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 250
        '
        'chSample
        '
        Me.chSample.Text = "Sample"
        '
        'tcPreview
        '
        Me.tcPreview.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.tcPreview.Controls.Add(Me.tpSizes)
        Me.tcPreview.Controls.Add(Me.tpChars)
        Me.tcPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcPreview.Location = New System.Drawing.Point(0, 0)
        Me.tcPreview.Name = "tcPreview"
        Me.tcPreview.SelectedIndex = 0
        Me.tcPreview.Size = New System.Drawing.Size(636, 212)
        Me.tcPreview.TabIndex = 0
        '
        'tpSizes
        '
        Me.tpSizes.Controls.Add(Me.rtbPreview)
        Me.tpSizes.Location = New System.Drawing.Point(4, 4)
        Me.tpSizes.Name = "tpSizes"
        Me.tpSizes.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSizes.Size = New System.Drawing.Size(628, 186)
        Me.tpSizes.TabIndex = 0
        Me.tpSizes.Text = "Font Sizes"
        Me.tpSizes.UseVisualStyleBackColor = True
        '
        'rtbPreview
        '
        Me.rtbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbPreview.Location = New System.Drawing.Point(3, 3)
        Me.rtbPreview.Name = "rtbPreview"
        Me.rtbPreview.Size = New System.Drawing.Size(622, 180)
        Me.rtbPreview.TabIndex = 1
        Me.rtbPreview.Text = ""
        '
        'tpChars
        '
        Me.tpChars.Controls.Add(Me.rtbChars)
        Me.tpChars.Location = New System.Drawing.Point(4, 4)
        Me.tpChars.Name = "tpChars"
        Me.tpChars.Padding = New System.Windows.Forms.Padding(3)
        Me.tpChars.Size = New System.Drawing.Size(628, 186)
        Me.tpChars.TabIndex = 1
        Me.tpChars.Text = "Font Characters"
        Me.tpChars.UseVisualStyleBackColor = True
        '
        'rtbChars
        '
        Me.rtbChars.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbChars.Location = New System.Drawing.Point(3, 3)
        Me.rtbChars.Name = "rtbChars"
        Me.rtbChars.Size = New System.Drawing.Size(622, 180)
        Me.rtbChars.TabIndex = 0
        Me.rtbChars.Text = ""
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrintDocument1
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(636, 479)
        Me.Controls.Add(Me.splitMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.ssMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(644, 506)
        Me.Name = "frmMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "jFont - Installed Fonts"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.splitMain.Panel1.ResumeLayout(False)
        Me.splitMain.Panel2.ResumeLayout(False)
        Me.splitMain.ResumeLayout(False)
        Me.tcPreview.ResumeLayout(False)
        Me.tpSizes.ResumeLayout(False)
        Me.tpChars.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents txtDisplayText As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents cmdPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
    Friend WithEvents lvwFonts As System.Windows.Forms.ListView
    Friend WithEvents chName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSample As System.Windows.Forms.ColumnHeader
    Friend WithEvents tcPreview As System.Windows.Forms.TabControl
    Friend WithEvents tpSizes As System.Windows.Forms.TabPage
    Friend WithEvents rtbPreview As RichTextExLib.RichTextBoxEx
    Friend WithEvents tpChars As System.Windows.Forms.TabPage
    Friend WithEvents rtbChars As RichTextExLib.RichTextBoxEx
    Friend WithEvents PrintPreviewDialog1 As System.Windows.Forms.PrintPreviewDialog
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents cmdSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel

End Class
