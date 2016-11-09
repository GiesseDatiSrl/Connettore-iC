Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.IO
Imports System.Configuration
Imports System.Reflection
Imports System.Threading.Thread
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data

Public Class FRMBUSNET

  '************** RICORDARSI DI COMPILARE SEMPRE CON L'OPZIONE CPU x86, ALTRIMENTI NON GIRA LA CREAZIONE DEGLI ACTIVEX DI BUSINESS SU 64 BIT!!! ***********  compila->opzioni di compilazione avanzate->cpu di destinazione=x86

  'quando si cambia l'icona di business deve essere fatto anche in bn__Chil e bn__std come icona collegata alla form!!!!

  'per mutex
  'Costanti nome dei MUTEX
  Public Const bsMTXBus = "NtsBusinessMutex" 'BUSINESS, BUSBATCH
  Public Const bsMTXUtl = "NtsBusUtilMutex" 'BUSADMINTOOLS
  Public Const bsMTXSet = "NtsBusSetupMutex" 'BUSSETUP
  Public Const bsMTXAgg = "NtsBusAggMutex" 'BUSAGG
  Public Const bsMTXSrv = "NtsBusServMutex" 'BUSSERV

  'Variabili pubbliche con gli handle dei mutex creati
  Public glNtsMutexGlobal As Integer
  Public glNtsMutexLocal As Integer
  Public bBatch As Boolean = False
  Public DirP As String = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
  'Public PrcConn As String = ""
  'Public GetSettingRegPerUser As Integer = 9
  Public strParam As String = ""
  Public strBusAggExe As String = "BusAgg64.exe"

  'Per controllo dell'applicazione in esecuzione attraverso Mutex globali (semaforo del s.o.)
  Private Declare Function CreateMutex Lib "kernel32" Alias "CreateMutexA" (ByVal lpMutexAttributes As Integer, ByVal bInitialOwner As Integer, ByVal lpName As String) As Integer
  Private Declare Function OpenMutex Lib "kernel32" Alias "OpenMutexA" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal lpName As String) As Integer
  Private Declare Function ReleaseMutex Lib "kernel32" (ByVal hMutex As Integer) As Integer
  Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
  Private Declare Function SetSecurityDescriptorDacl Lib "advapi32.dll" (ByVal pSecurityDescriptor As IntPtr, ByVal bDaclPresent As Integer, ByVal pDacl As Byte, ByVal bDaclDefaulted As Integer) As Integer
  Private Declare Function InitializeSecurityDescriptor Lib "advapi32.dll" (ByVal pSecurityDescriptor As IntPtr, ByVal dwRevision As Integer) As Integer
  Private Declare Function FormatMessage Lib "kernel32" Alias "FormatMessageA" (ByVal dwFlags As Long, ByVal lpSource As Long, ByVal dwMessageId As Long, ByVal dwLanguageId As Long, ByVal lpBuffer As String, ByVal nSize As Long, ByVal Args As Long) As Long

  Private Const MAX_PATH As Long = 260
  Private Const LB_SETTABSTOPS As Long = &H192
  Private Const FORMAT_MESSAGE_FROM_SYSTEM As Long = &H1000
  Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Long = &H200
  Private Const FORMAT_MESSAGE_MAX_WIDTH_MASK As Long = &HFF
  Private Const FORMAT_MESSAGE_ARGUMENT_ARRAY As Long = &H2000


  Structure SECURITY_ATTRIBUTES
    Dim nLength As Integer
    Dim lpSecurityDescriptor As Integer
    Dim bInheritHandle As Integer
  End Structure
  Private Structure ACL
    Dim AclRevision As Byte
    Dim Sbz1 As Byte
    Dim AclSize As Integer
    Dim AceCount As Integer
    Dim Sbz2 As Integer
  End Structure
  Private Structure SECURITY_DESCRIPTOR
    Dim Revision As Byte
    Dim Sbz1 As Byte
    Dim Control As Integer
    Dim Owner As Integer
    Dim Group As Integer
    Dim sACL As ACL
    Dim Dacl As ACL
  End Structure
  Private Const SECURITY_DESCRIPTOR_REVISION = 1
  Private Const ERROR_ALREADY_EXISTS = 183&
  Private Const STANDARD_RIGHTS_REQUIRED = &HF0000
  Private Const SYNCHRONIZE = &H100000
  Private Const MUTANT_QUERY_STATE = &H1
  Private Const MUTEX_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or MUTANT_QUERY_STATE

  Const LOGON_WITH_PROFILE As Integer = &H1
  Const CREATE_DEFAULT_ERROR_MODE As Integer = &H4000000

  Public Structure PROCESS_INFO
    Public hProcess As IntPtr
    Public hThread As IntPtr
    Public dwProcessId As Integer
    Public dwThreadId As Integer
  End Structure
  Public Structure STARTUP_INFO
    Public cb As Integer
    Public lpReserved As Integer
    <MarshalAs(UnmanagedType.LPTStr)> Public lpDesktop As String
    <MarshalAs(UnmanagedType.LPTStr)> Public lpTitle As String
    Public dwX As Long
    Public dwY As Integer
    Public dwXSize As Integer
    Public dwYSize As Integer
    Public dwXCountChars As Integer
    Public dwYCountChars As Integer
    Public dwFillAttribute As Integer
    Public dwFlags As Integer
    Public wShowWindow As Short
    Public cbReserved2 As Short
    Public lpReserved2 As Integer
    Public hStdInput As Integer
    Public hStdOutput As Integer
    Public hStdError As Integer
  End Structure
  <DllImport("advapi32.dll")> _
  Public Shared Function CreateProcessWithLogonW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpUsername As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpDomain As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpPassword As String, _
                                          ByVal dwLogonFlags As Integer, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpApplicationName As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpCommandLine As String, _
                                          ByVal lpCreationFlags As Integer, _
                                          ByVal lpVoid As Integer, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpCurrentDirectory As String, _
                                          ByRef lpStartupInfo As STARTUP_INFO, _
                                          ByRef lpProcessInfo As PROCESS_INFO) As Integer
  End Function
  Private Declare Function GetLastError Lib "kernel32" () As Long

  Private oStd As Object = Nothing

  Private Sub fmBusnet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    'Dim strTmp As String = ""
    Dim strDir As String
    Dim i As Integer
    Dim bDisableMutex As Boolean = False
    Dim bExists As Boolean = False
    Dim strProfilo As String = ""

    Me.Height = 0
    Me.Width = 0

    'strDir = Directory.GetCurrentDirectory
    strDir = Application.StartupPath

    If My.Application.CommandLineArgs.Count > 0 Then
      For i = 0 To My.Application.CommandLineArgs.Count - 1
        strParam = strParam & My.Application.CommandLineArgs(i).ToString.Replace(" ", "*§§§*§§§*") & " "
      Next
    End If
    If strParam = "" Then
      'a volte, in debug, non legge la riga di comando ...
      strParam = ". . . Business"
    Else
      strParam = Trim(strParam)
    End If


    '*************************************************************
    'carico BN__STD
    Try
      Dim assem1 As System.Reflection.Assembly
      Dim tpChild1 As Type

      assem1 = System.Reflection.Assembly.LoadFrom(strDir & "\BN__STD.DLL")
      tpChild1 = assem1.GetType("NTSInformatica.CLN__STD", True, True)
      oStd = Activator.CreateInstance(tpChild1)

    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error starting Bus.net: " & ex.Message, MsgBoxStyle.Critical, "Messaggio di Business NET")
      Else
        WriteMsgBoxToLog("Error starting Bus.net: " & ex.ToString, True)
      End If
      Me.Close()
      Return
    End Try





    'imposto la dir in bn__std
    strProfilo = CheckBusProfilo(Command, False)
    DirP = oStd.GetSettingReg("Business", strProfilo, "Dir", "")
    If DirP = "*-*" Then
      'non è stato trovato il file .cfg o la stringa di connessione è sbagliata: chiudo tutto
      Me.Close()
      Return
    End If
    If Not System.IO.Directory.Exists(DirP) Then
      DirP = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    End If

    If strParam.ToUpper.IndexOf(" /B ") > -1 Then
      bBatch = True
      WriteMsgBoxToLog("Busness NET Avviato con la seguente riga di comando: " & strParam, False)
    End If

    '*************************************************************
    'PER PRIMA COSA IMPOSTO LA DATA NEL FORMAT OCON ANNO A 4 CIFRE, DIVERSAMENTE SE INSERISCO UNA DATA NEL 
    'FORMATO 31/12/2099 VIENE TRADOTTA IN 31/12/1999 QUANDO SI FA .toshortdatestring!!!!!
    Try
      Dim strDateFormat As String = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
      If strDateFormat.IndexOf("yyyy") = -1 Then
        strDateFormat = strDateFormat.Replace("yy", "yyyy")
        Dim oCulture As CultureInfo = New CultureInfo(CurrentThread.CurrentCulture.Name)
        oCulture.DateTimeFormat.ShortDatePattern = strDateFormat
        CurrentThread.CurrentCulture = oCulture
        'strTmp = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
      End If
    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error starting Bus.net during set DATETIME format: " & ex.ToString)
      Else
        WriteMsgBoxToLog("Error starting Bus.net during set DATETIME format: " & ex.ToString, True)
      End If
      Me.Close()
      Return
    End Try

    '*************************************************************
    'preparo il canale per la comunicazione dei REMOTING:
    'dentro ai vari entity ci sarà una riga del tipo quella commentata per avvisare che il dal non è in locale ma è in remoto
    ChannelServices.RegisterChannel(New TcpClientChannel(), False)
    'RemotingConfiguration.RegisterActivatedClientType(GetType(CLDORGSOR), "tcp://Mirto:1973")

    'Controlla se sono disabilitati i Mutex
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0
    bDisableMutex = CBool(Val(oStd.GetSettingReg("Business", "Debug", "DisableMutex", "0")))
    If Not bDisableMutex Then
      'Controlla che nonsiano in esecuzione programmi di utilità (BusAdminTools)
      If NtsMutexTrova(bsMTXUtl) Then
        If bBatch = False Then
          MsgBox("Il programma BusAdminTools.exe è in esecuzione. Chiuderlo prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net")
        Else
          WriteMsgBoxToLog("Il programma BusAdminTools.exe è in esecuzione. Chiuderlo prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
      'Controlla che non ci siano in esecuzione BUSAGG
      If NtsMutexTrova(bsMTXAgg) Then
        If bBatch = False Then
          MsgBox("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & strBusAggExe & " (Installazione aggiornamenti)" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net ")
        Else
          WriteMsgBoxToLog("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & strBusAggExe & " (Installazione aggiornamenti)" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
      'Controlla che non ci siano in esecuzione BUSSETUP
      If NtsMutexTrova(bsMTXSet) Then
        If bBatch = False Then
          MsgBox("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSSETUP.EXE (Installazione di Business) " & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net")
        Else
          WriteMsgBoxToLog("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSSETUP.EXE (Installazione di Business) " & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
    End If
    'esco se l'aggiornamento non deve partire in automatico all'avvio di Business
    If bBatch = False Then
      If oStd.GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusaggAutoUpdate", "N") <> "N" Then
        'lancio la routine che verifica se ci sono aggiornamenti da caricare prima di avviare Busiess
        If Not bDisableMutex Then
          'L'installazione aggiornamenti la fa partire solo se non è già in esecuzione Business
          If Not NtsMutexTrova(bsMTXBus) Then
            If Not CaricaAgg() Then
              Me.Close()
              Return
            End If
          End If
        Else
          If Not CaricaAgg() Then
            Me.Close()
            Return
          End If
        End If
      End If    'If strTmp <> "N" Then
    End If    'If bBatch = False Then

    If Not bDisableMutex Then
      'Crea il mutex per il controllo se l'applicazione è in esecuzione
      'DOPO AVER INSTALLATO GLI AGGIORNAMENTI
      NtsMutexCrea(bsMTXBus, bExists)
    End If

    '*************************************************************
    'carico l'oggetto MENU e lo avvio passandogli la riga di comando
    Try
      Dim assem As System.Reflection.Assembly
      Dim tpChild As Type
      Dim fmMenu As Object

      assem = System.Reflection.Assembly.LoadFrom(strDir & "\BN__MENU.DLL")
      tpChild = assem.GetType("NTSInformatica.FRMMENU", True, True)
      fmMenu = Activator.CreateInstance(tpChild)

      If Not fmMenu.InitMenu(strParam, strDir, Me, Nothing, DirP) Then
        Me.Close()
        Return
      End If

      'visualizzo il menu (non modale)
      fmMenu.show()
    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error starting Bus.net: " & ex.Message, MsgBoxStyle.Critical, "Messaggio di Business NET")
      Else
        WriteMsgBoxToLog("Error starting Bus.net: " & ex.ToString, True)
      End If
      Me.Close()
      Return
    End Try
  End Sub

  Private Sub fmBusnet_Unload(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    'Elimina il mutex che eventualmente ha creato all'avvio
    Try
      NtsMutexElimina()
    Catch ex As Exception

    End Try
  End Sub

  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMBUSNET))
    Me.SuspendLayout()
    '
    'FRMBUSNET
    '
    Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
    Me.ClientSize = New System.Drawing.Size(124, 32)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Location = New System.Drawing.Point(-1000, 0)
    Me.Name = "FRMBUSNET"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
    Me.Text = "Business NET"
    Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
    Me.ResumeLayout(False)

  End Sub


  Public Overridable Function CaricaAgg() As Boolean
    'carico gli aggiornamenti all'avvio di busnet
    'Dim objAgg As Object
    Dim strBusAggdir As String
    Dim strProfilo As String
    Dim bUserXXX As Boolean = False
    Dim strAggUser As String = ""
    Dim strAggPwd As String = ""
    Dim strDomain As String = ""
    Dim strUser As String = ""
    Dim nAggNumberClient As Integer = 0
    Dim nAggNumberServer As Integer = 0
    Dim i As Integer = 0
    'Dim s As New Security.SecureString

    Try
      strProfilo = CheckBusProfilo(Command, bUserXXX)

      'leggo le dir dove dovrebbero esserci gli aggiornamenti

      strBusAggdir = oStd.GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggDir", "")
      If strBusAggdir.Length > 0 Then
        If strBusAggdir.Substring(strBusAggdir.Length - 1) <> "\" Then strBusAggdir += "\"
      End If

      'versione di Business locale
      nAggNumberServer = Val(oStd.GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "AggNumber", "0"))

      'versione di Business sul server
      'Controlla se il Server è raggiungibile
      'e se esiste il file .txt dove è salvato il numero di aggiornamento nel Server
      nAggNumberClient = nAggNumberServer
      If System.IO.File.Exists(strBusAggdir & "Aggnumber.txt") = False Then
        MsgBox("Non esiste il file '" & strBusAggdir & "Aggnumber.txt'," & vbCrLf & "o il Server non è raggiungibile.", vbExclamation)
        Return True
      Else
        Dim r1 As New System.IO.StreamReader(strBusAggdir & "Aggnumber.txt")
        If Not r1.EndOfStream Then
          nAggNumberClient = Val(r1.ReadLine)
        End If
        r1.Close()
      End If

      'esco se le versioni sono uguali
      If nAggNumberClient = nAggNumberServer Then Return True

      '-----------------------------------------
      'devo eseguire l'aggiornamento: se posso carico la versione aggiornata di busagg.exe
      'da NET 2013 il busagg.exe + busagm.exe sono sempre nella cartella \\server\bus\agg!!!!
      strAggUser = oStd.GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggUser", "")
      strAggPwd = oStd.GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggPwd", "")
      If Not System.IO.File.Exists(strBusAggdir & strBusAggExe) Then
        MsgBox("Attenzione: non è possibile avviare la procedura di aggiornamento in quanto non è stato trovato il file '" & strBusAggdir & strBusAggExe & "'", vbExclamation)
        Return True
      End If
      'davide: non serve
      'If Not System.IO.File.Exists(strBusAggdir & "Busagm.exe") Then
      '  MsgBox("Attenzione: non è possibile avviare la procedura di aggiornamento in quanto non è stato trovato il file '" & strBusAggdir & "Busagm.exe'", vbExclamation)
      '  Return True
      'End If

      '----------------------------------
      'lancio Busagg.exe e chiudo Busnet.exe
      MsgBox("Attenzione: sul server è presente un aggiornamento di Business. Business NET verrà chiuso e verrà avviata la procedura di aggiornamento", MsgBoxStyle.Information)

      'vecchio sistema: quello precedente dava errrore quando da xp utente di tipo user devo sfogliare la rete per accedere ad aggnumber.txt, quando in realtà tutto l'ambiente è impostato correttamente
      'Dim a As New ProcessStartInfo()
      'Dim p As New Process()
      'Try
      '  a.FileName = strBusAggdir & "Busagg.exe"
      '  a.Arguments = strProfilo & " /B"
      '  a.WorkingDirectory = strBusDir
      '  a.LoadUserProfile = True
      '  a.UseShellExecute = False
      '  a.RedirectStandardOutput = True
      '  If strAggUser.Trim <> "" Then
      '    i = strAggUser.IndexOf("\")
      '    If i < 0 Then
      '      strDomain = ""
      '      strUser = strAggUser
      '    Else
      '      strDomain = strAggUser.Substring(0, i)
      '      strUser = strAggUser.Substring(i + 1)
      '    End If
      '    a.Domain = strDomain
      '    a.UserName = strUser

      '    PwdDeCript(strAggPwd, strAggPwd)

      '    For i = 0 To strAggPwd.Length - 1
      '      s.AppendChar(strAggPwd(i))
      '    Next
      '    a.Password = s.Copy

      '    If System.Environment.OSVersion.Version.Major >= 6 Then
      '      p.StartInfo = a      'in XP così da errore !!!! handle non valido
      '      p.Start()
      '    Else
      '      'specifico per windows XP
      '      Process.Start(strBusAggdir & "Busagg.exe", strProfilo & " /B", strUser, s.Copy, strDomain)
      '    End If
      '  Else
      '    If System.Environment.OSVersion.Version.Major >= 6 Then
      '      p.StartInfo = a      'in XP così da errore !!!! handle non valido
      '      p.Start()
      '    Else
      '      'specifico per windows XP: non posso fare il test per vista, visto che se ho lo uacattivo non sono mai un admin
      '      'se l'utente passatomi non è una admin do un messaggio e non faccio partire l'aggiornamento
      '      Dim user As New System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())
      '      If user.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator) = False Then
      '        MsgBox("Attenzione: dovrebbero essere caricati degli aggiornamenti di Business NET" & vbCrLf & _
      '               "ma l'utente di windows corrente non è un amministratore" & vbCrLf & _
      '               "e nelle proprietà degli aggiornamenti non è stato indicato" & vbCrLf & _
      '               "un utente di tipo administrator. Gli aggiornamenti non verranno caricati", MsgBoxStyle.Critical)
      '        Return True
      '      Else
      '        Process.Start(strBusAggdir & "Busagg.exe", strProfilo & " /B")
      '      End If
      '    End If
      '  End If    'If strAggUser.Trim <> "" Then

      '  Me.Close()      'chiudo: l'utente dovrà riavviare bus a mano dopo la fine degli aggiornamenti
      '  Return False    'in questo modo busnet viene chiuso...
      'Catch ex As Exception
      '  MsgBox("Errore in fase di avvio '" & strBusAggdir & "Busagg.exe" & "' per installazione aggiornamenti." & vbCrLf & "Gli aggiornamenti non verranno caricati." & vbCrLf & vbCrLf & vbCrLf & "Errore: " & vbCrLf & ex.ToString)
      '  Return True
      'End Try

      'nuovo sistema
      Dim strParam As String = "/FILEPROFILO=""" & Application.StartupPath & "\" & strProfilo & ".cfg" & """ /BUSNET /SILENT"
      Try
        Dim pSecurityAttributes As SECURITY_ATTRIBUTES
        pSecurityAttributes.bInheritHandle = True
        pSecurityAttributes.nLength = Marshal.SizeOf(pSecurityAttributes)
        pSecurityAttributes.lpSecurityDescriptor = IntPtr.Zero
        Dim pStartInfo As STARTUP_INFO = Nothing
        pStartInfo.cb = Len(pStartInfo)
        Dim pProcessInfo As PROCESS_INFO

        If strAggUser.Trim <> "" Then
          i = strAggUser.IndexOf("\")
          If i < 0 Then
            strDomain = ""
            strUser = strAggUser
          Else
            strDomain = strAggUser.Substring(0, i)
            strUser = strAggUser.Substring(i + 1)
          End If

          oStd.PwdDeCript(strAggPwd, strAggPwd)


          'avvio gli aggiornamenti con credenziali specificate
          Dim iReturn As Integer = CreateProcessWithLogonW(strUser, strDomain, strAggPwd, _
                                          LOGON_WITH_PROFILE, Nothing, _
                                          strBusAggdir & strBusAggExe & " " & strParam, _
                                          CREATE_DEFAULT_ERROR_MODE, Nothing, _
                                          strBusAggdir, pStartInfo, pProcessInfo)
          If iReturn = 0 Then
            Dim lLastError As Integer = GetLastError()
            Select Case lLastError
              Case 1326, 1783
                MsgBox("Errore in fase di avvio di avvio aggiornamenti:" & vbCrLf & "dominio, utente o password errati o non impostati (o utente non di tipo administrator). Errore: " & vbCrLf & GetLastError().ToString, vbExclamation, "Messaggio di Busagg")

              Case 2
                MsgBox("Errore in fase di avvio di avvio aggiornamenti:" & vbCrLf & " programma " & strBusAggdir & strBusAggExe & "' non trovato. Errore: " & vbCrLf & GetLastError().ToString, vbExclamation, "Messaggio di Busagg")

              Case Else
                Dim strMessaggio As String = Space(200)
                FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, 0&, lLastError, 0&, strMessaggio, 200, 0&)

                MsgBox("Errore in fase di avvio di avvio aggiornamenti:" & vbCrLf & " Errore: " & vbCrLf & lLastError.ToString & " - Messaggio:" & strMessaggio.Trim, vbExclamation, "Messaggio di Busagg")
            End Select
            Return True 'proseguo senza avviare gli aggiornamenti
          End If
        Else
          'non ho indicato credenziali: se l'utente non è un administrator verrà generato un errore non gestito
          Process.Start(strBusAggdir & strBusAggExe, strParam)
        End If    'If strAggUser.Trim <> "" Then

        Me.Close()      'chiudo: l'utente dovrà riavviare bus a mano dopo la fine degli aggiornamenti
        Return False    'in questo modo busnet viene chiuso...

      Catch ex As Exception
        MsgBox("Errore in fase di avvio '" & strBusAggdir & strBusAggExe & "' per installazione aggiornamenti." & vbCrLf & "Gli aggiornamenti non verranno caricati." & vbCrLf & vbCrLf & vbCrLf & "Errore: " & vbCrLf & ex.ToString)
        Return True
      End Try

    Catch ex As Exception
      MsgBox("Error starting Business NET - CaricaAgg: " & ex.ToString)
      Me.Close()
    End Try
  End Function

  Public Function CheckBusProfilo(ByVal strComando As String, ByRef bUserXXX As Boolean) As String
    'scompone la riga di comando in : opnome passwd codazi profilo
    Dim nPos As Integer
    Dim nPos1 As Integer
    Dim nPos2 As Integer
    Dim nPos3 As Integer
    Dim nPos4 As Integer
    Dim strC As String

    CheckBusProfilo = "Business"

    If Len(Trim(strComando)) = 0 Then Exit Function

    'Legge la linea di comando
    strC = strComando
    nPos = InStr(1, strC, " ")
    If nPos <> 0 Then
      nPos1 = InStr(nPos + 1, strC, " ")
      If Trim(Microsoft.VisualBasic.Left(strC, nPos - 1)).ToUpper = "XXX" Then bUserXXX = True
      'gstrOpnome = Trim(Left$(strC, nPos - 1))
    End If
    If nPos1 <> 0 Then
      nPos2 = InStr(nPos1 + 1, strC, " ")
      'gstrPasswd = Mid$(strC, nPos + 1, nPos1 - nPos - 1)
    End If
    If nPos2 <> 0 Then
      nPos3 = InStr(nPos2 + 1, strC, " ")
      'gstrCodazi = Mid$(strC, nPos1 + 1, nPos2 - nPos1 - 1)
    Else
      'gstrCodazi = Mid$(strC, nPos1 + 1)
    End If
    'Aggiunto : profilo
    If nPos3 <> 0 Then
      nPos4 = InStr(nPos3 + 1, strC, " ")
      CheckBusProfilo = Mid$(strC, nPos2 + 1, nPos3 - nPos2 - 1)
    Else
      If nPos2 <> 0 Then
        CheckBusProfilo = Mid$(strC, nPos2 + 1)
      End If
    End If
    'Aggiunto: programma
    'Aggiunto: restricted /R puo' esserci solo se c'e' programma,
    'Aggiunto: parametro/i con cui chiamare il programma, può esserci solo se c'è programma
    'NOTA:     se c'è restricted il/i parametro/i devono essere scritti dopo
    'Esempio: Roby . Prova business bs--paga /R Apri;3
    '  If nPos4 <> 0 Then
    '    gstrProgramma = Mid$(strC, nPos3 + 1, nPos4 - nPos3 - 1)
    '    nPos5 = InStr(nPos4 + 1, strC, " ")
    '    If nPos5 <> 0 Then
    '      gstrRestr = Mid$(strC, nPos4 + 1, nPos5 - nPos4 - 1)
    '      gstrParam = Mid$(strC, nPos5 + 1)
    '    Else
    '      strAppo = Mid$(strC, nPos4 + 1)
    '      If strAppo = "/R" Then
    '        gstrRestr = Mid$(strC, nPos4 + 1)
    '      Else
    '        gstrParam = Mid$(strC, nPos4 + 1)
    '      End If
    '    End If
    '  Else
    '    If nPos3 <> 0 Then
    '     gstrProgramma = Mid$(strC, nPos3 + 1)
    '    End If
    '  End If
    'Ogni campo con . viene considerato poi stringa vuota ...
    '  If gstrPasswd = "." Then gstrPasswd = ""
    '  If gstrOpnome = "." Then gstrOpnome = ""
    '  If gstrCodazi = "." Then gstrCodazi = ""
    If CheckBusProfilo = "." Or CheckBusProfilo = "" Then CheckBusProfilo = "Business"
    '  If gstrProgramma = "." Then gstrProgramma = ""
    '  If gstrRestr = "." Then gstrRestr = ""
    '  If gstrParam = "." Then gstrParam = ""
  End Function

#Region "Mutex"
  Public Function NtsMutexCrea(ByVal strMutexName As String, ByRef bExist As Boolean) As Boolean
    Dim SecurityDesc As New SECURITY_DESCRIPTOR
    Dim SecurityAttr As New SECURITY_ATTRIBUTES
    Dim lr As Long, lLastErr As Long

    NtsMutexCrea = False
    bExist = False
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0

    'Non crea il mutex se siamo in DEBUG con VB6
    If oStd.IsDebugMode() Then Return False

    Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(SecurityDesc))
    Marshal.StructureToPtr(SecurityDesc, ptr, False)

    lr = InitializeSecurityDescriptor(ptr, SECURITY_DESCRIPTOR_REVISION)
    lLastErr = Err.LastDllError
    If lr = 0 Then Return False

    lr = SetSecurityDescriptorDacl(ptr, 1, 0&, 0)
    lLastErr = Err.LastDllError
    If lr = 0 Then Return False

    SecurityAttr.lpSecurityDescriptor = ptr
    SecurityAttr.bInheritHandle = 1
    SecurityAttr.nLength = Len(CObj(SecurityAttr))

    'Crea il mutex Global\
    lr = CreateMutex(VarPtr(SecurityAttr), 1&, "Global\" & strMutexName)
    lLastErr = Err.LastDllError
    If lLastErr = ERROR_ALREADY_EXISTS Then
      bExist = True
    End If
    glNtsMutexGlobal = lr

    'Crea il mutex Local\
    lr = CreateMutex(VarPtr(SecurityAttr), 0&, strMutexName)
    lLastErr = Err.LastDllError
    glNtsMutexLocal = lr

    Return True
RoutineErr:
    'Non segnala nessun errore ed esce come se lo avesse creato (però non esistente)
    'MsgBox "Errore nella routine NtsMutexCrea: " & Err.Number & " - " & Err.Description & "(LastDllErro " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
    NtsMutexCrea = False
    bExist = False
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0

  End Function
  Public Function NtsMutexElimina() As Boolean
    NtsMutexElimina = False
    Try

      'Rilascia/chiude il mutex (aperto con CreateMutex) \Global
      If glNtsMutexGlobal <> 0 Then
        ReleaseMutex(glNtsMutexGlobal)
        CloseHandle(glNtsMutexGlobal)
        glNtsMutexGlobal = 0
      End If

      'Rilascia/chiude il mutex (aperto con CreateMutex) \Local
      If glNtsMutexLocal <> 0 Then
        ReleaseMutex(glNtsMutexLocal)
        CloseHandle(glNtsMutexLocal)
        glNtsMutexLocal = 0
      End If

      Return True
    Catch ex As Exception
      'Non segnala nessun errore ed esce come se NON avesse trovato il mutex da eliminare
      'Tanto viene comunque eliminato dal sistema operativo quando il processo viene chiuso
      'MsgBox "Errore nella routine NtsMutexElimina: " & Err.Number & " - " & Err.Description & "(LastDllError " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
      Return False
    End Try
  End Function
  Public Function NtsMutexTrova(ByVal strMutexName As String) As Boolean
    Try
      'Controlla il mutex Local\
      If OpenMutex(MUTEX_ALL_ACCESS, 0&, strMutexName) Then
        Return True
      End If

      'Controlla il mutex Global\
      If OpenMutex(MUTEX_ALL_ACCESS, 0&, "Global\" & strMutexName) Then
        Return True
      End If

      Return False
    Catch ex As Exception
      'Non segnala nessun errore ed esce come se NON avesse trovato il mutex
      'MsgBox "Errore nella routine TrovaNtsMutex: " & Err.Number & " - " & Err.Description & "(LastDllErro " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
      Return False
    End Try

  End Function

  Public Function VarPtr(ByVal e As Object) As Integer
    Dim GC As GCHandle = GCHandle.Alloc(e, GCHandleType.Pinned)
    Dim GC2 As Integer = GC.AddrOfPinnedObject.ToInt32
    GC.Free()
    Return GC2
  End Function
#End Region

  Public Sub WriteMsgBoxToLog(ByVal strMsg As String, ByVal bAccoda As Boolean)
    'se sono stato avviato in modalità batch (tipo busbatch) i messaggi vengono a finire tutti qui (eccetto quelli du busnet.exe)
    Dim lw1 As New StreamWriter(DirP & "\BusNetBatch_" & System.Diagnostics.Process.GetCurrentProcess.Id.ToString & ".log", bAccoda)
    lw1.WriteLine(DateTime.Now.ToString & " " & strMsg)
    lw1.Flush()
    lw1.Close()
  End Sub

End Class
