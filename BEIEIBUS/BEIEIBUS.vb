Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Text
Imports System.IO
Imports AMHelper.WS
Imports AMHelper.CSV
Imports RestSharpApex
Imports ApexNetLIB
Imports System.Management


Public Class CLEIEIBUS
    Inherits CLE__BASN


    Private Moduli_P As Integer = bsModAll
    Private ModuliExt_P As Integer = 0
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property

    Public oCldIbus As CLDIEIBUS
    Const defRelracciati As String = "1.9"
    Public strReleaseTracciati As String = defRelracciati

    ' Chiavi di registro varie
    Public strDropBoxDir As String = ""
    Public strDropBoxBin As String = ""
    Public strFiltroGGStoArt As String = ""
    Public strFiltroGGDocumenti As String = ""
    Public strFiltroGGOfferte As String = ""
    Public strFiltroGGUltAcqVen As String = ""
    Public strFiltroCliConAgenti As String = ""
    Public strFiltroIncludiNonEvasi As String = "1"
    Public strDisattivaModificaDatiCliente As String = ""
    Public strIncludileadClienti As String = ""
    Public strDeterminazioneDescrizioneRigaOrdine As String = ""
    Public strEstraiSoloListiniUMV As String = "0"
    Public strBloccaSceltaUM As String = "0"

    Public eProxyUrl As String = ""
    Public eProxyUsername As String = ""
    Public eProxyPassword As String = ""
    Public eProxyPort As String = ""


    Public strMastro As String = ""
    Public strAuthKeyAM As String = ""
    Public strAuthKeyLM As String = ""
    Public strProduzioneAPI As String = "-1"

    Public ProduzioneLM As Boolean = True

    Public strAppManagerAPI As String = ""
    Public strCodProgetto As String = ""

    Public strUseAPI As String = ""

    Public strScontoMaxPercentuale As String = ""
    Public strPercentualeSuPrezzoMinimoVendita As String = ""
    Public strEsplodiKit As String = ""

    Public strAttivaAlert As String = ""

    Public strAccodaLog As String = ""
    Public strUsaUMVendita As String = ""
    Public strAttivaPush As String = ""
    Public strContiEsclusi As String = "0"  'conto NS stabilimento

    ' Variabili per la sostituzione della query
    Public strCustomQueryGetArt As String = ""
    Public strCustomQueryGetArtUM As String = ""
    Public strCustomQueryGetArtCatalogo As String = ""
    Public strCustomQueryGetArtUM_EstraiTutte As String = ""

    ' Variabili per la personalizzazione del filtro di where delle query
    Public strCustomWhereGetAgentiCliente As String = ""
    Public strCustomWhereGetAgenti As String = ""
    Public strCustomWhereGetArt As String = ""
    Public strCustomWhereGetArtUM As String = ""
    Public strCustomWhereGetArtCatalogo As String = ""
    Public strCustomWhereGetArtGiacenze As String = ""
    Public strCustomWhereGetArtLingua As String = ""
    Public strCustomWhereGetArtListini As String = ""
    Public strCustomWhereGetArtSconti As String = ""
    Public strCustomWhereGetArtStoart As String = ""
    Public strCustomWhereGetArtUltAcq As String = ""
    Public strCustomWhereGetArtUltVen As String = ""
    Public strCustomWhereGetCampagne As String = ""
    Public strCustomWhereGetClifor As String = ""
    Public strCustomWhereGetClassiSconto As String = ""
    Public strCustomWhereGetCanaliVendita As String = ""
    Public strCustomWhereGetCliforAge As String = ""
    Public strCustomWhereGetCliforBlo As String = ""
    Public strCustomWhereGetPorto As String = ""
    Public strCustomWhereGetCliforCate As String = ""
    Public strCustomWhereGetCliforDestdiv As String = ""
    Public strCustomWhereGetCliforDettCon As String = ""
    Public strCustomWhereGetCliforFatt As String = ""
    Public strCustomWhereGetCliforNote As String = ""
    Public strCustomWhereGetCliforRighDoc As String = ""
    Public strCustomWhereGetCliforScaDoc As String = ""
    Public strCustomWhereGetScaDocPush As String = ""
    Public strCustomWhereGetCliforSenzaCoordinate As String = ""
    Public strCustomWhereGetCliforTestDoc As String = ""
    Public strCustomWhereGetCodpaga As String = ""
    Public strCustomWhereGetComuni As String = ""
    Public strCustomWhereGetNazioni As String = ""
    Public strCustomWhereGetLeadAccessi As String = ""
    Public strCustomWhereGetLeadAccessiCrm As String = ""
    Public strCustomWhereGetLeadCate As String = ""
    Public strCustomWhereGetLeadConcorrenti As String = ""
    Public strCustomWhereGetLeadDetCon As String = ""
    Public strCustomWhereGetLeadMansioni As String = ""
    Public strCustomWhereGetLeadModAcquisizione As String = ""
    Public strCustomWhereGetLeadNote As String = ""
    Public strCustomWhereGetLeadRighOff As String = ""
    Public strCustomWhereGetLeads As String = ""
    Public strCustomWhereGetLeadTestOff As String = ""
    Public strDefaultOpNome As String = "Admin"

    Public arFileGen As New ArrayList       'elenco di file generatici che andranno copiati dalla dir TMP alla dir di dropbox
    Public strTipork As String = ""         'elenco operazioni da compiere in import/export



    'inizializzo BEORGSOR
    '----------------------------------------------------------------
    Public strSerie As String = " "
    Public nTipoBF As Integer = 0
    Public nMagaz As Integer = 0

    ' Recupero il nome dei tracciati
    ' -------------------------------
    Public Const cIMP_ART As String = "io_art.dat"
    Public Const cIMP_AGENTI As String = "io_agenti.dat"
    Public Const cIMP_ART_LANG As String = "io_art_lang.dat"
    Public Const cIMP_ARTICOLI_ASSORTIMENTI As String = "io_articoli_assortimenti.dat"
    Public Const cIMP_ART_ULTACQ As String = "io_art_ultacq.dat"
    Public Const cIMP_ART_ULTVEN As String = "io_art_ultven.dat"
    Public Const cIMP_ART_UM As String = "io_art_um.dat"
    Public Const cIMP_ASSORTIMENTI As String = "io_assortimenti.dat"
    Public Const cIMP_CAMPAGNE As String = "io_campagne.dat"
    Public Const cIMP_CITTA As String = "io_citta.dat"
    Public Const cIMP_NAZIONI As String = "io_nazioni.dat"
    Public Const cIMP_CLASSI_SCONTO As String = "io_classi_sconto.dat"
    Public Const cIMP_CANALI_VENDITA As String = "io_canali_vendita.dat"
    Public Const cIMP_CLIENTI_ASSORTIMENTI As String = "io_clienti_assortimenti.dat"
    Public Const cIMP_CLIFOR_GEN As String = "io_clifor_gen.dat"
    Public Const cIMP_CLIFOR_AGE As String = "io_clifor_age.dat"
    Public Const cIMP_CLIFOR_BLO As String = "io_clifor_blo.dat"
    Public Const cIMP_CLIFOR_CATE As String = "io_clifor_cate.dat"
    Public Const cIMP_CLIFOR_DEST As String = "io_clifor_dest.dat"
    Public Const cIMP_CLIFOR_DETCON As String = "io_clifor_detcon.dat"
    Public Const cIMP_CLIFOR_FATT As String = "io_clifor_fatt.dat"
    Public Const cIMP_CLIFOR_GIROVISITA As String = "io_clifor_girovisita.dat"
    Public Const cIMP_CLIFOR_NOTE As String = "io_clifor_note.dat"
    Public Const cIMP_CLIFOR_RIGHDOC As String = "io_clifor_righdoc.dat"
    Public Const cIMP_CLIFOR_SCADOC As String = "io_clifor_scadoc.dat"
    Public Const cIMP_CLIFOR_TESTDOC As String = "io_clifor_testdoc.dat"
    Public Const cIMP_CONDPAG As String = "io_condpag.dat"
    Public Const cIMP_PORTO As String = "io_porto.dat"
    Public Const cIMP_CONDPAG_LANG As String = "io_condpag_lang.dat"
    Public Const cIMP_CUSTOM_FIELDS As String = "io_custom_fields.dat"
    Public Const cIMP_GIACENZE As String = "io_giacenze.dat"
    Public Const cIMP_INFO As String = "io_info.dat"
    Public Const cIMP_LEADS As String = "io_leads.dat"
    Public Const cIMP_LEAD_ACCCRM As String = "io_lead_acccrm.dat"
    Public Const cIMP_LEAD_CONCORRENTI As String = "io_lead_concorrenti.dat"
    Public Const cIMP_LEAD_ACCESSI As String = "io_lead_accessi.dat"
    Public Const cIMP_LEAD_CATE As String = "io_lead_cate.dat"
    Public Const cIMP_LEAD_MANSIONI As String = "io_lead_mansioni.dat"
    Public Const cIMP_LEAD_MOD_ACQUISIZIONE As String = "io_lead_mod_acquisizione.dat"
    Public Const cIMP_LEAD_NOTE As String = "io_lead_note.dat"
    Public Const cIMP_LEAD_RIGHOFF As String = "io_lead_righoff.dat"
    Public Const cIMP_LEAD_SCONTI As String = "io_lead_sconti.dat"
    Public Const cIMP_LEAD_TESTOFF As String = "io_lead_testoff.dat"
    Public Const cIMP_LEAD_DETCON As String = "io_lead_detcon.dat"
    Public Const cIMP_LISTINI_FULL As String = "io_listini_full.dat"
    Public Const cIMP_STOART As String = "io_stoart.dat"
    Public Const cIMP_SCONTI As String = "io_sconti.dat"
    Public Const cIMP_VALUTE As String = "io_valute.dat"
    Public Const cIMP_TAGLIE_ASSORTIMENTI As String = "io_taglie_assortimenti.dat"
    Public Const cIMP_TAGLIE_CATALOGHI As String = "io_taglie_cataloghi.dat"
    Public Const cIMP_TAGLIE_CATALOGHI_ART As String = "io_taglie_cataloghi_art.dat"
    Public Const cIMP_TAGLIE_ESTENSIONI As String = "io_taglie_estensioni.dat"
    Public Const cIMP_TAGLIE_SVILUPPI As String = "io_taglie_sviluppi.dat"
    Public Const cIMP_TAGLIE_SVILUPPI_ART As String = "io_taglie_sviluppi_art.dat"
    Public Const cIMP_VAR_COMBINAZIONI As String = "io_var_combinazioni.dat"
    Public Const cIMP_CATALOGO As String = "io_catalogo.dat"
    Public Const cIMP_REPORT As String = "io_reports.dat"


    Public Overrides Function Init(ByRef App As CLE__APP, _
                                ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String, _
                                ByVal bRemoting As Boolean, ByVal strRemoteServer As String, _
                                ByVal strRemotePort As String) As Boolean
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDIEIBUS"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, bRemoting, strRemoteServer, strRemotePort)
        oCldIbus = CType(MyBase.ocldBase, CLDIEIBUS)
        oCldIbus.Init(oApp)

        Return True
    End Function


    Public Overridable Function SaveDatatableToFile(ByVal dttTmp As DataTable, ByVal FileName As String) As Boolean
        Dim RetVal As Boolean = False

        Try

            Using textWriter As TextWriter = File.CreateText(FileName)
                Using csv As CsvHelper.CsvWriter = New CsvHelper.CsvWriter(textWriter)
                    csv.Configuration.Delimiter = "|"
                    csv.Configuration.Encoding = Encoding.UTF8

                    ' Write columns
                    For Each column As DataColumn In dttTmp.Columns
                        csv.WriteField(column.ColumnName)
                    Next
                    csv.NextRecord()

                    ' Write row values
                    For Each row As DataRow In dttTmp.Rows
                        For i As Integer = 0 To dttTmp.Columns.Count - 1
                            csv.WriteField(row(i))
                        Next
                        csv.NextRecord()
                    Next
                End Using
            End Using

            RetVal = True
        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try

        Return RetVal
    End Function

    Public Overridable Function CopyIfDifferent(ByVal Origine As String, ByVal Destinazione As String) As Boolean
        ' Da testare
        Dim infoOr As FileInfo
        Dim infoDest As FileInfo
        infoOr = New FileInfo(Origine)

        infoDest = New FileInfo(Destinazione)
        If infoDest.LastWriteTime <> infoOr.LastWriteTime Then
            File.Copy(Origine, Destinazione, True)
        End If

        Return True
    End Function

    Public Overridable Function ConvTracciato(ByVal oIn As Object, Optional ByVal bUltAgg As Boolean = False) As String
        ' Da Testare
        Dim retVal As String = ""
        Dim TipoDato As String = oIn.GetType.Name

        Try

            If NTSCStr(oIn) <> "" Then

                Select Case TipoDato
                    Case "Int32"
                        retVal = NTSCStr(oIn)

                    Case "String"
                        retVal = NTSCStr(oIn).Replace(vbCrLf, " ").Replace("|", "_").Replace(vbLf, " ").Replace(vbCr, " ")

                    Case "DateTime"
                        If bUltAgg Then
                            retVal = NTSCDate(oIn).ToString("ddMMyyyyHHmmss")
                        Else
                            retVal = NTSCDate(oIn).ToString("ddMMyyyy")
                        End If
                    Case Else
                        ' di che tipo e?
                        retVal = NTSCStr(oIn)
                End Select

            Else
                retVal = ""
            End If


            Return retVal

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    Public Overridable Function ConvStr(ByVal oIn As Object, Optional ConvNewLine As Boolean = False) As String
        ConvStr = NTSCStr(oIn)
        Dim junk As String
        Dim RetValue As String = ""

        Try
            If ConvNewLine Then

                If InStr(ConvStr, vbNewLine) > 0 Then
                    junk = ""
                End If

                'Char(13) + CHAR(10), CHAR(10))
                RetValue = NTSCStr(oIn).Replace("|", "_")

                RetValue = RetValue.Replace(Chr(13) + Chr(10), Chr(10))
                RetValue = RetValue.Replace(Chr(10), Chr(13))
                RetValue = RetValue.Replace(Chr(13), CLDIEIBUS.iBNewline)

                Return RetValue
            Else
                Return NTSCStr(oIn).Replace(vbCrLf, " ").Replace("|", "_").Replace(vbLf, " ").Replace(vbCr, " ")
            End If


        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    Public Overridable Function ConvData(ByVal oIn As Object, Optional ByVal bUltAgg As Boolean = False) As String
        ConvData = ""

        If bUltAgg Then
            Return "01011900000000"
        End If

        Try
            If NTSCStr(oIn) <> "" Then
                If bUltAgg Then
                    Return NTSCDate(oIn).ToString("ddMMyyyyHHmmss")
                Else
                    Return NTSCDate(oIn).ToString("ddMMyyyy")
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    Public Overridable Function Elabora() As Boolean
        Dim strMsgLog As String = ""
        Dim i As Integer = 0
        Dim dttTm As New DataTable      'testate documenti di magazzino/ordini
        Dim dttCat As New DataTable     'articoli con immagine catalogo
        Dim dttRep As New DataTable
        Dim strFileCat As String = ""
        Dim strFileRep As String = ""
        Dim TipoCF As String = "CF"


        Try

            ' Ricordati di aggiornare http://doc.apexnet.it/iB.connettore_IB.ashx
            strDropBoxDir = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "DropBoxDir", "", " ", "")
            strDropBoxBin = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "DropBoxBin", "", " ", "")

            strContiEsclusi = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "ContiEsclusi", "0", " ", "0").Trim
            strFiltroCliConAgenti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroCliConAgenti", "0", " ", "0").Trim
            strDisattivaModificaDatiCliente = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "DisattivaModificaDatiCliente", "0", " ", "0").Trim

            strFiltroIncludiNonEvasi = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroIncludiNonEvasi", "1", " ", "1").Trim

            strFiltroGGOfferte = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroGGOfferte", "365", " ", "365").Trim
            strFiltroGGStoArt = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroGGStoArt", "180", " ", "180").Trim
            strFiltroGGDocumenti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroGGDocumenti", "365", " ", "365").Trim
            strFiltroGGUltAcqVen = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "FiltroGGUltAcqVen", "180", " ", "180").Trim
            strIncludileadClienti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "IncludiLeadClienti", "0", " ", "0").Trim
            strScontoMaxPercentuale = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "ScontoMaxPercentuale", "", " ", "").Trim
            strDeterminazioneDescrizioneRigaOrdine = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "DeterminazioneDescrizioneRigaOrdine", "0", " ", "0").Trim
            strPercentualeSuPrezzoMinimoVendita = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "PercentualeSuPrezzoMinimoVendita", "0", " ", "0").Trim
            strEsplodiKit = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "EsplodiKit", "0", " ", "0").Trim
            strAccodaLog = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "AccodaLog", "0", " ", "0").Trim
            strUsaUMVendita = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "UsaUMVendita", "1", " ", "1").Trim
            strEstraiSoloListiniUMV = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "EstraiSoloListiniUMV", "0", " ", "0").Trim
            strBloccaSceltaUM = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "BloccaSceltaUM", "0", " ", "0").Trim


            strUseAPI = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "UseAPI", "0", " ", "0").Trim
            strAuthKeyLM = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "AuthKeyLM", "", " ", "").Trim
            strAuthKeyAM = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "AuthKeyAM", "", " ", "").Trim
            strProduzioneAPI = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "ProduzioneAPI", "-1", " ", "-1").Trim

            'strAppManagerAPI
            strMastro = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "Mastro", "0", " ", "0").Trim

            strAttivaAlert = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "Abilita_Alert", "0", " ", "0").Trim
            strAttivaPush = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "Abilita_Push", "0", " ", "0").Trim


            ' Sostituzione query
            ' ------------------
            strCustomQueryGetArtUM_EstraiTutte = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "QueryGetArtUM_EstraiTutte", "0", " ", "0").Trim
            strCustomQueryGetArt = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "QueryGetArt", "", " ", "").Trim
            strCustomQueryGetArtUM = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "QueryGetArtUM", "", " ", "").Trim
            strCustomQueryGetArtCatalogo = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "QueryGetArtCatalogo", "", " ", "").Trim

            ' Filtri di Where
            ' ----------------

            ' Clienti
            strCustomWhereGetClifor = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetClifor", "", " ", "").Trim
            strCustomWhereGetCliforAge = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforAge", "", " ", "").Trim
            strCustomWhereGetCliforBlo = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforBlo", "", " ", "").Trim
            strCustomWhereGetCliforCate = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforCate", "", " ", "").Trim
            strCustomWhereGetCliforDestdiv = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforDestdiv", "", " ", "").Trim
            strCustomWhereGetCliforDettCon = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforDettCon", "", " ", "").Trim
            strCustomWhereGetCliforSenzaCoordinate = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforSenzaCoordinate", "", " ", "").Trim
            strCustomWhereGetCliforNote = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforNote", "", " ", "").Trim

            ' Articoli
            strCustomWhereGetArt = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArt", "", " ", "").Trim
            strCustomWhereGetArtUM = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtUM", "", " ", "").Trim
            strCustomWhereGetArtCatalogo = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtCatalogo", "", " ", "").Trim
            strCustomWhereGetArtGiacenze = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtGiacenze", "", " ", "").Trim
            strCustomWhereGetArtListini = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtListini", "", " ", "").Trim
            strCustomWhereGetArtLingua = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtLingua", "", " ", "").Trim
            strCustomWhereGetArtSconti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtSconti", "", " ", "").Trim
            strCustomWhereGetArtStoart = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtStoart", "", " ", "").Trim
            strCustomWhereGetArtUltAcq = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtUltAcq", "", " ", "").Trim
            strCustomWhereGetArtUltVen = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetArtUltVen", "", " ", "").Trim

            ' Documenti
            strCustomWhereGetCliforFatt = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforFatt", "", " ", "").Trim
            strCustomWhereGetCliforRighDoc = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforRighDoc", "", " ", "").Trim
            strCustomWhereGetCliforScaDoc = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforScaDoc", "", " ", "").Trim
            strCustomWhereGetCliforTestDoc = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCliforTestDoc", "", " ", "").Trim

            ' Leads
            strCustomWhereGetLeadAccessi = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadAccessi", "", " ", "").Trim
            strCustomWhereGetLeadAccessiCrm = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadAccessiCrm", "", " ", "").Trim
            strCustomWhereGetLeadCate = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadCate", "", " ", "").Trim
            strCustomWhereGetLeadConcorrenti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadConcorrenti", "", " ", "").Trim
            strCustomWhereGetLeadDetCon = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadDetCon", "", " ", "").Trim
            strCustomWhereGetLeadMansioni = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadMansioni", "", " ", "").Trim
            strCustomWhereGetLeadModAcquisizione = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadModAcquisizione", "", " ", "").Trim
            strCustomWhereGetLeadNote = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadNote", "", " ", "").Trim
            strCustomWhereGetLeadRighOff = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadRighOff", "", " ", "").Trim
            strCustomWhereGetLeads = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeads", "", " ", "").Trim
            strCustomWhereGetLeadTestOff = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetLeadTestOff", "", " ", "").Trim
            strCustomWhereGetCampagne = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCampagne", "", " ", "").Trim

            ' Altro
            strCustomWhereGetCodpaga = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCodpaga", "", " ", "").Trim
            strCustomWhereGetClassiSconto = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetClassiSconto", "", " ", "").Trim
            strCustomWhereGetCanaliVendita = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetCanaliVendita", "", " ", "").Trim
            strCustomWhereGetComuni = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetComuni", "", " ", "").Trim
            strCustomWhereGetNazioni = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetNazioni", "", " ", "").Trim
            strCustomWhereGetAgenti = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetAgenti", "", " ", "").Trim
            strCustomWhereGetAgentiCliente = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetAgentiCliente", "", " ", "").Trim
            strCustomWhereGetPorto = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetPorto", "", " ", "").Trim
            strCustomWhereGetScaDocPush = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "WhereGetScaDocPush", "", " ", "").Trim

            ' Operatore di default da usare
            strDefaultOpNome = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "DefaultOpNome", "", " ", "Admin").Trim

            ' Parametri di Business

            'inizializzo BEORGSOR
            '----------------------------------------------------------------
            strSerie = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "SERIE_ORDINI", " ", " ", " ")
            nTipoBF = NTSCInt(oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "TIPOBF_ORDINI", " ", " ", " "))
            nMagaz = NTSCInt(oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "MAGAZ_ORDINI", " ", " ", " "))


            arFileGen.Clear()

            'InviaAlert(1, "PIPPO")

            'avvio il file di log della procedura di import/export
            '--------------------


            Dim bLogAppendFromInterface As Boolean = False
            If strAccodaLog <> "0" Then
                bLogAppendFromInterface = True
            End If


            If oApp.Batch Then
                If Not LogStart("BNIEIBUS_BATCH", "Import/export Vs IBUS da batch" & vbCrLf, True) Then Return False
                LogWrite("Per dettagli sull'avvio in modalità BATCH consultare il file '|BusNetBatch_" & System.Diagnostics.Process.GetCurrentProcess.Id.ToString & ".log|'", False)
            Else

                If Not LogStart("BNIEIBUS", "Import/export Vs IBUS da interfaccia" & vbCrLf, bLogAppendFromInterface, False) Then Return False
                LogWrite("Esecuzione da interfaccia:", False)
            End If


            strMsgLog = oApp.Tr(Me, 129877602933085931, "INIZIO ELABORAZIONE")
            LogWrite(strMsgLog, False)
            LogWrite("Selezione: [" + strTipork + "]", False)


            'controlli pre-elaborazione
            '--------------------
            If strDropBoxDir.Trim = "" Then
                LogWrite(oApp.Tr(Me, 129877622189278434, String.Format("Directory DropBox non settata per la ditta [{0}]. Impostarla con l'opzione di registro |'Bsieibus/Opzioni/DropBoxDir'|. Elaborazione interrotta.", strDittaCorrente)), True)
                Return False
            End If
            If System.IO.Directory.Exists(strDropBoxDir) = False Then
                LogWrite(oApp.Tr(Me, 129877623144826060, String.Format("Directory DropBox |{0}| Inesistente. Elaborazione interrotta.", strDropBoxDir)), True)
                Return False
            End If

            ' Applico le personalizzazioni alla base dati
            If Not oCldIbus.AggiungiPersonalizzazioniDb() Then Return False


            If strTipork.EndsWith(";") = False Then strTipork += ";"
            strTipork = strTipork.ToUpper

            ' SI INZIA
            ' --------
            ' Aggiorno la versione sul license
            Dim CodProgetto As String = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(strDropBoxDir + "\"))
            Dim VersioneDLL_BN As String = FileVersionInfo.GetVersionInfo(oApp.NetDir & "\BNIEIBUS.dll").FileVersion
            If CodProgetto.Contains(".") Then
                LogWrite("Invio versione...", False)
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Invio versione..."))
                ApexNetLIB.UpdateRelease.SendVersion(CodProgetto, VersioneDLL_BN, oApp.BUSNET_VERSIONDB.ToString())
            End If


            ' Solo se effettuo un export devo creare il file INFO.
            If strTipork <> "ORD;" Then
                '--------------------
                'Export info
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export info..."))
                If Not Elabora_ExportInfo(oApp.AscDir & "\" + cIMP_INFO, VersioneDLL_BN) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_INFO)
            End If


            ' Invio le notifiche push
            If strTipork.Contains("PUS;") And strUseAPI <> "0" Then
                '--------------------
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Invio notifiche push..."))
                Elabora_NotifichePushInsoluti()

            End If

            ' Aggiorno i dati della Geolocalizzazione
            If strTipork.Contains("COO;") Then
                '--------------------
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Identificazione coordinate da Google..."))
                AggiornaCoordinate()

            End If

            '--------------------
            'Export causali
            If strTipork.Contains("PAG;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export causali..."))
                If Not Elabora_ExportCodpaga(oApp.AscDir & "\" + cIMP_CONDPAG) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CONDPAG)
            End If
            ' ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "20"))

            '--------------------
            'Export tabelle di base
            If strTipork.Contains("TBS;") Or strTipork.Contains("CIT;") Then

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export comuni..."))
                If Not Elabora_ExportComuni(oApp.AscDir & "\" + cIMP_CITTA) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CITTA)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export nazioni..."))
                If Not Elabora_ExportNazioni(oApp.AscDir & "\" + cIMP_NAZIONI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_NAZIONI)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export classi sconto..."))
                If Not Elabora_ExportClassiSconto(oApp.AscDir & "\" + cIMP_CLASSI_SCONTO) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLASSI_SCONTO)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export canali di vendita..."))
                If Not Elabora_ExportCanaliVendita(oApp.AscDir & "\" + cIMP_CANALI_VENDITA) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CANALI_VENDITA)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export categorie clienti..."))
                If Not Elabora_ExportCliforCate(oApp.AscDir & "\" + cIMP_CLIFOR_CATE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_CATE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export porto..."))
                If Not Elabora_ExportPorto(oApp.AscDir & "\" + cIMP_PORTO) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_PORTO)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export agenti..."))
                If Not Elabora_ExportAgenti(oApp.AscDir & "\" + cIMP_AGENTI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_AGENTI)

            End If
            'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "30"))


            ' If strTipork.Contains("CIT;") Then
            ' If Not Elabora_ExportCittaXML(oApp.AscDir & "\citta.xml") Then Return False
            ' CompressFile(oApp.AscDir & "\citta.xml", oApp.AscDir)
            'arFileGen.Add(oApp.AscDir & "\citta.zip")
            'End If

            '--------------------
            'Export clienti/fornitori e tabelle relative
            TipoCF = "CF"
            If strTipork.Contains("CLI;") Or strTipork.Contains("FOR;") Then
                If strTipork.Contains("CLI;") And strTipork.Contains("FOR;") Then
                    TipoCF = "CF"
                Else
                    If strTipork.Contains("CLI;") Then
                        TipoCF = "C"
                    End If
                    If strTipork.Contains("FOR;") Then
                        TipoCF = "F"
                    End If
                End If
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "40"))


                'ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export clienti/fornitori..."))
                'If Not Elabora_ExportClifor(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR, _
                '                oApp.AscDir & "\" + cIMP_CLIFOR_INFO, _
                '                oApp.AscDir & "\" + cIMP_CLIFOR_VEN) Then Return False

                'arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR)
                'arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_INFO)
                'arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_VEN)

                ' Elabora clifor gen
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export clienti/fornitori..."))
                If Not Elabora_ExportCliforGen(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_GEN) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_GEN)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export blocchi..."))
                If Not Elabora_ExportCliforBlo(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_BLO) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_BLO)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export destinazioni..."))
                If Not Elabora_ExportCliforDestdiv(oApp.AscDir & "\" + cIMP_CLIFOR_DEST) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_DEST)
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "45"))

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export agenti..."))
                If Not Elabora_ExportCliforAge(oApp.AscDir & "\" + cIMP_CLIFOR_AGE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_AGE)
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "50"))

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Clienti Note..."))
                If Not Elabora_ExportCliforNote(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_NOTE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_NOTE)

                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "50"))
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export dettagli contatti..."))
                If Not Elabora_ExportCliforDettCon(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_DETCON) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_DETCON)
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "55"))


            End If
            'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "70"))

            If strTipork.Contains("LEA;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads..."))
                If Not Elabora_ExportLeads(oApp.AscDir & "\" + cIMP_LEADS) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEADS)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Note..."))
                If Not Elabora_ExportLeadNote(oApp.AscDir & "\" + cIMP_LEAD_NOTE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_NOTE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Contatti..."))
                If Not Elabora_ExportLeadDetCon(oApp.AscDir & "\" + cIMP_LEAD_DETCON) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_DETCON)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Accessi CRM..."))
                If Not Elabora_ExportLeadAccessiCrm(oApp.AscDir & "\" + cIMP_LEAD_ACCCRM) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_ACCCRM)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead categorie..."))
                If Not Elabora_ExportLeadCate(oApp.AscDir & "\" + cIMP_LEAD_CATE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_CATE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Campagne..."))
                If Not Elabora_ExportCampagne(oApp.AscDir & "\" + cIMP_CAMPAGNE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CAMPAGNE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead modalità acquisizione..."))
                If Not Elabora_ExportLeadModAcquisizione(oApp.AscDir & "\" + cIMP_LEAD_MOD_ACQUISIZIONE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_MOD_ACQUISIZIONE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Concorrenti..."))
                If Not Elabora_ExportLeadConcorrenti(oApp.AscDir & "\" + cIMP_LEAD_CONCORRENTI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_CONCORRENTI)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Mansioni..."))
                If Not Elabora_ExportLeadMansioni(oApp.AscDir & "\" + cIMP_LEAD_MANSIONI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_MANSIONI)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Lead Operatori..."))
                If Not Elabora_ExportLeadAccessi(oApp.AscDir & "\" + cIMP_LEAD_ACCESSI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_ACCESSI)

            End If

            If strTipork.Contains("OFF;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Offerte (testate)..."))
                If Not Elabora_ExportLeadTestOff(oApp.AscDir & "\" + cIMP_LEAD_TESTOFF) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_TESTOFF)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Offerte (righe)..."))
                If Not Elabora_ExportLeadRighOff(oApp.AscDir & "\" + cIMP_LEAD_RIGHOFF) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LEAD_RIGHOFF)

            End If

            If strTipork.Contains("DOC;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export documenti (testate)..."))
                If Not Elabora_ExportCliforTestDoc(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_TESTDOC, dttTm) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_TESTDOC)
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "65"))

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export documenti (righe)..."))
                If Not Elabora_ExportCliforRighDoc(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_RIGHDOC) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_RIGHDOC)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export documenti (scadenze)..."))
                If Not Elabora_ExportCliforScadoc(TipoCF, oApp.AscDir & "\" + cIMP_CLIFOR_SCADOC, dttTm) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_SCADOC)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export fatturati..."))
                If Not Elabora_ExportCliforFatt(oApp.AscDir & "\" + cIMP_CLIFOR_FATT) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CLIFOR_FATT)
                'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "60"))
            End If


            '--------------------
            'Export articoli e tabelle relative
            If strTipork.Contains("ART;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export articoli..."))
                If Not Elabora_ExportArt(oApp.AscDir & "\" + cIMP_ART, strCustomQueryGetArt) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_ART)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export UM articoli..."))
                If Not Elabora_ExportArtUM(oApp.AscDir & "\" + cIMP_ART_UM, strCustomQueryGetArtUM) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_ART_UM)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export articoli in lingua..."))
                If Not Elabora_ExportArtLingua(oApp.AscDir & "\" + cIMP_ART_LANG) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_ART_LANG)

            End If
            'ThrowRemoteEvent(New NTSEventArgs("PROGRESSBA", "75"))


            '--------------------
            'Export giacenze articoli
            If strTipork.Contains("MAG;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export giacenze..."))
                If Not Elabora_ExportArtGiacenze(oApp.AscDir & "\" + cIMP_GIACENZE) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_GIACENZE)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export ultimi venduti..."))
                If Not Elabora_ExportArtUltVen(oApp.AscDir & "\" + cIMP_ART_ULTVEN) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_ART_ULTVEN)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export ultimi acquisti..."))
                If Not Elabora_ExportArtUltAcq(oApp.AscDir & "\" + cIMP_ART_ULTACQ) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_ART_ULTACQ)

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export storico articoli..."))
                If Not Elabora_ExportArtStoart(oApp.AscDir & "\" + cIMP_STOART) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_STOART)
            End If

            '--------------------
            'Export listini articoli
            If strTipork.Contains("LIS;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export listini..."))
                If Not Elabora_ExportListini(oApp.AscDir & "\" + cIMP_LISTINI_FULL) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_LISTINI_FULL)
            End If

            '--------------------
            'Export sconti articoli
            If strTipork.Contains("SCO;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export sconti..."))
                If Not Elabora_ExportSconti(oApp.AscDir & "\" + cIMP_SCONTI) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_SCONTI)
            End If


            ' Funzione vuota per personalizzazioni
            If Not Elabora_Child() Then Return False


            'Export catalogo articoli
            '-------------------------
            If strTipork.Contains("CAT;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export catalogo..."))
                If Not Elabora_ExportCatalogo(oApp.AscDir & "\" + cIMP_CATALOGO, dttCat, strCustomQueryGetArtCatalogo) Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_CATALOGO)
            End If

            'Export catalogo oggetti ole (per ora non attivo da interfaccia)
            '-------------------------
            If strTipork.Contains("OLE;") Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export ole objects..."))
                If Not Elabora_ExportOleObjects(oApp.AscDir & "\" + cIMP_REPORT, dttRep, "") Then Return False
                arFileGen.Add(oApp.AscDir & "\" + cIMP_REPORT)
            End If



            'Copio i files dei tracciati nella dir di dropbox
            '----------------------------------------------------
            ' NOTA: Se si crea una immagine con nome empty-image.jpg nella cartella immagini di Business,
            ' Questa viene mostrata nel catalogo di iB per tutte gli articoli che non hanno una foto
            ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Copia tracciati in " & strDropBoxDir))
            For i = 0 To arFileGen.Count - 1

                ' Mi occupo dei dati del catalogo
                If arFileGen(i).ToString.ToLower.EndsWith(cIMP_CATALOGO) Then
                    ' Cancello il file nella cartella dropbox
                    File.Delete(strDropBoxDir & "\multimedia\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length))
                    ' Se esiste un file immagine nella cartella di Business...
                    If File.Exists(arFileGen(i).ToString) Then
                        '... lo copio nella cartella dropbox
                        System.IO.File.Copy(arFileGen(i).ToString, strDropBoxDir & "\multimedia\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length), True)
                    End If

                    ' Mi occupo dei dati dei reports
                ElseIf arFileGen(i).ToString.ToLower.EndsWith(cIMP_REPORT) Then
                    File.Delete(strDropBoxDir & "\reports\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length))
                    If File.Exists(arFileGen(i).ToString) Then
                        System.IO.File.Copy(arFileGen(i).ToString, strDropBoxDir & "\reports\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length), True)
                    End If
                Else

                    ' Mi occupo dei dati gestionale
                    File.Delete(strDropBoxDir & "\gestionale\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length))
                    If File.Exists(arFileGen(i).ToString) Then
                        System.IO.File.Copy(arFileGen(i).ToString, strDropBoxDir & "\gestionale\" & arFileGen(i).ToString.Substring(oApp.AscDir.Length), True)
                    End If
                    File.Delete(arFileGen(i).ToString)
                End If
            Next


            'Se devo esportare anche il catalogo, copio le immagini
            '-----------------------------------------------------------
            If Not Directory.Exists(strDropBoxDir & "\multimedia") Then
                Directory.CreateDirectory(strDropBoxDir & "\multimedia")
            End If

            If dttCat.Rows.Count > 0 Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Copia immagini in " & strDropBoxDir & "\multimedia"))

                For Each dtrT As DataRow In dttCat.Rows
                    'tolgo l'eventale attributo di sola lettura
                    'strFileCat = strDropBoxDir & "\multimedia\" & NTSCStr(dtrT!ar_codart) & Path.GetExtension(oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1))
                    strFileCat = strDropBoxDir & "\multimedia\" & ConvStr(dtrT!ar_gif1)
                    If File.Exists(strFileCat) Then
                        File.SetAttributes(strFileCat, FileAttributes.Normal)
                    End If
                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Copia di " & oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1)))
                    Try
                        System.IO.File.Copy(oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1), strFileCat, True)
                    Catch ex As Exception
                        ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Salto read only" & oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1)))
                    End Try

                Next
            End If


            'Se devo esportare anche i reports, copio gli oggetti ole
            '-----------------------------------------------------------
            If Not Directory.Exists(strDropBoxDir & "\reports") Then
                Directory.CreateDirectory(strDropBoxDir & "\reports")
            End If

            If dttRep.Rows.Count > 0 Then
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Copia immagini in " & strDropBoxDir & "\reports"))

                For Each dtrT As DataRow In dttRep.Rows
                    'tolgo l'eventale attributo di sola lettura
                    'strFileRep = strDropBoxDir & "\multimedia\" & NTSCStr(dtrT!ar_codart) & Path.GetExtension(oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1))
                    strFileRep = strDropBoxDir & "\reports\" & Path.GetFileName(ConvStr(dtrT!xx_nome_doc))
                    If File.Exists(strFileRep) Then
                        File.SetAttributes(strFileRep, FileAttributes.Normal)
                    End If
                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Copia di " & ConvStr(dtrT!xx_nome_doc)))
                    Try
                        System.IO.File.Copy(ConvStr(dtrT!xx_nome_doc), strFileRep, True)
                    Catch ex As Exception
                        ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Salto read only" & ConvStr(dtrT!xx_nome_doc)))
                    End Try

                Next
            End If

            '--------------------
            'Import Ordini basato si file
            If strTipork.Contains("ORD;") And strUseAPI = "0" Then
                'meglio importare i dati dei clienti e note modificati o aggiunti
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import modifiche clienti da file..."))
                If Not Elabora_ImportAnagra() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import note clienti da file..."))
                If Not Elabora_ImportCliforNote() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import nuovi leads da file..."))
                If Not Elabora_ImportLead() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import note leads da file..."))
                If Not Elabora_ImportLeadNote() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import ordini da file..."))
                If Not Elabora_ImportOrdini() Then Return False

            End If



            ' Scommentare anche LastStoredID
            '--------------------
            'Import Ordini
            If strTipork.Contains("ORD;") And strUseAPI <> "0" Then

                ' Set variabili

                ' TODO: Scommentare solo in debug
                ' strUseAPI = "1"
                ' strAuthKeyLM = "LMKEY6"
                ' strAuthKeyAM = "AMKEY6"
                ' strMastro = "401"
                ' Produzione = False
                ' Scommentare anche LastStoredID

                ' strUseAPI = "1"
                ' strAuthKeyLM = "3818B678-8333-4DAE-9636-44142316F424"
                ' strAuthKeyAM = "DDC4C5C1-072F-41D8-A728-D8E4BA686588"
                'strMastro = "401"



                If strProduzioneAPI = "0" Then
                    ProduzioneLM = False
                Else
                    ProduzioneLM = True
                End If

                ' Controlli preelaborazione
                If strAuthKeyLM = "" Then
                    Dim msg As String = oApp.Tr(Me, 129919999269031600, "ERR: AuthKeyLM non configurata")
                    ThrowRemoteEvent(New NTSEventArgs("", msg))
                    LogWrite(msg, True)
                    Return False
                End If

                ' Controlli preelaborazione
                If strAuthKeyAM = "" Then
                    Dim msg As String = oApp.Tr(Me, 129919999269031600, "ERR: AuthKeyAM non configurata")
                    ThrowRemoteEvent(New NTSEventArgs("", msg))
                    LogWrite(msg, True)
                    Return False
                End If

                'Import da ws
                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Inizio import da ws..."))


                ' Istanzio l'oggetto Export dell'AMHelper
                'Dim ed As New GetDataLM(strAuthKeyLM)
                Dim ed As New GetDataLM(strAuthKeyLM, ProduzioneLM)

                If eProxyUrl <> "" Then
                    ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
                End If

                Dim AMData As ws_rec_lmparam = Nothing
                Dim RetVal As Boolean = ed.get_am_par(AMData)

                strAppManagerAPI = AMData.url_am_api

                If AMData.url_am_api = "" Or AMData.url_am_api.Length < 10 Then
                    Dim msg As String = oApp.Tr(Me, 129919999269031600, "ERR: Url api non recuperato da LM. Verifica i parametri nel registro")
                    ThrowRemoteEvent(New NTSEventArgs("", msg))
                    LogWrite(msg, True)
                    Return False
                End If



                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import modifiche clienti da ws..."))
                If Not Elabora_ImportAnagraAPI() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import note clienti da ws..."))
                If Not Elabora_ImportCliforNoteAPI() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import nuovi leads da ws..."))
                If Not Elabora_ImportLeadAPI() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import note leads da ws..."))
                If Not Elabora_ImportLeadNoteAPI() Then Return False

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Import ordini da ws..."))
                If Not Elabora_ImportOrdiniAPI() Then Return False



            End If


            ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Finito"))


            Dim UtenteWindows As String = GetWinUserName()
            Dim UtenteProcesso As String = ""


            If strDropBoxBin <> "" Then

                Dim user As String = GetWinUserName()
                Dim trovatoMioProcesso As Boolean = False

                Try
                    Dim query As String = "SELECT * FROM Win32_Process WHERE Name = '" + "Dropbox.exe" + "'"

                    Dim searcher As New ManagementObjectSearcher(query)
                    Dim observer As New ManagementOperationObserver()

                    'loop through each item in the collection
                    For Each process As ManagementObject In searcher.Get()
                        Try
                            'this string will hold the information returned
                            'from InvokeMethod("GetOwner")
                            Dim info(1) As String
                            'get the information on the current process
                            process.InvokeMethod("GetOwner", CType(info, Object()))

                            'now make sure the owner is correct
                            If info(0).ToString().ToUpper() <> user.ToUpper() Then
                                'kill the process
                                process.InvokeMethod(observer, "Terminate", Nothing)
                            Else
                                ' se sono qui e' perche' sono sul mio processo
                                trovatoMioProcesso = True
                            End If
                        Catch ex As Exception

                            Return False
                        End Try
                    Next
                Catch ex As Exception

                    Return False
                End Try

                If Not trovatoMioProcesso Then


                    Dim startInfo As New ProcessStartInfo(strDropBoxBin)
                    startInfo.Arguments = "/home"

                    startInfo.WorkingDirectory = Path.GetDirectoryName(strDropBoxBin)
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden

                    startInfo.CreateNoWindow = False


                    Process.Start(startInfo)

                    ' System.Diagnostics.Process.Start(strDropBoxBin)
                End If

            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------
        Finally
            dttTm.Clear()
            dttCat.Clear()
            LogStop()
        End Try
    End Function


    Public Function GetWinUserName() As String
        If TypeOf My.User.CurrentPrincipal Is 
          Security.Principal.WindowsPrincipal Then
            ' The application is using Windows authentication.
            ' The name format is DOMAIN\USERNAME.
            Dim parts() As String = Split(My.User.Name, "\")
            Dim username As String = parts(1)
            Return username
        Else
            ' The application is using custom authentication.
            Return My.User.Name
        End If
    End Function

    Public Overridable Function Elabora_NotifichePushInsoluti() As Boolean
        'restituisco le scadenze di cliente/fornitore ATTIVO o POTENZIALE
        'per collegare le scadenze ai relativi documenti precedentemente esportati, ...
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim DesScad As String = ""
        Dim MemoInviata As Boolean = False
        Dim MsgInsoluto As String = ""


        Try
            If Not oCldIbus.GetScaDocPush(strDittaCorrente, dttTmp, strCustomWhereGetScaDocPush) Then Return False

            For Each dtrT As DataRow In dttTmp.Rows

                MsgInsoluto = "Insoluto cliente " & ConvStr(dtrT!sc_conto) & " - " & ConvStr(dtrT!an_descr1) & " - " & ConvStr(dtrT!sc_importo) & " - " & "Rata:" & ConvStr(dtrT!sc_numrata)

                MemoInviata = False

                If ConvStr(dtrT!xx_agente1) <> "0" Then
                    MemoInviata = True
                    InviaPushByCodAgente(ConvStr(dtrT!xx_agente1), MsgInsoluto)
                End If
                If ConvStr(dtrT!xx_agente2) <> "0" Then
                    MemoInviata = True
                    InviaPushByCodAgente(ConvStr(dtrT!xx_agente2), MsgInsoluto)
                End If

                If MemoInviata Then
                    ' chiave: sc_conto, sc_annpar, sc_alfpar, sc_numpar, sc_integr, sc_numrata, codditt
                    ' Setta il record come insoluto gia' inviato
                    oCldIbus.SetDatePushInsolutiIB(ConvStr(dtrT!sc_conto),
                                                   ConvStr(dtrT!sc_annpar),
                                                   ConvStr(dtrT!sc_alfpar),
                                                   ConvStr(dtrT!sc_numpar),
                                                   ConvStr(dtrT!sc_integr),
                                                   ConvStr(dtrT!sc_numrata),
                                                   ConvStr(dtrT!codditt))
                End If


            Next

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try

    End Function

    Public Overridable Function AggiornaCoordinate() As Boolean
        Dim dttTmp As New DataTable
        Dim strQuerystring As String = ""
        Dim strConto As String = ""
        Dim retValue As Boolean
        Dim dLat As Double
        Dim dLon As Double
        Dim retCoord As Boolean
        Try


            If Not oCldIbus.GetCliforSenzaCoordinate(strDittaCorrente, dttTmp, strCustomWhereGetCliforSenzaCoordinate) Then Return False

            For Each dtrT As DataRow In dttTmp.Rows
                strQuerystring = ConvStr(dtrT!an_indir) & ", " & ConvStr(dtrT!an_citta) & ", " & ConvStr(dtrT!an_prov) & ", " & ConvStr(dtrT!xx_desstato)
                strConto = ConvStr(dtrT!an_conto)

                retValue = ApexNetLIB.Geocoding.GetCoordinate(strQuerystring, dLat, dLon)

                If retValue Then
                    If dLat > 0 Or dLon > 0 Then
                        retCoord = oCldIbus.UpdateCliforCoordinate(strDittaCorrente, strConto, dLat, dLon)
                    End If

                End If
            Next

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try

    End Function

    Public Overridable Function GetPasswordOperatore(ByVal OpNome As String, ByRef OpPasswd As String) As Boolean
        Dim localPasswd As String = ""
        Try

            If Not oCldIbus.GetPasswdOperatore(OpNome, localPasswd) Then Return False

            OpPasswd = localPasswd

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	

        End Try

    End Function

    Public Overridable Function Elabora_Child() As Boolean
        Try

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try

    End Function

    <Obsolete("Questo metodo è deprecato, usare GestisciEventiEntityGsorg")> _
    Public Overridable Sub GestisciEventiEntityPERS(ByVal sender As Object, ByRef e As NTSEventArgs)
        'Dim fErr As StreamWriter = Nothing
        'Dim fiErr As FileInfo = Nothing
        'Dim strNomeFileLog As String = ""
        Dim strMsg As String = ""
        Try

            'strNomeFileLog = oApp.AscDir & "\eventi-gsor.LOG"
            'fiErr = New FileInfo(strNomeFileLog)
            'If fiErr.Exists Then
            ' If fiErr.Length > 500000 Then fiErr.Delete()
            ' End If
            'fErr = New StreamWriter(strNomeFileLog, True)
            'fErr.Write(e.Message & vbCrLf)
            'fErr.Flush()
            'fErr.Close()

            If e.Message <> "" Then
                strMsg = oApp.Tr(Me, 129877602933085930, e.Message)
                LogWrite(strMsg, False)
                InviaAlert(99, strMsg)
            End If

            'magari poi inserire anche l'istruzione sotto, così un volta loggati li giriamo comunque alla UI
            'GestisciEventiEntity(sender, e)

            'oppure creiamo un alert
            'codice per generare un nuovo alert

        Catch ex As Exception
            Dim strErr As String = GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
        End Try
    End Sub


    Public Overridable Sub GestisciEventiEntityGsor(ByVal sender As Object, ByRef e As NTSEventArgs)
        Try
            'gli eventuali messaggi dati da BEORGSOR tramite la ThrowRemoteEvent li passo a BNIEIBUS
            ThrowRemoteEvent(e)
        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
                'ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128517126976816000, "Una riga a prezzo netto deve avere tutti gli sconti pari a 0.")))
            End If
            '--------------------------------------------------------------	
        End Try
    End Sub

#Region "Export tracciati"

    Public Overridable Function Elabora_ExportCodpaga(ByVal strFileOut As String) As Boolean
        'esporta tutti i codici pagamento
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCodpaga(dttTmp, strCustomWhereGetCodpaga) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codpaga) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codpaga) & "|" & _
                                ConvStr(dtrT!tb_despaga) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportComuni(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetComuni(dttTmp, strCustomWhereGetComuni) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE|CAP|PROVINCIA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!co_codcomu) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!co_codcomu) & "|" & _
                                ConvStr(dtrT!co_denom) & "|" & _
                                ConvStr(dtrT!co_cap) & "|" & _
                                ConvStr(dtrT!co_prov) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportNazioni(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetNazioni(dttTmp, strCustomWhereGetNazioni) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codstat) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codstat) & "|" & _
                                ConvStr(dtrT!tb_desstat) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    ' Esempio di utilizzo di CSV Helper
    'Public Overridable Function Elabora_ExportNazioniTEST(ByVal strFileOut As String) As Boolean


    '    Dim dttTmp As New DataTable

    '    Try
    '        If Not oCldIbus.GetNazioni(dttTmp, strCustomWhereGetNazioni) Then Return False


    '        Dim lista As New List(Of AMHelper.CSV.rec_nazioni)()

    '        For Each dtrT As DataRow In dttTmp.Rows
    '            lista.Add(New AMHelper.CSV.rec_nazioni() With {
    '                 .CHIAVE = strDittaCorrente & "§" & ConvStr(dtrT!tb_codstat),
    '                 .COD_DITTA = strDittaCorrente, _
    '                 .CODICE = ConvStr(dtrT!tb_codstat),
    '                 .DESCRIZIONE = ConvStr(dtrT!tb_desstat)
    '        })

    '        Next

    '        ' Scrivo il file
    '        Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)

    '        Dim writer As New CsvHelper.CsvWriter(w1)

    '        writer.Configuration.Delimiter = "|"
    '        writer.Configuration.HasHeaderRecord = True


    '        Dim row As New AMHelper.CSV.rec_nazioni


    '        For Each row In lista
    '            writer.WriteField(row.CHIAVE)
    '            writer.WriteField(row.COD_DITTA)
    '            writer.NextRecord()

    '        Next
    '        writer.Dispose()

    '        Return True

    '    Catch ex As Exception
    '        '--------------------------------------------------------------
    '        If GestErrorCallThrow() Then
    '            Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
    '        Else
    '            ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
    '        End If
    '        '--------------------------------------------------------------	
    '    Finally
    '        dttTmp.Clear()
    '    End Try
    'End Function


    Public Overridable Function Elabora_ExportClassiSconto(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetClassiSconto(strDittaCorrente, dttTmp, strCustomWhereGetClassiSconto) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codcscl) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcscl) & "|" & _
                                ConvStr(dtrT!tb_descscl) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCanaliVendita(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetCanaliVendita(strDittaCorrente, dttTmp, strCustomWhereGetCanaliVendita) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codcana) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcana) & "|" & _
                                ConvStr(dtrT!tb_descana) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforCate(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetCliforCate(strDittaCorrente, dttTmp, strCustomWhereGetCliforCate) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codcate) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcate) & "|" & _
                                ConvStr(dtrT!tb_descate) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportPorto(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetPorto(strDittaCorrente, dttTmp, strCustomWhereGetPorto) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|CODICE|DESCRIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codport) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codport) & "|" & _
                                ConvStr(dtrT!tb_desport) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCittaXML(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder


        Try
            If Not oCldIbus.GetComuni(dttTmp, strCustomWhereGetComuni) Then Return False

            Dim XMLobj As Xml.XmlTextWriter
            Dim enc As New System.[Text].UnicodeEncoding()
            XMLobj = New Xml.XmlTextWriter(strFileOut.Replace("txt", "xml"), enc)

            XMLobj.Formatting = Xml.Formatting.Indented
            XMLobj.Indentation = 3
            XMLobj.WriteStartDocument()

            XMLobj.WriteStartElement("comuni")
            For Each dtrT As DataRow In dttTmp.Rows

                XMLobj.WriteStartElement("comune")

                'XMLobj.WriteAttributeString("chiave", strDittaCorrente & "§" & ConvStr(dtrT!co_codcomu))
                'XMLobj.WriteAttributeString("cod_ditta", strDittaCorrente)
                'XMLobj.WriteAttributeString("codice", ConvStr(dtrT!co_codcomu))
                'XMLobj.WriteAttributeString("descrizione", ConvStr(dtrT!co_denom))
                'XMLobj.WriteAttributeString("cap", ConvStr(dtrT!co_cap))
                'XMLobj.WriteAttributeString("provincia", ConvStr(dtrT!co_prov))
                'XMLobj.WriteAttributeString("dat_last_change", "")

                XMLobj.WriteStartElement("k")
                XMLobj.WriteString(strDittaCorrente & "§" & ConvStr(dtrT!co_codcomu))
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("ditta")
                XMLobj.WriteString(strDittaCorrente)
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("cod")
                XMLobj.WriteString(ConvStr(dtrT!co_codcomu))
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("desc")
                XMLobj.WriteString(ConvStr(dtrT!co_denom))
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("cap")
                XMLobj.WriteString(ConvStr(dtrT!co_cap))
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("prov")
                XMLobj.WriteString(ConvStr(dtrT!co_prov))
                XMLobj.WriteEndElement()

                XMLobj.WriteStartElement("dlc")
                XMLobj.WriteString("")
                XMLobj.WriteEndElement()


                XMLobj.WriteEndElement()

            Next
            XMLobj.WriteEndElement()

            XMLobj.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportInfo(ByVal strFileOut As String, ByVal BNIEVersion As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetInfo(dttTmp, strReleaseTracciati) Then Return False

            sbFile.Append("DESCRIZIONE|VALORE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!descr) & "|" & ConvStr(dtrT!val) & vbCrLf)
            Next

            sbFile.Append("Assembly|" & BNIEVersion & vbCrLf)

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadCate(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetLeadCate(strDittaCorrente, dttTmp, strCustomWhereGetLeadCate) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_CATEGORIA|DES_CATEGORIA" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcate) & "|" & _
                                ConvStr(dtrT!tb_descate) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadConcorrenti(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetLeadConcorrenti(strDittaCorrente, dttTmp, strCustomWhereGetLeadConcorrenti) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_CONCORRENTE|DES_CONCORRENTE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcptr) & "|" & _
                                ConvStr(dtrT!tb_descptr) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadDetCon(ByVal strFileOut As String) As Boolean
        'esporta l'organizzazione di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeadDetCon(strDittaCorrente, dttTmp, strCustomWhereGetLeadDetCon, _
                 strIncludiLeadConClienti:=strIncludileadClienti) Then Return False

            sbFile.Append(
                "CHIAVE                |" & _
                "COD_DITTA             |" & _
                "COD_LEAD              |" & _
                "COD_CONTATTO          |" & _
                "TIPO_CONTATTO         |" & _
                "COGNOME               |" & _
                "NOME                  |" & _
                "INDIRIZZO             |" & _
                "CAP                   |" & _
                "CITTA                 |" & _
                "PROVINCIA             |" & _
                "TELEFONO              |" & _
                "CELLULARE             |" & _
                "FAX                   |" & _
                "EMAIL                 |" & _
                "DAT_ULT_MOD            " & _
                vbCrLf).Replace(" ", "")


            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(
                                ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!og_codlead) & "|" & _
                                ConvStr(dtrT!og_progr) & "|" & _
                                ConvStr(dtrT!tb_desruaz) & "|" & _
                                (ConvStr(dtrT!og_descont)).Trim & "|" & _
                                (ConvStr(dtrT!og_descont2)).Trim & "|" & _
                                ConvStr(dtrT!og_indir) & "|" & _
                                ConvStr(dtrT!og_cap) & "|" & _
                                ConvStr(dtrT!og_citta) & "|" & _
                                ConvStr(dtrT!og_prov) & "|" & _
                                ConvStr(dtrT!og_telef) & "|" & _
                                ConvStr(dtrT!og_cell) & "|" & _
                                ConvStr(dtrT!og_fax) & "|" & _
                                ConvStr(dtrT!og_email) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeads(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeads(strDittaCorrente, dttTmp, strCustomWhereGetLeads, _
                                     strIncludiLeadConClienti:=strIncludileadClienti) Then Return False

            sbFile.Append(
                "CHIAVE               |" & _
                "COD_DITTA            |" & _
                "COD_LEAD             |" & _
                "COD_CLIFOR           |" & _
                "DESCRIZIONE1         |" & _
                "DESCRIZIONE2         |" & _
                "INDIRIZZO            |" & _
                "CITTA                |" & _
                "CAP                  |" & _
                "PROVINCIA            |" & _
                "DES_PROVINCIA        |" & _
                "DES_NOTE             |" & _
                "PARTITA_IVA          |" & _
                "CODICE_FISCALE       |" & _
                "TELEFONO             |" & _
                "FAX                  |" & _
                "CELLULARE            |" & _
                "EMAIL                |" & _
                "INTERNET             |" & _
                "LATITUDINE           |" & _
                "LONGITUDINE          |" & _
                "FLG_MOD_NEL_DISP     |" & _
                "COD_CATEGORIA        |" & _
                "CATEGORIA            |" & _
                "COD_ZONA             |" & _
                "ZONA                 |" & _
                "DATA_ULT_OFFERTA     |" & _
                "DATA_CREAZIONE       |" & _
                "BANCA1               |" & _
                "BANCA2               |" & _
                "COD_LISTINO          |" & _
                "LISTINO              |" & _
                "COD_PRIVACY          |" & _
                "DES_PRIVACY          |" & _
                "COD_CONTATTATO       |" & _
                "DES_CONTATTATO       |" & _
                "COD_NON_INTERESSATO  |" & _
                "DES_NON_INTERESSATO  |" & _
                "NAZIONE              |" & _
                "COD_STATUS           |" & _
                "STATUS               |" & _
                "DAT_ULT_MOD           " & _
                vbCrLf).Replace(" ", "")

            For Each dtrT As DataRow In dttTmp.Rows

                sbFile.Append(
                           ConvStr(dtrT!xx_chiave) & "|" & _
                           strDittaCorrente & "|" & _
                           ConvStr(dtrT!le_codlead) & "|" & _
                           ConvStr(dtrT!le_conto) & "|" & _
                           ConvStr(dtrT!le_descr1).Trim & "|" & _
                           ConvStr(dtrT!le_descr2).Trim & "|" & _
                           ConvStr(dtrT!le_indir).Trim & "|" & _
                           ConvStr(dtrT!le_citta).Trim & "|" & _
                           ConvStr(dtrT!le_cap).Trim & "|" & _
                           ConvStr(dtrT!le_prov).Trim & "|" & _
                           ConvStr(dtrT!tb_desprov).Trim & "|" & _
                           ConvStr(dtrT!le_note).Trim & "|" & _
                           ConvStr(dtrT!le_pariva) & "|" & _
                           ConvStr(dtrT!le_codfis) & "|" & _
                           ConvStr(dtrT!le_telef).Trim & "|" & _
                           ConvStr(dtrT!le_faxtlx).Trim & "|" & _
                           ConvStr(dtrT!le_cell).Trim & "|" & _
                           ConvStr(dtrT!le_email).Trim & "|" & _
                           ConvStr(dtrT!le_website).Trim & "|" & _
                           "|" & _
                           "|" & _
                           "0|" & _
                           ConvStr(dtrT!tb_codcate) & "|" & _
                           ConvStr(dtrT!tb_descate).Trim & "|" & _
                           ConvStr(dtrT!tb_codzone) & "|" & _
                           ConvStr(dtrT!tb_deszone).Trim & "|" & _
                           ConvData(dtrT!xx_ultoff, False) & "|" & _
                           "|" & _
                           ConvStr(dtrT!le_banc1) & "|" & _
                           ConvStr(dtrT!le_banc2) & "|" & _
                           ConvStr(dtrT!le_listino) & "|" & _
                           ConvStr(dtrT!tb_deslist) & "|" & _
                           ConvStr(dtrT!le_privacy) & "|" & _
                           ConvStr(dtrT!xx_des_privacy) & "|" & _
                           ConvStr(dtrT!le_contattato) & "|" & _
                           ConvStr(dtrT!xx_des_contattato) & "|" & _
                           ConvStr(dtrT!le_nonint) & "|" & _
                           ConvStr(dtrT!xx_des_nonint) & "|" & _
                           ConvStr(dtrT!tb_desstat) & "|" & _
                           ConvStr(dtrT!le_status) & "|" & _
                           ConvStr(dtrT!xx_des_status) & "|" & _
                           ConvData(dtrT!le_ultagg, True) & vbCrLf)

            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally

            dttTmp.Clear()
            dttTmp.Dispose()

        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadAccessi(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetLeadAccessi(strDittaCorrente, dttTmp, strCustomWhereGetLeadAccessi) Then Return False


            Using textWriter As TextWriter = File.CreateText(strFileOut)
                Using csv As CsvHelper.CsvWriter = New CsvHelper.CsvWriter(textWriter)
                    csv.Configuration.Delimiter = "|"
                    csv.Configuration.Encoding = Encoding.UTF8

                    csv.WriteField("CHIAVE")
                    csv.WriteField("COD_DITTA")
                    csv.WriteField("COD_OPERATORE")
                    csv.WriteField("COD_LEAD")
                    csv.WriteField("FLG_VISUALIZZA")
                    csv.WriteField("FLG_MODIFICA")
                    csv.WriteField("DAT_ULT_MOD")
                    csv.NextRecord()

                    ' Write row values
                    For Each dtrT As DataRow In dttTmp.Rows

                        csv.WriteField(ConvStr(dtrT!xx_chiave))
                        csv.WriteField(strDittaCorrente)
                        csv.WriteField(ConvStr(dtrT!opcr_opnome))
                        csv.WriteField(ConvStr(dtrT!opcr_codlead))
                        csv.WriteField(ConvStr(dtrT!opcr_crmvis))
                        csv.WriteField(ConvStr(dtrT!opcr_crmmod))
                        csv.WriteField(ConvData(dtrT!xx_ultagg, True))
                
                        csv.NextRecord()
                    Next
                End Using
            End Using

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function


    Public Overridable Function Elabora_ExportLeadAccessiCrm(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeadAccessiCrm(strDittaCorrente, dttTmp, strCustomWhereGetLeadAccessiCrm) Then Return False

            sbFile.Append(
                "CHIAVE               |" & _
                "COD_DITTA            |" & _
                "COD_OPERATORE        |" & _
                "COD_OPERATORE_FIGLIO |" & _
                "FLG_VISUALIZZA       |" & _
                "FLG_MODIFICA         |" & _
                "DAT_ULT_MOD           " & _
                vbCrLf).Replace(" ", "")

            For Each dtrT As DataRow In dttTmp.Rows

                sbFile.Append(
                            ConvStr(dtrT!xx_chiave) & "|" & _
                            strDittaCorrente & "|" & _
                            ConvStr(dtrT!opcr_opnome) & "|" & _
                            ConvStr(dtrT!opcr_alopnome) & "|" & _
                            ConvStr(dtrT!opcr_crmvis) & "|" & _
                            ConvStr(dtrT!opcr_crmmod) & "|" & _
                            ConvData(dtrT!xx_ultagg, True) & vbCrLf)

            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadTestOff(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeadTestOff(strDittaCorrente, dttTmp, strIncludileadClienti, strCustomWhereGetLeadTestOff, _
                                            strFiltroGiorniOfferte:=strFiltroGGOfferte) Then Return False

            sbFile.Append(
                "CHIAVE                  |" & _
                "COD_DITTA               |" & _
                "COD_LEAD                |" & _
                "COD_CLIFOR              |" & _
                "ANNO                    |" & _
                "SERIE                   |" & _
                "NUMERO                  |" & _
                "REVISIONE               |" & _
                "DATA                    |" & _
                "REFERENTE               |" & _
                "RIFERIMENTO             |" & _
                "OGGETTO                 |" & _
                "COD_OPPORTUNITA         |" & _
                "DES_OPPORTUNITA         |" & _
                "GG_VALIDITA             |" & _
                "COD_TIPO_OPERAZIONE     |" & _
                "DES_TIPO_OPERAZIONE     |" & _
                "COD_LISTINO             |" & _
                "DES_LISTINO             |" & _
                "COD_PAGAMENTO           |" & _
                "DES_PAGAMENTO           |" & _
                "DATA_CONSEGNA           |" & _
                "GG_CONSEGNA             |" & _
                "DES_CONSEGNA            |" & _
                "NOTE                    |" & _
                "RILASCIATO              |" & _
                "STAMPATO                |" & _
                "CONFERMATO              |" & _
                "RIFIUTATO               |" & _
                "EVASO                   |" & _
                "ANNULLATO               |" & _
                "CHIUSO                  |" & _
                "COD_VALUTA              |" & _
                "TOTALE_OFFERTA          |" & _
                "DAT_ULT_MOD              " & _
                vbCrLf).Replace(" ", "")

            For Each dtrT As DataRow In dttTmp.Rows

                sbFile.Append(
                            ConvStr(dtrT!xx_chiave) & "|" & _
                            strDittaCorrente & "|" & _
                            ConvStr(dtrT!td_codlead) & "|" & _
                            "" & "|" & _
                            ConvStr(dtrT!td_anno) & "|" & _
                            ConvStr(dtrT!td_serie) & "|" & _
                            ConvStr(dtrT!td_numord) & "|" & _
                            ConvStr(dtrT!td_vers) & "|" & _
                            ConvData(dtrT!td_datord) & "|" & _
                            ConvStr(dtrT!td_ca) & "|" & _
                            ConvStr(dtrT!td_riferim) & "|" & _
                            ConvStr(dtrT!td_oggetto) & "|" & _
                            ConvStr(dtrT!td_codoppo) & "|" & _
                            ConvStr(dtrT!op_oggetto) & "|" & _
                            ConvStr(dtrT!td_validgg) & "|" & _
                            ConvStr(dtrT!td_tipobf) & "|" & _
                            ConvStr(dtrT!tb_destpbf) & "|" & _
                            ConvStr(dtrT!td_listino) & "|" & _
                            ConvStr(dtrT!tb_deslist) & "|" & _
                            ConvStr(dtrT!td_codpaga) & "|" & _
                            ConvStr(dtrT!tb_despaga) & "|" & _
                            ConvData(dtrT!td_datcons) & "|" & _
                            ConvStr(dtrT!td_consggconf) & "|" & _
                            ConvStr(dtrT!td_desconsx) & "|" & _
                            ConvStr(dtrT!td_note, True) & "|" & _
                            ConvStr(dtrT!td_rilasciato) & "|" & _
                            ConvStr(dtrT!td_flstam) & "|" & _
                            ConvStr(dtrT!td_confermato) & "|" & _
                            ConvStr(dtrT!td_abband) & "|" & _
                            ConvStr(dtrT!td_flevas) & "|" & _
                            ConvStr(dtrT!td_annull) & "|" & _
                            ConvStr(dtrT!td_chiuso) & "|" & _
                            ConvStr(dtrT!td_valuta) & "|" & _
                            ConvStr(dtrT!td_totdoc) & "|" & _
                            ConvData(dtrT!xx_ultagg, True) & vbCrLf)

            Next
            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadMansioni(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetLeadMansioni(strDittaCorrente, dttTmp, strCustomWhereGetLeadMansioni) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_MANSIONE|DES_MANSIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tb_codruaz) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codruaz) & "|" & _
                                ConvStr(dtrT!tb_desruaz) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadModAcquisizione(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetLeadModAcquisizione(strDittaCorrente, dttTmp, strCustomWhereGetLeadModAcquisizione) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_ACQUISIZIONE|DES_ACQUISIZIONE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codperv) & "|" & _
                                ConvStr(dtrT!tb_desperv) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadNote(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeadNote(strDittaCorrente, dttTmp, strCustomWhereGetLeadNote) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_LEAD|PROGRESSIVO|TIPO_NOTA|NOTA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(
                                ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!le_codlead) & "|" & _
                                ConvStr(dtrT!xx_progressivo) & "|" & _
                                ConvStr(dtrT!xx_codnote) & "|" & _
                                Left(ConvStr(dtrT!xx_desnote, True), 4000) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadRighOff(ByVal strFileOut As String) As Boolean
        'restituisco le righe dei documenti degli ultimi 3 anni di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetLeadRighOff(strDittaCorrente, dttTmp, strIncludileadClienti, strCustomWhereGetLeadRighOff, _
                                            strFiltroGiorniOfferte:=strFiltroGGOfferte) Then Return False

            sbFile.Append(
            "CHIAVE         |" & _
            "CHIAVE_TESTATA |" & _
            "COD_DITTA      |" & _
            "COD_LEAD       |" & _
            "PRG_RIGA       |" & _
            "COD_RIGA       |" & _
            "DES_RIGA       |" & _
            "COD_UM         |" & _
            "QTA            |" & _
            "PRZ_LORDO      |" & _
            "PRZ_NETTO      |" & _
            "COD_VALUTA     |" & _
            "IMPORTO        |" & _
            "SC_1           |" & _
            "SC_2           |" & _
            "SC_3           |" & _
            "SC_4           |" & _
            "SC_5           |" & _
            "SC_6           |" & _
            "DATA_CONFERMA  |" & _
            "COD_CLIFOR     |" & _
            "DAT_ULT_MOD    " & _
            vbCrLf).Replace(" ", "")


            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(
                                ConvStr(dtrT!xx_chiave) & "|" & _
                                ConvStr(dtrT!xx_chiave_testata) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!td_codlead) & "|" & _
                                ConvStr(dtrT!mo_riga) & "|" & _
                                (ConvStr(dtrT!mo_codart) & IIf(NTSCStr(dtrT!mo_fase) <> "0", "." & ConvStr(dtrT!mo_fase), "").ToString).Trim & "|" & _
                                (ConvStr(dtrT!mo_descr) & " " & ConvStr(dtrT!mo_desint, True).Trim) & "|" & _
                                ConvStr(dtrT!mo_ump) & "|" & _
                                NTSCDec(dtrT!mo_quant).ToString("0.00000") & "|" & _
                                NTSCDec(dtrT!mo_prezzo).ToString("0.0000") & "|" & _
                                NTSCDec(dtrT!xx_prezzo).ToString("0.0000") & "|" & _
                                ConvStr(dtrT!td_valuta) & "|" & _
                                NTSCDec(dtrT!mo_valore).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont1).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont2).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont3).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont4).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont5).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont6).ToString("0.00") & "|" & _
                                ConvData(dtrT!td_datconf) & "|" & _
                                ConvStr(dtrT!td_codlead) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforGen(ByVal TipoCliFor As String, ByVal strFileOut As String) As Boolean
        'esporta tutti i clienti/fornitori ATTIVI o POTENZIALI con relativi dati associati
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim strBlocco As String = ""
        Try
            If Not oCldIbus.GetClifor(TipoCliFor, strDittaCorrente, dttTmp, strFiltroCliConAgenti, strCustomWhereGetClifor) Then Return False


            sbFile.Append(
                        "CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|RAG_SOC|INDIRIZZO|PARTITA_IVA|CODICE_FISCALE|TELEFONO1|TELEFONO2|" & _
                        "FAX|CELLULARE|EMAIL|INTERNET|CAP|CITTA|PROVINCIA|LATITUDINE|LONGITUDINE|COD_CLASSE_SCONTO|" & _
                        "FLG_MOD_NEL_DISP|FLG_DEPERIBILITA|COD_CAT_EXTRA_SCONTO|NAZIONE|PAGAMENTO|BANCA|AGENZIA|LISTINO_ANAGRAFICO|VALUTA|" & _
                        "SCONTI_ANAG_PERC|SCONTI_ANAG_IMP|" & _
                        "MAGGIORAZIONE_ANAG_PERC|SCONTO_PIEDE|COD_LISTINO|COD_CONDPAG|COD_VALUTA|MACROAREA|DATA_CREAZIONE|" & _
                        "ZONA|COD_CATEGORIA|CATEGORIA|DATA_ULT_DOC_FT|DATA_ULT_ORDINE|" & _
                        "FIDO_AZIENDALE|FLG_NEW_CLIFOR|TAG|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(
                            strDittaCorrente & "§" & ConvStr(dtrT!an_conto) & "|" & _
                            strDittaCorrente & "|" & _
                            IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                            ConvStr(dtrT!an_conto) & "|" & _
                            (ConvStr(dtrT!an_descr1).Trim & " " & ConvStr(dtrT!an_descr2)).Trim & "|" & _
                            ConvStr(dtrT!an_indir).Trim & "|" & _
                            ConvStr(dtrT!an_pariva) & "|" & _
                            ConvStr(dtrT!an_codfis) & "|" & _
                            ConvStr(dtrT!an_telef) & "|" & _
                            "" & "|" & _
                                ConvStr(dtrT!an_faxtlx) & "|" & _
                                ConvStr(dtrT!an_cell) & "|" & _
                                ConvStr(dtrT!an_email) & "|" & _
                                ConvStr(dtrT!an_website) & "|" & _
                                ConvStr(dtrT!an_cap) & "|" & _
                                ConvStr(dtrT!an_citta) & "|" & _
                                ConvStr(dtrT!an_prov) & "|" & _
                                ConvStr(dtrT!an_latitud) & "|" & _
                                ConvStr(dtrT!an_longitud) & "|" & _
                                dtrT!an_clascon.ToString & "|" & _
                            IIf(strDisattivaModificaDatiCliente = "0", ConvStr(dtrT!xx_flg_mod_nel_disp), "0").ToString & "|" & _
                            ConvStr(dtrT!xx_flg_deperibilita) & "|" & _
                            "" & "|" & _
                            ConvStr(dtrT!tb_desstat) & "|" & _
                            ConvStr(dtrT!tb_despaga) & "|" & _
                            ConvStr(dtrT!an_banc1) & "|" & _
                            ConvStr(dtrT!an_banc2) & "|" & _
                            (ConvStr(dtrT!an_listino) & " - " & ConvStr(dtrT!tb_deslist)).Trim & "|" & _
                            ConvStr(dtrT!tb_desvalu) & "|" & _
                            "0" & "|" & _
                                "0" & "|" & _
                                "0" & "|" & _
                                NTSCDec(dtrT!tb_scopaga).ToString(oApp.FormatSconti) & "|" & _
                                ConvStr(dtrT!an_listino) & "|" & _
                                ConvStr(dtrT!an_codpag) & "|" & _
                                ConvStr(dtrT!an_valuta) & "|" & _
                            ConvStr(dtrT!tb_descana) & "|" & _
                            ConvData(dtrT!an_dtaper, False) & "|" & _
                            ConvStr(dtrT!tb_deszone) & "|" & _
                            ConvStr(dtrT!xx_categ) & "|" & _
                            ConvStr(dtrT!tb_descate) & "|" & _
                            ConvData(dtrT!xx_ultfatt, False) & "|" & _
                            ConvData(dtrT!xx_ultord, False) & "|" & _
                            NTSCDec(dtrT!an_fido).ToString(oApp.FormatSconti) & "|" & _
                            ConvStr(dtrT!xx_flg_new) & "|" & _
                            ConvStr(dtrT!an_siglaric) & "|" & _
                            ConvData(dtrT!an_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As StreamWriter

                w1 = New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If



            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforBlo(ByVal TipoCliFor As String, ByVal strFileOut As String) As Boolean

        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Dim strBlocco As String = ""

        Try
            If Not oCldIbus.GetCliforBlo(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforBlo) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|COD_BLOCCO|TIPO_BLOCCO|NOTA_BLOCCO|DATA_BLOCCO|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!an_conto) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                "0" & "|" & _
                                ConvStr(dtrT!an_blocco) & "|" & _
                                ConvStr(dtrT!xx_blocco) & "|" & _
                                "" & "|" & _
                                ConvData(dtrT!an_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As StreamWriter

                w1 = New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforDestdiv(ByVal strFileOut As String) As Boolean
        'esporta tutte le destinazioni diverse di clienti/fornitori ATTIVI o POTENZIALI 
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCliforDestdiv(strDittaCorrente, dttTmp, strCustomWhereGetCliforDestdiv) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|PREFERENZIALE|COD_DEST|RAG_SOC_DEST|" & _
                        "INDIRIZZO|CAP|CITTA|PROVINCIA|STAMPA_PREF_DOC|TELEFONO|CELLULARE|" & _
                        "MAIL|FAX|NOTE_DEST|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!dd_conto) & "§" & ConvStr(dtrT!dd_coddest) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!dd_conto) & "|" & _
                                ConvStr(dtrT!xx_preferenziale) & "|" & _
                                ConvStr(dtrT!dd_coddest) & "|" & _
                                ConvStr(dtrT!dd_nomdest) & "|" & _
                                ConvStr(dtrT!dd_inddest) & "|" & _
                                ConvStr(dtrT!dd_capdest) & "|" & _
                                ConvStr(dtrT!dd_locdest) & "|" & _
                                ConvStr(dtrT!dd_prodest) & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!dd_telef) & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!dd_email) & "|" & _
                                ConvStr(dtrT!dd_faxtlx) & "|" & _
                                ConvStr(dtrT!dd_note, True) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforAge(ByVal strFileOut As String) As Boolean
        'esporta gli agenti di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCliforAge(strDittaCorrente, dttTmp, strCustomWhereGetCliforAge) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|COD_AGE|RAGSOC_AGE|PREFERENZIALE|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                ConvStr(dtrT!xx_agente) & "|" & _
                                ConvStr(dtrT!tb_descage) & "|" & _
                                ConvStr(dtrT!xx_prefer) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforDettCon(ByVal TipoCliFor As String, ByVal strFileOut As String) As Boolean
        'esporta l'organizzazione di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCliforDettCon(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforDettCon) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|ID_CONTATTO|TIPO_CONTATTO|PREF|" & _
                        "COGNOME_NOME|NOME|INDIRIZZO|CAP|CITTA|PR|ORARIO_LAVORO|TELEFONO1|TELEFONO2|" & _
                        "CELLULARE1|CELLULARE2|TELEF_CASA|FAX|EMAIL1|EMAIL2|ALTRO_INDIRIZZO1|" & _
                        "ALTRO_INDIRIZZO2|NOTE|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!an_conto) & "§" & ConvStr(dtrT!og_progr) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                ConvStr(dtrT!og_progr) & "|" & _
                                ConvStr(dtrT!tb_desruaz) & "|" & _
                                "1" & "|" & _
                                (ConvStr(dtrT!og_descont)).Trim & "|" & _
                                (ConvStr(dtrT!og_descont2)).Trim & "|" & _
                                ConvStr(dtrT!og_indir) & "|" & _
                                ConvStr(dtrT!og_cap) & "|" & _
                                ConvStr(dtrT!og_citta) & "|" & _
                                ConvStr(dtrT!og_prov) & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!og_telef) & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!og_cell) & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!og_fax) & "|" & _
                                ConvStr(dtrT!og_email) & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next
            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforFatt(ByVal strFileOut As String) As Boolean
        'restituisco il fatturato degli ultimi 3 anno diviso per mese di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCliforFatt(strDittaCorrente, dttTmp, strCustomWhereGetCliforFatt) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|ANNO|MESE|COD_VALUTA|FATTURATO|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!xx_codvalu) & "§" & ConvStr(dtrT!an_tipo) & "§" & ConvStr(dtrT!an_conto) & "§" & ConvStr(dtrT!xx_anno) & "§" & ConvStr(dtrT!xx_mese) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                ConvStr(dtrT!xx_anno) & "|" & _
                                ConvStr(dtrT!xx_mese) & "|" & _
                                ConvStr(dtrT!xx_codvalu) & "|" & _
                                NTSCDec(dtrT!xx_fatturato).ToString(oApp.FormatSconti) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)


            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforTestDoc(ByVal TipoCliFor As String, ByVal strFileOut As String, ByRef dttTmp As DataTable) As Boolean
        'restituisco le testate dei documenti di magazino/ordini di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim sbFile As New StringBuilder
        Dim strTipoDoc As String = ""
        Dim strDescEvas As String = ""
        Dim strKeyNumReg As String = ""
        Try
            ' I filtri sui documenti che vengono fatti nell'iPad sono basati sulle distinct dei seguenti campi:
            '
            ' - COD_TIPODOC   con decodifica descrittiva del campo TIPO          (Es: 68)
            ' - COD_STIPODOC  con decodifica descrittiva del campo SOTTOTIPO     (Es: DDT emesso, DDT Ricevuto, ecc.)
            ' - TIPOSTATO_DOC con decodifica descrittiva del campo DESSTATO_DOC  (Es: Evaso, Da evadere, ecc.)
            '
            ' Personalizzando i valori di queste 3 coppie di campi si possono modificare i filtri.

            If Not oCldIbus.GetCliforTestDoc(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforTestDoc,
                                            strGiorniStoricoDocumenti:=strFiltroGGDocumenti) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|COD_TIPODOC|COD_STIPODOC|" & _
                        "DATA_DOC|NUMREG|TIPODOC|TIPO|SOTTOTIPO|SEZIONALE|NUM_DOC|" & _
                        "DEPOSITO|TOTALEDOC|TIPOSTATO_DOC|DESSTATO_DOC|DATA_FATT|NUM_FATT|" & _
                        "DES_DOC|DES_NOTE|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Rows

                Select Case ConvStr(dtrT!xx_flevas)
                    Case "0" : strDescEvas = "Da evadere"
                    Case "1" : strDescEvas = "Evaso"
                End Select

                ' Se il tipodoc  puo' contenere scadenze aggiungo il tipo doc per rendere univoca la chiave
                If oCldIbus.TipiDocConScadenze(NTSCStr(dtrT!tm_tipork)) Then
                    strKeyNumReg = ConvStr(dtrT!xx_numreg)
                Else
                    strKeyNumReg = ConvStr(dtrT!xx_numreg) & "§" & ConvStr(dtrT!tm_tipork)
                End If

                ' Decodifico il tipo doc
                oCldIbus.DecodeDBValue("tipork", NTSCStr(dtrT!tm_tipork), strTipoDoc)

                ' ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "|" & _
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                Asc(ConvStr(dtrT!tm_tipork)) & "|" & _
                                ConvStr(dtrT!xx_tipobf) & "|" & _
                                ConvData(dtrT!tm_datdoc, False) & "|" & _
                                strKeyNumReg & "|" & _
                                strKeyNumReg & "|" & _
                                strTipoDoc.PadRight(20).Substring(0, 20).Trim & "|" & _
                                ConvStr(dtrT!tb_destpbf).PadRight(20).Substring(0, 20).Trim & "|" & _
                                ConvStr(dtrT!tm_serie) & "|" & _
                                 (ConvStr(dtrT!tm_numdoc) & IIf(NTSCStr(dtrT!tm_serie) <> " ", "/" & ConvStr(dtrT!tm_serie), "").ToString).Trim & "|" & _
                                ConvStr(dtrT!tm_magaz) & "|" & _
                                NTSCDec(dtrT!tm_totdoc).ToString(oApp.FormatSconti) & "|" & _
                                ConvStr(dtrT!xx_flevas) & "|" & _
                                strDescEvas & "|" & _
                                ConvData(dtrT!tm_datfat, False) & "|" & _
                                (ConvStr(dtrT!tm_numfat) & IIf(NTSCStr(dtrT!tm_alffat) <> " ", "/" & ConvStr(dtrT!tm_alffat), "").ToString).Trim & "|" & _
                                ConvStr(dtrT!tm_riferim) & "|" & _
                                ConvStr(dtrT!tm_note) & "|" & _
                                ConvStr(dtrT!tm_valuta) & "|" & _
                                ConvData(dtrT!tm_ultagg, True) & vbCrLf)

                ' ConvStr(dtrT!tm_note).PadRight(1000).Substring(0, 1000).Trim & "|" & _
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforNote(ByVal TipoCliFor As String, ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCliforNote(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforNote) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|PROGRESSIVO|TIPO_NOTA|NOTA|DAT_ULT_MOD" & vbCrLf)

            For Each dtrTmp As DataRow In dttTmp.Rows
                'FILE note
                sbFile.Append(
                                ConvStr(dtrTmp!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrTmp!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrTmp!an_conto) & "|" & _
                                ConvStr(dtrTmp!xx_progressivo) & "|" & _
                                Left(ConvStr(dtrTmp!xx_codnote, True).Trim, 3900) & "|" & _
                                ConvStr(dtrTmp!xx_desnote, True) & "|" & _
                                ConvData(dtrTmp!xx_ultagg, True) & vbCrLf)

            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()

            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforRighDoc(ByVal TipoCliFor As String, ByVal strFileOut As String) As Boolean
        'restituisco le righe dei documenti degli ultimi 3 anni di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim descOmaggio As String = ""
        Dim RetValOmaggio As Boolean
        Dim strKeyNumReg As String

        Try
            If Not oCldIbus.GetCliforRighDoc(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforRighDoc,
                                            strGiorniStoricoDocumenti:=strFiltroGGDocumenti) Then Return False

            'ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "|" & _
            sbFile.Append("CHIAVE|COD_DITTA|NUM_REG|COD_ART|PRG_RIGA|COD_RIGA|DES_RIGA|COD_UM|QTA|PRZ_LORDO|PRZ_NETTO|IMPORTO|" & _
                        "SC_1|SC_2|TIPO_CLIFOR|COD_CLIFOR|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows

                RetValOmaggio = oCldIbus.DecodeDBValue("stasino", ConvStr(dtrT!mm_stasino), descOmaggio)
                If descOmaggio <> "" Then descOmaggio = " - (" & descOmaggio & ")"


                ' Se il tipodoc  puo' contenere scadenze aggiungo il tipo doc per rendere univoca la chiave
                If oCldIbus.TipiDocConScadenze(NTSCStr(dtrT!tm_tipork)) Then
                    strKeyNumReg = ConvStr(dtrT!xx_numreg)
                Else
                    strKeyNumReg = ConvStr(dtrT!xx_numreg) & "§" & ConvStr(dtrT!tm_tipork)
                End If


                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "§" & ConvStr(dtrT!tm_numdoc1) & "§" & ConvStr(dtrT!mm_riga) & "§" & ConvStr(dtrT!mm_codart) & "|" & _
                                strDittaCorrente & "|" & _
                                strKeyNumReg & "|" & _
                                ConvStr(dtrT!xx_codart) & "|" & _
                                ConvStr(dtrT!mm_riga) & "|" & _
                                (ConvStr(dtrT!mm_codart) & IIf(NTSCStr(dtrT!mm_fase) <> "0", "." & ConvStr(dtrT!mm_fase), "").ToString).Trim & "|" & _
                                (ConvStr(dtrT!mm_descr) & " " & ConvStr(dtrT!mm_desint, True)).Trim & descOmaggio & "|" & _
                                ConvStr(dtrT!mm_ump) & "|" & _
                                NTSCDec(dtrT!mm_quant).ToString("0.00000") & "|" & _
                                "0" & "|" & _
                                NTSCDec(dtrT!xx_prezzo).ToString("0.0000") & "|" & _
                                NTSCDec(dtrT!mm_valore).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mm_scont1).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mm_scont2).ToString("0.00") & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                ConvStr(dtrT!tm_valuta) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforScadoc(ByVal TipoCliFor As String, ByVal strFileOut As String, ByRef dttTm As DataTable) As Boolean
        'restituisco le scadenze di cliente/fornitore ATTIVO o POTENZIALE
        'per collegare le scadenze ai relativi documenti precedentemente esportati, ...
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim DesScad As String = ""

        Try
            If Not oCldIbus.GetCliforScaDoc(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforScaDoc) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|NUM_REG|COD_RATA|DAT_SCAD|IMPORTO|NETTO_PREV|DES_TIPO|DES_STATO|DES_TIPO_PRES|DES_OPERAZIONE|" & _
                        "FLG_DA_LIB|FLG_SOSP|DES_BANCA_AGENZIA|TIPO_CLIFOR|COD_CLIFOR|COD_VALUTA|DATA_DOC|NUM_DOC|DES_SCAD|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Rows

                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!an_conto) & "§" & ConvStr(dtrT!sc_annpar) & "§" & ConvStr(dtrT!sc_alfpar) & "§" & ConvStr(dtrT!sc_numdoc) & "§" & ConvStr(dtrT!sc_numpar) & "§" & ConvStr(dtrT!sc_integr) & "§" & ConvStr(dtrT!sc_numrata) & "|" & _
                                    strDittaCorrente & "|" & _
                                    ConvStr(dtrT!xx_numreg) & "|" & _
                                    ConvStr(dtrT!sc_numrata) & "|" & _
                                    ConvData(dtrT!sc_datsca, False) & "|" & _
                                    NTSCDec(dtrT!sc_importo).ToString(oApp.FormatSconti) & "|" & _
                                    "0" & "|" & _
                                    ConvStr(dtrT!tb_despaga) & "|" & _
                                    ConvStr(dtrT!xx_flsaldato) & "|" & _
                                    "" & "|" & _
                                    ConvStr(IIf(dtrT!sc_insolu.ToString = "S", "Insol.", "").ToString) & "|" & _
                                    "0" & "|" & _
                                    "0" & "|" & _
                                    (ConvStr(dtrT!sc_banc1) & " - " & ConvStr(dtrT!sc_banc2)).Trim & "|" & _
                                    IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                    ConvStr(dtrT!an_conto) & "|" & _
                                    ConvStr(dtrT!sc_codvalu) & "|" & _
                                    ConvData(dtrT!sc_datdoc, False) & "|" & _
                                    (ConvStr(dtrT!sc_numdoc) & IIf(NTSCStr(dtrT!sc_alfpar) <> " ", "/" & ConvStr(dtrT!sc_alfpar), "").ToString).Trim & "|" & _
                                    ConvStr(dtrT!xx_des_scad) & "|" & _
                                    ConvData(dtrT!xx_ultagg, True) & vbCrLf)

                ' dtrT!sc_numdoc.ToString & "|" & _
            Next
            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportAgenti(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetAgenti(strDittaCorrente, dttTmp, strCustomWhereGetAgenti) Then Return False

            ' La chiave del datatable deve essere univoca. Meglio verificare l'integrità all'origine
            Dim uc As New UniqueConstraint(dttTmp.Columns("xx_chiave"))
            dttTmp.Constraints.Add(uc)

            sbFile.Append("CHIAVE|COD_DITTA|COD_AGENTE|DES_AGENTE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcage) & "|" & _
                                ConvStr(dtrT!tb_descage) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArt(ByVal strFileOut As String, ByVal strCustomQuery As String) As Boolean

        'esporta gli articoli (e relative fasi) NO articoli TCO
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim sbFileUM As New StringBuilder
        Dim PrezzoMinimoDiVendita As Decimal
        Dim UltimoCosto As Decimal
        Dim TrovatoPrezzo As Boolean
        Dim scontoMax As String = ""

        Dim L1 As String = ""
        Dim L2 As String = ""
        Dim L3 As String = ""


        Try
            If Not oCldIbus.GetArt(strDittaCorrente, dttTmp, strCustomQuery, strCustomWhereGetArt) Then Return False

            If strBloccaSceltaUM <> "0" Then
                strBloccaSceltaUM = "-1"
            End If

            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|DES_ART|COD_FAM|DES_FAM|COD_SFAM|DES_SFAM|COD_GRUPPO1|DES_GRUPPO1|" & _
                          "COD_GRUPPO2|DES_GRUPPO2|UM1|UM2|FATTORE_CONVERSIONE|DES_GR_STAT1|DES_GR_STAT2|QTA_MIN_VEND|" & _
                          "COD_CLASSE_SCONTO|COD_DEPERIBILITA|PREZZO_MIN_VEN|SCONTO_MAX_VEN|MAX_EXTRA_SCONTO|DES_FILTRO1|DES_FILTRO2|DES_FILTRO3|FLG_LOCK_UM_DEFAULT|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Rows

                ' Modifica temporanea per gestire il passaggio fino alla 18.2.6.2
                If dtrT.Table.Columns("des_livello1") IsNot Nothing Then
                    L1 = ConvStr(dtrT!des_livello1)
                    L2 = ConvStr(dtrT!des_livello2)
                    L3 = ConvStr(dtrT!des_livello3)
                Else
                    L1 = ConvStr(dtrT!tb_desmarc)
                    L2 = ConvStr(dtrT!tb_desgmer)
                    L3 = ConvStr(dtrT!tb_dessgme)
                End If

                ' PrezzoMinimoDiVendita
                ' ----------------------
                PrezzoMinimoDiVendita = 0

                If strPercentualeSuPrezzoMinimoVendita <> "0" And Integer.TryParse(strPercentualeSuPrezzoMinimoVendita, Nothing) Then
                    TrovatoPrezzo = oCldIbus.FindArtUltCost(strDittaCorrente, ConvStr(dtrT!ar_codart), UltimoCosto)

                    If TrovatoPrezzo Then
                        PrezzoMinimoDiVendita = UltimoCosto + ((UltimoCosto / 100) * CInt(strPercentualeSuPrezzoMinimoVendita))
                    End If
                End If


                ' Sconto massimo in percentuale
                ' ------------------------------
                scontoMax = ""

                ' Se lo sconto max non è stato impostato (da chiave registro)
                If strScontoMaxPercentuale = "" Or strScontoMaxPercentuale = "0" Then
                    ' ... valuto lo sconto max ritornato dalla query. Se anch'esso non è impostato (0 o empty string)
                    If ConvStr(dtrT!xx_sconto_max_ven) = "" Or ConvStr(dtrT!xx_sconto_max_ven) = "0" Then
                        ' per me vale null
                        scontoMax = ""
                    Else
                        scontoMax = NTSCDec(dtrT!xx_sconto_max_ven).ToString("0.0000")
                    End If
                Else
                    ' altrimenti vale come il parametro
                    scontoMax = strScontoMaxPercentuale
                End If

                sbFile.Append(strDittaCorrente & "§" & NTSCStr(dtrT!ar_codart) & "|" & _
                              strDittaCorrente & "|" & _
                              ConvStr(dtrT!ar_codart) & "|" & _
                              (ConvStr(dtrT!ar_descr) & " " & ConvStr(dtrT!ar_desint) & " " & ConvStr(dtrT!af_descr)).Trim & "|" & _
                              ConvStr(dtrT!ar_gruppo) & "|" & _
                              ConvStr(dtrT!tb_desgmer) & "|" & _
                              ConvStr(dtrT!ar_sotgru) & "|" & _
                              ConvStr(dtrT!tb_dessgme) & "|" & _
                              ConvStr(dtrT!ar_famprod) & "|" & _
                              ConvStr(dtrT!tb_descfam) & "|" & _
                              "" & "|" & _
                              "" & "|" & _
                              ConvStr(dtrT!ar_unmis) & "|" & _
                              ConvStr(dtrT!ar_unmis2) & "|" & _
                              NTSCDec(dtrT!ar_conver).ToString("0.0000") & "|" & _
                              "" & "|" & _
                              "" & "|" & _
                              "0" & "|" & _
                              dtrT!ar_clascon.ToString & "|" & _
                              ConvStr(dtrT!ar_tipo) & "|" & _
                              NTSCDec(PrezzoMinimoDiVendita).ToString("0.0000") & "|" & _
                              scontoMax & "|" & _
                              "0,000000" & "|" & _
                              L1 & "|" & _
                              L2 & "|" & _
                              L3 & "|" & _
                              strBloccaSceltaUM & "|" & _
                              ConvData(dtrT!ar_ultagg, True) & vbCrLf)
            Next

            Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
            w1.Write(sbFile.ToString)
            w1.Flush()
            w1.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArtUM(ByVal strFileOut As String, ByVal strCustomQuery As String) As Boolean

        'esporta gli articoli (e relative fasi) NO articoli TCO
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim my_ar_conver As String
        Dim my_ar_qtaconf2 As String
        Dim scontoMax As String = ""
        Dim UMVendita As String = "P"

        Try
            If Not oCldIbus.GetArtUM(strDittaCorrente, dttTmp, strCustomQuery, strCustomWhereGetArtUM) Then Return False

            'IB_ART_UM.DAT
            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|UM|DESC_UM|FAT_CONV|TIPO_UM|FLG_DEFAULT|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Select("1=1", "ar_codart")

                If strUsaUMVendita = "1" Then
                    UMVendita = ConvStr(dtrT!ar_umdapr)
                Else
                    UMVendita = "P"
                End If

                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§1" & "|" & _
                          strDittaCorrente & "|" & _
                          ConvStr(dtrT!ar_codart) & "|" & _
                          ConvStr(dtrT!ar_unmis) & "|" & _
                          ConvStr(dtrT!tb_desunmis) & "|" & _
                          "" & "|" & _
                          "1" & "|" & _
                          CStr(IIf(UMVendita = "P" Or UMVendita = "F", "-1", "0")) & "|" & _
                          ConvData(dtrT!ar_ultagg, True) & vbCrLf)

                If ConvStr(dtrT!ar_unmis2) <> "" Then
                    ' Estrae la seconda unità di misura solo se ne è stato impostato il fattore di conversione
                    ' oppure solo se ne forzo l'estrazione con l'opportuno parametro QueryGetArtUM_EstraiTutte = 1
                    If (NTSCDec(dtrT!ar_conver) <> NTSCDec(0)) Or (strCustomQueryGetArtUM_EstraiTutte <> "0") Then

                        If ConvStr(dtrT!ar_conver) = "0" Then
                            my_ar_conver = "1"
                        Else
                            my_ar_conver = (1 / NTSCDec(dtrT!ar_conver)).ToString("0.00000000")
                        End If

                        sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§2" & "|" &
                                  strDittaCorrente & "|" & _
                                  ConvStr(dtrT!ar_codart) & "|" & _
                                  ConvStr(dtrT!ar_unmis2) & "|" & _
                                  ConvStr(dtrT!tb_desunmis2) & " (" & ConvStr(dtrT!ar_conver) & ")" & "|" & _
                                  my_ar_conver & "|" & _
                                  "2" & "|" & _
                                  CStr(IIf(UMVendita = "S", "-1", "0")) & "|" & _
                                  ConvData(dtrT!ar_ultagg, True) & vbCrLf)
                    End If
                End If

                If ConvStr(dtrT!ar_confez2) <> "" Then
                    ' Estrae la terza unità di misura (confezioni) solo se ne è stato impostato il fattore di conversione
                    ' oppure solo se ne forzo l'estrazione con l'opportuno parametro QueryGetArtUM_EstraiTutte = 1
                    If (NTSCDec(dtrT!ar_qtacon2) <> NTSCDec(0)) Or (strCustomQueryGetArtUM_EstraiTutte <> "0") Then

                        If ConvStr(dtrT!ar_qtacon2) = "0" Then
                            my_ar_qtaconf2 = "1"
                        Else
                            my_ar_qtaconf2 = NTSCDec(dtrT!ar_qtacon2).ToString("0.0000")
                        End If


                        sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§3" & "|" &
                                  strDittaCorrente & "|" & _
                                  ConvStr(dtrT!ar_codart) & "|" & _
                                  ConvStr(dtrT!ar_confez2) & "|" & _
                                  ConvStr(dtrT!tb_desconfez2) & " (" & ConvStr(dtrT!ar_qtacon2) & ")" & "|" & _
                                  my_ar_qtaconf2 & "|" & _
                                  "3" & "|" & _
                                  CStr(IIf(UMVendita = "C", "-1", "0")) & "|" & _
                                  ConvData(dtrT!ar_ultagg, True) & vbCrLf)
                    End If
                End If
            Next

            Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
            w1.Write(sbFile.ToString)
            w1.Flush()
            w1.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function


    Public Overridable Function Elabora_ExportArtLingua(ByVal strFileOut As String) As Boolean
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetArtLingua(strDittaCorrente, dttTmp, strCustomWhereGetArtLingua) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|COD_LINGUA|DES_ART|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!ax_codart) & "|" & _
                                ConvStr(dtrT!ax_codvalu) & "|" & _
                                ConvStr(dtrT!ax_descr) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next
            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArtGiacenze(ByVal strFileOut As String) As Boolean
        'esporta le giacenze divise per magazzino degli articoli (e relative fasi) 
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetArtGiacenze(strDittaCorrente, dttTmp, strCustomWhereGetArtGiacenze) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_ARTICOLO|COD_DEPOSITO|DES_DEPOSITO|GIACENZA|DISPONIBILITA|UM1|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!ap_magaz) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!ar_codart) & "|" & _
                                ConvStr(dtrT!ap_magaz) & "|" & _
                                ConvStr(dtrT!tb_desmaga) & "|" & _
                                NTSCDec(dtrT!ap_esist).ToString("0.0000") & "|" & _
                                NTSCDec(NTSCDec(dtrT!ap_esist) + NTSCDec(dtrT!ap_ordin) - NTSCDec(dtrT!ap_impeg)).ToString("0.0000") & "|" & _
                                ConvStr(dtrT!ar_unmis) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArtUltVen(ByVal strFileOutVen As String) As Boolean
        'esporta l'ultima vendita e l'ultimo acquisto degli articoli (e relative fasi) 
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim sbFileA As New StringBuilder
        Dim i As Integer = 0
        Dim dPrezzo As Decimal = 0
        Dim lClifor As Integer = 0
        Dim strT() As String = Nothing
        Dim strDtdoc As String = ""
        Dim lNumdoc As Integer = 0
        Dim strTipork As String = ""
        Dim RotturaConto As String = ""
        Dim bRottura As Boolean = False
        Dim ContaRighe As Integer = 1


        Try
            If Not oCldIbus.GetArtUltVen(strDittaCorrente, dttTmp, strCustomWhereGetArtUltVen, _
                                        strGiorniUltVen:=strFiltroGGUltAcqVen) Then Return False


            'IB_ART_ULTVEN.DAT
            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|PROG|PRZ|DATA_DOC|NUM_DOC|COD_CLFOR|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows

                ' Per ogni coppia client/articolo devo estrarre solo un valore. La query ritorna piu' righe per questa coppia.
                ' Ho implementato la "distinct" da codice per non complicare la leggibilita' della query e per problemi di prestazioni
                'If RotturaConto <> ConvStr(dtrT!km_conto) Then
                ' RotturaConto = ConvStr(dtrT!km_conto)
                ' bRottura = True
                ' Else
                ' bRottura = False
                ' End If
                '
                '  If bRottura Then
                'ContaRighe = 1
                'Else
                'ContaRighe = +1
                'End If


                'lClifor = NTSCInt(strT(2))                         km_conto
                'dPrezzo = NTSCDec(strT(1).Replace(".", ","))       mm_val
                'strDtdoc = strT(0).Substring(0, 10)                km_aammgg
                'lNumdoc = NTSCInt(strT(0).Substring(11, 9))        km_numdoc
                i += 1
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!km_numdoc) & "§" & ConvStr(i) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!ar_codart) & "|" & _
                                ConvStr(i) & "|" & _
                                NTSCDec(dtrT!mm_val).ToString("0.0000") & "|" & _
                                ConvData(dtrT!km_aammgg, False) & "|" & _
                                ConvStr(dtrT!km_numdoc) & "|" & _
                                ConvStr(dtrT!km_conto) & "|" & _
                                ConvStr(dtrT!tm_valuta) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOutVen, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArtUltAcq(ByVal strFileOutAcq As String) As Boolean
        'esporta l'ultima vendita e l'ultimo acquisto degli articoli (e relative fasi) 
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim sbFileA As New StringBuilder
        Dim i As Integer = 0
        Dim dPrezzo As Decimal = 0
        Dim lClifor As Integer = 0
        Dim strT() As String = Nothing
        Dim strDtdoc As String = ""
        Dim lNumdoc As Integer = 0
        Dim strTipork As String = ""


        Try
            If Not oCldIbus.GetArtUltAcq(strDittaCorrente, dttTmp, strCustomWhereGetArtUltAcq, _
                                        strGiorniUltAcq:=strFiltroGGUltAcqVen) Then Return False

            'IB_ART_ULTACQ.DAT
            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|PROG|PRZ|DATA_DOC|NUM_DOC|COD_CLFOR|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)
            i = 0
            For Each dtrT As DataRow In dttTmp.Rows
                i += 1

                'lClifor = NTSCInt(strT(2))                         km_conto
                'dPrezzo = NTSCDec(strT(1).Replace(".", ","))       mm_val
                'strDtdoc = strT(0).Substring(0, 10)                km_aammgg
                'lNumdoc = NTSCInt(strT(0).Substring(11, 9))        km_numdoc

                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!km_numdoc) & "§" & ConvStr(i) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!ar_codart) & "|" & _
                                ConvStr(i) & "|" & _
                                NTSCDec(dtrT!mm_val).ToString("0.0000") & "|" & _
                                ConvData(dtrT!km_aammgg, False) & "|" & _
                                ConvStr(dtrT!km_numdoc) & "|" & _
                                ConvStr(dtrT!km_conto) & "|" & _
                                ConvStr(dtrT!tm_valuta) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOutAcq, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If



            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportArtStoart(ByVal strFileOut As String) As Boolean
        'esporta l'ultimo documento veduto per ogni cliente  
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim dPrezzo As Decimal = 0
        Dim dQuant As Decimal = 0
        Dim strT() As String = Nothing
        Dim strDesart As String = ""
        Dim strData As String = ""

        Dim RotturaConto As String = ""
        Dim RotturaArticolo As String = ""
        Dim bRottura As Boolean = False

        Try
            If Not oCldIbus.GetArtStoart(strDittaCorrente, dttTmp, strCustomWhereGetArtStoart, _
                                        strFiltroGiorniStoArt:=strFiltroGGStoArt, strIncludiNonEvasi:=strFiltroIncludiNonEvasi) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_CLIFOR|COD_ART|DESC_ARTICOLO|NUM_RIGHE|ULT_NUM_REG|ULT_PROG_RIGA|" & _
                            "ULT_QTA|ULT_PRZ|ULT_UM|" & _
                            "ULT_QTA2|ULT_PRZ2|ULT_UM2|" & _
                            "COD_DEST|ULT_SC_PER1|ULT_SC_PER2|ULT_SC_PER3|ULT_SC_PER4|" & _
                            "ULT_SC_PER5|ULT_SC_PER6|ULT_SC_IMPORTO|ULT_MAG_PER1|ULT_MAG_PER2|" & _
                            "ULT_MAG_IMPORTO|ULT_DATA|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttTmp.Select("", "ar_codart, td_conto, td_datord DESC")
                ' Per ogni coppia cliente/articolo devo estrarre solo un valore. La query ritorna piu' righe per questa coppia.
                ' Ho implementato la "distinct" da codice per non complicare la leggibilita' della query e per problemi di prestazioni
                If RotturaConto <> ConvStr(dtrT!td_conto) Or RotturaArticolo <> ConvStr(dtrT!ar_codart) Then
                    RotturaConto = ConvStr(dtrT!td_conto)
                    RotturaArticolo = ConvStr(dtrT!ar_codart)
                    bRottura = True
                Else
                    bRottura = False
                End If

                If bRottura Then
                    sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!xx_code) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!td_conto) & "|" & _
                                ConvStr(dtrT!ar_codart) & "|" & _
                                ConvStr(dtrT!mo_descr) & "|" & _
                                "1" & "|" & _
                                "0" & "|" & _
                                "0" & "|" & _
                                NTSCDec(dtrT!mo_quant).ToString("0.00000") & "|" & _
                                NTSCDec(dtrT!mo_prezzo).ToString("0.00000") & "|" & _
                                ConvStr(dtrT!mo_ump) & "|" & _
                                NTSCDec(dtrT!xx_quantv).ToString("0.00000") & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!xx_umv) & "|" & _
                                IIf(ConvStr(dtrT!td_coddest) = "0", "", ConvStr(dtrT!td_coddest)).ToString & "|" & _
                                NTSCDec(dtrT!mo_scont1).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont2).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont3).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont4).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont5).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!mo_scont6).ToString("0.00") & "|" & _
                                "0" & "|" & _
                                "0" & "|" & _
                                "0" & "|" & _
                                "0" & "|" & _
                                ConvData(dtrT!td_datord, False) & "|" & _
                                ConvStr(dtrT!td_valuta) & "|" & _
                                ConvData(dtrT!mo_ultagg, True) & vbCrLf)
                End If
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)

                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCampagne(ByVal strFileOut As String) As Boolean
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetCampagne(strDittaCorrente, dttTmp, strCustomWhereGetCampagne) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_CAMPAGNA|DES_CAMPAGNA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codcamp) & "|" & _
                                ConvStr(dtrT!tb_descamp) & "|" & _
                                ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next
            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCatalogo(ByVal strFileOut As String, ByRef dttCat As DataTable, ByVal strCustomQuery As String) As Boolean
        'esporta il catalogo degli articoli  
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetArtCatalogo(strDittaCorrente, dttCat, strCustomQuery, strCustomWhereGetArtCatalogo) Then Return False

            sbFile.Append("NOMEFILE|TITOLO|COD_ART|L1|L2|L3|L4|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttCat.Rows
                If File.Exists(oApp.ImgDir & "\" & NTSCStr(dtrT!ar_gif1)) Then

                    sbFile.Append(ConvStr(dtrT!ar_gif1) & "|" & _
                                  (ConvStr(dtrT!ar_descr) & " " & ConvStr(dtrT!ar_desint) & " " & ConvStr(dtrT!af_descr)).Trim & "|" & _
                                  ConvStr(dtrT!ar_codart) & "|" & _
                                  ConvStr(dtrT!xx_l1) & "|" & _
                                  ConvStr(dtrT!xx_l2) & "|" & _
                                  ConvStr(dtrT!xx_l3) & "|" & _
                                  ConvStr(dtrT!xx_l4) & "|" & _
                                  ConvData(dtrT!xx_ultagg, True) & vbCrLf)
                Else
                    dtrT.Delete()
                End If
            Next
            dttCat.AcceptChanges()

            If dttCat.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function


    Public Overridable Function Elabora_ExportOleObjects(ByVal strFileOut As String, ByRef dttOle As DataTable, ByVal strCustomQuery As String) As Boolean
        'esporta il catalogo degli articoli  
        Dim sbFile As New StringBuilder

        Try
            If Not oCldIbus.GetOleObjects(strDittaCorrente, dttOle, strCustomQuery, strCustomWhereGetArtCatalogo) Then Return False

            sbFile.Append("NOMEFILE|TITOLO|COD_ART|L1|L2|L3|L4|DAT_ULT_MOD" & vbCrLf)

            For Each dtrT As DataRow In dttOle.Rows
                If File.Exists(NTSCStr(dtrT!xx_nome_doc)) Then

                    sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                  ConvStr(dtrT!xx_nome_doc).Trim & "|" & _
                                  ConvStr(dtrT!xx_ext_code) & "|" & _
                                  ConvStr(dtrT!xx_l1) & "|" & _
                                  ConvStr(dtrT!xx_l2) & "|" & _
                                  ConvStr(dtrT!xx_l3) & "|" & _
                                  ConvStr(dtrT!xx_l4) & "|" & _
                                  ConvData(dtrT!xx_ultagg, True) & vbCrLf)
                Else
                    dtrT.Delete()
                End If
            Next
            dttOle.AcceptChanges()

            Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
            w1.Write(sbFile.ToString)
            w1.Flush()
            w1.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function


    Public Overridable Function Elabora_ExportListini(ByVal strFileOut As String) As Boolean
        'esporta i listini in vigore alla data odierna
        'no listini in valuta
        'no listini per lavorazioni
        'no listini per unità di misura diversa dalla ump

        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Try
            If Not oCldIbus.GetArtListini(strDittaCorrente, dttTmp, strCustomWhereGetArtListini, strEstraiSoloListiniUMV) Then Return False
            '53
            sbFile.Append("CHIAVE|COD_DITTA|TIPO_LISTINO|COD_ART|TIPO_CLIFOR|COD_CLIFOR|" & _
                        "COD_LISTINO|QUANTITA_INIZIO|QUANTITA_FINE|DATA_INIZIO|DATA_FINE|" & _
                        "PREZZO|PRIORITA|" & _
                        "SCONTO1|SCONTO2|SCONTO3|SCONTO4|SCONTO5|SCONTO6|" & _
                        "SCONTO_IMP|MAG_PERC1|MAG_PERC2|MAG_IMP|" & _
                        "IND_GES_PREZZO|IND_GES_SC1_PER|IND_GES_SC2_PER|IND_GES_SC3_PER|IND_GES_SC4_PER|IND_GES_SC5_PER|IND_GES_SC6_PER|" & _
                        "IND_GES_SC_IMP|IND_GES_MAG1_PER|IND_GES_MAG2_PER|IND_GES_MAG_IMP|FLG_ESCLUDI_SCONTI|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)

            'COD_COMBINAZIONE|
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!lc_listino) & "§" & ConvStr(dtrT!lc_progr) & "|" & _
                    strDittaCorrente & "|" & _
                    "0" & "|" & _
                    ConvStr(dtrT!ar_codart) & "|" & _
                    "0" & "|" & _
                    ConvStr(dtrT!lc_conto) & "|" & _
                    ConvStr(dtrT!lc_listino) & "|" & _
                    NTSCDec(dtrT!lc_daquant).ToString("0.00") & "|" & _
                    NTSCDec(dtrT!lc_aquant).ToString("0.00") & "|" & _
                    ConvData(dtrT!lc_datagg, False) & "|" & _
                    ConvData(dtrT!lc_datscad, False) & "|" & _
                    (NTSCDec(dtrT!lc_prezzo) / NTSCDec(dtrT!lc_perqta)).ToString("0.00000000") & "|" & _
                    dtrT!xx_prior.ToString & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "4" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    "0" & "|" & _
                    ConvStr(dtrT!lc_netto) & "|" & _
                    ConvStr(dtrT!lc_codvalu) & "|" & _
                    ConvData(dtrT!xx_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If



            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

    Public Overridable Function Elabora_ExportSconti(ByVal strFileOut As String) As Boolean
        'esporta gli sconti in vigore alla data odierna

        Dim strTrattaSc1 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc1", "S", " ", "S")
        Dim strTrattaSc2 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc2", "S", " ", "S")
        Dim strTrattaSc3 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc3", "S", " ", "S")
        Dim strTrattaSc4 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc4", "S", " ", "S")
        Dim strTrattaSc5 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc5", "S", " ", "S")
        Dim strTrattaSc6 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc6", "S", " ", "S")




        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim strPrior As String = ""

        Try
            If Not oCldIbus.GetArtSconti(strDittaCorrente, dttTmp, strCustomWhereGetArtSconti) Then Return False


            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|TIPO_CLIFOR|COD_CLIFOR|CLASSE_ARTICOLO|DES_CLASSE_ARTICOLO|CLASSE_CLIENTE|DES_CLASSE_CLIENTE|COD_PROMO|" & _
                    "QUANTITA_INIZIO|QUANTITA_FINE|DATA_INIZIO|DATA_FINE|PRIORITA|SCONTO1|SCONTO2|SCONTO3|SCONTO4|" & _
                    "SCONTO5|SCONTO6|TIPO_SCONTO1|TIPO_SCONTO2|TIPO_SCONTO3|TIPO_SCONTO4|TIPO_SCONTO5|TIPO_SCONTO6|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                If oApp.oGvar.strPriorSconti <> "ACBDFE" Then
                    strPrior = dtrT!xx_prior1.ToString
                Else
                    strPrior = dtrT!xx_prior2.ToString
                End If

                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!so_conto) & "§" & ConvStr(dtrT!so_clscan) & "§" & ConvStr(dtrT!so_clscar) & "§" & ConvStr(dtrT!so_codtpro) & "§" & ConvStr(dtrT!so_daquant) & "§" & ConvData(dtrT!so_datagg, False) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!ar_codart) = "0", "", ConvStr(dtrT!ar_codart)).ToString & "|" & _
                                "0" & "|" & _
                                IIf(ConvStr(dtrT!so_conto) = "0", "", ConvStr(dtrT!so_conto)).ToString & "|" & _
                                IIf(ConvStr(dtrT!so_clscar) = "0", "", ConvStr(dtrT!so_clscar)).ToString & "|" & _
                                ConvStr(dtrT!tb_descsar) & "|" & _
                                IIf(ConvStr(dtrT!so_clscan) = "0", "", ConvStr(dtrT!so_clscan)).ToString & "|" & _
                                ConvStr(dtrT!tb_descscl) & "|" & _
                                IIf(ConvStr(dtrT!so_codtpro) = "0", "", ConvStr(dtrT!so_codtpro)).ToString & "|" & _
                                NTSCDec(dtrT!so_daquant).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_aquant).ToString("0.00") & "|" & _
                                ConvData(dtrT!so_datagg, False) & "|" & _
                                ConvData(dtrT!so_datscad, False) & "|" & _
                                strPrior & "|" & _
                                NTSCDec(dtrT!so_scont1).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_scont2).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_scont3).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_scont4).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_scont5).ToString("0.00") & "|" & _
                                NTSCDec(dtrT!so_scont6).ToString("0.00") & "|" & _
                                strTrattaSc1 & "|" & _
                                strTrattaSc2 & "|" & _
                                strTrattaSc3 & "|" & _
                                strTrattaSc4 & "|" & _
                                strTrattaSc5 & "|" & _
                                strTrattaSc5 & "|" & _
                                ConvData(dtrT!so_ultagg, True) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
            dttTmp.Dispose()
        End Try
    End Function

#End Region

#Region "Import da API"

    Public Overridable Function Elabora_ImportOrdiniAPI() As Boolean

        ' Variabili di uso locale.
        ' Identificano la chiave dell'ordine creato
        Dim tNumOrd As Integer
        Dim tAnno As Integer = 0
        Dim tSerie As String = ""
        Dim tTipork As String = ""
        Dim tCodDitta As String = ""

        Dim msg As String = ""
        Dim NewCodCli As Integer
        Dim NewCodDest As Integer

        Try

            Dim LastStoredID As Integer = CInt(oCldIbus.GetCustomData(strDittaCorrente, "order_id", "0"))

            ' TODO:  Togliere 
            'LastStoredID = 344

            ' Istanzio l'oggetto Export dell'AMHelper
            Dim ed As New GetDataAM(strAuthKeyAM, strAppManagerAPI)

            If eProxyUrl <> "" Then
                ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
            End If

            Dim OrdersData As ws_rec_orders = Nothing
            Dim RetVal As Boolean = ed.exp_orders(LastStoredID, OrdersData)


            If RetVal AndAlso OrdersData IsNot Nothing Then

                For Each t As TestataOrdineExport In OrdersData.testate

                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import ordine di {0}, ID Coda: [{1}]", t.utente, t.id.ToString)))


                    If t.id < LastStoredID Then
                        msg = String.Format("Le API hanno risposto con un id={0} su Import Ordini.", t.id)
                        WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                        Throw New NTSException(msg)
                    End If

                    'ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import ordine cliente {0} - [{1}]", t.cod_clifor, t.guid_test_ord)))

                    PreInsert_Ordine(t)

                    ' Mi preoccupo dell'esistenza del codice Mastro solo se se devo inserire un nuovo cliente
                    If t.cod_clifor Is Nothing And strMastro = "" Then
                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Codice Mastro non configurato. Non posso inserire il cliente. Elaboro il prossimo ordine, Unique ID: {0}", t.unique_id))
                        LogWrite(msg, True)

                    Else

                        ' Sto trattando un cliente nuovo. Prima di continuare lo devo inserire
                        If t.cod_clifor Is Nothing Then
                            ' Esempio : GeneraClienteAPI con Mastro 126 ritorna 

                            GeneraClienteAPI(t, CInt(strMastro), NewCodCli, NewCodDest)
                            t.cod_clifor = NewCodCli.ToString()
                            t.cod_destinazione = NewCodDest.ToString()

                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Nuovo cliente [{0} - {1}], Inserito da agente: [{2}], Utente: [{3}], Unique ID: [{4}]", t.cod_clifor, t.clienti(0).ragione_sociale, t.cod_agente, t.utente, t.unique_id))
                            LogWrite(msg, True)
                            InviaAlert(2, msg)
                        End If

                        'GeneraOffertaAPI(t, tNumOrd, tAnno, tSerie, tTipork, tCodDitta)

                        If GeneraOrdineAPI(t, tNumOrd, tAnno, tSerie, tTipork, tCodDitta) Then
                            ' Memorizzo in un campo personalizzato il guid dell'ordine di IB
                            oCldIbus.SetNumordIB(t.guid_test_ord, tNumOrd, tAnno, tSerie, tTipork, tCodDitta)

                            ' Procedura per modificare l'ordine appena inserito
                            PostInsert_Ordine(t.guid_test_ord, tNumOrd, tAnno, tSerie, tTipork, tCodDitta)

                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Import ordini effettuato. Numero: [{0}], Cliente: [{1}], Agente: [{2}], Unique ID: [{3}]", tNumOrd.ToString, t.cod_clifor, t.cod_agente, t.unique_id))
                            LogWrite(msg, True)
                            InviaAlert(1, msg, t.cod_clifor)

                            InviaPushByUsername(t.utente, "Il tuo ordine del cliente " + t.cod_clifor + ", è stato acquisito dal gestionale")
                        Else
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Import ordini avvenuto con ERRORE. Cliente: [{0}], Agente: [{1}], Unique ID: [{2}]", t.cod_clifor, t.cod_agente, t.unique_id))
                            LogWrite(msg, True)
                            InviaAlert(99, msg, t.cod_clifor)
                        End If
                    End If

                    Dim AggResult As Boolean = oCldIbus.SetCustomData(strDittaCorrente, "order_id", t.id.ToString())

                Next
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try

    End Function


    Public Overridable Function Elabora_ImportAnagraAPI() As Boolean

        ' Variabili di uso locale
        Dim msg As String = ""

        Try

            Dim LastStoredID As Integer = CInt(oCldIbus.GetCustomData(strDittaCorrente, "anagra_id", "0"))

            ' TODO:  Togliere 
            'LastStoredID = 30

            ' Istanzio l'oggetto Export dell'AMHelper
            Dim ed As New GetDataAM(strAuthKeyAM, strAppManagerAPI)

            If eProxyUrl <> "" Then
                ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
            End If

            Dim CliforData As ws_rec_clifor = Nothing
            Dim RetVal As Boolean = ed.exp_clifor(LastStoredID, CliforData)


            If RetVal AndAlso CliforData IsNot Nothing Then

                For Each t As TestataCf In CliforData.clienti

                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import anagra di {0}, ID Coda: [{1}]", t.utente, t.id.ToString)))

                    If t.id < LastStoredID Then
                        msg = String.Format("Le API hanno risposto con un id={0} su Import Anagra.", t.id)
                        WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                        Throw New NTSException(msg)
                    End If

                    ' --------------

                    Dim dttAnagra As New DataTable
                    If oCldIbus.ValCodiceDb(t.cod_cliente, strDittaCorrente, "ANAGRA", "N", "", dttAnagra) Then
                        If dttAnagra.Rows.Count > 0 Then
                            oCldIbus.UpdateCliforData(strDittaCorrente, t)
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Modifica dati anagrafici cliente {0}", t.cod_cliente))
                            LogWrite(msg, True)
                            InviaAlert(2, msg)
                        Else
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Il cliente da modificare non esiste in anagrafica {0}", t.cod_cliente))
                            LogWrite(msg, True)
                        End If
                    Else
                        msg = oApp.Tr(Me, 129919999269031600, String.Format("La ValCodiceDb ha fallito {0}", t.cod_cliente))
                        LogWrite(msg, True)
                    End If

                    ' ----------------

                    Dim AggResult As Boolean = oCldIbus.SetCustomData(strDittaCorrente, "anagra_id", t.id.ToString())

                Next
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try



    End Function

    Public Overridable Function Elabora_ImportCliforNoteAPI() As Boolean

        ' Variabili di uso locale
        Dim msg As String = ""

        Try

            Dim LastStoredID As Integer = CInt(oCldIbus.GetCustomData(strDittaCorrente, "anagra_note_id", "0"))

            ' TODO:  Togliere 
            ' LastStoredID = 30

            ' Istanzio l'oggetto Export dell'AMHelper
            Dim ed As New GetDataAM(strAuthKeyAM, strAppManagerAPI)

            If eProxyUrl <> "" Then
                ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
            End If

            Dim CliforNoteData As ws_rec_clifor_note = Nothing
            Dim RetVal As Boolean = ed.exp_clifor_note(LastStoredID, CliforNoteData)


            If RetVal AndAlso CliforNoteData IsNot Nothing Then

                For Each t As TestataCfNote In CliforNoteData.note

                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import note cliente di {0}, ID Coda: [{1}]", t.utente, t.id.ToString)))

                    If t.id < LastStoredID Then
                        msg = String.Format("Le API hanno risposto con un id={0} su Import Note Clienti.", t.id)
                        WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                        Throw New NTSException(msg)
                    End If

                    ' ------------------------------

                    Dim dttAnagra As New DataTable
                    If oCldIbus.ValCodiceDb(t.cod_cliente, strDittaCorrente, "ANAGRA", "N", "", dttAnagra) Then
                        If dttAnagra.Rows.Count > 0 Then
                            Select Case NTSCStr(t.progressivo).Trim
                                Case "0"
                                    oCldIbus.UpdateCliforNoteData(strDittaCorrente, t)
                                    msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", t.cod_cliente))
                                    LogWrite(msg, True)
                                    InviaAlert(2, msg)
                                Case ""
                                    'se non trovo la nota su tabnote la devo creare
                                    'pero' prima provo a vedere se sull'anagrafica cliente
                                    'le note generiche sono vuote
                                    'Se lo sono compilo quelle
                                    If NTSCStr(dttAnagra.Rows(0)!an_note2).Trim = "" Then
                                        oCldIbus.UpdateCliforNoteData(strDittaCorrente, t)
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", t.cod_cliente))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    Else
                                        'insert tabnote
                                        oCldIbus.InsertCliforTabNoteData(strDittaCorrente, t)
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota tabnote per cliente {0}", t.cod_cliente))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    End If
                                Case Else
                                    If oCldIbus.CheckTabNote(strDittaCorrente, t.cod_cliente, t.progressivo) Then
                                        'update tabnote
                                        oCldIbus.UpdateCliforTabNoteData(strDittaCorrente, t)
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota tabnote per cliente {0}", t.cod_cliente))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    Else
                                        'se non trovo la nota su tabnote la devo creare
                                        'pero' prima provo a vedere se sull'anagrafica cliente
                                        'le note generiche sono vuote
                                        'Se lo sono compilo quelle
                                        If NTSCStr(dttAnagra.Rows(0)!an_note2).Trim = "" Then
                                            oCldIbus.UpdateCliforNoteData(strDittaCorrente, t)
                                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", t.cod_cliente))
                                            LogWrite(msg, True)
                                            InviaAlert(2, msg)
                                        Else
                                            'insert tabnote
                                            oCldIbus.InsertCliforTabNoteData(strDittaCorrente, t)
                                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota tabnote per cliente {0}", t.cod_cliente))
                                            LogWrite(msg, True)
                                            InviaAlert(2, msg)
                                        End If
                                    End If
                            End Select
                        End If
                    End If


                    ' ------------------------------
                    Dim AggResult As Boolean = oCldIbus.SetCustomData(strDittaCorrente, "anagra_note_id", t.id.ToString())


                Next
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try



    End Function

    Public Overridable Function Elabora_ImportLeadAPI() As Boolean

        ' Variabili di uso locale
        Dim msg As String = ""
        Dim CodLead As Integer

        Try

            Dim LastStoredID As Integer = CInt(oCldIbus.GetCustomData(strDittaCorrente, "lead_id", "0"))

            ' TODO:  Togliere 
            ' LastStoredID = 30

            ' Istanzio l'oggetto Export dell'AMHelper
            Dim ed As New GetDataAM(strAuthKeyAM, strAppManagerAPI)

            If eProxyUrl <> "" Then
                ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
            End If

            Dim LeadsData As ws_rec_leads = Nothing
            Dim RetVal As Boolean = ed.exp_leads(LastStoredID, LeadsData)


            If RetVal AndAlso LeadsData IsNot Nothing Then

                For Each t As TestataLeadsExport In LeadsData.leads

                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import lead di {0}, ID Coda: [{1}]", t.utente, t.id.ToString)))

                    If t.id < LastStoredID Then
                        msg = String.Format("Le API hanno risposto con un id={0} su Import Lead.", t.id)
                        WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                        Throw New NTSException(msg)
                    End If

                    ' --------------

                    If t.cod_lead = "" Then
                        CodLead = 0

                        oCldIbus.InsertLeadData(strDittaCorrente, t, CodLead)
                        LogWrite(oApp.Tr(Me, 129919999269031600, "Import nuovo lead codice " & CodLead), True)

                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Import nuovo lead codice: {0}", t.cod_lead))
                        InviaAlert(2, msg)
                    Else
                        oCldIbus.UpdateLeadData(strDittaCorrente, t)
                        LogWrite(oApp.Tr(Me, 129919999269031600, "Modifica lead codice " & t.cod_lead), True)
                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Modifica anagrafica lead: {0}", t.cod_lead))
                        InviaAlert(2, msg)
                    End If

                    ' ----------------

                    Dim AggResult As Boolean = oCldIbus.SetCustomData(strDittaCorrente, "lead_id", t.id.ToString())

                Next
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try



    End Function

    Public Overridable Function Elabora_ImportLeadNoteAPI() As Boolean

        ' Variabili di uso locale
        Dim msg As String = ""

        'Dim strAppManagerAPI As String = "http://test.apexnet.it/appmanager/api/v1/progetti/iorder.test2"
        'Dim strAuthKey As String = "E24EFDA3-9878-42D8-90FE-C00F847FE434"  ' String di autenticazione
        'Dim strMastro As Integer = 401
        Try

            Dim LastStoredID As Integer = CInt(oCldIbus.GetCustomData(strDittaCorrente, "lead_note_id", "0"))

            ' TODO:  Togliere 
            ' LastStoredID = 30

            ' Istanzio l'oggetto Export dell'AMHelper
            Dim ed As New GetDataAM(strAuthKeyAM, strAppManagerAPI)

            If eProxyUrl <> "" Then
                ed.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
            End If

            Dim LeadNoteData As ws_rec_leads_note = Nothing
            Dim RetVal As Boolean = ed.exp_leads_note(LastStoredID, LeadNoteData)


            If RetVal AndAlso LeadNoteData IsNot Nothing Then

                For Each t As TestataLeadsNoteExport In LeadNoteData.note

                    ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", String.Format("Import note lead di {0}, ID Coda: [{1}]", t.utente, t.id.ToString)))

                    If t.id < LastStoredID Then
                        msg = String.Format("Le API hanno risposto con un id={0} su Import Note API", t.id)
                        WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                        Throw New NTSException(msg)
                    End If

                    ' --------------

                    oCldIbus.InsertLeadNoteData(strDittaCorrente, t)
                    msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota sul lead {0}", t.cod_lead))
                    LogWrite(msg, True)
                    InviaAlert(2, msg)
                    ' ----------------

                    Dim AggResult As Boolean = oCldIbus.SetCustomData(strDittaCorrente, "lead_note_id", t.id.ToString())

                Next
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try


    End Function

    Public Overridable Function GeneraOrdineAPI(ByVal Ordine As TestataOrdineExport, ByRef pNumOrd As Integer, ByRef pAnno As Integer, ByRef pSerie As String, ByRef pTipork As String, ByRef pCodDitta As String) As Boolean

        Dim oCleGsor As CLEORGSOR
        Dim lNumord As Integer = 0
        Dim ds As New DataSet
        Dim strTipoOrdine As String = ""
        Dim strStasino As String = ""

        ' Valorizzo queste variabili a seconda dell'utilizzo della prima o seconda unità di misura
        Dim strUnitaMisuraP As String = ""
        Dim strUnitaMisura As String = ""
        Dim dQuantita As Decimal
        Dim dPrezzo As Decimal
        Dim dColli As Decimal
        Dim dTipoUM As Decimal
        Dim DescNote As String = ""

        Dim DBCodAgente1 As Integer = 0
        Dim DBCodAgente2 As Integer = 0
        Dim CodAgente1 As Integer = 0
        Dim CodAgente2 As Integer = 0
        Dim msg As String = ""



        Try
            ' Controlli preelaborazione sui dati
            If Ordine.cod_clifor Is Nothing Then
                msg = String.Format("Il codice cliente nel WS non può essere null. Unique ID: {0}", Ordine.unique_id)
                WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 129887230283255079, msg)))
                Return False
            End If



            ' Esegui trascodifiche di testata AM / Business
            ' ==============================================

            ' Gestisco il tipo ordine: R = Ordine, Q = Preventivo

            Select Case Ordine.ext_cod_tipo_ord
                Case "CLI-MOBORD", "CLI-IGAMMAORD", "R"
                    strTipoOrdine = "R"
                Case "CLI-MOBPRE", "CLI-IGAMMAPRE", "P", "Q"
                    strTipoOrdine = "Q"
                Case Else
                    strTipoOrdine = "R"
            End Select


            ' Verifico se l'ordine e' stato inserito da un agente o da un subagente
            If oCldIbus.GetAgentiCliente(strDittaCorrente, Ordine.cod_clifor, DBCodAgente1, DBCodAgente2, strCustomWhereGetAgentiCliente) Then
                Select Case NTSCInt(Ordine.cod_agente)
                    Case DBCodAgente1
                        CodAgente1 = DBCodAgente1
                        CodAgente2 = DBCodAgente2
                    Case DBCodAgente2
                        CodAgente1 = DBCodAgente1
                        CodAgente2 = DBCodAgente2
                    Case Else
                        CodAgente1 = NTSCInt(Ordine.cod_agente)
                        CodAgente2 = 0
                End Select

            Else
                CodAgente1 = NTSCInt(Ordine.cod_agente)
                CodAgente2 = 0
            End If
            ' -----------------------------



            Dim strErr As String = ""
            Dim oTmp As Object = Nothing
            If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BETVTRAS", "BEORGSOR", oTmp, strErr, False, "", "") = False Then
                msg = "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"
                WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)
                Throw New NTSException(oApp.Tr(Me, 128895477321672967, msg))
                Return False
            End If
            oCleGsor = CType(oTmp, CLEORGSOR)

            AddHandler oCleGsor.RemoteEvent, AddressOf GestisciEventiEntityGsor
            'AddHandler oCleGsor.RemoteEvent, AddressOf GestisciEventiEntityPERS

            If oCleGsor.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False
            If Not oCleGsor.InitExt() Then Return False
            oCleGsor.bModuloCRM = False
            oCleGsor.bIsCRMUser = False

            If Not oCleGsor.ApriOrdine(strDittaCorrente, False, "Q", 1900, " ", -1, ds) Then Return False
            oCleGsor.bInApriDocSilent = True
            oCleGsor.ResetVar()

            ' Inizio la creazione dell'ordine. La numerazione e' attiva ?
            lNumord = oCldIbus.LegNuma(strDittaCorrente, strTipoOrdine, strSerie, NTSCDate(Ordine.data_ordine).Year, False)
            If lNumord = 0 Then
                ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 129887230283255079, "Prima di creare un nuovo Preventivo/Impegno cliente è necessario attivare la numerazione")))
                Return False
            End If

            ' Creo un nuovo ordine
            LogWrite(String.Format("Elaboro dati cliente {0}", Ordine.cod_clifor), False)
            If Not oCleGsor.NuovoOrdine(strDittaCorrente, strTipoOrdine, NTSCDate(Ordine.data_ordine).Year, strSerie, lNumord) Then
                Return False
            End If

            ' Cerco la data di consegna piu' alta presente nelle righe.
            ' Mi serve per valorizzare la data di consegna nelle testate
            Dim dateMaxCons As Date = NTSCDate(Ordine.data_consegna)
            For Each r2 As RigaOrdineExport In Ordine.righe
                If dateMaxCons < NTSCDate(r2.data_consegna_riga) Then
                    dateMaxCons = NTSCDate(r2.data_consegna_riga)
                End If
            Next


            ' Compilo i campi di testata
            oCleGsor.dttET.Rows(0)!et_datdoc = NTSCDate(Ordine.data_ordine)
            oCleGsor.dttET.Rows(0)!et_conto = NTSCInt(Ordine.cod_clifor)
            oCleGsor.dttET.Rows(0)!et_coddest = NTSCInt(Ordine.cod_destinazione)
            oCleGsor.dttET.Rows(0)!et_datcons = NTSCDate(dateMaxCons)
            If nTipoBF > 0 Then oCleGsor.dttET.Rows(0)!et_tipobf = nTipoBF
            If nMagaz > 0 Then oCleGsor.dttET.Rows(0)!et_magaz = nMagaz
            oCleGsor.dttET.Rows(0)!et_codagen = NTSCInt(CodAgente1)
            oCleGsor.dttET.Rows(0)!et_codagen2 = NTSCInt(CodAgente2)


            ' Aggiungo le note di testata
            If Not String.IsNullOrEmpty(Ordine.note) Then
                oCleGsor.dttET.Rows(0)!et_note = NTSCStr(Ordine.note)
            End If


            ' Disabilitazione blocchi di interfaccia
            ' ---------------------------------------
            'Valide per la creazione di un ordine 
            oCleGsor.strContrFidoInsolinInsOrd = "N"
            oCleGsor.bConsentiCreazOrdiniCliFornBloccoFisso = True
            oCleGsor.bSegnalaCreazOrdiniCliFornBloccati = False ' false x non segnalare 

            'Valide per la creazione di un documento 
            oCleGsor.strContrFidoInsolinInsDoc = "N"
            oCleGsor.bConsentiCreazDocumCliFornBloccoFisso = True
            oCleGsor.bSegnalaCreazDocumCliFornBloccati = False  ' false x non segnalare 
            oCleGsor.bInNuovoDocSilent = True

            ' Disabilitazione blocchi di interfaccia
            oCleGsor.bDisabilitaCheckDateAnteriori = True
            oCleGsor.bInCreaDocDaGnor = True

            If strEsplodiKit = "0" Then
                oCleGsor.bInDuplicadoc = True             'tolgo un po' di messaggi tipo 'confermi riga con qta = 0, con prezzo = 0, non faccio esplodere righe kit, oppure gestione articoli accessori/succedanei, ...
                oCleGsor.bSaltaAfterInsert = True         'non fa esplodere la diba e le righe kit 
            Else
                oCleGsor.bInDuplicadoc = False
                oCleGsor.bSaltaAfterInsert = False
            End If


            ' Valorizzo il codice di pagamento degli articoli deperibili
            If NTSCInt(Ordine.cod_cond_pag) <> 0 Then
                oCleGsor.dttET.Rows(0)!et_codpaga = NTSCInt(Ordine.cod_cond_pag)
            End If

            If NTSCInt(Ordine.cod_cond_pag_deperibilita) <> 0 Then
                oCleGsor.dttET.Rows(0)!et_codpaga = NTSCInt(Ordine.cod_cond_pag_deperibilita)
            End If

            ' Fine decodifiche di testata



            ' Ciclo per ogni riga
            ' -------------------
            For Each r As RigaOrdineExport In Ordine.righe

                ' Esegui trascodifiche di riga AM / Business
                ' ===========================================

                ' Il codice articolo potrebbe contenere il codice della fase.
                ' Cerco di splittarlo basandomi sul punto come separatore
                Dim dttTmp As New DataTable
                Dim strCodArt As String = ""
                Dim nFase As Integer = 0

                If oCldIbus.ValCodiceDb(r.codice_articolo, strDittaCorrente, "ARTICO", "S", "", dttTmp) AndAlso dttTmp.Rows.Count > 0 Then
                    strCodArt = r.codice_articolo
                    nFase = 0
                Else
                    If r.codice_articolo.Contains(".") Then
                        Dim nPosition As Integer = r.codice_articolo.LastIndexOf("."c)
                        If nPosition > -1 Then
                            strCodArt = r.codice_articolo.Substring(0, nPosition)
                            nFase = NTSCInt(r.codice_articolo.Substring(nPosition + 1))
                        Else
                            'si dovrebbe loggare (potrebbe essere riga descrittiva)
                        End If
                    Else
                        ' si dovrebbe loggare (potrebbe essere riga descrittiva)
                    End If
                End If

                ' Gestisco il tipo riga ordine: 0 = Articolo (valorizzato sopra), 2 = Descrittiva
                Select Case r.ext_cod_tipo_riga_ord
                    Case "2"
                        strCodArt = "D"
                        nFase = 0
                End Select

                ' Se il codArt non e' valorizzato lo tratto come riga descrittiva
                If strCodArt = "" Then
                    strCodArt = "D"
                    nFase = 0
                End If

                ' Se l'articolo e' bloccato lo inserisco come articolo descrittivo
                Dim dttArti As New DataTable
                If ocldBase.ValCodiceDb(strCodArt, strDittaCorrente, "ARTICO", "S", "", dttArti) Then
                    If Not dttArti Is Nothing Then
                        If dttArti.Rows.Count > 0 Then
                            If dttArti.Rows(0)!ar_blocco.ToString = "S" Then
                                'articolo bloccato quindi magari inserisco come codice l'articolo D descrittivo
                                strCodArt = "D"
                                ' e nelle note metto il codice articolo bloccato
                                ' oCleGsor.dttET.Rows(0)!ec_note = "BLOCCATO ('" & strCodart & "')"
                            End If
                        End If
                    End If
                End If

                ' Gestisco il tipo omaggio
                strStasino = "S"
                Select Case r.ext_cod_tipo_riga_omag
                    Case "0" : strStasino = "S"   'riga normale
                    Case "1" : strStasino = "O"   'omaggio con rivalsa
                    Case "2" : strStasino = "P"   'omaggio senza rivalsa
                    Case "3" : strStasino = "M"   'sconto merce
                    Case "4" : strStasino = "X"   'sconto merce NC
                End Select

                ' Gestisco il tipo unita di misura
                dTipoUM = 1
                Select Case r.tipo_um
                    Case "1" : dTipoUM = 1 ' Unità di misura principale
                    Case "2" : dTipoUM = 2 ' Unità di misura secondaria
                    Case "3" : dTipoUM = 3 ' Codice confezione
                End Select

                '  dTipoUM = 1   SEPA

                Select Case dTipoUM
                    Case 1
                        'strUnitaMisuraP = r.cod_um_1
                        dColli = NTSCDec(r.qta) ' Quantità inserita dall'agente per la presa dell'ordine
                        strUnitaMisura = r.cod_um_1 ' Unità di misura scelta dall'agente per la presa dell'ordine
                        dPrezzo = NTSCDec(r.prezzo) ' Prezzoscelto dall'agente durante la presa dell'ordine

                        dQuantita = NTSCDec(r.qta) ' Quantità dell'UM Prncipale
                    Case 2, 3
                        'strUnitaMisuraP = r.cod_um_1
                        dColli = NTSCDec(r.qta_2) ' Quantità inserita dall'agente per la presa dell'ordine
                        strUnitaMisura = r.cod_um_2 ' Unità di misura scelta dall'agente per la presa dell'ordine
                        dPrezzo = NTSCDec(r.prezzo) ' Prezzoscelto dall'agente durante la presa dell'ordine

                        dQuantita = NTSCDec(r.qta) ' Quantità dell'UM Prncipale
                End Select


                ' Ripristino i newline
                If String.IsNullOrEmpty(r.note) Then
                    DescNote = r.note
                Else
                    DescNote = r.note.Replace(CLDIEIBUS.iBNewline, vbNewLine)
                End If

                ' Fine tracodifiche di riga  ======================
                ' Se nel terzo parametro metto 0 (nRiga = 9) il framework incrementa automaticamente il numero riga
                ' BusNet2015Sr2_SorgentiNET_mgve_003/BEMGDOCU/BEMGDOCU.VB
                If Not oCleGsor.AggiungiRigaCorpo(False, strCodArt, nFase, 0) Then
                    Return False
                End If


                With oCleGsor.dttEC.Rows(oCleGsor.dttEC.Rows.Count - 1)

                    ' default
                    Select Case strDeterminazioneDescrizioneRigaOrdine
                        Case "0", "1" ' Modalità con decodifica per Determinazione descrizione ordine
                            ' L'articolo è descrittivo ?
                            If strCodArt = "D" Then
                                '!ec_ump = ""


                                ' Se esiste una descrizione
                                If r.descrizione_riga.Trim <> "" Then
                                    ' ...metti i primi 40 caratteri nella descrizione... 
                                    !ec_descr = r.descrizione_riga.PadRight(300).Substring(0, 40)
                                End If
                                ' ... se la descrizione è superiore a 40 caratteri
                                If r.descrizione_riga.Length > 40 Then
                                    ' ... i caratteri eccedenti li metto nelle note
                                    !ec_desint = r.descrizione_riga.PadRight(300).Substring(40, 40)
                                End If

                                ' ... se la descrizione è superiore a 80 caratteri
                                If r.descrizione_riga.Length > 80 Then
                                    ' i caratteri eccedenti li metto nelle note per non perderli
                                    !ec_note = r.descrizione_riga.PadRight(300).Substring(80, 40)
                                End If

                            Else
                                ' In questo caso, se la riga non è descrittiva non valorizzo le descrizioni.
                                ' Business dovrebbe prenderle dall'anagrafica articolo

                            End If
                        Case "2" ' Modalità standard per Determinazione descrizione ordine
                            ' Se la descrizione non è empty string
                            If r.descrizione_riga.Trim <> "" Then
                                ' Prendo i primi 40 caratteri
                                !ec_descr = r.descrizione_riga.PadRight(40).Substring(0, 40)
                            End If

                            ' Se la descrizione è superiore a 40 caratteri
                            If r.descrizione_riga.Length > 40 Then
                                ' I caratteri eccedenti li metto nelle note
                                !ec_note = r.descrizione_riga.PadRight(200).Substring(40, 40)
                            End If
                    End Select


                    If NTSCStr(strUnitaMisura) = "" Then
                        strUnitaMisura = "."
                    End If


                    !ec_note = Trim(NTSCStr(!ec_note) & " " & NTSCStr(r.note))

                    !ec_unmis = NTSCStr(strUnitaMisura)

                    !ec_colli = NTSCDec(dColli)
                    !ec_quant = NTSCDec(dQuantita)


                    ' !ec_quant = NTSCDec(dColli) ' SEPA
                    ' !ec_colli = NTSCDec(dQuantita) ' SEPA

                    ' Se ho attivato l'esplosione dei kit non devo impostare il prezzo dell'articolo
                    If strEsplodiKit <> "0" Then
                        ' Esplosione KIT attiva...
                        ' Sulle righe con articoli di tipo 'Kit analitico' o 'Componente sintetico', il Prezzo deve essere a zero.
                        If NTSCStr(!ec_flkit) = "A" Or NTSCStr(!ec_flkit) = "T" Then
                            !ec_prezzo = 0
                        Else
                            !ec_prezzo = NTSCDec(dPrezzo) * NTSCDec(!ec_perqta)
                        End If
                    Else
                        ' Esplosione KIT Spenta
                        !ec_prezzo = NTSCDec(dPrezzo) * NTSCDec(!ec_perqta)
                    End If

                    !ec_scont1 = NTSCDec(r.sconto_1)
                    !ec_scont2 = NTSCDec(r.sconto_2)
                    !ec_scont3 = NTSCDec(r.sconto_3)
                    !ec_scont4 = NTSCDec(r.sconto_4)
                    !ec_scont5 = NTSCDec(r.sconto_5)
                    !ec_scont6 = NTSCDec(r.sconto_6)
                    !ec_datcons = NTSCDate(r.data_consegna_riga)
                    !ec_datconsor = NTSCDate(r.data_consegna_riga)
                    !ec_stasino = NTSCStr(strStasino)




                End With

                If Not oCleGsor.RecordSalva(oCleGsor.dttEC.Rows.Count - 1, False, Nothing) Then
                    Return False
                End If

            Next

            ' Salvo l'ordine
            If Not oCleGsor.SalvaOrdine("N") Then
                Return False
            End If

            pNumOrd = lNumord
            pAnno = NTSCDate(Ordine.data_ordine).Year
            pTipork = strTipoOrdine
            pSerie = strSerie
            pCodDitta = strDittaCorrente



            Return True


        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try

    End Function

    ' NON USATA
    Public Overridable Function GeneraOffertaAPI(ByVal Ordine As TestataOrdineExport, ByRef pNumOrd As Integer, ByRef pAnno As Integer, ByRef pSerie As String, ByRef pTipork As String, ByRef pCodDitta As String) As Boolean

        Dim oCleGsof As CLECRGSOF
        Dim lNumord As Integer = 0
        Dim ds As New DataSet
        Dim strTipoOrdine As String = ""
        Dim strStasino As String = ""

        ' Valorizzo queste variabili a seconda dell'utilizzo della prima o seconda unità di misura
        Dim strUnitaMisuraP As String = ""
        Dim strUnitaMisura As String = ""
        Dim dQuantita As Decimal
        Dim dPrezzo As Decimal
        Dim dColli As Decimal
        Dim dTipoUM As Decimal
        Dim DescNote As String = ""

        Dim DBCodAgente1 As Integer = 0
        Dim DBCodAgente2 As Integer = 0
        Dim CodAgente1 As Integer = 0
        Dim CodAgente2 As Integer = 0



        Try
            ' Controlli preelaborazione sui dati
            If Ordine.cod_clifor Is Nothing Then
                ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 129887230283255079, "Il codice cliente nel WS non può essere null")))
                Return False
            End If



            ' Esegui trascodifiche di testata AM / Business
            ' ==============================================

            ' Gestisco il tipo ordine: R = Ordine, Q = Preventivo
            strTipoOrdine = "!"

            'Select Case Ordine.ext_cod_tipo_ord
            '    Case "CLI-MOBORD", "CLI-IGAMMAORD", "R"
            'strTipoOrdine = "R"
            '    Case "CLI-MOBPRE", "CLI-IGAMMAPRE", "P"
            'strTipoOrdine = "Q"
            '    Case Else
            'strTipoOrdine = Ordine.ext_cod_tipo_ord
            'End Select


            ' Verifico se l'ordine e' stato inserito da un agente o da un subagente
            If oCldIbus.GetAgentiCliente(strDittaCorrente, Ordine.cod_clifor, DBCodAgente1, DBCodAgente2, strCustomWhereGetAgentiCliente) Then
                Select Case NTSCInt(Ordine.cod_agente)
                    Case DBCodAgente1
                        CodAgente1 = DBCodAgente1
                        CodAgente2 = DBCodAgente2
                    Case DBCodAgente2
                        CodAgente1 = DBCodAgente1
                        CodAgente2 = DBCodAgente2
                    Case Else
                        CodAgente1 = NTSCInt(Ordine.cod_agente)
                        CodAgente2 = 0
                End Select

            Else
                CodAgente1 = NTSCInt(Ordine.cod_agente)
                CodAgente2 = 0
            End If
            ' -----------------------------


            Dim strErr As String = ""
            Dim oTmp As Object = Nothing
            If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BETVTRAS", "BECRGSOF", oTmp, strErr, False, "", "") = False Then
                Throw New NTSException(oApp.Tr(Me, 128895477321672967, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
                Return False
            End If
            oCleGsof = CType(oTmp, CLECRGSOF)

            AddHandler oCleGsof.RemoteEvent, AddressOf GestisciEventiEntityGsor
            'AddHandler oCleGsor.RemoteEvent, AddressOf GestisciEventiEntityPERS

            If oCleGsof.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False
            If Not oCleGsof.InitExt() Then Return False
            oCleGsof.bModuloCRM = False
            oCleGsof.bIsCRMUser = False



            If Not oCleGsof.ApriOfferta(strDittaCorrente, False, "!", 1900, " ", -1, 1, ds) Then Return False
            'oCleGsof.bInApriDocSilent = True COMMMENTATO
            oCleGsof.ResetVar()




            ' Inizio la creazione dell'ordine. La numerazione e' attiva ?
            lNumord = oCldIbus.LegNuma(strDittaCorrente, strTipoOrdine, strSerie, NTSCDate(Ordine.data_ordine).Year, False)
            If lNumord = 0 Then
                ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 129887230283255079, "Prima di creare una nuova offerta è necessario attivare la numerazione")))
                Return False
            End If

            ' Creo un nuovo ordine
            LogWrite(String.Format("Elaboro dati cliente {0}", Ordine.cod_clifor), False)
            If Not oCleGsof.NuovoOfferta(strDittaCorrente, strTipoOrdine, NTSCDate(Ordine.data_ordine).Year, strSerie, lNumord, 1) Then
                Return False
            End If

            ' Cerco la data di consegna piu' alta presente nelle righe.
            ' Mi serve per valorizzare la data di consegna nelle testate
            Dim dateMaxCons As Date = NTSCDate(Ordine.data_consegna)
            For Each r2 As RigaOrdineExport In Ordine.righe
                If dateMaxCons < NTSCDate(r2.data_consegna_riga) Then
                    dateMaxCons = NTSCDate(r2.data_consegna_riga)
                End If
            Next

            ' Compilo i campi di testata con quello che viene passato da IBUS
            oCleGsof.dttET.Rows(0)!et_datdoc = NTSCDate(Ordine.data_ordine)
            oCleGsof.dttET.Rows(0)!et_conto = NTSCInt(Ordine.cod_clifor)
            oCleGsof.dttET.Rows(0)!et_coddest = NTSCInt(Ordine.cod_destinazione)
            oCleGsof.dttET.Rows(0)!et_datcons = NTSCDate(dateMaxCons)
            If nTipoBF > 0 Then oCleGsof.dttET.Rows(0)!et_tipobf = nTipoBF
            If nMagaz > 0 Then oCleGsof.dttET.Rows(0)!et_magaz = nMagaz
            oCleGsof.dttET.Rows(0)!et_codagen = NTSCInt(CodAgente1)
            oCleGsof.dttET.Rows(0)!et_codagen2 = NTSCInt(CodAgente2)


            ' Disabilitazione blocchi di interfaccia
            ' ---------------------------------------
            'Valide per la creazione di un ordine 
            oCleGsof.strContrFidoInsolinInsOrd = "N"
            'oCleGsof.bConsentiCreazOCliFornBloccoFisso = True
            'oCleGsof.bSegnalaCreazOrdiniCliFornBloccati = False ' false x non segnalare 

            'Valide per la creazione di un documento 
            'oCleGsof.strContrFidoInsolinInsDoc = "N"   COMMMENTATO
            'oCleGsof.bConsentiCreazDocumCliFornBloccoFisso = True  COMMMENTATO
            ' oCleGsof.bSegnalaCreazDocumCliFornBloccati = False  ' false x non segnalare  COMMMENTATO

            ' Disabilitazione blocchi di interfaccia
            'oCleGsof.bDisabilitaCheckDateAnteriori = True COMMMENTATO
            ' oCleGsof.bInCreaDocDaGnor = True COMMMENTATO

            'oCleGsof.bInDuplicadoc = True             'tolgo un po' di messaggi tipo 'confermi riga con qta = 0, con prezzo = 0, non faccio esplodere righe kit, oppure gestione articoli accessori/succedanei, ...
            'oCleGsof.bSaltaAfterInsert = True         'non fa esplodere la diba e le righe kit 



            ' Valorizzo il codice di pagamento degli articoli deperibili
            If NTSCInt(Ordine.cod_cond_pag) <> 0 Then
                oCleGsof.dttET.Rows(0)!et_codpaga = NTSCInt(Ordine.cod_cond_pag)
            End If

            If NTSCInt(Ordine.cod_cond_pag_deperibilita) <> 0 Then
                oCleGsof.dttET.Rows(0)!et_codpaga = NTSCInt(Ordine.cod_cond_pag_deperibilita)
            End If

            ' Fine decodifiche di testata



            ' Ciclo per ogni riga
            ' -------------------
            For Each r As RigaOrdineExport In Ordine.righe

                ' Esegui trascodifiche di riga AM / Business
                ' ===========================================

                ' Il codice articolo potrebbe contenere il codice della fase.
                ' Cerco di splittarlo basandomi sul punto come separatore
                Dim dttTmp As New DataTable
                Dim strCodArt As String = ""
                Dim nFase As Integer = 0

                If oCldIbus.ValCodiceDb(r.codice_articolo, strDittaCorrente, "ARTICO", "S", "", dttTmp) AndAlso dttTmp.Rows.Count > 0 Then
                    strCodArt = r.codice_articolo
                    nFase = 0
                Else
                    If r.codice_articolo.Contains(".") Then
                        Dim nPosition As Integer = r.codice_articolo.LastIndexOf("."c)
                        If nPosition > -1 Then
                            strCodArt = r.codice_articolo.Substring(0, nPosition)
                            nFase = NTSCInt(r.codice_articolo.Substring(nPosition + 1))
                        Else
                            'si dovrebbe loggare (potrebbe essere riga descrittiva)
                        End If
                    Else
                        ' si dovrebbe loggare (potrebbe essere riga descrittiva)
                    End If
                End If

                ' Gestisco il tipo riga ordine: 0 = Articolo (valorizzato sopra), 2 = Descrittiva
                Select Case r.ext_cod_tipo_riga_ord
                    Case "2"
                        strCodArt = "D"
                        nFase = 0
                End Select

                ' Se il codArt non e' valorizzato lo tratto come riga descrittiva
                If strCodArt = "" Then
                    strCodArt = "D"
                    nFase = 0
                End If

                ' Se l'articolo e' bloccato lo inserisco come articolo descrittivo
                Dim dttArti As New DataTable
                If ocldBase.ValCodiceDb(strCodArt, strDittaCorrente, "ARTICO", "S", "", dttArti) Then
                    If Not dttArti Is Nothing Then
                        If dttArti.Rows.Count > 0 Then
                            If dttArti.Rows(0)!ar_blocco.ToString = "S" Then
                                'articolo bloccato quindi magari inserisco come codice l'articolo D descrittivo
                                strCodArt = "D"
                                ' e nelle note metto il codice articolo bloccato
                                ' oCleGsor.dttET.Rows(0)!ec_note = "BLOCCATO ('" & strCodart & "')"
                            End If
                        End If
                    End If
                End If

                ' Gestisco il tipo omaggio
                strStasino = "S"
                Select Case r.ext_cod_tipo_riga_omag
                    Case "0" : strStasino = "S"   'riga normale
                    Case "1" : strStasino = "O"   'omaggio con rivalsa
                    Case "2" : strStasino = "P"   'omaggio senza rivalsa
                    Case "3" : strStasino = "M"   'sconto merce
                End Select

                ' Gestisco il tipo unita di misura
                dTipoUM = 1
                Select Case r.tipo_um
                    Case "1" : dTipoUM = 1 ' Unità di misura principale
                    Case "2" : dTipoUM = 2 ' Unità di misura secondaria
                    Case "3" : dTipoUM = 3 ' Codice confezione
                End Select
                Select Case dTipoUM
                    Case 1
                        strUnitaMisuraP = r.cod_um_1
                        dColli = NTSCDec(r.qta)
                        strUnitaMisura = r.cod_um_1
                        dQuantita = NTSCDec(r.qta)
                        dPrezzo = NTSCDec(r.prezzo)
                    Case 2 Or 3
                        strUnitaMisuraP = r.cod_um_1
                        dColli = NTSCDec(r.qta_2)
                        strUnitaMisura = r.cod_um_2
                        dQuantita = NTSCDec(r.qta)
                        dPrezzo = NTSCDec(r.prezzo)
                End Select


                ' Ripristino i newline
                If String.IsNullOrEmpty(r.note) Then
                    DescNote = r.note
                Else
                    DescNote = r.note.Replace(CLDIEIBUS.iBNewline, vbNewLine)
                End If

                ' Fine tracodifiche di riga  ======================

                ' Se nel terzo parametro metto 0 (nRiga = 9) il framework incrementa automaticamente il numero riga
                'If Not oCleGsof.AggiungiRigaCorpo(False, strCodArt, nFase, 0) Then
                ' Return False
                ' End If




                With oCleGsof.dttEC.Rows(oCleGsof.dttEC.Rows.Count - 1)


                    Select Case strDeterminazioneDescrizioneRigaOrdine
                        Case "0", "1" ' Modalità con decodifica per Determinazione descrizione ordine
                            ' L'articolo è descrittivo ?
                            If strCodArt = "D" Then
                                ' Se esiste una descrizione
                                If r.descrizione_riga.Trim <> "" Then
                                    ' ...metti i primi 40 caratteri nella descrizione... 
                                    !ec_descr = r.descrizione_riga.PadRight(300).Substring(0, 40)
                                End If
                                ' ... se la descrizione è superiore a 40 caratteri
                                If r.descrizione_riga.Length > 40 Then
                                    ' ... i caratteri eccedenti li metto nelle note
                                    !ec_desint = r.descrizione_riga.PadRight(300).Substring(40, 40)
                                End If

                                ' ... se la descrizione è superiore a 80 caratteri
                                If r.descrizione_riga.Length > 80 Then
                                    ' i caratteri eccedenti li metto nelle note per non perderli
                                    !ec_note = r.descrizione_riga.PadRight(300).Substring(80, 40)
                                End If

                            Else
                                ' In questo caso, se la riga non è descrittiva non valorizzo le descrizioni.
                                ' Business dovrebbe prenderle dall'anagrafica articolo

                            End If
                        Case "2" ' Modalità standard per Determinazione descrizione ordine
                            ' Se la descrizione non è empty string
                            If r.descrizione_riga.Trim <> "" Then
                                ' Prendo i primi 40 caratteri
                                !ec_descr = r.descrizione_riga.PadRight(40).Substring(0, 40)
                            End If

                            ' Se la descrizione è superiore a 40 caratteri
                            If r.descrizione_riga.Length > 40 Then
                                ' I caratteri eccedenti li metto nelle note
                                !ec_note = r.descrizione_riga.PadRight(200).Substring(40, 40)
                            End If
                    End Select


                    !ec_note = Trim(NTSCStr(!ec_note) & " " & NTSCStr(r.note))
                    !ec_unmis = NTSCStr(strUnitaMisura)
                    !ec_colli = NTSCDec(dColli)
                    !ec_quant = NTSCDec(dQuantita)
                    !ec_prezzo = NTSCDec(dPrezzo) * NTSCDec(!ec_perqta)
                    !ec_scont1 = NTSCDec(r.sconto_1)
                    !ec_scont2 = NTSCDec(r.sconto_2)
                    !ec_scont3 = NTSCDec(r.sconto_3)
                    !ec_scont4 = NTSCDec(r.sconto_4)
                    !ec_scont5 = NTSCDec(r.sconto_5)
                    !ec_scont6 = NTSCDec(r.sconto_6)
                    !ec_datcons = NTSCDate(r.data_consegna_riga)
                    !ec_datconsor = NTSCDate(r.data_consegna_riga)
                    !ec_stasino = NTSCStr(strStasino)
                End With

                If Not oCleGsof.RecordSalva(oCleGsof.dttEC.Rows.Count - 1, False, Nothing) Then
                    Return False
                End If

            Next

            ' Salvo l'ordine
            If Not oCleGsof.SalvaOfferta("N") Then
                Return False
            End If

            pNumOrd = lNumord
            pAnno = NTSCDate(Ordine.data_ordine).Year
            pTipork = strTipoOrdine
            pSerie = strSerie
            pCodDitta = strDittaCorrente



            Return True


        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try

    End Function

    ' Solo con WS
    Public Overridable Function GeneraClienteAPI(ByRef Ordine As TestataOrdineExport, ByVal Mastro As Integer, ByRef CodClienteCompleto As Integer, ByRef CodDest As Integer) As Boolean
        Try

            ' Esempio: Chiamo la insert cliente e passo il mastro 126. Ritorna CodCliente = , CodCliente completo= 
            Dim ClienteCodificatoCorrettamente As Boolean = oCldIbus.InsertCliData(strDittaCorrente, Ordine.clienti(0), Ordine.cod_agente, Mastro, CodClienteCompleto)

            If ClienteCodificatoCorrettamente Then
                If Not Ordine.clienti(0).cap_consegna Is Nothing Or _
                   Not Ordine.clienti(0).cod_citta_consegna Is Nothing Or _
                   Not Ordine.clienti(0).cod_nazione_consegna Is Nothing Or _
                   Not Ordine.clienti(0).des_citta_consegna Is Nothing Or _
                   Not Ordine.clienti(0).des_nazione_consegna Is Nothing Or _
                   Not Ordine.clienti(0).indirizzo_consegna Is Nothing Or _
                   Not Ordine.clienti(0).provincia_consegna Is Nothing Or _
                   Not Ordine.clienti(0).telefono_consegna Is Nothing _
                Then
                    oCldIbus.InsertCliDest(strDittaCorrente, Ordine.clienti(0), Mastro, CodClienteCompleto, CodDest)
                End If

                ' Se c'è il modulo CRM, Devo inserire un Lead collegato
                If CBool(ModuliExtDittaDitt(strDittaCorrente) And bsModExtCRM) Then

                    Dim CodOperatore As String = ""
                    If Not String.IsNullOrEmpty(Ordine.cod_operatore) Then
                        Dim bCheckOperatore As Boolean = False

                        oCldIbus.CheckIfOperatExists(Ordine.cod_operatore, bCheckOperatore)
                        CodOperatore = IIf(bCheckOperatore, Ordine.cod_operatore, strDefaultOpNome).ToString()
                    Else
                        CodOperatore = strDefaultOpNome
                    End If

                    Dim CodLead() As Integer = {}
                    oCldIbus.InsertLeadFromCliData(strDittaCorrente, CodClienteCompleto.ToString(), CodOperatore, CodLead)
                End If

                ' C'è il modulo Anagrafiche generali ?
                If CBool(ModuliExtDittaDitt(strDittaCorrente) And bsModExtANG) Then
                    Dim CodAnagen As Integer
                    oCldIbus.InsertAnagenFromCliData(strDittaCorrente, CodClienteCompleto.ToString(), CodAnagen)
                End If

            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

#End Region

#Region "Import da Tracciati (metodi obsoleti non più manutenuti)"

    <Obsolete("Questo metodo è deprecato, usare Elabora_ImportOrdiniAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function Elabora_ImportOrdini() As Boolean
        Dim strF() As String = Nothing  'elenco di ordini da importare
        Dim nF As Integer = 0
        Dim r1 As StreamReader = Nothing
        Dim dttFile As New DataTable
        Dim strRow() As String = Nothing
        Dim strCodart As String = ""
        Dim nFase As Integer = 0
        Dim i As Integer = 0
        Dim strStasino As String = ""
        Dim strTipoOrdine As String = ""
        Dim msg As String = ""
        Dim NumOrd As Integer = 0



        ' Mandare una notifica push

        'ApexNetLIB.PushNotification.Send("307", "ib.appstore", "Buona giornata dal team iB" + " - " + strDittaCorrente)

        ' Valorizzo queste variabili a seconda dell'utilizzo della prima o seconda unità di misura
        Dim strUnitaMisuraP As String = ""
        Dim strUnitaMisura As String = ""
        Dim dQuantita As Decimal
        Dim dPrezzo As Decimal
        Dim dColli As Decimal
        Dim dTipoUM As Decimal

        Dim DBCodAgente1 As Integer = 0
        Dim DBCodAgente2 As Integer = 0

        Dim CodAgente1 As Integer = 0
        Dim CodAgente2 As Integer = 0

        Dim DescNote As String = ""

        Const posDitta As Integer = 0
        Const posCodExtOrdine As Integer = 1 ' Preventivo = Q, Impegno cliente (ordine) = R
        Const posDataOrdine As Integer = 2
        ' Const posNumOrdOri As Integer = 3 NON USATO
        Const posCodClifor As Integer = 4
        Const posCodAgente As Integer = 5
        Const posDataConsegna As Integer = 6
        Const posDesNote As Integer = 7
        Const posCodDest As Integer = 8
        Const posTipoRiga As Integer = 9 ' (Articolo o Descrittivo)
        Const posCodArt As Integer = 10
        Const posDesArticolo As Integer = 11
        Const posUM1 As Integer = 12
        Const posUM2 As Integer = 13
        Const posQta1 As Integer = 14
        Const posQta2 As Integer = 15
        Const posPrezzo1 As Integer = 16
        'Const posPrezzo2 As Integer = 17
        Const posTipoUM As Integer = 18
        Const posTipoOmaggio As Integer = 19
        Const posSc1 As Integer = 20
        Const posSc2 As Integer = 21
        Const posSc3 As Integer = 22
        Const posSc4 As Integer = 23
        Const posSc5 As Integer = 24
        Const posSc6 As Integer = 25
        'Const posScImporto As Integer = 26
        'Const posMagPerc1 As Integer = 27
        'Const posMagPerc2 As Integer = 28
        'Const posMagImporto As Integer = 29
        Const posCodConf As Integer = 30
        Const posQtaConf As Integer = 31
        Const posCodPag As Integer = 32
        Const posCodPagDep As Integer = 33
        Const posCodOperatore As Integer = 34



        Try
            'verifico se ci sono preventivi/ordini da importare
            'anche se nel file c'è la ditta su tutti i record, in realtà un file contiene sempre e solo una ditta
            'una volta letto il file, lo cancello
            strF = System.IO.Directory.GetFiles(strDropBoxDir & "\appmanager", "IB_AM_ORD*.DAT")

            dttFile.Columns.Add("tipork", GetType(String))
            dttFile.Columns.Add("datord", GetType(DateTime))
            dttFile.Columns.Add("conto", GetType(Integer))
            dttFile.Columns.Add("agente", GetType(Integer))
            dttFile.Columns.Add("datcons", GetType(DateTime))
            dttFile.Columns.Add("codart", GetType(String))
            dttFile.Columns.Add("fase", GetType(Integer))
            dttFile.Columns.Add("desart", GetType(String))
            dttFile.Columns.Add("note", GetType(String))
            dttFile.Columns.Add("um", GetType(String))
            dttFile.Columns.Add("ump", GetType(String))
            dttFile.Columns.Add("colli", GetType(Decimal))
            dttFile.Columns.Add("quant", GetType(Decimal))
            dttFile.Columns.Add("prezzo", GetType(Decimal))
            dttFile.Columns.Add("scont1", GetType(Decimal))
            dttFile.Columns.Add("scont2", GetType(Decimal))
            dttFile.Columns.Add("scont3", GetType(Decimal))
            dttFile.Columns.Add("scont4", GetType(Decimal))
            dttFile.Columns.Add("scont5", GetType(Decimal))
            dttFile.Columns.Add("scont6", GetType(Decimal))
            dttFile.Columns.Add("stasino", GetType(String))
            dttFile.Columns.Add("coddest", GetType(String))
            dttFile.Columns.Add("agente2", GetType(Integer))
            dttFile.Columns.Add("codconf", GetType(String))
            dttFile.Columns.Add("qtaconf", GetType(Integer))

            dttFile.Columns.Add("codpag", GetType(Integer))
            dttFile.Columns.Add("codpagdep", GetType(Integer))
            dttFile.Columns.Add("codoperatore", GetType(String))

            'If ocldBase.get Then
            'per prova
            'dttFile.Rows.Add(New Object() {"R", CDate("05/08/2012"), 4010001, 1, CDate("13/08/2012"), "mp1", 0, "Descr. art. 1", "note mp1", "CT", "NR", 1.5, 45, 1.555, 10, 5, 0, 0, 0, 0, "N"})
            'dttFile.Rows.Add(New Object() {"R", CDate("05/08/2012"), 4010001, 1, CDate("13/08/2012"), "ARTFASI", 2, "Descr. art. 1", "note mp1", "NR", "NR", 10, 10, 1.555, 10, 5, 0, 0, 0, 0, "N"})
            'dttFile.Rows.Add(New Object() {"R", CDate("05/08/2012"), 4010002, 1, CDate("13/08/2012"), "mp1", 0, "Descr. art. 1", "note mp1", "CT", "NR", 1.5, 45, 1.555, 0, 0, 0, 0, 0, 0, "1"})
            'dttFile.Rows.Add(New Object() {"Q", CDate("05/08/2012"), 4010001, 2, CDate("13/08/2012"), "mp1", 0, "Descr. art. 1", "note mp1", "CT", "NR", 1.5, 45, 1.555, 0, 0, 0, 0, 0, 0, "2"})

            ' Ciclo per tutti i files
            For nF = 0 To strF.Length - 1
                dttFile.Clear()
                r1 = New StreamReader(strF(nF))

                'r1.ReadLine()  'Se la prima riga fosse l'intestazione, la skipperei con questo comanto

                ' Ciclo per tutte le righe del file in esame
                While Not r1.EndOfStream
                    strRow = r1.ReadLine.Split("|"c)

                    ' Se il file non è della mia ditta lo scarto e passo a quello successivo
                    If strRow(posDitta).ToLower.Trim <> strDittaCorrente.ToLower Then
                        r1.Close()
                        GoTo NEXT_FILE
                    End If


                    ' Il codice articolo potrebbe contenere il codice della fase.
                    ' Cerco di splittarlo basandomi sul punto come separatore
                    Dim strArt As String = strRow(posCodArt).Trim
                    Dim dttTmp As New DataTable
                    If oCldIbus.ValCodiceDb(strArt, strDittaCorrente, "ARTICO", "S", "", dttTmp) AndAlso dttTmp.Rows.Count > 0 Then
                        strCodart = strRow(posCodArt).Trim
                        nFase = 0
                    Else
                        If strArt.Contains(".") Then
                            Dim nPosition As Integer = strArt.LastIndexOf("."c)
                            If nPosition > -1 Then
                                strCodart = strRow(posCodArt).Substring(0, nPosition)
                                nFase = NTSCInt(strRow(posCodArt).Substring(nPosition + 1))
                            Else
                                'si dovrebbe loggare (potrebbe essere riga descrittiva)
                            End If
                        Else
                            ' si dovrebbe loggare (potrebbe essere riga descrittiva)
                        End If
                    End If

                    ' Gestico il tipo riga ordine: 0 = Articolo (valorizzato sopra), 2 = Descrittiva
                    Select Case strRow(posTipoRiga)
                        Case "2"
                            strCodart = "D"
                            nFase = 0
                    End Select


                    ' Gestisco il tipo omaggio
                    strStasino = "S"
                    Select Case strRow(posTipoOmaggio)
                        Case "0" : strStasino = "S"   'riga normale
                        Case "1" : strStasino = "O"   'omaggio con rivalsa
                        Case "2" : strStasino = "P"   'omaggio senza rivalsa
                        Case "3" : strStasino = "M"   'sconto merce
                    End Select

                    ' Gestisco il tipo ordine: R = Ordine, Q = Preventivo
                    strTipoOrdine = "R"
                    Select Case strRow(posCodExtOrdine)
                        Case "CLI-MOBORD", "CLI-IGAMMAORD", "R" : strTipoOrdine = "R"
                        Case "CLI-MOBPRE", "CLI-IGAMMAPRE", "P" : strTipoOrdine = "Q"
                    End Select


                    Select Case strRow(posTipoUM)
                        Case "1" : dTipoUM = 1 ' Unità di misura principale
                        Case "2" : dTipoUM = 2 ' Unità di misura secondaria
                        Case "3" : dTipoUM = 3 ' Codice confezione
                    End Select

                    ' Se la UM2 è valorizzata significa che ho inserito l'ordine con la seconda unità di misura
                    ' Esempio di tracciato: 
                    ' UM1 | UM2 | QTA1 | QTA2 | PRZ1  | PRZ2     Intestazione tracciato
                    ' PZ  |     | 3,0  |      | 11,29 |          Caso 1 (utilizzo prima unità di misura. Pezzi) 3 pezzi a 11,29 € cad.
                    '     | SC  |      | 16   |       | 11,29    Caso 2 (utilizzo seconda unità di misura. Scatole) 2 scatole per 16 pezzi totali a € 11,28 cad. (2 da dove lo prendo?)
                    'If strRow(posUM2) <> "" Then
                    ' strUnitaMisuraP = strRow(posUM2) ' UM che deve essere presa da articoli CALCOLARE - per ora non usata
                    ' strUnitaMisura = strRow(posUM2) ' UM che arriva dall'ipad
                    ' dQuantita = NTSCDec(strRow(posQta2)) ' contiene la qta in UM1.
                    ' dPrezzo = NTSCDec(strRow(posPrezzo2))
                    ' dColli = NTSCDec(strRow(posQta2)) ' da calcolare sul db CALCOLARE - per ora metto uguale a quantita'
                    ' Else
                    ' ' Se sto usando la prima unità di misura, tutto quello che mi arriva dall'iPad è giusto
                    ' ' perchè la prima UM è sempre quella del valore minimo (pz., kg, lt, mt., ecc)
                    ' strUnitaMisuraP = strRow(posUM1)
                    ' strUnitaMisura = strRow(posUM1)
                    ' dQuantita = NTSCDec(strRow(posQta1))
                    ' dPrezzo = NTSCDec(strRow(posPrezzo1))
                    ' dColli = NTSCDec(strRow(posQta1))
                    ' End If

                    ' Nuova gestione unita' di misura. Ora arrivano le info nel seguente modo
                    ' Esempio di tracciato: 
                    ' UM1 | UM2 | QTA1 | QTA2 | PRZ1  | PRZ2     Intestazione tracciato
                    ' PZ  |     | 3,0  |      | 11,29 |          Tipo UM 1 UMP  (utilizzo prima unità di misura. Pezzi) 3 pezzi a 11,29 € cad.
                    ' PZ  | NR  | 3,0  | 16   | 11,29 | 11,29    Tipo UM 2 UM2  (utilizzo seconda unità di misura. NR)
                    ' PZ  | SC  | 3,0  |  5   | 11,29 |   30     Tipo UM 3 CONF (utilizzo confezioni)


                    Select Case NTSCInt(dTipoUM)
                        Case 1
                            strUnitaMisuraP = strRow(posUM1)
                            dColli = NTSCDec(strRow(posQta1))
                            strUnitaMisura = strRow(posUM1)
                            dQuantita = NTSCDec(strRow(posQta1))
                            dPrezzo = NTSCDec(strRow(posPrezzo1))
                        Case 2, 3
                            strUnitaMisuraP = strRow(posUM1)
                            dColli = NTSCDec(strRow(posQta2))
                            strUnitaMisura = strRow(posUM2)
                            dQuantita = NTSCDec(strRow(posQta1))
                            dPrezzo = NTSCDec(strRow(posPrezzo1))
                    End Select



                    ' Verifico se l'ordine e' stato inserito da un agente o da un subagente
                    If oCldIbus.GetAgentiCliente(strDittaCorrente, strRow(posCodClifor), DBCodAgente1, DBCodAgente2, strCustomWhereGetAgentiCliente) Then
                        Select Case NTSCInt(strRow(posCodAgente))
                            Case DBCodAgente1
                                CodAgente1 = DBCodAgente1
                                CodAgente2 = DBCodAgente2
                            Case DBCodAgente2
                                CodAgente1 = DBCodAgente1
                                CodAgente2 = DBCodAgente2
                            Case Else
                                CodAgente1 = NTSCInt(strRow(posCodAgente))
                                CodAgente2 = 0
                        End Select

                    Else
                        CodAgente1 = NTSCInt(strRow(posCodAgente))
                        CodAgente2 = 0
                    End If

                    ' Aggiungo i newline
                    If String.IsNullOrEmpty(strRow(posDesNote)) Then
                        DescNote = strRow(posDesNote)
                    Else
                        DescNote = strRow(posDesNote).Replace(CLDIEIBUS.iBNewline, vbNewLine)
                    End If

                    ' Ho tutte le informazioni. Valorizzo un datatable con tutti i dati dell'ordine
                    dttFile.Rows.Add(New Object() {strTipoOrdine, _
                                                   NTSCDate(IntSetDate(strRow(posDataOrdine).Substring(0, 2) & "/" & strRow(posDataOrdine).Substring(2, 2) & "/" & strRow(posDataOrdine).Substring(4, 4))), _
                                                   NTSCInt(strRow(posCodClifor)), _
                                                   CodAgente1, _
                                                   NTSCDate(IntSetDate(strRow(posDataConsegna).Substring(0, 2) & "/" & strRow(posDataConsegna).Substring(2, 2) & "/" & strRow(posDataConsegna).Substring(4, 4))), _
                                                   strCodart, _
                                                   nFase, _
                                                   strRow(posDesArticolo).Trim, _
                                                   DescNote, _
                                                   strUnitaMisura, _
                                                   strUnitaMisuraP, _
                                                   dColli, _
                                                   dQuantita, _
                                                   dPrezzo, _
                                                   NTSCDec(strRow(posSc1)), _
                                                   NTSCDec(strRow(posSc2)), _
                                                   NTSCDec(strRow(posSc3)), _
                                                   NTSCDec(strRow(posSc4)), _
                                                   NTSCDec(strRow(posSc5)), _
                                                   NTSCDec(strRow(posSc6)), _
                                                   strStasino, _
                                                   NTSCInt(strRow(posCodDest)), _
                                                   CodAgente2, _
                                                   strRow(posCodConf), _
                                                   NTSCInt(strRow(posQtaConf)), _
                                                   NTSCInt(strRow(posCodPag)), _
                                                   NTSCInt(strRow(posCodPagDep)), _
                                                   strRow(posCodOperatore) _
                                                  })
                End While
                r1.Close()

                'ho letto l'intero file: ora genero il documento e cancello il file
                'In un file ci deve essere solo un ordine.
                'Diversamente se nel file possono esserci 3 ordini e la routine GeneraOrdini ne crea correttamente solo 2
                'se cancello il file 1 ordine viene perso, se tengo il file alla prossima rielaborazione  importo nuovamente anche i 2 ordini già importati
                If GeneraOrdini(dttFile, NumOrd) Then
                    msg = oApp.Tr(Me, 129919999269031600, String.Format("Import ordini effettuato. Numero:{0}, Cliente: {1}, Agente: {2}", NumOrd.ToString, strRow(posCodClifor), strRow(posCodAgente)))
                    LogWrite(msg, True)
                    InviaAlert(1, msg, Conto:=strRow(posCodClifor))
                    System.IO.File.Delete(strF(nF))
                Else
                    msg = oApp.Tr(Me, 129919999269031600, String.Format("Import ordini avvenuto con ERRORE. Cliente: {0}, Agente: {1}", strRow(posCodClifor), strRow(posCodAgente)))
                    InviaAlert(99, msg, Conto:=strRow(posCodClifor))
                End If


NEXT_FILE:
            Next    'For nF = 0 To strF.Length - 1

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare Elabora_ImportAnagraAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function Elabora_ImportAnagra() As Boolean
        Dim strF() As String = Nothing  'elenco di ordini da importare
        Dim nF As Integer = 0
        Dim r1 As StreamReader = Nothing
        Dim strRow() As String = Nothing

        Const posDitta As Integer = 0
        Const posConto As Integer = 1

        'Const posTipoConto As Integer = 2
        'Const posTelefono1 As Integer = 3
        'Const posTelefono2 As Integer = 4
        'Const posFax As Integer = 5
        'Const posCell As Integer = 6
        'Const posEmail As Integer = 7
        'Const posPIVA As Integer = 8
        'Const posRagioneSociale As Integer = 9
        'Const posAddress As Integer = 10
        'Const posCAP As Integer = 11
        'Const posCity As Integer = 12
        'Const posProvincia As Integer = 13
        'Const posCodicePagamento As Integer = 14
        'Const posCodiceFiscale As Integer = 15

        Dim msg As String = ""

        Try
            'verifico se ci sono preventivi/ordini da importare
            'anche se nel file c'è la ditta su tutti i record, in realtà un file contiene sempre e solo una ditta
            'una volta letto il file, lo cancello
            strF = System.IO.Directory.GetFiles(strDropBoxDir & "\appmanager", "IB_AM_CF_ANA*.DAT")



            For nF = 0 To strF.Length - 1

                r1 = New StreamReader(strF(nF))

                'r1.ReadLine()  'Se la prima riga fosse l'intestazione, la skipperei con questo comanto
                While Not r1.EndOfStream
                    strRow = r1.ReadLine.Split("|"c)

                    ' Se il file non è della mia ditta lo scarto e passo a quello successivo
                    If strRow(posDitta).ToLower.Trim <> strDittaCorrente.ToLower Then
                        r1.Close()
                        GoTo NEXT_FILE
                    End If

                    Dim strConto As String = strRow(posConto)
                    Dim dttAnagra As New DataTable
                    If oCldIbus.ValCodiceDb(strConto, strDittaCorrente, "ANAGRA", "N", "", dttAnagra) Then
                        If dttAnagra.Rows.Count > 0 Then
                            oCldIbus.UpdateClifor(strDittaCorrente, strConto, strRow)
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Modifica dati anagrafici cliente {0}", strRow(posConto)))
                            LogWrite(msg, True)
                            InviaAlert(2, msg)

                        End If
                    End If

                End While
                r1.Close()

                System.IO.File.Delete(strF(nF))

NEXT_FILE:
            Next    'For nF = 0 To strF.Length - 1



            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare Elabora_ImportCliforNoteAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function Elabora_ImportCliforNote() As Boolean
        Dim strF() As String = Nothing  'elenco di ordini da importare
        Dim nF As Integer = 0
        Dim r1 As StreamReader = Nothing
        Dim strRow() As String = Nothing

        Const posDitta As Integer = 0
        Const posConto As Integer = 1
        'Const posTipoConto As Integer = 2
        Const posProgressivo As Integer = 3
        Const posTitolo As Integer = 4
        Const posNota As Integer = 5

        Dim msg As String = ""


        Try
            'verifico se ci sono preventivi/ordini da importare
            'anche se nel file c'è la ditta su tutti i record, in realtà un file contiene sempre e solo una ditta
            'una volta letto il file, lo cancello
            strF = System.IO.Directory.GetFiles(strDropBoxDir & "\appmanager", "IB_AM_CF_NOTE*.DAT")

            For nF = 0 To strF.Length - 1

                r1 = New StreamReader(strF(nF))

                'r1.ReadLine()  'Se la prima riga fosse l'intestazione, la skipperei con questo comanto
                While Not r1.EndOfStream
                    strRow = r1.ReadLine.Split("|"c)

                    ' Se il file non è della mia ditta lo scarto e passo a quello successivo
                    If strRow(posDitta).ToLower.Trim <> strDittaCorrente.ToLower Then
                        r1.Close()
                        GoTo NEXT_FILE
                    End If

                    Dim strConto As String = strRow(posConto)
                    Dim dttAnagra As New DataTable
                    If oCldIbus.ValCodiceDb(strConto, strDittaCorrente, "ANAGRA", "N", "", dttAnagra) Then
                        If dttAnagra.Rows.Count > 0 Then
                            Select Case NTSCStr(strRow(posProgressivo)).Trim
                                Case "0"
                                    oCldIbus.UpdateCliforNote(strDittaCorrente, strConto, strRow(posNota), strRow(posTitolo))
                                    msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", strConto))
                                    LogWrite(msg, True)
                                    InviaAlert(2, msg)
                                Case ""
                                    'se non trovo la nota su tabnote la devo creare
                                    'pero' prima provo a vedere se sull'anagrafica cliente
                                    'le note generiche sono vuote
                                    'Se lo sono compilo quelle
                                    If NTSCStr(dttAnagra.Rows(0)!an_note2).Trim = "" Then
                                        oCldIbus.UpdateCliforNote(strDittaCorrente, strConto, strRow(posNota), strRow(posTitolo))
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", strConto))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    Else
                                        'insert tabnote
                                        oCldIbus.InsertCliforTabNote(strDittaCorrente, strConto, strRow(posProgressivo), strRow(posTitolo), strRow(posNota))
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota tabnote per cliente {0}", strConto))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    End If
                                Case Else
                                    If oCldIbus.CheckTabNote(strDittaCorrente, strConto, strRow(posProgressivo)) Then
                                        'update tabnote
                                        oCldIbus.UpdateCliforTabNote(strDittaCorrente, strConto, strRow(posProgressivo), strRow(posTitolo), strRow(posNota))
                                        msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota tabnote per cliente {0}", strConto))
                                        LogWrite(msg, True)
                                        InviaAlert(2, msg)
                                    Else
                                        'se non trovo la nota su tabnote la devo creare
                                        'pero' prima provo a vedere se sull'anagrafica cliente
                                        'le note generiche sono vuote
                                        'Se lo sono compilo quelle
                                        If NTSCStr(dttAnagra.Rows(0)!an_note2).Trim = "" Then
                                            oCldIbus.UpdateCliforNote(strDittaCorrente, strConto, strRow(posNota), strRow(posTitolo))
                                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Aggiornamento nota principale cliente {0}", strConto))
                                            LogWrite(msg, True)
                                            InviaAlert(2, msg)
                                        Else
                                            'insert tabnote
                                            oCldIbus.InsertCliforTabNote(strDittaCorrente, strConto, strRow(posProgressivo), strRow(posTitolo), strRow(posNota))
                                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota tabnote per cliente {0}", strConto))
                                            LogWrite(msg, True)
                                            InviaAlert(2, msg)
                                        End If
                                    End If
                            End Select
                        End If
                    End If

                End While
                r1.Close()

                System.IO.File.Delete(strF(nF))

NEXT_FILE:
            Next    'For nF = 0 To strF.Length - 1

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare Elabora_ImportLeadAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function Elabora_ImportLead() As Boolean
        Dim strF() As String = Nothing  'elenco di ordini da importare
        Dim nF As Integer = 0
        Dim msg As String = ""
        Dim dsLeads As New DataSet
        Dim CodLead As Integer

        Try
            ' Leggo la lista dei files
            strF = System.IO.Directory.GetFiles(strDropBoxDir & "\appmanager", "leads_data_*.xml")

            ' Mi passo un file alla volta
            For nF = 0 To strF.Length - 1

                ' Carico l'xml
                dsLeads.Clear()
                dsLeads.ReadXml(strF(nF))

                ' Lo valuto solo se contiene delle righe
                If dsLeads.Tables(0).Rows.Count > 0 Then

                    ' Mi passo una riga alla volta
                    For Each dR As DataRow In dsLeads.Tables(0).Rows

                        ' Il cod ditta che e' corretto ?
                        If dR("COD_DITTA").ToString = strDittaCorrente Then
                            If dR("COD_LEAD").ToString = "" Then

                                oCldIbus.InsertLead(strDittaCorrente, dR, CodLead)
                                LogWrite(oApp.Tr(Me, 129919999269031600, "Import nuovo lead codice " & CodLead), True)

                                msg = oApp.Tr(Me, 129919999269031600, String.Format("Import nuovo lead codice: {0}", CodLead))
                                InviaAlert(2, msg)
                            Else
                                oCldIbus.UpdateLead(strDittaCorrente, dR, CodLead)
                                LogWrite(oApp.Tr(Me, 129919999269031600, "Modifica lead codice " & CodLead), True)
                                msg = oApp.Tr(Me, 129919999269031600, String.Format("Modifica anagrafica lead: {0}", CodLead))
                                InviaAlert(2, msg)
                            End If
                        End If
                    Next
                End If

                System.IO.File.Delete(strF(nF))

            Next



            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare Elabora_ImportAnagraAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function Elabora_ImportLeadNote() As Boolean
        Dim strF() As String = Nothing  'elenco di ordini da importare
        Dim nF As Integer = 0

        Dim dsLeads As New DataSet
        Dim CodLead As Integer
        Dim msg As String = ""


        Try
            ' Leggo la lista dei files
            strF = System.IO.Directory.GetFiles(strDropBoxDir & "\appmanager", "leads_note_*.xml")

            ' Mi passo un file alla volta
            For nF = 0 To strF.Length - 1

                ' Carico l'xml
                dsLeads.Clear()
                dsLeads.ReadXml(strF(nF))

                ' Lo valuto solo se contiene delle righe
                If dsLeads.Tables(0).Rows.Count > 0 Then

                    ' Mi passo una riga alla volta
                    For Each dR As DataRow In dsLeads.Tables(0).Rows

                        ' Il cod ditta che e' corretto ?
                        If dR("COD_DITTA").ToString = strDittaCorrente Then
                            oCldIbus.InsertLeadNote(strDittaCorrente, dR, CodLead)
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Inserimento nota sul lead {0}", CodLead))
                            LogWrite(msg, True)
                            InviaAlert(2, msg)
                        Else
                            msg = oApp.Tr(Me, 129919999269031600, String.Format("Attenzione: Nel file trovata riga con codice ditta '{0}' invece di '{1}'", dR("cod_ditta").ToString, strDittaCorrente))
                            LogWrite(msg, True)
                            InviaAlert(99, msg)
                        End If
                    Next
                End If

                System.IO.File.Delete(strF(nF))

            Next

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare GeneraOrdiniAPI per leggere i dati direttamente da Web Services.")> _
    Public Overridable Function GeneraOrdini(ByRef dttIn As DataTable, ByRef NumOrd As Integer) As Boolean
        'nel datatabase vengono passati gli ordini della ditta corrente
        'devo generare più ordini, raggruppando per cliente/data ordine
        Dim strKey As String = ""
        Dim strLastKey As String = ""
        Dim ds As New DataSet
        Dim oCleGsor As CLEORGSOR
        Dim bTestaCreata As Boolean = False   'se true la testata dell'ordine è già stata creata
        Dim lNumord As Integer = 0
        'Dim nRiga As Integer = 0
        Dim strCodart As String = ""
        Dim lFaseArt As Integer = 0
        Try
            '----------------------------

            Dim strErr As String = ""
            Dim oTmp As Object = Nothing
            If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BETVTRAS", "BEORGSOR", oTmp, strErr, False, "", "") = False Then
                Throw New NTSException(oApp.Tr(Me, 128895477321672967, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
                Return False
            End If
            oCleGsor = CType(oTmp, CLEORGSOR)
            '------------------------------------------------
            'AddHandler oCleGsor.RemoteEvent, AddressOf GestisciEventiEntityGsor
            AddHandler oCleGsor.RemoteEvent, AddressOf GestisciEventiEntityPERS

            If oCleGsor.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False
            If Not oCleGsor.InitExt() Then Return False
            oCleGsor.bModuloCRM = False
            oCleGsor.bIsCRMUser = False

            If Not oCleGsor.ApriOrdine(strDittaCorrente, False, "Q", 1900, " ", -1, ds) Then Return False
            oCleGsor.bInApriDocSilent = True
            oCleGsor.ResetVar()


            ' INIZIO ciclo sul datatable 
            ' --------------------------
            'per ogni riga genero preventivi/ordini
            For Each dtrT As DataRow In dttIn.Select("", "tipork, datord, conto, agente")
                strKey = NTSCStr(dtrT!tipork) + "|" + NTSCDate(dtrT!datord).ToShortDateString + "|" + NTSCStr(dtrT!conto) + "|" + NTSCStr(dtrT!agente)
                If strLastKey <> "" And strKey <> strLastKey Then
                    '-------------------
                    'salvo l'ordine visto che ne devo generare un altro
                    If bTestaCreata Then
                        If Not oCleGsor.SalvaOrdine("N") Then
                            Return False
                        End If
                    End If
                    bTestaCreata = False
                End If

                '-------------------
                'creo la nuova testata d'ordine se serve
                If Not bTestaCreata Then
                    lNumord = oCldIbus.LegNuma(strDittaCorrente, NTSCStr(dtrT!tipork), strSerie, NTSCDate(dtrT!datord).Year, False)
                    If lNumord = 0 Then
                        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 129887230283255079, "Prima di creare un nuovo Preventivo/Impegno cliente è necessario attivare la numerazione")))
                        Return False
                    End If

                    LogWrite(String.Format("Elaboro dati cliente {0}", dtrT!conto.ToString()), False)
                    If Not oCleGsor.NuovoOrdine(strDittaCorrente, NTSCStr(dtrT!tipork), NTSCDate(dtrT!datord).Year, strSerie, lNumord) Then
                        Return False
                    End If

                    ' Cerco la data di consegna piu' alta presente nelle righe.
                    ' Mi serve per valorizzare la data di consegna nelle testate
                    Dim dateMaxCons As Date = NTSCDate(dtrT!datcons)
                    For Each dtrMax As DataRow In dttIn.Select("", "tipork, datord, conto, agente")
                        If NTSCStr(dtrT!tipork) = NTSCStr(dtrMax!tipork) And NTSCDate(dtrT!datord).ToShortDateString = NTSCDate(dtrMax!datord).ToShortDateString And NTSCStr(dtrT!conto) = NTSCStr(dtrMax!conto) And NTSCStr(dtrT!agente) = NTSCStr(dtrMax!agente) Then
                            If dateMaxCons < NTSCDate(dtrMax!datcons) Then
                                dateMaxCons = NTSCDate(dtrMax!datcons)
                            End If
                        End If
                    Next


                    'compilo i campi di testata con quello che viene passato da IBUS
                    oCleGsor.dttET.Rows(0)!et_datdoc = NTSCDate(dtrT!datord)
                    oCleGsor.dttET.Rows(0)!et_conto = NTSCInt(dtrT!conto)
                    oCleGsor.dttET.Rows(0)!et_coddest = NTSCStr(dtrT!coddest)
                    oCleGsor.dttET.Rows(0)!et_datcons = NTSCDate(dateMaxCons) ' NTSCDate(dtrT!datord)
                    If nTipoBF > 0 Then oCleGsor.dttET.Rows(0)!et_tipobf = nTipoBF
                    If nMagaz > 0 Then oCleGsor.dttET.Rows(0)!et_magaz = nMagaz
                    oCleGsor.dttET.Rows(0)!et_codagen = NTSCInt(dtrT!agente)
                    oCleGsor.dttET.Rows(0)!et_codagen2 = NTSCInt(dtrT!agente2)

                    ' Disabilitazione blocchi di interfaccia
                    ' ---------------------------------------
                    'Valide per la creazione di un ordine 
                    oCleGsor.strContrFidoInsolinInsOrd = "N"
                    oCleGsor.bConsentiCreazOrdiniCliFornBloccoFisso = True
                    oCleGsor.bSegnalaCreazOrdiniCliFornBloccati = False ' false x non segnalare 

                    'Valide per la creazione di un documento 
                    oCleGsor.strContrFidoInsolinInsDoc = "N"
                    oCleGsor.bConsentiCreazDocumCliFornBloccoFisso = True
                    oCleGsor.bSegnalaCreazDocumCliFornBloccati = False  ' false x non segnalare 

                    ' Disabilitazione blocchi di interfaccia
                    oCleGsor.bDisabilitaCheckDateAnteriori = True
                    oCleGsor.bInCreaDocDaGnor = True

                    If strEsplodiKit = "0" Then
                        oCleGsor.bInDuplicadoc = True             'tolgo un po' di messaggi tipo 'confermi riga con qta = 0, con prezzo = 0, non faccio esplodere righe kit, oppure gestione articoli accessori/succedanei, ...
                        oCleGsor.bSaltaAfterInsert = True         'non fa esplodere la diba e le righe kit 
                    Else
                        oCleGsor.bInDuplicadoc = False
                        oCleGsor.bSaltaAfterInsert = False
                    End If


                    '
                    ' ---------------------------------------

                    ' Valorizzo il codice di pagamento degli articoli deperibili
                    If NTSCInt(dtrT!codpagdep) <> 0 Then oCleGsor.dttET.Rows(0)!et_codpaga = NTSCInt(dtrT!codpagdep)
                    bTestaCreata = True
                End If

                '-------------------
                'aggiungo la riga d'ordine
                'nRiga += 10
                strCodart = NTSCStr(dtrT!codart)
                lFaseArt = NTSCInt(dtrT!fase)
                If NTSCStr(dtrT!codart).Trim = "" Then
                    strCodart = "D"
                    lFaseArt = 0
                End If

                ' Se l'articolo e' bloccato lo inserisco come articolo descrittivo
                Dim dttArti As New DataTable
                If ocldBase.ValCodiceDb(strCodart, strDittaCorrente, "ARTICO", "S", "", dttArti) Then
                    If Not dttArti Is Nothing Then
                        If dttArti.Rows.Count > 0 Then
                            If dttArti.Rows(0)!ar_blocco.ToString = "S" Then
                                'articolo bloccato quindi magari inserisco come codice l'articolo D descrittivo
                                strCodart = "D"
                                ' e nelle note metto il codice articolo bloccato
                                ' oCleGsor.dttET.Rows(0)!ec_note = "BLOCCATO ('" & strCodart & "')"
                            End If
                        End If
                    End If
                End If


                ' Se nel terzo parametro metto 0 (nRiga = 9) il framework incrementa automaticamente il numero riga
                If Not oCleGsor.AggiungiRigaCorpo(False, strCodart, lFaseArt, 0) Then
                    Return False
                End If
                With oCleGsor.dttEC.Rows(oCleGsor.dttEC.Rows.Count - 1)

                    Select Case strDeterminazioneDescrizioneRigaOrdine
                        Case "0" ' Modalità standard per Determinazione descrizione ordine

                            If NTSCStr(dtrT!desart).Trim <> "" Then !ec_descr = NTSCStr(dtrT!desart).PadRight(40).Substring(0, 40)
                            If NTSCStr(dtrT!desart).Length > 40 Then
                                !ec_note = NTSCStr(dtrT!desart).PadRight(200).Substring(40, 40)
                            End If

                        Case "1" ' Modalità con decodifica per Determinazione descrizione ordine
                            ' L'articolo è descrittivo ?
                            If strCodart = "D" Then
                                ' Se esiste una descrizione
                                If NTSCStr(dtrT!desart).Trim <> "" Then
                                    ' ...metti i primi 40 caratteri nella descrizione... 
                                    !ec_descr = NTSCStr(dtrT!desart).PadRight(300).Substring(0, 40)
                                End If
                                ' ... se la descrizione è superiore a 40 caratteri
                                If NTSCStr(dtrT!desart).Length > 40 Then
                                    ' ... i caratteri eccedenti li metto nelle note
                                    !ec_desint = NTSCStr(dtrT!desart).PadRight(300).Substring(40, 40)
                                End If

                                ' ... se la descrizione è superiore a 80 caratteri
                                If NTSCStr(dtrT!desart).Length > 80 Then
                                    ' i caratteri eccedenti li metto nelle note per non perderli
                                    !ec_note = NTSCStr(dtrT!desart).PadRight(300).Substring(80, 40)
                                End If

                            Else
                                ' In questo caso, se la riga non è descrittiva non valorizzo le descrizioni.
                                ' Business dovrebbe prenderle dall'anagrafica articolo

                            End If

                    End Select



                    !ec_note = NTSCStr(!ec_note) & " " & NTSCStr(dtrT!note)
                    !ec_unmis = NTSCStr(dtrT!um)
                    !ec_colli = NTSCDec(dtrT!colli)
                    !ec_quant = NTSCDec(dtrT!quant)
                    !ec_prezzo = NTSCDec(dtrT!prezzo) ' * NTSCDec(!ec_perqta)
                    !ec_scont1 = NTSCDec(dtrT!scont1)
                    !ec_scont2 = NTSCDec(dtrT!scont2)
                    !ec_scont3 = NTSCDec(dtrT!scont3)
                    !ec_scont4 = NTSCDec(dtrT!scont4)
                    !ec_scont5 = NTSCDec(dtrT!scont5)
                    !ec_scont6 = NTSCDec(dtrT!scont6)
                    !ec_datcons = NTSCDate(dtrT!datcons)
                    !ec_datconsor = NTSCDate(dtrT!datcons)
                    !ec_stasino = NTSCStr(dtrT!stasino)
                End With

                If Not oCleGsor.RecordSalva(oCleGsor.dttEC.Rows.Count - 1, False, Nothing) Then Return False

                strLastKey = strKey
            Next    'For Each dtrT As DataRow In dttIn.Select("", "tipork, datord, conto")

            '-------------------
            'salvo l'ultimo ordine
            If bTestaCreata Then
                If Not oCleGsor.SalvaOrdine("N") Then
                    Return False
                End If
                bTestaCreata = False
            End If

            NumOrd = lNumord
            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

#End Region

#Region "utility"

    Public Overridable Function InviaAlert(ByVal idEvento As Integer, ByVal Messaggio As String, Optional Conto As String = "") As Boolean
        Dim dttAlert As DataTable = Nothing
        Dim strTipork As String = ""
        Dim strCliente As String = ""
        Dim msg As String = ""
        ' msgTipo puo' valere :

        If strAttivaAlert = "0" Then Return True


        Try
            'Public Overridable Function Verifica_Genera_Alert(ByVal nTipoOperazione As Integer, ByVal strDitta As String, _
            '                                             ByVal strProgramma As String, ByVal lIdEvento As Integer, _
            '                                            ByRef lIdAlert As Integer, ByVal dttMsgOutParam As DataTable) As Boolean
            ' CONTROLLA SE C'E' UN ALERT DA GENERARE, E NEL CASO LO GENERA
            '
            ' IN: nTipoOperazione -> 0 = solo verifica
            '                     -> 1 = solo genera
            '                     -> 2 = verifica e genera
            '
            '     strCodditt      -> Ditta del programma chiamante
            '
            '     strProgramma    -> Programma chiamante
            '     lIdEvento       -> Id. dell'evento all'interno del programma chiamante
            '     objfmChiamante  -> riferimento al form del programma chiamante
            '
            '     lIdAlert        -> Alesets da verificare
            '     dttMsgOutParam  -> recordset dei messaggi ed altri eventuali parametri
            '                        per ogni alert da generare
            ' OU:
            '     Verifica_Genera_Alert ->  = True  significa Alert verificato/generato
            '                               = False significa Alert non verificato/non generato
            ' Impegno

            dttAlert = CType(oCleComm, CLELBMENU).CreaDynasetAlert
            dttAlert.Rows.Add(dttAlert.NewRow)
            dttAlert.Rows(0)!codditt = strDittaCorrente
            dttAlert.Rows(0)!strMsg = Messaggio


            If Conto <> "" And Conto <> "0" Then
                dttAlert.Rows(0)!strConto = Conto
            End If

            dttAlert.AcceptChanges()

            CType(oCleComm, CLELBMENU).Verifica_Genera_Alert(2, strDittaCorrente, "BSIEIBUS", idEvento, 0, dttAlert)
            Return True

        Catch ex As Exception

            msg = String.Format("La creazione dell'alert ha dato errore inviando il messaggio={0}.", Messaggio)
            WEDOLogger.WriteToRegistry(msg, EventLogEntryType.Error)

            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------
        Finally
            If Not dttAlert Is Nothing Then dttAlert.Clear()
        End Try

    End Function

    Public Overridable Function InviaPushByUsername(ByVal Username As String, ByVal Messaggio As String) As Boolean
        ' msgTipo puo' valere :

        If strAttivaPush = "0" Then Return True


        Dim lmPush As New GetDataLM(strAuthKeyLM, ProduzioneLM)

        If eProxyUrl <> "" Then
            lmPush.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
        End If

        Dim AMData As ws_rec_lmparam = Nothing
        Dim RetVal As Boolean = lmPush.get_am_par(AMData)


        Dim PushRetVal As Boolean = lmPush.send_push_notification_by_username(Username, Messaggio)


        Try

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------
        End Try



    End Function

    Public Overridable Function InviaPushByCodAgente(ByVal CodAgente As String, ByVal Messaggio As String) As Boolean
        ' msgTipo puo' valere :

        If strAttivaPush = "0" Then Return True


        Dim lmPush As New GetDataLM(strAuthKeyLM, ProduzioneLM)

        If eProxyUrl <> "" Then
            lmPush.HttpProxyAutentication(eProxyUsername, eProxyPassword, eProxyUrl, CInt(eProxyPort))
        End If

        Dim AMData As ws_rec_lmparam = Nothing
        Dim RetVal As Boolean = lmPush.get_am_par(AMData)

        Dim PushRetVal As Boolean = lmPush.send_push_notification_by_agent(CodAgente, Messaggio)

        Try

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------
        End Try



    End Function

    Public Function CompressFile(ByRef file As String, ByRef destination As String) As String
        'Make sure user provided a valid file with path
        If IO.File.Exists(file) = False Then
            Return "Please specify a valid file (and path) to compress"
            Exit Function
        Else
            'Make sure the destination directory exists
            If IO.Directory.Exists(destination) = False Then
                Return "Please provide a destination location"
                Exit Function
            End If
        End If

        Try
            'Get just the name of the file
            Dim name As String = Path.GetFileName(file)
            'Convert the file to a byte array
            Dim source() As Byte = System.IO.File.ReadAllBytes(file)
            'Now we need to compress the byte array
            Dim compressed() As Byte = ConvertToByteArray(source)
            'Write the new file in the destination directory
            System.IO.File.WriteAllBytes(destination & "\" & name & ".zip", compressed)

            Return "Compression Successful!"
        Catch ex As Exception
            Return "Compression Error: " & ex.ToString()
        End Try
    End Function

    Public Function ConvertToByteArray(ByVal source() As Byte) As Byte()
        'Create a MemoryStrea
        Dim memoryStream As New MemoryStream()

        'Create a new GZipStream for holding the file bytes
        Dim gZipStream As New System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Compress, True)
        'Write the bytes to the stream
        gZipStream.Write(source, 0, source.Length)
        gZipStream.Dispose()
        memoryStream.Position = 0
        'Create a byte array to act as our buffer
        Dim buffer(CInt(memoryStream.Length)) As Byte
        'read the stream
        memoryStream.Read(buffer, 0, buffer.Length)
        'CLose & clean up
        memoryStream.Dispose()
        'Return the byte array
        Return buffer
    End Function

    Private Shared Function GetValue(codProgetto As String, key As String) As String
        Dim keyReg As Microsoft.Win32.RegistryKey
        'keyReg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("ApexNet AM");

        keyReg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\ApexNet\iB\" & codProgetto)


        Dim valore As String
        Try
            valore = keyReg.GetValue(key).ToString()
        Catch
            valore = ""
        End Try

        keyReg.Close()

        Return valore
    End Function

    Public Overridable Function PreInsert_Ordine(ByRef t As TestataOrdineExport) As Boolean
        Try

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try


    End Function

    Public Overridable Function PostInsert_Ordine(ByVal pIBNumOrd As String, ByVal pNumOrd As Integer, ByVal pAnno As Integer, ByVal pSerie As String, ByVal pTipork As String, ByVal pCodDitta As String) As Boolean
        Try

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try


    End Function


    Private Shared Sub SetValue(codProgetto As String, key As String, value As String)
        Dim keyReg As Microsoft.Win32.RegistryKey
        'keyReg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("ApexNet AM");
        keyReg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\ApexNet\iB\" & codProgetto)
        keyReg.SetValue(key, value)
        keyReg.Close()
    End Sub

#End Region


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
