
Partial Class genericerror
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        'Dim ex As Exception = Server.GetLastError
        'Dim err, errMessage, errSource, errStack As String
        'If Not ex Is Nothing Then
        '    If Not ex.InnerException Is Nothing Then
        '        err = ex.Message
        '        errMessage = ex.InnerException.Message
        '        errSource = ex.InnerException.Source
        '        errStack = ex.InnerException.StackTrace
        '    End If
        'End If
        'Server.ClearError()
    End Sub


    Sub Page_Error(ByVal src As Object, ByVal args As EventArgs) Handles MyBase.Error
        Dim e As System.Exception = Server.GetLastError()
        Trace.Write("Message", e.Message)
        Trace.Write("Source", e.Source)
        Trace.Write("Stack Trace", e.StackTrace)
        Response.Write("Sorry, an error was encountered.")
        Context.ClearError()
    End Sub
End Class
