Imports System.Net
Imports System.IO
Imports System.Data
Imports Ionic

'Imports NTSInformatica.CLN__STD

Public Class Download
    Public Event AmountDownloadedChanged(ByVal iNewProgress As Long)
    Public Event FileDownloadSizeObtained(ByVal iFileSize As Long)
    Public Event FileDownloadComplete()
    Public Event FileDownloadFailed(ByVal ex As Exception)

    Private filesize As Long

    'Vars in the dialog
    Public vars_unzipdir As String
    Public vars_url_update As String = ""
    Public vars_url_version As String = ""
    Public vars_local_version As String = ""
    Public vars_newVersion As String = ""
    Public vars_unzipped_folder As String = ""

    Private mCurrentFile As String = String.Empty

    Public Function DownloadAndUnzip() As Boolean
        ' Se chiamo il download senza parametri, assume che:
        ' 1. La cartella in cui estrarre i files e' quella temporanea di Windows
        ' 2. L'url da cui prelevare i files e' quelle di ib (caso non gestito)

        vars_url_update = "http://lm.apexnet.it/iBUpdate/iBUpdate.zip"
        ' vars_url_version = "http://lm.apexnet.it/iBUpdate/iBUpdate.txt"

        vars_unzipdir = Path.GetTempPath

        Return DownloadAndUnzip(vars_url_update, vars_unzipdir)

    End Function

    Public Function DownloadAndUnzip(ByVal urlToDownload As String, ByVal unzipdir As String) As Boolean
        Try

            Dim tempName As String = Path.GetTempPath + "ib-" + vars_newVersion + "-" + Path.GetRandomFileName()
            Dim localZipFile As String = tempName + ".zip"
            Dim localZipFolder As String = tempName

            DownloadFileWithProgress(urlToDownload, localZipFile)

            Dim a As New Zip.ZipFile(localZipFile)
            'for each loop for every file in zip
            For Each zipfile As Zip.ZipEntry In a
                'If this file already exist, delete it
                If My.Computer.FileSystem.FileExists(localZipFolder) Then
                    My.Computer.FileSystem.DeleteFile(localZipFolder)
                End If
                zipfile.Extract(localZipFolder) 'Extract this file
            Next
            vars_unzipped_folder = localZipFolder
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return Nothing
    End Function

    Public Function NewVersionAvailable() As Boolean
        Try
            Dim UrlVersionUpdate As String = vars_url_version
            Dim CurrentVersion As String = vars_local_version

            Dim tokenNewVersion() As String = Nothing
            Dim newVersionString As String = ""

            Dim tokenMyVersion() As String = Nothing
            Dim myVersionString As String = ""

            Dim newver_with_dot As String = ""

            Dim client As WebClient = New System.Net.WebClient() 'Create new var to get newest version available

            newver_with_dot = client.DownloadString(UrlVersionUpdate)
            tokenNewVersion = newver_with_dot.Split("."c)

            newVersionString = tokenNewVersion(0).PadLeft(1, "0"c) + tokenNewVersion(1).PadLeft(2, "0"c) + tokenNewVersion(2).PadLeft(2, "0"c) + tokenNewVersion(3).PadLeft(3, "0"c)

            tokenMyVersion = CurrentVersion.Split("."c)
            myVersionString = tokenMyVersion(0).PadLeft(1, "0"c) + tokenMyVersion(1).PadLeft(2, "0"c) + tokenMyVersion(2).PadLeft(2, "0"c) + tokenMyVersion(3).PadLeft(3, "0"c)


            Dim newVersion As Integer = CInt(newVersionString)
            Dim myVersion As Integer = CInt(myVersionString)

            If CLng(newVersion) > CLng(myVersion) Then 'Check if newest version is > than installed version
                vars_newVersion = newver_with_dot
                Return True 'If so, return true
            Else
                Return False 'Otherwise, return false
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region "Internal Methods"
    Private ReadOnly Property CurrentFile() As String
        Get
            Return mCurrentFile
        End Get
    End Property


    Private Function DownloadFile(ByVal URL As String, ByVal Location As String) As Boolean
        Try
            mCurrentFile = GetFileName(URL)
            Dim WC As New WebClient
            WC.DownloadFile(URL, Location)
            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function

    Private Function GetFileName(ByVal URL As String) As String
        Try
            Return URL.Substring(URL.LastIndexOf("/") + 1)
        Catch ex As Exception
            Return URL
        End Try
    End Function

    Private Function DownloadFileWithProgress(ByVal URL As String, ByVal Location As String) As Boolean
        Dim FS As FileStream = Nothing

        Try
            mCurrentFile = GetFileName(URL)
            Dim wRemote As WebRequest
            Dim bBuffer As Byte()
            ReDim bBuffer(256)
            Dim iBytesRead As Integer
            Dim iTotalBytesRead As Integer

            Certificates.IgnoreBadCertificates()

            FS = New FileStream(Location, FileMode.Create, FileAccess.Write)
            wRemote = WebRequest.Create(URL)
            Dim myWebResponse As WebResponse = wRemote.GetResponse
            RaiseEvent FileDownloadSizeObtained(myWebResponse.ContentLength)
            Dim sChunks As Stream = myWebResponse.GetResponseStream
            Do
                iBytesRead = sChunks.Read(bBuffer, 0, 256)
                FS.Write(bBuffer, 0, iBytesRead)
                iTotalBytesRead += iBytesRead
                If myWebResponse.ContentLength < iTotalBytesRead Then
                    RaiseEvent AmountDownloadedChanged(myWebResponse.ContentLength)
                Else
                    RaiseEvent AmountDownloadedChanged(iTotalBytesRead)
                End If
            Loop While Not iBytesRead = 0
            sChunks.Close()
            FS.Close()
            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            If Not (FS Is Nothing) Then
                FS.Close()
                FS = Nothing
            End If
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function
#End Region

End Class
