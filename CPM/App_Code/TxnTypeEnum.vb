Imports Microsoft.VisualBasic

Public Class TxnTypeEnum

    Public Const CREDITNOTE As String = "CN"
    Public Const DEBITNOTE As String = "DN"
    Public Const INVOICE As String = "I"
    Public Const RECEIPT As String = "R"

    'Usage for xRef in the DebtorAccountDetail to identify the transaction type from where
    Public Const INVOICEENTRYSEASON As String = "1"
    Public Const INVOICEENTRYSEASONDEPOSIT As String = "2"
    Public Const MANUALTAXINVOICE As String = "3"
    Public Const INVOICEENTRYGST As String = "4"
    Public Const MANUALTAXINVOICEGST As String = "5"
    Public Const DEBITNOTESR As String = "6"
    Public Const DEBITNOTESRGST As String = "7"
    Public Const DEBITNOTEOS As String = "8"
    Public Const DEBITNOTEZRL As String = "9"



End Class
