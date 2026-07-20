Imports System.text
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Resources

Public Class frmMain
    Private Const mcsStandardText As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

    Private miFirstChar As Integer = 0
    Private miPageCount As Integer = 0

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
Public Class LOGFONT
        Public lfHeight As Integer
        Public lfWidth As Integer
        Public lfEscapement As Integer
        Public lfOrientation As Integer
        Public lfWeight As Integer
        Public lfItalic As Byte
        Public lfUnderline As Byte
        Public lfStrikeOut As Byte
        Public lfCharSet As Byte
        Public lfOutPrecision As Byte
        Public lfClipPrecision As Byte
        Public lfQuality As Byte
        Public lfPitchAndFamily As Byte
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public lfFaceName As String
    End Class

    Private Sub ListAllInstalledFonts()
        Dim oInstalledFontCollection As New System.Drawing.Text.InstalledFontCollection
        For Each oFontFamily As FontFamily In oInstalledFontCollection.Families
            Dim oItem As New ListViewItem(oFontFamily.Name)
            oItem.ToolTipText = oFontFamily.Name
            oItem.UseItemStyleForSubItems = False

            Dim oSubItem As New ListViewItem.ListViewSubItem(oItem, txtDisplayText.Text)
            oSubItem.Font = New System.Drawing.Font(oFontFamily, 10, GetAvailableDefaultStyleForFamily(oFontFamily.Name))

            oItem.SubItems.Add(oSubItem)
            lvwFonts.Items.Add(oItem)
        Next
    End Sub

    Private Sub ShowPreview(ByVal sFontFamilyName As String)
        '---List fonts in a variety of sizes
        Dim lSizeList() As Integer = {12, 14, 18, 24, 36, 72}

        rtbPreview.Clear()
        rtbPreview.Rtf = FontListAsRTF(sFontFamilyName, lSizeList)

        rtbChars.Clear()
        rtbChars.Rtf = CharListAsRTF(sFontFamilyName)
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ListAllInstalledFonts()

        ExpandSampleColumn()
    End Sub

    Private Sub frmMain_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        ExpandSampleColumn()    'width: 644, height: 506
    End Sub

    Private Sub ExpandSampleColumn()
        lvwFonts.Columns(1).Width = splitMain.Panel1.Width - lvwFonts.Columns(0).Width
    End Sub

    Private Function GetAvailableDefaultStyleForFamily(ByVal sFontFamilyName As String) As FontStyle
        Dim iFontStyle As FontStyle = FontStyle.Regular

        Dim oFontFamily As New FontFamily(sFontFamilyName)
        If oFontFamily.IsStyleAvailable(FontStyle.Regular) Then
            iFontStyle = FontStyle.Regular

        ElseIf oFontFamily.IsStyleAvailable(FontStyle.Italic) Then
            iFontStyle = FontStyle.Italic

        ElseIf oFontFamily.IsStyleAvailable(FontStyle.Bold) Then
            iFontStyle = FontStyle.Bold

        ElseIf oFontFamily.IsStyleAvailable(FontStyle.Underline) Then
            iFontStyle = FontStyle.Underline

        ElseIf oFontFamily.IsStyleAvailable(FontStyle.Strikeout) Then
            iFontStyle = FontStyle.Strikeout
        End If

        Return iFontStyle
    End Function

    Private Sub lvwFonts_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwFonts.ItemSelectionChanged
        If e.IsSelected = True Then
            ShowPreview(e.Item.Text)
        End If
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Function FontListAsRTF( _
                        ByVal sFontFamily As String, _
                        ByVal lSizeList() As Integer) As String

        Dim sText As New StringBuilder("{\rtf1\ansi\deff0{\fonttbl")
        sText.Append("{\f0\fnil\fcharset" & FontCharSet(sFontFamily) & " " & sFontFamily & ";}")
        sText.Append("{\f1\fnil\fcharset0 Arial;}")
        sText.Append("{\f2\fnil\fcharset0 Microsoft Sans Serif;}}")
        sText.Append("{\viewkind4\uc1\pard\lang1033\f0\b\f1\fs32 " & sFontFamily & "\par}")
        sText.Append("\par")

        For Each iSize As Integer In lSizeList
            sText.Append("\f1\fs14 " & iSize & "pt")
            sText.Append("\par")

            sText.Append("{\rtlch\fcs1 \f0\fs" & iSize & txtDisplayText.Text & "\par }")
            sText.Append("{\rtlch\fcs1 \b\f0\fs" & iSize & txtDisplayText.Text & "\par }")
            sText.Append("{\rtlch\fcs1 \i\f0\fs" & iSize & txtDisplayText.Text & "\par }")
            sText.Append("\par")
        Next iSize

        sText.Append("\b0\f1\fs17\par")
        sText.Append("}")

        Return sText.ToString
    End Function

    Private Function CharListAsRTF( _
                        ByVal sFontFamily As String) As String

        Dim oRM As New ResourceManager("jFont.jFont", _
                              Assembly.GetExecutingAssembly())

        Dim oText As New StringBuilder(oRM.GetString("Pretext_1"))
        oText.Append("{\fonttbl")
        oText.Append("{\f0\nil\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}")
        oText.Append("{\f1\nil\fcharset0\fprq2{\*\panose 020b0604020202020204}Arial;}")
        oText.Append("{\f123\nil\fcharset" & FontCharSet(sFontFamily) & "\fprq2 " & sFontFamily & ";}")
        oText.Append("}")
        oText.Append(oRM.GetString("Pretext_2"))

        Dim iLine As Integer = 12
        Do
            Dim sResText As String = oRM.GetString("lines" & iLine & "_to_" & iLine + 10)
            sResText = sResText.Replace("{myfontname}", sFontFamily)
            oText.Append(sResText)
            iLine = iLine + 11
        Loop Until iLine > 877

        Return oText.ToString
    End Function

    Private Function CharListAsRTF1( _
                        ByVal sFontFamily As String) As String

        Const COL_HEIGHT As Integer = 56

        Dim sText As New StringBuilder("{\rtf1\ansi\deff0{\fonttbl" & vbCrLf)
        sText.Append("{\f0\fnil\fprq2\fcharset0 Microsoft Sans Serif;}" & vbCrLf)
        sText.Append("{\f1\fnil\fprq2\fcharset0 Arial;}" & vbCrLf)
        sText.Append("{\f2\fnil\fprq2\fcharset" & FontCharSet(sFontFamily) & " " & sFontFamily & ";}" & vbCrLf)
        sText.Append("{\f3\fswiss\fprq2\fcharset178 Arial;}}")
        sText.Append("\paperw12240\paperh15840\margl720\margr720\margt1440\margb1440\gutter0\ltrsect" & vbCrLf)
        sText.Append("\viewkind4\uc1\pard\ltrpar\nowidctlpar\b\f1\fs32 " & sFontFamily & "\par" & vbCrLf)
        sText.Append("\pard\ltrpar\nowidctlpar\tx1620\b0\fs17\par" & vbCrLf)

        sText.Append("\ltrrow\trgaph108\trleft-108")
        sText.Append("\trbrdrt\brdrs\brdrw10 ")
        sText.Append("\trbrdrl\brdrs\brdrw10 ")
        sText.Append("\trbrdrb\brdrs\brdrw10 ")
        sText.Append("\trbrdrr\brdrs\brdrw10 ")
        sText.Append("\trbrdrh\brdrs\brdrw10 ")
        sText.Append("\trbrdrv")
        sText.Append("\brdrs\brdrw10 ")

        For i As Integer = 33 To 33 + COL_HEIGHT - 1
            sText.Append(CharViewColumnFormat)

            sText.Append("\pard")
            sText.Append("\intbl\ltrpar\ql\li0\ri0\widctlpar\intbl\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0 ")
            sText.Append("\rtlch\fcs1 \af0\afs24\ltrch\fcs0 \fs24\cgrid")
            sText.Append("{\rtlch" & vbCrLf)

            '---Column 1
            sText.Append("\f1\fs12 ")
            sText.Append(FormatAsDec(i) & "\cell")

            sText.Append("\f0\fs12 ")
            sText.Append(ShowKeystroke(i) & "\f1\cell")

            sText.Append("\li0\f2\fs12 ")
            sText.Append(ShowCharacter(i) & "\cell")

            sText.Append("\cell ")  'Gutter

            '---Column 2
            sText.Append("\f0\fs12 ")
            sText.Append(FormatAsDec(i + COL_HEIGHT) & "\cell")

            sText.Append("\f1\fs12 ")
            sText.Append(ShowKeystroke(i + COL_HEIGHT) & "\cell")

            sText.Append("\f3\fs12 ")
            sText.Append(ShowCharacter(i + COL_HEIGHT) & "\cell")

            sText.Append("\cell ")  'Gutter

            '---Column 3
            sText.Append("\f0\fs12 ")
            sText.Append(FormatAsDec(i + COL_HEIGHT * 2) & "\cell")

            sText.Append("\f1\fs12 ")
            sText.Append(ShowKeystroke(i + COL_HEIGHT * 2) & "\cell")

            sText.Append("\f3\fs12 ")
            sText.Append(ShowCharacter(i + COL_HEIGHT * 2) & "\cell")

            sText.Append("\cell ")  'Gutter

            '---Column 4
            If (i + COL_HEIGHT * 3) < 256 Then
                sText.Append("\f0\fs12 ")
                sText.Append(FormatAsDec(i + COL_HEIGHT * 3) & "\cell")

                sText.Append("\f1\fs12 ")
                sText.Append(ShowKeystroke(i + COL_HEIGHT * 3) & "\cell")

                sText.Append("\f3\fs12 ")
                sText.Append(ShowCharacter(i + COL_HEIGHT * 3) & "\cell")
            End If

            sText.Append("}")
            sText.Append("\pard " & vbCrLf)
            sText.Append("\ltrpar\ql \li0\ri0\widctlpar\intbl\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0 ")
            sText.Append("{\rtlch\fcs1 \af0 \ltrch\fcs0 \trowd \irow0\irowband0\ltrrow\ts15\trgaph108\trleft-108\trbrdrt\brdrs\brdrw10 \trbrdrl\brdrs\brdrw10 \trbrdrb\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 \trbrdrh\brdrs\brdrw10 \trbrdrv\brdrs\brdrw10 ")

            sText.Append(CharViewColumnFormat)

            sText.Append("\row")

            If i = 1 Then sText.Append("\ltrrow")

            sText.Append(" }")
        Next i

        sText.Append("\pard ")
        sText.Append("\ltrpar\ql \li0\ri0\widctlpar\wrapdefault\aspalpha\aspnum\faauto\adjustright\rin0\lin0\itap0 {\rtlch\fcs1 \af0 \ltrch\fcs0  \par }}")

        Return sText.ToString
    End Function

    Private Function CharViewColumnFormat() As StringBuilder
        Dim sText As New StringBuilder

        sText.Append("\trftsWidth1\trftsWidthB3\trftsWidthA3\trautofit1\trpaddl108\trpaddr108\trpaddfl3\trpaddft3\trpaddfb3\trpaddfr3\tblrsid14577351\tbllkhdrrows\tbllklastrow\tbllkhdrcols\tbllklastcol\tblind0\tblindtype3\clvertalt\clbrdrt\brdrs\brdrw10 ")
        sText.Append("\clbrdrl")
        sText.Append("\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10\cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx697\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx1502\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10\cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx1890\clvertalt\clbrdrt\brdrnone \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrnone \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth236\clshdrawnil ")

        sText.Append("\cellx2126\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10\cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx2931\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx3736\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx4140\clvertalt\clbrdrt\brdrnone \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrnone \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth262\clshdrawnil ")

        sText.Append("\cellx4402\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx5207\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx6012\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10\clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth806\clshdrawnil ")
        sText.Append("\cellx6390\clvertalt\clbrdrt\brdrnone \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrnone \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth236\clshdrawnil")

        sText.Append("\cellx6626\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10 \clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth805\clshdrawnil ")
        sText.Append("\cellx7431\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10\clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth806\clshdrawnil")
        sText.Append("\cellx8236\clvertalt\clbrdrt\brdrs\brdrw10 \clbrdrl\brdrs\brdrw10\clbrdrb\brdrs\brdrw10 \clbrdrr\brdrs\brdrw10 \cltxlrtb\clftsWidth3\clwWidth806\clshdrawnil")
        sText.Append("\cellx8640")

        Return sText
    End Function

    Private Function FormatAsDec(ByVal iValue As Integer) As String
        Dim sValue As String = iValue.ToString

        sValue = "0x" & New String("0", 3 - sValue.Length) & sValue & "D"

        Return sValue
    End Function

    Private Function ShowKeystroke(ByVal iValue As Integer) As String
        If iValue > 32 Then
            If iValue < 127 Then
                If iValue = 92 Then
                    Return "\\"
                Else
                    Return Chr(iValue)
                End If

            ElseIf iValue < 256 Then
                Dim sValue As String = iValue.ToString
                Return "Alt+" & New String("0", 4 - sValue.Length) & sValue
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Private Function ShowCharacter(ByVal iValue As Integer) As String
        If iValue > 32 AndAlso iValue < 256 Then
            If iValue = 92 Then
                Return "\\"
            Else
                Return Chr(iValue)
            End If
        Else
            Return ""
        End If
    End Function

    Private Function FontCharSet(ByVal sFontFamily As String) As Integer
        Dim oFont As New System.Drawing.Font(sFontFamily, 12, GetAvailableDefaultStyleForFamily(sFontFamily))
        Dim oLogFont As New LOGFONT
        oFont.ToLogFont(oLogFont)

        Return oLogFont.lfCharSet
    End Function

    Private Sub PrintPanel()
        PrintDocument1.Print()
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        miFirstChar = 0
        miPageCount = 0
    End Sub

    Private Sub PrintDocument1_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint
        If tcPreview.SelectedTab Is tpChars Then
            rtbChars.FormatRangeDone()

        ElseIf tcPreview.SelectedTab Is tpSizes Then
            rtbPreview.FormatRangeDone()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        'e.Graphics.DrawRectangle(System.Drawing.Pens.Blue, e.MarginBounds)
        If tcPreview.SelectedTab Is tpChars Then
            miFirstChar = rtbChars.FormatRange(False, e, miFirstChar, rtbChars.TextLength)

            If (miFirstChar < rtbChars.TextLength) Then
                e.HasMorePages = True
                miPageCount = miPageCount + 1
            Else
                e.HasMorePages = False
            End If

        ElseIf tcPreview.SelectedTab Is tpSizes Then
            miFirstChar = rtbPreview.FormatRange(False, e, miFirstChar, rtbPreview.TextLength)

            If (miFirstChar < rtbPreview.TextLength) Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim oFile As New SaveFileDialog
        oFile.Title = "Save as RTF"
        oFile.AddExtension = True
        oFile.DefaultExt = "RTF"
        oFile.Filter = "RTF files (*.RTF)|*.RTF"
        oFile.ShowDialog()

        If oFile.FileName <> "" Then
            If tcPreview.SelectedTab Is tpChars Then
                WriteToFile(oFile.FileName, CharListAsRTF(lvwFonts.SelectedItems(0).Text))

            ElseIf tcPreview.SelectedTab Is tpSizes Then
                rtbPreview.SaveFile(oFile.FileName, RichTextBoxStreamType.RichText)

            End If
        End If
    End Sub

    Private Sub StartExecutable(ByVal sExecutable As String)
        Dim bSuccess As Boolean = True

        Try
            Dim oProcess As System.Diagnostics.Process = New System.Diagnostics.Process
            oProcess.StartInfo.FileName = sExecutable
            oProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory
            oProcess.Start()

        Catch ex As Exception
            bSuccess = False
        End Try
    End Sub

    Private Sub ToolStripStatusLabel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripStatusLabel2.Click
        StartExecutable("http://www.jlion.com")
    End Sub

    Private Sub WriteToFile( _
                ByVal sFileName As String, _
                ByVal sText As String)

        Dim oFile As New System.IO.StreamWriter(sFileName, False)
        oFile.WriteLine(sText)
        oFile.Flush()
        oFile.Close()
    End Sub
End Class
