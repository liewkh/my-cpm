Imports Microsoft.VisualBasic

Public Class SQLMapLookup
    Private nameMap As New Collection
    Public Sub add(ByVal key As String, ByVal value As String)
        nameMap.Add(value, key)
    End Sub
    Public Function lookup(ByVal key As String)
        Return nameMap.Item(key)
    End Function
End Class



