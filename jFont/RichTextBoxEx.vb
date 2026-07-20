Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Public Class RichTextBoxEx
    '---converted to vb from:
    'http://www.codeguru.com/Csharp/Csharp/cs_controls/richtext/article.php/c4781

    Private moRTB As RichTextBox

    Public Sub New(ByVal oRTB As RichTextBox)
        moRTB = oRTB
    End Sub

    <StructLayout(LayoutKind.Sequential)> _
    Private Class RECT
        Public left As Int16
        Public top As Int16
        Public right As Int16
        Public bottom As Int16
    End Class

    <StructLayout(LayoutKind.Sequential)> _
   Private Class CHARRANGE
        Public cpMin As Int16
        Public cpMax As Int16
    End Class

    <StructLayout(LayoutKind.Sequential)> _
   Private Class _FORMATRANGE
        Public hdc As IntPtr
        Public hdcTarget As IntPtr
        Public rc As RECT
        Public rcPage As RECT
        Public chrg As CHARRANGE
    End Class

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
     Private Shared Function SendMessage( _
        ByVal hWnd As IntPtr, _
        ByVal msg As Int16, _
        ByVal wParam As IntPtr, _
        ByVal lParam As IntPtr) As IntPtr

    End Function

    Const WM_USER As Int16 = 400
    Const EM_FORMATRANGE As Int16 = WM_USER + 57
    Const EM_SETTARGETDEVICE As Int16 = WM_USER + 72

    Private Function HundredthInchToTwips(ByVal n As Int16) As Int16
        Return CInt((n * 14.4))
    End Function

    Public Function SetTargetDevice( _
        ByVal g As Graphics, _
        ByVal lineLen As Int16) As Boolean

        Dim res As IntPtr
        Dim wpar As IntPtr = g.GetHdc
        Dim lpar As IntPtr = New IntPtr(HundredthInchToTwips(lineLen))

        res = SendMessage(moRTB.Handle, EM_SETTARGETDEVICE, wpar, lpar)

        g.ReleaseHdc(wpar)

        Return (res.ToInt32() <> 0)
    End Function

    Public Function FormatRange( _
        ByVal measureonly As Boolean, _
        ByVal e As PrintPageEventArgs, _
        ByVal charFrom As Int16, _
        ByVal charTo As Int16) As Int16

        Dim cr As New CHARRANGE
        cr.cpMin = charFrom
        cr.cpMax = charTo

        Dim rc As New RECT
        rc.top = HundredthInchToTwips(e.MarginBounds.Top)
        rc.bottom = HundredthInchToTwips(e.MarginBounds.Bottom)
        rc.left = HundredthInchToTwips(e.MarginBounds.Left)
        rc.right = HundredthInchToTwips(e.MarginBounds.Right)

        Dim rcPage As New RECT
        rcPage.top = HundredthInchToTwips(e.PageBounds.Top)
        rcPage.bottom = HundredthInchToTwips(e.PageBounds.Bottom)
        rcPage.left = HundredthInchToTwips(e.PageBounds.Left)
        rcPage.right = HundredthInchToTwips(e.PageBounds.Right)

        Dim hdc As IntPtr = e.Graphics.GetHdc()

        Dim fr As New _FORMATRANGE
        fr.chrg = cr
        fr.hdc = hdc
        fr.hdcTarget = hdc
        fr.rc = rc
        fr.rcPage = rcPage

        Dim res As IntPtr

        Dim wpar As IntPtr
        If measureonly Then
            wpar = New IntPtr(0)
        Else
            wpar = New IntPtr(1)
        End If

        Dim lpar As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr))
        Marshal.StructureToPtr(fr, lpar, False)

        res = SendMessage(moRTB.Handle, EM_FORMATRANGE, wpar, lpar)

        Marshal.FreeCoTaskMem(lpar)

        e.Graphics.ReleaseHdc(hdc)

        Return res.ToInt32()
    End Function

    Public Sub FormatRangeDone()
        Dim wpar As New IntPtr(0)
        Dim lpar As New IntPtr(0)
        SendMessage(moRTB.Handle, EM_FORMATRANGE, wpar, lpar)
    End Sub
End Class
