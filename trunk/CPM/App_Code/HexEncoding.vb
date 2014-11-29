Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Namespace Utility
    Public Class HexEncoding
        Public Sub New()
        End Sub
        Public Shared Function GetByteCount(ByVal hexString As String) As Integer
            Dim numHexChars As Integer = 0
            Dim c As Char
            Dim i As Integer = 0
            Do While i < hexString.Length
                c = hexString.Chars(i)
                If IsHexDigit(c) Then
                    numHexChars += 1
                End If
                i += 1
            Loop
            If numHexChars Mod 2 <> 0 Then
                numHexChars -= 1
            End If
            Return numHexChars / 2 ' 2 characters per byte
        End Function
        Public Shared Function StringToHex(ByVal hexString As String, <System.Runtime.InteropServices.Out()> ByRef discarded As Integer) As Byte()
            discarded = 0
            Dim newString As String = ""
            Dim c As Char
            Dim i As Integer = 0
            Do While i < hexString.Length
                c = hexString.Chars(i)
                If IsHexDigit(c) Then
                    newString &= c
                Else
                    discarded += 1
                End If
                i += 1
            Loop
            If newString.Length Mod 2 <> 0 Then
                discarded += 1
                newString = newString.Substring(0, newString.Length - 1)
            End If

            Dim byteLength As Integer = newString.Length / 2
            Dim bytes As Byte() = New Byte(byteLength - 1) {}
            Dim hex As String
            Dim j As Integer = 0
            i = 0
            Do While i < bytes.Length
                hex = New String(New Char() {newString.Chars(j), newString.Chars(j + 1)})
                bytes(i) = HexToByte(hex)
                j = j + 2
                i += 1
            Loop
            Return bytes
        End Function
        Public Shared Function HexToString(ByVal bytes As Byte()) As String
            Dim hexString As String = ""
            Dim i As Integer = 0
            Do While i < bytes.Length
                hexString &= bytes(i).ToString("X2")
                i += 1
            Loop
            Return hexString
        End Function
        Public Shared Function InHexFormat(ByVal hexString As String) As Boolean
            Dim hexFormat As Boolean = True

            For Each digit As Char In hexString
                If (Not IsHexDigit(digit)) Then
                    hexFormat = False
                    Exit For
                End If
            Next digit
            Return hexFormat
        End Function
        Public Shared Function IsHexDigit(ByVal c As Char) As Boolean
            Dim numChar As Integer
            Dim numA As Integer = Convert.ToInt32("A"c)
            Dim num1 As Integer = Convert.ToInt32("0"c)
            c = Char.ToUpper(c)
            numChar = Convert.ToInt32(c)
            If numChar >= numA AndAlso numChar < (numA + 6) Then
                Return True
            End If
            If numChar >= num1 AndAlso numChar < (num1 + 10) Then
                Return True
            End If
            Return False
        End Function
        Private Shared Function HexToByte(ByVal hex As String) As Byte
            If hex.Length > 2 OrElse hex.Length <= 0 Then
                Throw New ArgumentException("hex must be 1 or 2 characters in length")
            End If
            Dim newByte As Byte = Byte.Parse(hex, System.Globalization.NumberStyles.HexNumber)
            Return newByte
        End Function
    End Class
End Namespace

