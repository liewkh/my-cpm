Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_updateDailyCollection
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        If Not Page.IsPostBack Then
            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            SpecialDays.AddHolidays(popCalendar1)
            SpecialDays.AddSpecialDays(popCalendar1)

        End If

        Session.LCID = 2057

    End Sub

    Private Sub clear()

        lblmsg.Text = ""
        ddLocation.Enabled = True
        txtDate.Enabled = True

        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        'txtDate.Text = Utility.DataTypeUtils.formatDateString(Now.AddDays(-1))
        txtDate.Text = ""

        txtCashierShift1.Text = ""
        txtCashierShift2.Text = ""
        txtCashierShift3.Text = ""
        txtValet1.Text = ""
        txtValet2.Text = ""
        txtValet3.Text = ""
        txtMotorcycle1.Text = ""
        txtMotorcycle2.Text = ""
        txtMotorcycle3.Text = ""
        txtAPS1.Text = ""
        txtAPS2.Text = ""
        txtAPS3.Text = ""
        txtAPS4.Text = ""
        txtAPS5.Text = ""
        txtAPS6.Text = ""
        txtCarCash.Text = ""
        txtCarChq.Text = ""
        txtCarCreditCard.Text = ""
        txtMotorCash.Text = ""
        txtMotorChq.Text = ""
        txtMotorCreditCard.Text = ""
        txtDepositCar.Text = ""
        txtDepositMotor.Text = ""
        txtDepositOther.Text = ""
        txtClamp.Text = ""
        txtTransponder.Text = ""
        txtNoPlate.Text = ""
        txtTemporaryRental.Text = ""
        txtMisc.Text = ""
        txtEarlyBird.Text = ""
        txtComplimentaryTicket.Text = ""
        txtManualRaised.Text = ""
        txtOutstandingTicket.Text = ""
        txtInfoOthers.Text = ""

        txtCashierTotal.Text = ""
        txtValetTotal.Text = ""
        txtMotorcyleTotal.Text = ""
        txtAPSTotal.Text = ""
        txtTotalDaily.Text = ""
        txtSeasonTotal.Text = ""
        txtTotalDeposit.Text = ""
        txtOtherTotal.Text = ""
        txtInfoTotal.Text = ""
        txtTotalCollection.Text = ""

    End Sub

    Protected Sub doUpdate(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dailyEnt As New CPM.DailyCollectionEntity
        Dim dailyDao As New CPM.DailyCollectionDAO
        Dim strSQL As String = ""
        Dim isExist As Boolean = False

        If hidDailyCollectionId.Value = "" Then
            lblmsg.Text = "Please select the daily collection record for update."
            Exit Sub
        End If

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction


        Try


            dailyEnt.setDailyCollectionId(hidDailyCollectionId.Value)

            If Trim(txtCashierShift1.Text) <> "" Then
                dailyEnt.setCashier1(txtCashierShift1.Text)
            Else
                dailyEnt.setCashier1(0)
            End If

            If Trim(txtCashierShift2.Text) <> "" Then
                dailyEnt.setCashier2(txtCashierShift2.Text)
            Else
                dailyEnt.setCashier2(0)
            End If

            If Trim(txtCashierShift3.Text) <> "" Then
                dailyEnt.setCashier3(txtCashierShift3.Text)
            Else
                dailyEnt.setCashier3(0)
            End If

            If Trim(txtValet1.Text) <> "" Then
                dailyEnt.setValetService1(txtValet1.Text)
            Else
                dailyEnt.setValetService1(0)
            End If

            If Trim(txtValet2.Text) <> "" Then
                dailyEnt.setValetService2(txtValet2.Text)
            Else
                dailyEnt.setValetService2(0)
            End If

            If Trim(txtValet3.Text) <> "" Then
                dailyEnt.setValetService3(txtValet3.Text)
            Else
                dailyEnt.setValetService3(0)
            End If

            If Trim(txtMotorcycle1.Text) <> "" Then
                dailyEnt.setMotorcycle1(txtMotorcycle1.Text)
            Else
                dailyEnt.setMotorcycle1(0)
            End If

            If Trim(txtMotorcycle2.Text) <> "" Then
                dailyEnt.setMotorcycle2(txtMotorcycle2.Text)
            Else
                dailyEnt.setMotorcycle2(0)
            End If

            If Trim(txtMotorcycle3.Text) <> "" Then
                dailyEnt.setMotorcycle3(txtMotorcycle3.Text)
            Else
                dailyEnt.setMotorcycle3(0)
            End If

            If Trim(txtAPS1.Text) <> "" Then
                dailyEnt.setAps1(txtAPS1.Text)
            Else
                dailyEnt.setAps1(0)
            End If

            If Trim(txtAPS2.Text) <> "" Then
                dailyEnt.setAps2(txtAPS2.Text)
            Else
                dailyEnt.setAps2(0)
            End If

            If Trim(txtAPS3.Text) <> "" Then
                dailyEnt.setAps3(txtAPS3.Text)
            Else
                dailyEnt.setAps3(0)
            End If

            If Trim(txtAPS4.Text) <> "" Then
                dailyEnt.setAps4(txtAPS4.Text)
            Else
                dailyEnt.setAps4(0)
            End If

            If Trim(txtAPS5.Text) <> "" Then
                dailyEnt.setAps5(txtAPS5.Text)
            Else
                dailyEnt.setAps5(0)
            End If

            If Trim(txtAPS6.Text) <> "" Then
                dailyEnt.setAps6(txtAPS6.Text)
            Else
                dailyEnt.setAps6(0)
            End If

            If Trim(txtCarCash.Text) <> "" Then
                dailyEnt.setSeasonCarCash(txtCarCash.Text)
            Else
                dailyEnt.setSeasonCarCash(0)
            End If

            If Trim(txtCarChq.Text) <> "" Then
                dailyEnt.setSeasonCarCheque(txtCarChq.Text)
            Else
                dailyEnt.setSeasonCarCheque(0)
            End If

            If Trim(txtCarCreditCard.Text) <> "" Then
                dailyEnt.setSeasonCarCreditCard(txtCarCreditCard.Text)
            Else
                dailyEnt.setSeasonCarCreditCard(0)
            End If

            If Trim(txtMotorCash.Text) <> "" Then
                dailyEnt.setSeasonMotorcycleCash(txtMotorCash.Text)
            Else
                dailyEnt.setSeasonMotorcycleCash(0)
            End If

            If Trim(txtMotorChq.Text) <> "" Then
                dailyEnt.setSeasonMotorcycleCheque(txtMotorChq.Text)
            Else
                dailyEnt.setSeasonMotorcycleCheque(0)
            End If

            If Trim(txtMotorCreditCard.Text) <> "" Then
                dailyEnt.setSeasonMotorcycleCreditCard(txtMotorCreditCard.Text)
            Else
                dailyEnt.setSeasonMotorcycleCreditCard(0)
            End If

            If Trim(txtDepositCar.Text) <> "" Then
                dailyEnt.setDepositCar(txtDepositCar.Text)
            Else
                dailyEnt.setDepositCar(0)
            End If

            If Trim(txtDepositMotor.Text) <> "" Then
                dailyEnt.setDepositMotorcycle(txtDepositMotor.Text)
            Else
                dailyEnt.setDepositMotorcycle(0)
            End If

            If Trim(txtDepositOther.Text) <> "" Then
                dailyEnt.setDepositOther(txtDepositOther.Text)
            Else
                dailyEnt.setDepositOther(0)
            End If

            If Trim(txtClamp.Text) <> "" Then
                dailyEnt.setClamp(txtClamp.Text)
            Else
                dailyEnt.setClamp(0)
            End If

            If Trim(txtTransponder.Text) <> "" Then
                dailyEnt.setTransponderSticker(txtTransponder.Text)
            Else
                dailyEnt.setTransponderSticker(0)
            End If

            If Trim(txtNoPlate.Text) <> "" Then
                dailyEnt.setNumberPlate(txtNoPlate.Text)
            Else
                dailyEnt.setNumberPlate(0)
            End If

            If Trim(txtTemporaryRental.Text) <> "" Then
                dailyEnt.setTemporaryRental(txtTemporaryRental.Text)
            Else
                dailyEnt.setTemporaryRental(0)
            End If

            If Trim(txtMisc.Text) <> "" Then
                dailyEnt.setMisc(txtMisc.Text)
            Else
                dailyEnt.setMisc(0)
            End If

            If Trim(txtEarlyBird.Text) <> "" Then
                dailyEnt.setInfoEarlyBird(txtEarlyBird.Text)
            Else
                dailyEnt.setInfoEarlyBird(0)
            End If

            If Trim(txtComplimentaryTicket.Text) <> "" Then
                dailyEnt.setInfoComplimentaryTicket(txtComplimentaryTicket.Text)
            Else
                dailyEnt.setInfoComplimentaryTicket(0)
            End If

            If Trim(txtManualRaised.Text) <> "" Then
                dailyEnt.setInfoManualRaised(txtManualRaised.Text)
            Else
                dailyEnt.setInfoManualRaised(0)
            End If

            If Trim(txtOutstandingTicket.Text) <> "" Then
                dailyEnt.setInfoOSTicket(txtOutstandingTicket.Text)
            Else
                dailyEnt.setInfoOSTicket(0)
            End If

            If Trim(txtInfoOthers.Text) <> "" Then
                dailyEnt.setInfoOther(txtInfoOthers.Text)
            Else
                dailyEnt.setInfoOther(0)
            End If

            dailyEnt.setLocationInfoId(ddLocation.SelectedValue)
            dailyEnt.setLastUpdatedDatetime(Now)
            dailyEnt.setLastUpdatedBy(lp.getUserMstrId)
            dailyDao.updateDB(dailyEnt, cn, trans)
            trans.Commit()
            clear()
            lblmsg.Text = ConstantGlobal.Record_Updated

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Message", "doSum();", True)
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)


        txtCashierShift1.Attributes.Add("onkeyup", "javascript:sumCashierTotal();")
        txtCashierShift2.Attributes.Add("onkeyup", "javascript:sumCashierTotal();")
        txtCashierShift3.Attributes.Add("onkeyup", "javascript:sumCashierTotal();")

        txtValet1.Attributes.Add("onkeyup", "javascript:sumValetTotal();")
        txtValet2.Attributes.Add("onkeyup", "javascript:sumValetTotal();")
        txtValet3.Attributes.Add("onkeyup", "javascript:sumValetTotal();")

        txtMotorcycle1.Attributes.Add("onkeyup", "javascript:sumMotorcycleTotal();")
        txtMotorcycle2.Attributes.Add("onkeyup", "javascript:sumMotorcycleTotal();")
        txtMotorcycle3.Attributes.Add("onkeyup", "javascript:sumMotorcycleTotal();")

        txtAPS1.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")
        txtAPS2.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")
        txtAPS3.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")
        txtAPS4.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")
        txtAPS5.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")
        txtAPS6.Attributes.Add("onkeyup", "javascript:sumAPSTotal();")

        txtCarCash.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")
        txtCarChq.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")
        txtCarCreditCard.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")
        txtMotorCash.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")
        txtMotorChq.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")
        txtMotorCreditCard.Attributes.Add("onkeyup", "javascript:sumSeasonTotal();")

        txtDepositCar.Attributes.Add("onkeyup", "javascript:sumTotalDeposit();")
        txtDepositMotor.Attributes.Add("onkeyup", "javascript:sumTotalDeposit();")
        txtDepositOther.Attributes.Add("onkeyup", "javascript:sumTotalDeposit();")

        txtClamp.Attributes.Add("onkeyup", "javascript:sumOtherTotal();")
        txtTransponder.Attributes.Add("onkeyup", "javascript:sumOtherTotal();")
        txtNoPlate.Attributes.Add("onkeyup", "javascript:sumOtherTotal();")
        txtMisc.Attributes.Add("onkeyup", "javascript:sumOtherTotal();")
        txtTemporaryRental.Attributes.Add("onkeyup", "javascript:sumOtherTotal();")

        txtEarlyBird.Attributes.Add("onkeyup", "javascript:sumInfoTotal();")
        txtComplimentaryTicket.Attributes.Add("onkeyup", "javascript:sumInfoTotal();")
        txtManualRaised.Attributes.Add("onkeyup", "javascript:sumInfoTotal();")
        txtOutstandingTicket.Attributes.Add("onkeyup", "javascript:sumInfoTotal();")
        txtInfoOthers.Attributes.Add("onkeyup", "javascript:sumInfoTotal();")

        MyBase.Render(writer)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdatePage", "onUpdating();", True)
        bindData()
    End Sub

    Private Sub bindData()
        Dim searchModel As New dailyCollectionSearchModel
        Dim dcDao As New CPM.DailyCollectionDAO
        Dim sqlmap As New SQLMap
        Dim strSQL As String
        Dim dsDailyCollection As New DataTable

        Try

            If Trim(txtDate.Text) = "" Then
                lblmsg.Text = "Date is a mandatory field."
                Exit Sub
            End If

            If ddLocation.SelectedIndex = 0 Then
                lblmsg.Text = "Location is a mandatory field."
                Exit Sub
            End If


            searchModel.setLocationInfoId(ddLocation.SelectedValue)

            If Trim(txtDate.Text) <> "" Then
                searchModel.setTransactionDate(Format(CDate(txtDate.Text), "dd/MM/yyyy"))
            End If


            strSQL = sqlmap.getMappedStatement("DailyCollection/Update-DailyCollection", searchModel)

            ViewState("strSQL") = strSQL

            dsDailyCollection = dm.execTable(strSQL)

            If dsDailyCollection.Rows.Count = 0 Then
                clear()
                lblmsg.Text = ConstantGlobal.No_Record_Found
                ddLocation.Enabled = True
                txtDate.Enabled = True
            Else
                lblmsg.Text = dsDailyCollection.Rows.Count.ToString + " " + "Record Found"
                ddLocation.Enabled = False
                txtDate.Enabled = False
                With dsDailyCollection
                    txtCashierShift1.Text = .Rows(0).Item(dcDao.COLUMN_Cashier1)
                    txtCashierShift2.Text = .Rows(0).Item(dcDao.COLUMN_Cashier2)
                    txtCashierShift3.Text = .Rows(0).Item(dcDao.COLUMN_Cashier3)
                    txtValet1.Text = .Rows(0).Item(dcDao.COLUMN_ValetService1)
                    txtValet2.Text = .Rows(0).Item(dcDao.COLUMN_ValetService2)
                    txtValet3.Text = .Rows(0).Item(dcDao.COLUMN_ValetService3)
                    txtMotorcycle1.Text = .Rows(0).Item(dcDao.COLUMN_Motorcycle1)
                    txtMotorcycle2.Text = .Rows(0).Item(dcDao.COLUMN_Motorcycle2)
                    txtMotorcycle3.Text = .Rows(0).Item(dcDao.COLUMN_Motorcycle3)
                    txtAPS1.Text = .Rows(0).Item(dcDao.COLUMN_Aps1)
                    txtAPS2.Text = .Rows(0).Item(dcDao.COLUMN_Aps2)
                    txtAPS3.Text = .Rows(0).Item(dcDao.COLUMN_Aps3)
                    txtAPS4.Text = .Rows(0).Item(dcDao.COLUMN_Aps4)
                    txtAPS5.Text = .Rows(0).Item(dcDao.COLUMN_Aps5)
                    txtAPS6.Text = .Rows(0).Item(dcDao.COLUMN_Aps6)
                    txtCarCash.Text = .Rows(0).Item(dcDao.COLUMN_SeasonCarCash)
                    txtCarChq.Text = .Rows(0).Item(dcDao.COLUMN_SeasonCarCheque)
                    txtCarCreditCard.Text = .Rows(0).Item(dcDao.COLUMN_SeasonCarCreditCard)
                    txtMotorCash.Text = .Rows(0).Item(dcDao.COLUMN_SeasonMotorcycleCash)
                    txtMotorChq.Text = .Rows(0).Item(dcDao.COLUMN_SeasonMotorcycleCheque)
                    txtMotorCreditCard.Text = .Rows(0).Item(dcDao.COLUMN_SeasonMotorcycleCreditCard)
                    txtDepositCar.Text = .Rows(0).Item(dcDao.COLUMN_DepositCar)
                    txtDepositMotor.Text = .Rows(0).Item(dcDao.COLUMN_DepositMotorcycle)
                    txtDepositOther.Text = .Rows(0).Item(dcDao.COLUMN_DepositOther)
                    txtClamp.Text = .Rows(0).Item(dcDao.COLUMN_Clamp)
                    txtTransponder.Text = .Rows(0).Item(dcDao.COLUMN_TransponderSticker)
                    txtNoPlate.Text = .Rows(0).Item(dcDao.COLUMN_NumberPlate)
                    txtTemporaryRental.Text = .Rows(0).Item(dcDao.COLUMN_TemporaryRental)
                    txtMisc.Text = .Rows(0).Item(dcDao.COLUMN_Misc)
                    txtEarlyBird.Text = .Rows(0).Item(dcDao.COLUMN_InfoEarlyBird)
                    txtComplimentaryTicket.Text = .Rows(0).Item(dcDao.COLUMN_InfoComplimentaryTicket)
                    txtManualRaised.Text = .Rows(0).Item(dcDao.COLUMN_InfoManualRaised)
                    txtOutstandingTicket.Text = .Rows(0).Item(dcDao.COLUMN_InfoOSTicket)
                    txtInfoOthers.Text = .Rows(0).Item(dcDao.COLUMN_InfoOther)
                End With



                hidDailyCollectionId.Value = dsDailyCollection.Rows(0).Item(dcDao.COLUMN_DailyCollectionID)


            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub
End Class
