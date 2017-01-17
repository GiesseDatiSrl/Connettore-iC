Imports System.Data
Imports System
Imports System.Net
Imports RestSharpApex
Imports NewtonsoftApex


Public Class UpdateRelease

    Public Shared Sub SendVersion(ByVal CodProgetto As String, ByVal VersioneConnettore As String, ByVal VersioneGestionale As String)

        Certificates.IgnoreBadCertificates()

        ' test
        'Dim wsUrl As String = "http://test.apexnet.it/licenseManagerAPI/v1/update_versione_connettore"
        ' prod
        Dim wsUrl As String = "http://lm.apexnet.it/lmAPI/v1/update_versione_connettore"

        Dim client As RestClient = New RestClient(wsUrl)

        Dim request1 As RestRequest = New RestRequest(Method.POST)

        request1.RequestFormat = DataFormat.Json

        request1.AddBody(New With { _
          Key .CodiceProgetto = CodProgetto, _
          Key .VersioneConnettore = VersioneConnettore, _
          Key .VersioneGestionale = VersioneGestionale _
              })

        Dim response1 As IRestResponse = client.Execute(request1)

    End Sub


End Class
