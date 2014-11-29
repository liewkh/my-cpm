Imports Microsoft.VisualBasic

Namespace Utility
    Public Class DataTypeUtils
        Public Shared Function booleanToChar(ByVal boolVal As String)
            Dim retVal As String
            If boolVal.ToUpper.Equals("TRUE") Then
                retVal = "Y"
            Else
                retVal = "N"
            End If
            Return retVal
        End Function

        Public Shared Function parseStringToHTMLSafe(ByVal strText)
            'returns safe code to html
            Dim tmpString
            If IsDBNull(strText) = True Then
                strText = ""
            End If
            tmpString = Trim(strText)
            If Not IsDBNull(tmpString) Then
                tmpString = Replace(tmpString, "<script", "")
                tmpString = Replace(tmpString, "</script>", "")

                'convert all types of single quotes
                tmpString = Replace(tmpString, Chr(145), Chr(39))
                tmpString = Replace(tmpString, Chr(146), Chr(39))
                'tmpString = Replace(tmpString, "'", "&#39;")
                tmpString = Replace(tmpString, "'", "\'")

                'convert all types of double quotes
                tmpString = Replace(tmpString, Chr(147), Chr(34))
                tmpString = Replace(tmpString, Chr(148), Chr(34))
                tmpString = Replace(tmpString, """", "\""")

                'replace carriage returns & line feeds
                tmpString = Replace(tmpString, Chr(10), " ")
                tmpString = Replace(tmpString, Chr(13), " ")
            Else
                tmpString = ""
            End If
            Return tmpString

        End Function

        Public Shared Function parseHTMLSafeToString(ByVal strText)
            'returns safe code to html
            Dim tmpString
            If IsDBNull(strText) = True Then
                strText = ""
            End If
            tmpString = Trim(strText)
            If Not IsDBNull(tmpString) Then

                'convert all types of single quotes
                tmpString = Replace(tmpString, "&amp;", "&")
                tmpString = Replace(tmpString, "&quot;", "'")
                tmpString = Replace(tmpString, "&nbsp;", " ")
                tmpString = Replace(tmpString, "&lt;", "<")
                tmpString = Replace(tmpString, "&gt;", ">")



            Else
                tmpString = ""
            End If
            Return tmpString

        End Function

        Public Shared Function formatDateString(ByVal strVal As String) As String
            Dim defaultDateFormat = "dd/MM/yyyy"
            Dim retVal As String = formatDateString(strVal, defaultDateFormat)

            Return retVal
        End Function

        Public Shared Function formatDateString(ByVal strVal As String, ByVal formatStr As String) As String
            Dim retVal As String
            If Not (String.IsNullOrEmpty(strVal) Or strVal.Equals("&nbsp;")) Then
                retVal = Format(CDate(strVal), formatStr)
            Else
                retVal = ""
            End If
            Return retVal
        End Function

        Public Shared Function formatString(ByVal strVal As String) As String
            Dim retVal As String
            If Not (String.IsNullOrEmpty(strVal) Or strVal.Equals("&nbsp;")) Then
                retVal = Trim(strVal)
            Else
                retVal = ""
            End If
            Return retVal
        End Function
        Public Shared Function StrToByteArray(ByVal str As String) As Byte()
            Dim encoding As New System.Text.UTF8Encoding
            Return encoding.GetBytes(str)
        End Function 'StrToByteArray

        Public Shared Function getGUID() As String
            Return Replace(System.Guid.NewGuid().ToString.ToUpper, "-", "")
        End Function

        Public Shared Function formatCurrency(ByVal strVal As String) As String
            Dim retVal As String
            If Not (String.IsNullOrEmpty(strVal) Or strVal.Equals("&nbsp;")) Then
                'retVal = Trim(strVal) & ".00"
                retVal = Format(CDec(Trim(strVal)), "##########0.#0")
            Else
                retVal = ""
            End If
            Return retVal
        End Function

        Public Shared Function formatNumber(ByVal strVal As String, ByVal decPlace As Integer) As String
            Dim retVal As String
            If Not (String.IsNullOrEmpty(strVal) Or strVal.Equals("&nbsp;")) Then
                If decPlace = 2 Then
                    retVal = Format(CDec(Trim(strVal)), "##,###,###,##0.#0")
                ElseIf decPlace = 3 Then
                    retVal = Format(CDec(Trim(strVal)), "##,###,###,##0.000")
                Else
                    retVal = Format(CDec(Trim(strVal)), "##,###,###,##0")
                End If
            Else
                retVal = ""
            End If
            Return retVal
        End Function

        Public Shared Function unformatNumber(ByVal strVal As String, ByVal decPlace As Integer) As String
            Dim retVal As String
            If Not (String.IsNullOrEmpty(strVal) Or strVal.Equals("&nbsp;")) Then
                If decPlace = 2 Then
                    retVal = Format(CDec(Trim(strVal)), "##########0.#0")
                Else
                    retVal = Format(CDec(Trim(strVal)), "##########0")
                End If
            Else
                retVal = ""
            End If
            Return retVal
        End Function

        Public Shared Function parseDateQueryStringSafe(ByVal dateValue As String) As String
            Dim retVal As String
            If Not String.IsNullOrEmpty(dateValue) Then
                retVal = dateValue.Substring(0, 2) + "/" + dateValue.Substring(2, 2) + "/" + dateValue.Substring(4, 4)
            Else
                retVal = ""
            End If
            Return retVal
        End Function

    End Class
End Namespace
