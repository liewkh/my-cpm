Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Namespace Utility

    Public Class Tools
        Private Declare Function SQLDataSources Lib "ODBC32.DLL" (ByVal henv As Integer, ByVal fDirection As Short, ByVal szDSN As String, ByVal cbDSNMax As Short, ByRef pcbDSN As Short, ByVal szDescription As String, ByVal cbDescriptionMax As Short, ByRef pcbDescription As Short) As Short
        Private Declare Function SQLAllocEnv Lib "ODBC32.DLL" (ByRef env As Integer) As Short
        Private Declare Function SQLConfigDataSource Lib "ODBCCP32.DLL" (ByVal hwndParent As Integer, ByVal ByValfRequest As Integer, ByVal lpszDriver As String, ByVal lpszAttributes As String) As Integer
        Const SQL_SUCCESS As Integer = 0
        Const SQL_FETCH_NEXT As Integer = 1
        Private Const ODBC_ADD_DSN As Short = 1 ' Add user data source
        Private Const ODBC_CONFIG_DSN As Short = 2 ' Configure (edit) data source
        Private Const ODBC_REMOVE_DSN As Short = 3 ' Remove data source
        Private Const ODBC_ADD_SYS_DSN As Short = 4 'Add system data source
        Private Const vbAPINull As Integer = 0 ' NULL Pointer

        Dim dm As New DBManager
        Dim conn As SqlConnection
        Dim trans As SqlTransaction

        Public Sub FetchDSNs()

            Dim ReturnValue As Short
            Dim DSNName As String
            Dim DriverName As String
            Dim DSNNameLen As Short
            Dim DriverNameLen As Short
            Dim SQLEnv As Integer 'handle to the environment

            If SQLAllocEnv(SQLEnv) <> -1 Then
                Do Until ReturnValue <> SQL_SUCCESS
                    DSNName = Space(1024)
                    DriverName = Space(1024)
                    ReturnValue = SQLDataSources(SQLEnv, SQL_FETCH_NEXT, DSNName, 1024, DSNNameLen, DriverName, 1024, DriverNameLen)
                    DSNName = Left(DSNName, DSNNameLen)
                    DriverName = Left(DriverName, DriverNameLen)

                    If DSNName <> Space(DSNNameLen) Then
                        System.Diagnostics.Debug.WriteLine(DSNName)
                        System.Diagnostics.Debug.WriteLine(DriverName)
                    End If
                Loop
            End If

        End Sub

        Public Sub CreateSystemDSN()

            Dim ReturnValue As Integer
            Dim Driver As String
            Dim Attributes As String

            Try

                'Set the driver to SQL Server because it is most common.
                Driver = "Microsoft Access Driver (*.mdb)"
                'Set the attributes delimited by null.
                'See driver documentation for a complete
                'list of supported attributes.
                'Attributes = "SERVER=SomeServer" & Chr(0)
                'Attributes = Attributes & "DESCRIPTION=MMS Database" & Chr(0)
                Attributes = "DESCRIPTION=MMS Database;"
                Attributes = Attributes & "DSN=MMS;"
                'Attributes = Attributes & "DATABASE=C:\Temp\Pos.mdb;"
                Attributes = Attributes & "DBQ=" & System.Configuration.ConfigurationManager.AppSettings("DSN") & ";"
                Attributes = Attributes & "Jet OLEDB:Database Password=" '& ''dbPass
                'To show dialog, use Form1.Hwnd instead of vbAPINull.
                ReturnValue = SQLConfigDataSource(vbAPINull, ODBC_ADD_SYS_DSN, Driver, Attributes)
                If ReturnValue <> 0 Then
                    'MsgBox("DSN Created")
                Else
                    MsgBox("System DSN Create Failed. Please Contact Administrator.")
                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try


        End Sub


        Public Shared Function NumericValidation(ByRef value As String) As Boolean
            Dim allowedChars As String = "0123456789."
            For i As Integer = value.Length - 1 To 0 Step -1
                If allowedChars.IndexOf(value(i)) = -1 Then
                    Return False
                End If
            Next
            Return True
        End Function


        Public Shared Function EmailAddressCheck(ByVal emailAddress As String) As Boolean

            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)

            If emailAddressMatch.Success Then
                EmailAddressCheck = True
            Else
                EmailAddressCheck = False
            End If

        End Function

        Public Function CheckMapDriveConnect() As Boolean
            Dim p As New IO.DriveInfo("P")
            If p.IsReady Then
                Console.WriteLine("Map Drive 'P:' Online.")
                Return True
            Else
                Console.WriteLine("Map Drive 'P:' Offline.")
                MsgBox("Error Connect To POS Database. Make sure network connectivity available 'P:'")
                Return False
            End If
            Return True

        End Function

        Public Function SecurityCheck() As Boolean

            'If My.Computer.FileSystem.FileExists("C:\WINDOWS\system32\mbintsys.dll") And My.Computer.FileSystem.FileExists("C:\WINDOWS\system32\mtasys.dll") Then
            '    Return True
            'ElseIf My.Computer.FileSystem.FileExists("C:\WINNT\system32\mbintsys.dll") And My.Computer.FileSystem.FileExists("C:\WINNT\system32\mtasys.dll") Then
            '    Return True
            'Else
            '    MsgBox("Configuration Error!", MsgBoxStyle.Critical)
            '    Return False
            'End If


        End Function


        Public Function parseString(ByVal strText)
            'returns safe code to html
            Dim tmpString
            If IsDBNull(strText) = True Then
                strText = ""
            End If
            tmpString = Trim(strText)
            If Not IsDBNull(tmpString) Then
                tmpString = Replace(tmpString, "'", "''")

                'replace carriage returns & line feeds
                tmpString = Replace(tmpString, Chr(10), " ")
                tmpString = Replace(tmpString, Chr(13), " ")
            Else
                tmpString = ""
            End If
            Return tmpString

        End Function

        Public Function ConvertTo(ByVal ImageArray As Byte()) As System.Drawing.Image
            '<summary>
            ' Convert a SQL Server database byte Array (byte array) to a Image.
            '</summary>
            Dim objMS As System.IO.MemoryStream
            Dim objImage As System.Drawing.Image
            objMS = New System.IO.MemoryStream(ImageArray)
            ' Convert the database byte array to a System Drawing Image.
            ConvertTo = System.Drawing.Image.FromStream(objMS)

        End Function

        Public Function resizeImage(ByVal pic() As Byte) As Byte()
            ' Const THUMBNAIL_IMAGE_PATH As String = "C:\Thumbnails\Test.jpg"
            Const maxWidth As Integer = 120
            Const maxHeight As Integer = 120
            Dim inp As New IntPtr
            Dim imgHeight, imgWidth As Double
            Try
                Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(New System.IO.MemoryStream(pic))
                Dim bm = New System.Drawing.Bitmap(image)
                Dim imgHres, imgVres As Single
                'Added this for testing - usually 96 dpi for every picture
                imgHres = bm.horizontalresolution
                imgVres = bm.verticalresolution
                imgHeight = bm.Height
                imgWidth = bm.Width
                If imgWidth > maxWidth Or imgHeight > maxHeight Then
                    'Determine what dimension is off by more
                    Dim deltaWidth As Double = imgWidth - maxWidth
                    Dim deltaHeight As Double = imgHeight - maxHeight
                    Dim scaleFactor As Double
                    If deltaHeight > deltaWidth Then
                        'Scale by the height
                        scaleFactor = maxHeight / imgHeight
                    Else
                        'Scale by the Width
                        scaleFactor = maxWidth / imgWidth
                    End If
                    imgWidth *= scaleFactor
                    imgHeight *= scaleFactor
                End If
                Dim w As Integer = Convert.ToInt32(imgWidth)
                Dim h As Integer = Convert.ToInt32(imgHeight)
                Dim imgbmp As System.Drawing.Image = bm.GetThumbnailImage(w, h, Nothing, inp)
                Dim ms As New System.IO.MemoryStream
                imgbmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim b(ms.Length - 1) As Byte
                ms.Position = 0
                ms.Read(b, 0, ms.Length)
                Return b


                'bitmap.Save(THUMBNAIL_IMAGE_PATH, Imaging.ImageFormat.Jpeg)



            Catch ex As Exception

            End Try


        End Function 'resizeImage

        Function GetAge(ByVal Birthdate As System.DateTime, Optional ByVal AsOf As System.DateTime = #1/1/1700#) As Integer

            'Don't set second parameter if you want Age as of today

            'Demo 1: get age of person born 2/11/1954
            'Dim objDate As New System.DateTime(1954, 2, 11)
            'Debug.WriteLine(GetAge(objDate))

            'Demo 1: get same person's age 10 years from now
            'Dim objDate As New System.DateTime(1954, 2, 11)
            'Dim objdate2 As System.DateTime
            'objdate2 = Now.AddYears(10)
            'Debug.WriteLine(GetAge(objDate, objdate2))

            Dim iMonths As Integer
            Dim iYears As Integer
            Dim dYears As Decimal
            Dim lDayOfBirth As Long
            Dim lAsOf As Long
            Dim iBirthMonth As Integer
            Dim iAsOFMonth As Integer

            If AsOf = "#1/1/1700#" Then
                AsOf = DateTime.Now
            End If
            lDayOfBirth = DatePart(DateInterval.Day, Birthdate)
            lAsOf = DatePart(DateInterval.Day, AsOf)

            iBirthMonth = DatePart(DateInterval.Month, Birthdate)
            iAsOFMonth = DatePart(DateInterval.Month, AsOf)

            iMonths = DateDiff(DateInterval.Month, Birthdate, AsOf)

            dYears = iMonths / 12

            iYears = Math.Floor(dYears)

            If iBirthMonth = iAsOFMonth Then
                If lAsOf < lDayOfBirth Then
                    iYears = iYears - 1
                End If
            End If

            Return iYears
        End Function

        Public Sub getSequenceMstr(ByVal conn As IDbConnection, ByVal trans As IDbTransaction)
            'Dim debtorEnt As New CPM.DebtorEntity
            'Dim debtorDao As New CPM.DebtorDAO

            'Try
            '    debtorEnt.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))
            '    debtorEnt.setCategory(CategoryEnum.COMPANY)
            '    debtorEnt.setCommencementDate(txtCommencementDate.Text)
            '    debtorEnt.setTypes(ddSeasonType.SelectedValue)
            '    debtorEnt.setBranchInfoId(ddLocation.SelectedValue)
            '    debtorEnt.setCarRegistrationNo(Trim(txtCarRegNo.Text.ToUpper))
            '    debtorEnt.setMake(Trim(txtMake.Text.ToUpper))
            '    debtorEnt.setModel(Trim(txtModel.Text.ToUpper))
            '    debtorEnt.setDeposit(txtDeposit.Text)
            '    debtorEnt.setRemark(Trim(txtRemark.Text))


            '    If rbCompany.Checked = True Then
            '        debtorEnt.setName(Trim(txtCompanyName.Text.ToUpper))
            '        debtorEnt.setUserName(Trim(txtUserName.Text.ToUpper))
            '        debtorEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            '        debtorEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            '        debtorEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            '        debtorEnt.setPostCode(Trim(txtPostCode2.Text.ToUpper))
            '        debtorEnt.setTelNoHome(Trim(txtTelNo.Text.ToUpper))
            '        debtorEnt.setContactPerson(Trim(txtContactPerson.Text.ToUpper))
            '        debtorEnt.setDesignation(Trim(txtDesignation.Text.ToUpper))
            '        debtorEnt.setUnitNo(Trim(txtUnitNo.Text.ToUpper))
            '        debtorEnt.setBlock(Trim(txtBlock.Text.ToUpper))
            '    Else
            '        debtorEnt.setName(Trim(txtName.Text.ToUpper))
            '        debtorEnt.setICNo(Trim(txtIC.Text.ToUpper))
            '        debtorEnt.setTelNoMobile(Trim(txtTelMob.Text.ToUpper))
            '        debtorEnt.setTelNoOffice(Trim(txtTelOff.Text.ToUpper))
            '        debtorEnt.setTelNoHome(Trim(txtTelHome.Text.ToUpper))
            '        debtorEnt.setEmployerName(Trim(txtEmployerName.Text.ToUpper))
            '        debtorEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            '    End If

            '    debtorEnt.setLastUpdatedDatetime(gvDebtor.SelectedDataKey("LUDT"))
            '    debtorEnt.setLastUpdatedBy(lp.getUserMstrId)
            '    debtorDao.updateDB(debtorEnt, cn, trans)

            'Catch ex As Exception
            '    Throw ex
            'Finally
            '    debtorEnt = Nothing
            '    debtorDao = Nothing

            'End Try

        End Sub

        Public Shared Function updatePassCardHist(ByVal conn As SqlConnection, ByVal trans As SqlTransaction, ByVal passCardMstrEnt As CPM.PassCardMstrEntity)
            Dim passHistEnt As New CPM.PassCardHistoryEntity
            Dim passHistDao As New CPM.PassCardHistoryDAO

            Try
                passHistEnt.setPassCardMstrId(passCardMstrEnt.getPassCardMstrId)
                passHistEnt.setDebtorId(passCardMstrEnt.getDebtorId)
                passHistEnt.setStartDate(Now)
                passHistEnt.setLastUpdatedDatetime(Now)
                passHistEnt.setLastUpdatedBy(passCardMstrEnt.getLastUpdatedBy)
                passHistDao.insertDB(passHistEnt, conn, trans)

            Catch ex As Exception
                Throw ex
            Finally
                passHistEnt = Nothing
                passHistDao = Nothing
            End Try
            Return 0
        End Function

        Public Shared Function SpellNumber(ByVal MyNumber As String) As String
            Dim Dollars As String = ""
            Dim Sen As String = ""
            Dim Temp As String = ""
            Dim DecimalPlace, Count As Integer
            Dim Place(9) As String
            Place(2) = " Thousand "
            Place(3) = " Million "
            Place(4) = " Billion "
            Place(5) = " Trillion "
            ' String representation of amount.
            MyNumber = Trim(Str(MyNumber))
            ' Position of decimal place 0 if none.
            DecimalPlace = InStr(MyNumber, ".")
            ' Convert Sen and set MyNumber to dollar amount.
            If DecimalPlace > 0 Then
                Sen = GetTens(Left(Mid(MyNumber, DecimalPlace + 1) & _
                "00", 2))
                MyNumber = Trim(Left(MyNumber, DecimalPlace - 1))
            End If
            Count = 1
            Do While MyNumber <> ""
                Temp = GetHundreds(Right(MyNumber, 3))
                If Temp <> "" Then Dollars = Temp & Place(Count) & Dollars
                If Len(MyNumber) > 3 Then
                    MyNumber = Left(MyNumber, Len(MyNumber) - 3)
                Else
                    MyNumber = ""
                End If
                Count = Count + 1
            Loop
            Select Case Dollars
                Case ""
                    Dollars = "No "
                Case "One"
                    Dollars = "One "
                Case Else
                    Dollars = Dollars & " "
            End Select
            Select Case Sen
                Case ""
                    Sen = "and Sen Zero Only."
                Case "One"
                    Sen = "and One Sen Only."
                Case Else
                    Sen = " and " & Sen & " Sen Only"
            End Select
            SpellNumber = Dollars & Sen
        End Function
        ' Converts a number from 100-999 into text 
        Public Shared Function GetHundreds(ByVal MyNumber As String) As String
            Dim Result As String
            If Val(MyNumber) = 0 Then : Return "" : Exit Function : End If
            MyNumber = Right("000" & MyNumber, 3)
            ' Convert the hundreds place.
            If Mid(MyNumber, 1, 1) <> "0" Then
                Result = GetDigit(Mid(MyNumber, 1, 1)) & " Hundred "
            End If
            ' Convert the tens and ones place.
            If Mid(MyNumber, 2, 1) <> "0" Then
                Result = Result & GetTens(Mid(MyNumber, 2))
            Else
                Result = Result & GetDigit(Mid(MyNumber, 3))
            End If
            GetHundreds = Result
        End Function
        ' Converts a number from 10 to 99 into text. 
        Public Shared Function GetTens(ByVal TensText As String) As String
            Dim Result As String
            Result = ""           ' Null out the temporary function value.
            If Val(Left(TensText, 1)) = 1 Then   ' If value between 10-19...
                Select Case Val(TensText)
                    Case 10 : Result = "Ten"
                    Case 11 : Result = "Eleven"
                    Case 12 : Result = "Twelve"
                    Case 13 : Result = "Thirteen"
                    Case 14 : Result = "Fourteen"
                    Case 15 : Result = "Fifteen"
                    Case 16 : Result = "Sixteen"
                    Case 17 : Result = "Seventeen"
                    Case 18 : Result = "Eighteen"
                    Case 19 : Result = "Nineteen"
                    Case Else
                End Select
            Else                                 ' If value between 20-99...
                Select Case Val(Left(TensText, 1))
                    Case 2 : Result = "Twenty "
                    Case 3 : Result = "Thirty "
                    Case 4 : Result = "Forty "
                    Case 5 : Result = "Fifty "
                    Case 6 : Result = "Sixty "
                    Case 7 : Result = "Seventy "
                    Case 8 : Result = "Eighty "
                    Case 9 : Result = "Ninety "
                    Case Else
                End Select
                Result = Result & GetDigit _
                (Right(TensText, 1))  ' Retrieve ones place.
            End If
            GetTens = Result
        End Function
        ' Converts a number from 1 to 9 into text. 
        Public Shared Function GetDigit(ByVal Digit As String) As String
            Select Case Val(Digit)
                Case 1 : GetDigit = "One"
                Case 2 : GetDigit = "Two"
                Case 3 : GetDigit = "Three"
                Case 4 : GetDigit = "Four"
                Case 5 : GetDigit = "Five"
                Case 6 : GetDigit = "Six"
                Case 7 : GetDigit = "Seven"
                Case 8 : GetDigit = "Eight"
                Case 9 : GetDigit = "Nine"
                Case Else : GetDigit = ""
            End Select
        End Function

    End Class



End Namespace
