Imports System
Imports System.Net
Imports System.Xml.XPath
Imports System.IO
Imports System.Text

''' <summary>
'''  Push Notification
''' </summary>
''' <remarks></remarks>
Public Class PushNotification

    Public Shared Function Send(ByVal CodAgente As String, ByRef CodProgetto As String, ByRef Messaggio As String) As Boolean
        Try

            ' Token di autorizzazione
            ' String iCommerceAuthToken = "A297B0AD-EE75-4A6C-B11A-92632D1452B4";
            ' String iGammaAuthToken = "3892DB2C-53A9-4B57-9638-08C1E91319C6";
            ' String iBAuthToken = "3892DB2C-53A9-4B57-9638-08C1E91319C8";

            Certificates.IgnoreBadCertificates()


            Dim ApexNotificationService As String = "http://lm.apexnet.it/lmAPI/v1/notifica_push_send"
            Dim content As String = "{" & _
                           "  ""CodiceAgente"":""|1"", " & _
                           "  ""CodiceProgetto"":""|2"", " & _
                           "  ""CodiceOperatore"":""|4"", " & _
                           "  ""Messaggio"":""|3"" " & _
                        "}"

            'content = content.Replace("|1", "307")
            'content = content.Replace("|2", "ib.appstore")
            'content = content.Replace("|3", "Buona giornata dal team iB")

            content = content.Replace("|1", CodAgente)
            content = content.Replace("|2", CodProgetto)
            content = content.Replace("|3", Messaggio)


            Dim urlEncoded As String = content
            ' HttpUtility.UrlEncode(content);
            Dim encodedRequest As Byte() = New ASCIIEncoding().GetBytes(urlEncoded)

            'Console.WriteLine("Creating POST request")

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(ApexNotificationService), HttpWebRequest)

            request.Method = "POST"
            request.Accept = "*/*"
            request.ContentType = "application/json"

            request.UserAgent = "Custom REST client v1.0"
            request.ContentLength = encodedRequest.Length

            Dim reqStream As Stream = request.GetRequestStream()

            'Console.WriteLine("Sending PUT request")

            reqStream.Write(encodedRequest, 0, encodedRequest.Length)
            reqStream.Flush()
            reqStream.Close()

            'Console.WriteLine("Reading PUT response")

            Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim streamReader As New StreamReader(responseStream)
            Dim responseContent As Char() = New Char(255) {}
            streamReader.Read(responseContent, 0, CInt(responseContent.Length))


            Return True

            'Console.WriteLine(responseContent)
        Catch ex As Exception
            Throw (New Exception("PushNotification.Send:" + ex.Message))
            Return False
        End Try

    End Function

End Class
