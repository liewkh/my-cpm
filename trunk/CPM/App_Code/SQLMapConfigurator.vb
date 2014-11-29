Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Xml.XPath
Imports System.Reflection


    Public Class SQLMapConfigurator
        Public Shared Function configure()
            Dim pConfigDoc As New XmlDocument
            Dim pConfigNavigator As XPathNavigator
        Dim pConfigIterator As XPathNodeIterator
        Dim lookup As New SQLMapLookup
        Dim sql As String = ""
            Dim root As String = System.Configuration.ConfigurationManager.AppSettings("sqlmappath")

            ' Load SqlMapConfig.xml
            pConfigDoc.Load(root + "sqlmaps\SQLMapConfig.xml")
            pConfigNavigator = pConfigDoc.CreateNavigator
            pConfigIterator = pConfigNavigator.Select("sql-map-config/sql-map")
            While pConfigIterator.MoveNext()
                lookup.add(pConfigIterator.Current.GetAttribute("name", "") _
                , root + pConfigIterator.Current.GetAttribute("resource", ""))
            End While
            Return lookup
        End Function
    End Class


