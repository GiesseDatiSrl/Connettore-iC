Imports NTSInformatica.CLN__STD
Imports System.Data.Common
Imports NTSInformatica
Imports System.IO
Imports AMHelper.WS


Public Class CLDIEIBUS
    Inherits CLD__BASE

    Public Const iBNewline As String = "§"

#Region "utils"

    Public Overridable Function AddWhereCondition(ByRef sqlString As String, ByVal whereCondition As String) As Boolean
        Dim strSQL As String = ""
        Try

            ' Tolgo gli spazi all'inizio
            whereCondition = LTrim(whereCondition)

            ' Se e' stato messo l'and all'inizio, lo tolgo
            If whereCondition.Length > 3 And UCase(Left(whereCondition, 3)) = "AND" Then
                whereCondition = LTrim(Mid(whereCondition, 4))
            End If

            sqlString = Replace(sqlString, "WHERE", "WHERE " & whereCondition & " AND ", Compare:=Microsoft.VisualBasic.CompareMethod.Text)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function CheckIfOperatExists(ByRef strNomeOperatore As String, ByRef bResult As Boolean) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "SELECT 1 FROM operat WHERE OpNome = " & CStrSQL(strNomeOperatore)

            bResult = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC).Rows.Count > 0

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    ''' <summary>
    ''' Identifica i tipi documento che possono avere scadenze
    ''' </summary>
    ''' <param name="Tipork">Tipo di documento da valutare</param>
    ''' <param name="Valore">Torna true se può avere scadenze</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function TipiDocConScadenze(ByVal Tipork As String) As Boolean
        Dim RetValue As Boolean = False

        ' Lista dei tipo documenti che possono avere scadenze
        Select Case Tipork

            Case "E", "F", "I", "N", "£", "P", "S", "A", "C", "D"   ' Possono esserci scadenze (clienti)

                ' - E  Note di Add. em. 
                ' - F  Ric.Fisc. em. 
                ' - I  Riem. Ric.Fisc. 
                ' - N  Note Accr. em. 
                ' - £  Nota accr. diff. em. 
                ' - P  Fatt.Ric.Fisc.Diff. 
                ' - S  Fatt.Ric.Fisc. em. 
                ' - A  Fatt. Imm. em
                ' - C  Corr. emesso
                ' - D  Fatt. Diff. em.

                RetValue = True


            Case "J", "(", "K", "L" ' Possono esserci scadenze (fornitori)

                ' - J  Note Accr. ric. 
                ' - (  Nota accr. diff. ric. 
                ' - K  Fatt. Diff. ric. 
                ' - L  Fatt. Imm. ric. 

                RetValue = True

            Case Else ' Tutti gli altri tipi (Non possono esserci scadenze)

                RetValue = False

        End Select

        Return RetValue

    End Function

    Public Overridable Function DecodeDBValue(ByVal Contesto As String, ByVal Campo As String, ByRef Valore As String) As Boolean
        Dim RetValue As String = ""
        Try
            Select Case Contesto
                Case "tipork"
                    Select Case Campo
                        Case "A" : RetValue = "Fatt. Imm. em." '
                        Case "B" : RetValue = "DDT emesso" ' 
                        Case "C" : RetValue = "Corr. emesso"
                        Case "D" : RetValue = "Fatt. Diff. em." '
                        Case "E" : RetValue = "Note di Add. em."
                        Case "F" : RetValue = "Ric.Fisc. em."
                        Case "I" : RetValue = "Riem. Ric.Fisc."
                        Case "J" : RetValue = "Note Accr. ric."
                        Case "(" : RetValue = "Nota accr. diff. ric."
                        Case "K" : RetValue = "Fatt. Diff. ric."
                        Case "L" : RetValue = "Fatt. Imm. ric."
                        Case "M" : RetValue = "DDT ricevuto"
                        Case "N" : RetValue = "Note Accr. em."
                        Case "£" : RetValue = "Nota accr. diff. em."
                        Case "P" : RetValue = "Fatt.Ric.Fisc.Diff."
                        Case "S" : RetValue = "Fatt.Ric.Fisc. em."
                        Case "T" : RetValue = "Carico da prod."
                        Case "U" : RetValue = "Scarico a prod."
                        Case "Z" : RetValue = "Bolle di mov.int."
                        Case "W" : RetValue = "Note di prel."
                        Case "R" : RetValue = "Imp. cliente"
                        Case "O" : RetValue = "Ord. forn."
                        Case "H" : RetValue = "Ord. di prod."
                        Case "X" : RetValue = "Imp. Trasfer."
                        Case "Q" : RetValue = "Prev."
                        Case "#" : RetValue = "Imp. di comm."
                        Case "V" : RetValue = "Imp. cli aperto"
                        Case "$" : RetValue = "Ord. forn aperto"
                        Case "Y" : RetValue = "Imp. di prod."
                    End Select

                Case "stasino"
                    Select Case Campo
                        Case "O" : RetValue = "Omaggio con rivalsa"
                        Case "P" : RetValue = "Omaggio senza rivalsa" ' 
                        Case "M" : RetValue = "Sconto merce"
                        Case "X" : RetValue = "Sconto merce NC" ' (non contabile)
                    End Select
            End Select


            Valore = RetValue
            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, Contesto + " - " + Campo, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    ''' <summary>
    ''' Converte una Stringa con valore Nothing in "" (Empty string).
    ''' </summary>
    ''' <param name="strDesc">Stringa da trattare</param>
    ''' <param name="MaxLength">Lunghezza massima della stringa di ritorno</param>
    ''' <returns>Stringa troncata a MaxLenght caratteri</returns>
    ''' <remarks></remarks>
    Public Overridable Function ToSql(ByVal strDesc As String, Optional ByVal MaxLength As Integer = 0) As String
        Dim RetVal As String

        Try

            If strDesc Is Nothing Then
                RetVal = ""
            Else
                RetVal = strDesc
            End If

            If MaxLength > 0 And Len(RetVal) >= MaxLength Then
                RetVal = Left(RetVal, MaxLength)
            End If

            RetVal = ConvEmptyToNull(CStrSQL(RetVal))


        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strDesc, oApp.InfoError, "", False)))
        End Try
        Return RetVal
    End Function

    Public Overridable Function ConvEmptyToNull(ByVal strDesc As String) As String
        Dim RetVal As String

        Try

            If strDesc = "''" Then
                RetVal = "null"
            Else
                RetVal = strDesc
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strDesc, oApp.InfoError, "", False)))
        End Try
        Return RetVal
    End Function

    Public Overridable Function GetCustomData(ByVal strDitta As String, ByVal key As String, ByVal defvalue As String) As String
        Dim strSQL As String = ""
        Dim RetValue As String = "1"

        Dim dttOut As New DataTable
        Try

            strSQL = "select value from hh_ibdata where codditt = @ditta and [key] = @key"
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@key", CStrSQL(key))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Execute(String.Format("insert into hh_ibdata ([key], [value], [codditt]) values ({0}, {1}, {2})", CStrSQL(key), CStrSQL(defvalue), CStrSQL(strDitta)), CLE__APP.DBTIPO.DBAZI)
                RetValue = defvalue
            Else
                RetValue = dttOut.Rows(0)!value.ToString
            End If

            Return RetValue

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function SetNumordIB(ByVal pIBNumOrd As String, ByVal pNumOrd As Integer, ByVal pAnno As Integer, ByVal pSerie As String, ByVal pTipork As String, ByVal pCodDitta As String) As Boolean
        Dim strSQL As String = ""

        Try

            strSQL = "update testord set td_hhnumord_ib = @ibnumord where codditt = @ditta and td_numord=@numord and td_anno=@anno and td_serie=@serie and td_tipork=@tipork"
            ' set 
            strSQL = strSQL.Replace("@ibnumord", CStrSQL(pIBNumOrd))
            ' where
            strSQL = strSQL.Replace("@ditta", CStrSQL(pCodDitta))
            strSQL = strSQL.Replace("@numord", CStrSQL(pNumOrd))
            strSQL = strSQL.Replace("@anno", CStrSQL(pAnno))
            strSQL = strSQL.Replace("@serie", CStrSQL(pSerie))
            strSQL = strSQL.Replace("@tipork", CStrSQL(pTipork))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function SetDatePushInsolutiIB(ByVal pConto As String, ByVal pAnnpar As String, ByVal pAlfpar As String, ByVal pNumpar As String, ByVal pIntegr As String, ByVal pNumrata As String, ByVal pCodDitta As String) As Boolean
        Dim strSQL As String = ""

        Try
            strSQL = "update scaden set sc_hhdatapush_ib = getdate() where codditt = @ditta and sc_conto=@p_conto and sc_annpar=@p_annpar and sc_alfpar=@p_alfpar and sc_numpar=@p_numpar and sc_integr=@p_integr and sc_numrata=@p_numrata"

            ' where
            strSQL = strSQL.Replace("@ditta", CStrSQL(pCodDitta))
            strSQL = strSQL.Replace("@p_conto", CStrSQL(pConto))
            strSQL = strSQL.Replace("@p_annpar", CStrSQL(pAnnpar))
            strSQL = strSQL.Replace("@p_alfpar", CStrSQL(pAlfpar))
            strSQL = strSQL.Replace("@p_numpar", CStrSQL(pNumpar))
            strSQL = strSQL.Replace("@p_integr", CStrSQL(pIntegr))
            strSQL = strSQL.Replace("@p_numrata", CStrSQL(pNumrata))


            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function


    Public Overridable Function SetCustomData(ByVal strDitta As String, ByVal key As String, ByVal value As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = String.Format("update hh_ibdata set value = {0} where codditt = @ditta and [key] = {1}", CStrSQL(value), CStrSQL(key))
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function AggiungiPersonalizzazioniDb() As Boolean

        Dim strSQL As String = ""

        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "CustomSchema.sql", oApp.NetDir)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

#End Region

#Region "Others"
    Public Overridable Function CheckTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try
            strSQL = "select * from tabnote" & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND tb_codnote =" & strProgressivo & _
                     " AND tb_contonot =" & strConto

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count > 0 Then
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetPasswdOperatore(ByVal strOpNome As String, ByRef strPasswrd As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetPasswdOperatore.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@opnome", CStrSQL(strOpNome))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                strPasswrd = NTSCStr(dttOut.Rows(0)!xx_passwd)

                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetAgentiCliente(ByVal strDitta As String, ByVal strConto As String, ByRef codAgente1 As Integer, ByRef codAgente2 As Integer, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetAgentiCliente.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@conto", CStrSQL(strConto))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                codAgente1 = NTSCInt(dttOut.Rows(0)!an_agente)
                codAgente2 = NTSCInt(dttOut.Rows(0)!an_agente2)
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetDatiAzienda(ByVal strDitta As String, ByRef numCifreConto As Integer, ByRef CodPianoConti As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetDatiAzienda.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                numCifreConto = NTSCInt(dttOut.Rows(0)!xx_num_cifre)
                CodPianoConti = NTSCStr(dttOut.Rows(0)!tb_azcodpcon)

                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function


    Public Overridable Function FindArtUltCost(ByVal strDitta As String, ByVal codArticolo As String, ByRef UltCost As Decimal) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "FindArtUltCost.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@codart", CStrSQL(codArticolo))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                UltCost = NTSCDec(dttOut.Rows(0)!ultcos)
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforCoordinate(ByVal strDitta As String, ByVal strConto As String, ByVal dLatitudine As Double, ByVal dLongitudine As Double) As Boolean
        Dim strSQL As String = ""
        Try


            If oApp.BUSNET_VERSIONDB < 19 Then
                strSQL = "UPDATE anagra" & _
                     " SET an_hhlat_ib=" & CDblSQL(dLatitudine.ToString()) & "," & _
                     " an_hhlon_ib=" & CDblSQL(dLongitudine.ToString()) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & strConto
            Else
                strSQL = "UPDATE anagra" & _
                     " SET an_latitud=" & CDblSQL(dLatitudine.ToString()) & "," & _
                     " an_longitud=" & CDblSQL(dLongitudine.ToString()) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & strConto
            End If


            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetCliforSenzaCoordinate(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            If oApp.BUSNET_VERSIONDB < 19 Then
                strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforSenzaCoordinate.sql", oApp.NetDir)
            Else
                strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforSenzaCoordinateStandard.sql", oApp.NetDir)
            End If


            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

#End Region

#Region "Import da API"

    Public Overridable Function InsertLeadData(ByVal strDitta As String, Lead As TestataLeadsExport, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Try
            If Lead.cod_lead = "" Or Lead.cod_lead Is Nothing Then
                If Lead.cod_agente = "" Or Lead.cod_agente Is Nothing Then
                    Lead.cod_agente = "0"
                End If

                nNumero = 0
                nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                ' Dati di default
                If String.IsNullOrEmpty(Lead.cod_agente_lead) Then
                    Lead.cod_agente_lead = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_canale_vendita) Then
                    Lead.cod_canale_vendita = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_concorrente) Then
                    Lead.cod_concorrente = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_interesse) Then
                    Lead.cod_interesse = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_categoria) Then
                    Lead.cod_categoria = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_modalita_acquisizione) Then
                    Lead.cod_modalita_acquisizione = "0"
                End If

                strSQL = _
                "INSERT INTO leads (" & _
                "le_codlead ," & _
                "codditt ," & _
                "le_zona  ," & _
                "le_categ  ," & _
                "le_abi  ," & _
                "le_cab  ," & _
                "le_conto  ," & _
                "le_privacy  ," & _
                "le_contattato  ," & _
                "le_nonint  ," & _
                "le_listino  ," & _
                "le_agente2  ," & _
                "le_status  ," & _
                "le_opnome  ," & _
                "le_descr1  ," & _
                "le_descr2  ," & _
                "le_indir   ," & _
                "le_cap     ," & _
                "le_citta   ," & _
                "le_prov    ," & _
                "le_stato   ," & _
                "le_note2   ," & _
                "le_pariva  ," & _
                "le_codfis  ," & _
                "le_telef   ," & _
                "le_faxtlx  ," & _
                "le_cell    ," & _
                "le_email   ," & _
                "le_website ," & _
                 "le_agente  ," & _
                 "le_codcana  ," & _
                 "le_codcptr  ," & _
                 "le_codperv  )" & _
                "VALUES (   " & _
                CStrSQL(nNumero) & "," & _
                CStrSQL(strDitta) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("N") & "," & _
                CStr(1) & "," & _
                CStr(0) & "," & _
                CStrSQL("P") & "," & _
                ToSql(Lead.cod_operatore) & "," & _
                ToSql(Lead.descrizione1, 50) & "," & _
                ToSql(Lead.descrizione2, 50) & "," & _
                ToSql(Lead.indirizzo, 70) & "," & _
                ToSql(Lead.cap, 9) & "," & _
                ToSql(Lead.citta, 50) & "," & _
                ToSql(Lead.provincia, 2) & "," & _
                ToSql(Lead.nazione, 3) & "," & _
                ToSql(Lead.note) & "," & _
                ToSql(Lead.partita_iva, 11) & "," & _
                ToSql(Lead.codice_fiscale, 16) & "," & _
                ToSql(Lead.telefono, 18) & "," & _
                ToSql(Lead.fax, 18) & "," & _
                ToSql(Lead.cellulare, 18) & "," & _
                ToSql(Lead.email, 50) & "," & _
                ToSql(Lead.internet, 50) & "," & _
                ToSql(Lead.cod_agente_lead) & "," & _
                ToSql(Lead.cod_canale_vendita) & "," & _
                ToSql(Lead.cod_concorrente) & "," & _
                ToSql(Lead.cod_modalita_acquisizione) & _
                ")"

                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                Dim RetValAssociazione As Boolean = AssociaOperatoreLead(strDitta, nNumero, Lead.cod_operatore)


                ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
                'strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                '                       ToSql(strDitta), _
                '                       CodiceLead.ToString, _
                '                       ToSql(Lead.cod_operatore) _
                '                       )
                'dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                'No, non lo é. Lo associo
                'If dttOut.Rows.Count = 0 Then

                ' Se il cod_operatore che viene dai web service non  empty e contiene un valore...
                'If Not Lead.cod_operatore Is Nothing And Lead.cod_operatore <> "" Then
                ' Inserisco i dati nella tabella accessi          
                'strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                '                      ToSql(strDitta), _
                '                      ToSql(Lead.cod_operatore), _
                '                      CStrSQL(nNumero) _
                '                     )
                'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
                'End If

                ' End If


                CodLead = nNumero
            Else
                Return False
            End If


            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function AssociaOperatoreLead(ByVal strDitta As String, CodLead As Integer, ByVal CodOperatore As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable

        Try

            ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
            strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                                   ToSql(strDitta), _
                                   CodLead.ToString, _
                                   ToSql(CodOperatore) _
                                   )
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            'No, non lo é. Lo associo
            If dttOut.Rows.Count = 0 Then

                ' Se il cod_operatore che viene dai web service non  empty e contiene un valore...
                If Not CodOperatore Is Nothing And CodOperatore <> "" Then
                    ' Inserisco i dati nella tabella accessi          
                    strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                                           ToSql(strDitta), _
                                           ToSql(CodOperatore), _
                                           CStrSQL(CodLead) _
                                           )
                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
                End If
            End If

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function InsertLeadNoteData(ByVal strDitta As String, t As TestataLeadsNoteExport) As Boolean

        Dim strSQL As String = ""
        Dim TestoNota As String = ""

        Dim dttOut As New DataTable

        Try
            ' In Business le note dei leads non sono in una tabella di dettaglio sulla tabella principale.
            ' Inserire una nota equivale quindi a fare un update e non una insert

            strSQL = String.Format("select le_note2 from leads where codditt = {0} and le_codlead = {1}", CStrSQL(strDitta), t.cod_lead)
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            If dttOut.Rows.Count = 1 Then

                TestoNota = String.Format("{0}{1}{1}***Nota {4} del {2}***{1}{3}", _
                                          dttOut.Rows(0)("le_note2"), _
                                          vbCrLf, _
                                          t.data_import, _
                                          t.nota, _
                                          t.cod_operatore _
                                          )

            End If

            strSQL = String.Format("update leads set le_note2 = {0} where codditt = {1} and le_codlead = {2}", _
                                   ToSql(TestoNota), _
                                   ToSql(strDitta), _
                                   t.cod_lead)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateLeadData(ByVal strDitta As String, Lead As TestataLeadsExport) As Boolean
        Dim strSQL As String = ""
        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable


        Try

            ' Verifico se il codice lead esiste sul db. Se esiste devo fare un update, altrimenti una insert
            strSQL = "select le_codlead from leads where codditt = @ditta and le_codlead = @codlead"
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@codlead", Lead.cod_lead)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 1 Then
                strSQL = _
                "UPDATE leads set " & _
                " le_descr1 = " & ToSql(Lead.descrizione1, 60) & "," & _
                    " le_descr1 = " & ToSql(Lead.descrizione1, 60) & "," & _
                    " le_descr2 = " & ToSql(Lead.descrizione2, 50) & "," & _
                    " le_indir = " & ToSql(Lead.indirizzo, 70) & "," & _
                    " le_cap = " & ToSql(Lead.cap, 9) & "," & _
                    " le_citta = " & ToSql(Lead.citta, 50) & "," & _
                    " le_prov = " & ToSql(Lead.provincia, 2) & "," & _
                    " le_stato = " & ToSql(Lead.nazione, 3) & "," & _
                    " le_note2 = " & ToSql(Lead.note) & "," & _
                    " le_pariva = " & ToSql(Lead.partita_iva, 11) & "," & _
                    " le_codfis = " & ToSql(Lead.codice_fiscale, 16) & "," & _
                    " le_telef = " & ToSql(Lead.telefono, 18) & "," & _
                    " le_faxtlx = " & ToSql(Lead.fax, 18) & "," & _
                    " le_cell = " & ToSql(Lead.cellulare, 18) & "," & _
                    " le_email = " & ToSql(Lead.email, 50) & "," & _
                    " le_website = " & ToSql(Lead.internet, 50) & "," & _
                    " le_note = " & ToSql(Lead.cod_campagna, 50) & "," & _
                " where codditt = " & ToSql(strDitta) & " and le_codlead= " & ToSql(Lead.cod_lead)

                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Else
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function InsertCliforTabNoteData(ByVal strDitta As String, Nota As TestataCfNote) As Boolean
        Dim strSQL As String = ""
        Try
            Dim nNumero As Integer = 0
            nNumero = LegNuma(strDitta, "NT", " ", 0, True)
            nNumero = AggNuma(strDitta, "NT", " ", 0, nNumero, False, False, "")

            If Nota.tipo_nota = "" Then
                Nota.tipo_nota = "Senza titolo"
            End If


            strSQL = String.Format( _
                          "insert into tabnote (codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot) values ({0}, {1}, {2}, {3}, {4}, {5})", _
                                      CStrSQL(strDitta), _
                                      nNumero, _
                                      ToSql(Nota.tipo_nota), _
                                      ToSql("S"), _
                                      Nota.cod_cliente, _
                                      ToSql(Nota.nota) _
                                  )

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforTabNoteData(ByVal strDitta As String, Nota As TestataCfNote) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE tabnote" & _
                     " SET tb_desnote=" & ToSql(Nota.tipo_nota) & "," & _
                     " tb_testonot=" & ToSql(Nota.nota) & "," & _
                     " WHERE codditt=" & ToSql(strDitta) & _
                     " AND tb_codnote =" & Nota.progressivo & _
                     " AND tb_contonot =" & Nota.cod_cliente
            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforData(ByVal strDitta As String, ByVal Clifor As TestataCf) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE anagra" & _
                    " SET an_telef=" & ToSql(Clifor.telefono1, 18) & "," & _
                    " an_faxtlx=" & ToSql(Clifor.fax, 18) & "," & _
                    " an_cell=" & ToSql(Clifor.cellulare, 18) & "," & _
                    " an_email=" & ToSql(Clifor.email, 50) & "," & _
                    " an_pariva=" & ToSql(Clifor.partita_iva, 11) & "," & _
                    " an_descr1=" & ToSql(Clifor.ragione_sociale, 50) & "," & _
                    " an_indir=" & ToSql(Clifor.indirizzo, 70) & "," & _
                    " an_cap=" & ToSql(Clifor.cap, 5) & "," & _
                    " an_citta=" & ToSql(Clifor.citta, 50) & "," & _
                    " an_prov=" & ToSql(Clifor.provincia, 2) & "," & _
                    " an_codpag=" & NTSCInt(Clifor.cod_cond_pag) & "," & _
                    " an_codfis=" & ToSql(Clifor.codice_fiscale, 16) & "," & _
                    " an_opnome=" & ToSql(Clifor.utente, 20) & "," & _
                    " an_ultagg=" & CDataOraSQL(Now) & _
                    " WHERE codditt=" & ToSql(strDitta) & _
                    " AND an_conto =" & ToSql(Clifor.cod_cliente)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforNoteData(ByVal strDitta As String, Nota As TestataCfNote) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE anagra" & _
                     " SET an_note2=" & ToSql(Nota.nota) & "," & _
                     " an_opnome=" & ToSql(Nota.utente, 20) & "," & _
                     " an_ultagg=" & CDataOraSQL(Now) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & ToSql(Nota.cod_cliente)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    ''' <summary>
    ''' Crea una destinazione per un cliente
    ''' </summary>
    ''' <param name="strDitta">Codice della ditta</param>
    ''' <param name="Dest">Struttura dati con i valori della destinazione da creare</param>
    ''' <param name="Mastro">Codice Mastro (NON USATO)</param>
    ''' <param name="CodClienteCompleto">Codice cliente da associare alla destinazione.</param>
    ''' <returns>Ritorna true se la creazione è andata a buon fine</returns>
    ''' <remarks></remarks>
    Public Overridable Function InsertCliDest(ByVal strDitta As String, ByVal Dest As Clienti, ByVal Mastro As Integer, ByVal CodClienteCompleto As Integer, ByRef CliDest As Integer) As Boolean
        Dim strSQLDest As String = ""
        Dim strSQL As String = ""
        Dim dttDestinazioniRiservate As New DataTable
        Dim cod_dest_free As Integer
        Dim results() As DataRow



        Try
            'Dest.cap_consegna
            'Dest.cod_citta_consegna
            'Dest.cod_nazione_consegna
            'Dest.des_citta_consegna
            'Dest.des_nazione_consegna
            'Dest.indirizzo_consegna
            'Dest.provincia_consegna
            'Dest.telefono_consegna

            ' Cerco un cod_destinazione valido
            ' ---------------------------------

            strSQLDest = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCodDestRiservate.sql", oApp.NetDir)
            dttDestinazioniRiservate = OpenRecordset(strSQLDest, CLE__APP.DBTIPO.DBAZI)

            For cod_dest_free = 1 To 1000
                results = dttDestinazioniRiservate.Select("xx_cod_dest_ris = " + cod_dest_free.ToString)

                If results.Length = 0 Then
                    Exit For
                End If
            Next

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "InsertCliDest.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@codditt@", ToSql(strDitta))
            strSQL = strSQL.Replace("@dd_conto@", ToSql(CodClienteCompleto.ToString()))
            strSQL = strSQL.Replace("@dd_coddest@", ToSql(cod_dest_free.ToString()))
            strSQL = strSQL.Replace("@dd_nomdest@", ToSql(Dest.des_citta_consegna, 50))

            strSQL = strSQL.Replace("@dd_inddest@", ToSql(Dest.indirizzo_consegna, 70))
            strSQL = strSQL.Replace("@dd_capdest@", ToSql(Dest.cap_consegna, 9))
            strSQL = strSQL.Replace("@dd_locdest@", ToSql(Dest.des_citta_consegna, 50))

            strSQL = strSQL.Replace("@dd_prodest@", ToSql(Dest.provincia_consegna, 2))
            'strSQL = strSQL.Replace("@dd_turno@", ToSql(Ordine))
            strSQL = strSQL.Replace("@dd_telef@", ToSql(Dest.telefono_consegna, 18))
            'strSQL = strSQL.Replace("@dd_codzona@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_codfis@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_pariva@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_faxtlx@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_agente@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_agente2@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_email@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_usaem@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_vett@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_vett2@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_nomdest2@", ToSql(Ordine))
            strSQL = strSQL.Replace("@dd_stato@", ToSql(Dest.cod_nazione_consegna, 3))
            'strSQL = strSQL.Replace("@dd_note@", ToSql(Ordine))
            strSQL = strSQL.Replace("@dd_codcomu@", ToSql(Dest.cod_citta_consegna, 5))
            'strSQL = strSQL.Replace("@dd_codfisest@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_statofed@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_codlead@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_latitud@", ToSql(Ordine))
            'strSQL = strSQL.Replace("@dd_longitud@", ToSql(Ordine))

            ' Dati di ritorno
            'CodDestinazione = cod_dest_free

            CliDest = cod_dest_free

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            ' Execute(String.Format("update anagra set an_destin=1 where codditt={0} and an_conto={1} and an_tipo = 'C'", ToSql(strDitta), ToSql(Mastro.ToString() + CodCliente.ToString())), CLE__APP.DBTIPO.DBAZI)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    ''' <summary>
    ''' Genera una entry in anagrafica generale. Da chiamare solo se il cliente ha le anagrafiche generali attivate
    ''' </summary>
    ''' <param name="strDitta">Codice della Ditta</param>
    ''' <param name="CodCLiente">Codice del cliente per il quala creare l'anagrafica generale</param>
    ''' <param name="CodAnagen">Ritorna il codice dell'anagrafica generale creata</param>
    ''' <returns>Ritorna true se la creazione è andata a buon fine</returns>
    ''' <remarks></remarks>
    Public Overridable Function InsertAnagenFromCliData(ByVal strDitta As String, ByVal CodCLiente As String, ByRef CodAnagen As Integer) As Boolean

        Dim nNumero As Integer = 0
        Dim strSQL As String = ""

        Dim dttTmp As New DataTable

        Try

            ' Prenoto il progressivo codLead 
            nNumero = 0
            nNumero = LegNumg("AG", " ", 0, True)
            ValCodiceDb(nNumero.ToString, strDitta, "ANAGEN", "N", "", dttTmp)
            If dttTmp.Rows.Count > 0 Then
                ' Attenzione: il progressivo da attribuire alla nuova anagrafica generale letto dalla tabella numerazioni risulta essere già usato. Correggere la tabella numerazioni
                Throw (New NTSException("progressivo da attribuire alla nuova anagrafica generale letto dalla tabella numerazioni risulta essere già usato"))
                Return False
            End If

            Dim strErr As String = ""
            nNumero = AggNumg("AG", "", 0, nNumero, True, True, strErr)

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "InsertAnagenFromCliente.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta@", ToSql(strDitta))
            strSQL = strSQL.Replace("@conto@", ToSql(CodCLiente))
            strSQL = strSQL.Replace("@codanagen@", CStrSQL(nNumero))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            CodAnagen = nNumero

            If nNumero = 0 Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    ''' <summary>
    ''' Crea un lead partendo da un cliente esistente
    ''' </summary>
    ''' <param name="strDitta">Codice della ditta</param>
    ''' <param name="CodCLiente">Codice del cliente per il quale creare il lead</param>
    ''' <param name="CodOperatore">Codice dell'operatore da associare</param>
    ''' <param name="CodLead">Ritorna il codice del lead generato</param>
    ''' <returns>Ritorna true se la creazione è andata a buon fine</returns>
    ''' <remarks></remarks>
    Public Overridable Function InsertLeadFromCliData(ByVal strDitta As String, ByVal CodCLiente As String, ByVal CodOperatore As String, ByRef CodLead() As Integer) As Boolean

        Dim nNumero As Integer = 0
        Dim strSQLDest As String = ""
        Dim strSQLCli As String = ""
        Dim strSQLTemp As String = ""
        Dim dttOut As New DataTable
        Dim actualDest As Integer
        Dim RetValAssociazione As Boolean = False
        Dim inc As Integer = 0
        ' Dim row As DataRow
        Try

            ' Leggo la lista delle destinazioni per le quali devo creare un lead
            strSQLDest = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetDestDiv.sql", oApp.NetDir)
            strSQLDest = strSQLDest.Replace("@conto@", ToSql(CodCLiente))
            dttOut = OpenRecordset(strSQLDest, CLE__APP.DBTIPO.DBAZI)

            strSQLCli = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "InsertLeadFromCliente.sql", oApp.NetDir)
            strSQLCli = strSQLCli.Replace("@ditta@", ToSql(strDitta))
            strSQLCli = strSQLCli.Replace("@conto@", ToSql(CodCLiente))



            For Each row As DataRow In dttOut.Rows
                actualDest = CInt(row("xx_an_dest"))

                ' Prenoto il progressivo codLead 
                nNumero = 0
                nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                ' Importo il codice lead nella insert
                strSQLTemp = strSQLCli.Replace("@codlead@", CStrSQL(nNumero))
                strSQLTemp = strSQLTemp.Replace("@coddest@", CStrSQL(actualDest))

                ' Inserisco il Lead
                Execute(strSQLTemp, CLE__APP.DBTIPO.DBAZI)

                RetValAssociazione = AssociaOperatoreLead(strDitta, nNumero, CodOperatore)

                ReDim Preserve CodLead(inc)
                CodLead(inc) = nNumero
                inc = inc + 1
            Next


            If nNumero = 0 Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQLCli, oApp.InfoError, "", False)))
        End Try
    End Function

    ' Solo per API
    ''' <summary>
    ''' Inserisce un cliente nella relativa tabella anagra
    ''' </summary>
    ''' <param name="strDitta">Codice della Ditta</param>
    ''' <param name="ClienteData">Struttura dati che contiene i dati del cliente (da WS)</param>
    ''' <param name="CodAgente">Codice dell'agente da associare al cliente</param>
    ''' <param name="Mastro">Codice Mastro (per il calcolo del codice cliente)</param>
    ''' <param name="CodClienteCompleto">Ritorna il Codice del cliente inserito</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function InsertCliData(ByVal strDitta As String, ByVal ClienteData As Clienti, ByVal CodAgente As String, ByVal Mastro As Integer, ByRef CodClienteCompleto As Integer) As Boolean

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim strSQL As String = ""

        Dim numCifre As Integer = 0
        Dim CodPianoConti As String = "0"
        Dim RetValCifre As Boolean
        Dim CodiceClienteCompleto As String

        CodClienteCompleto = 0

        Try
            ' Quento deve essere il filler del cod cliente
            RetValCifre = GetDatiAzienda(strDitta, numCifre, CodPianoConti)

            ' Leggo il progressivo (ovvero il cod conto)
            nNumero = 0
            nNumero = LegNuma(strDitta, "CC", " ", Mastro, True)
            nNumero = AggNuma(strDitta, "CC", " ", Mastro, nNumero, False, False, "")


            ' Calcolo il cod cliente = Nastro + nNumero (con fill di zero a sx in base a numCifre)
            CodiceClienteCompleto = Mastro.ToString() + nNumero.ToString().PadLeft(numCifre, CChar("0"))

            If Len(nNumero.ToString()) > numCifre Then
                Return False
            End If

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "InsertCliente.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@codditt@", ToSql(strDitta))
            strSQL = strSQL.Replace("@an_codcomu@", ToSql(ClienteData.cod_citta))
            strSQL = strSQL.Replace("@an_partite@", ToSql("S"))
            strSQL = strSQL.Replace("@an_conto@", ToSql(CodiceClienteCompleto))
            strSQL = strSQL.Replace("@an_tipo@", CStrSQL("C"))
            strSQL = strSQL.Replace("@an_codmast@", CStrSQL(Mastro))
            strSQL = strSQL.Replace("@an_cap@", ToSql(ClienteData.cap, 9))
            strSQL = strSQL.Replace("@an_cell@", ToSql(ClienteData.cellulare, 18))
            strSQL = strSQL.Replace("@an_codcana@", ToSql(ClienteData.cod_canale_vendita))
            strSQL = strSQL.Replace("@an_categ@", ToSql(ClienteData.cod_categoria))
            strSQL = strSQL.Replace("@an_citta@", ToSql(ClienteData.des_citta, 50))
            strSQL = strSQL.Replace("@an_clascon@", ToSql(ClienteData.cod_classe_sconto))
            strSQL = strSQL.Replace("@an_codpag@", ToSql(ClienteData.cod_cond_pag))
            strSQL = strSQL.Replace("@an_nazion1@", ToSql(ClienteData.cod_nazione))
            strSQL = strSQL.Replace("@an_porto@", ToSql(ClienteData.cod_porto_sped))
            strSQL = strSQL.Replace("@an_codfis@", ToSql(ClienteData.codice_fiscale, 16))
            strSQL = strSQL.Replace("@an_email@", ToSql(ClienteData.email))
            strSQL = strSQL.Replace("@an_faxtlx@", ToSql(ClienteData.fax))
            strSQL = strSQL.Replace("@an_iban@", ToSql(ClienteData.iban))
            strSQL = strSQL.Replace("@an_indir@", ToSql(ClienteData.indirizzo, 70))
            strSQL = strSQL.Replace("@an_note@", ToSql("Inserito da iPad"))
            strSQL = strSQL.Replace("@an_note2@", ToSql(ClienteData.note))
            strSQL = strSQL.Replace("@an_pariva@", ToSql(ClienteData.partita_iva, 11))
            strSQL = strSQL.Replace("@an_prov@", ToSql(ClienteData.provincia, 2))
            strSQL = strSQL.Replace("@an_descr1@", ToSql(ClienteData.ragione_sociale, 50))
            strSQL = strSQL.Replace("@an_telef@", ToSql(ClienteData.telefono1, 18))
            strSQL = strSQL.Replace("@an_codpcon@", ToSql(CodPianoConti))
            strSQL = strSQL.Replace("@an_agente@", ToSql(CodAgente))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            CodClienteCompleto = CInt(CodiceClienteCompleto)

            If nNumero = 0 Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

#End Region

#Region "Import da tracciati"

    <Obsolete("Questo metodo è deprecato, usare InsertLeadData")> _
    Public Overridable Function InsertLead(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Try
            If dRow("COD_LEAD").ToString = "" Then
                If dRow("COD_AGENTE").ToString = "" Then
                    dRow("COD_AGENTE") = "0"
                End If

                nNumero = 0
                nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                strSQL = _
                "INSERT INTO leads (" & _
                "le_codlead ," & _
                "codditt ," & _
                "le_zona  ," & _
                "le_categ  ," & _
                "le_abi  ," & _
                "le_cab  ," & _
                "le_conto  ," & _
                "le_privacy  ," & _
                "le_contattato  ," & _
                "le_nonint  ," & _
                "le_listino  ," & _
                "le_agente2  ," & _
                "le_status  ," & _
                "le_codcana  ," & _
                "le_agente  ," & _
                "le_opnome  ," & _
                "le_descr1  ," & _
                "le_descr2  ," & _
                "le_indir   ," & _
                "le_cap     ," & _
                "le_citta   ," & _
                "le_prov    ," & _
                "le_stato   ," & _
                "le_note2   ," & _
                "le_pariva  ," & _
                "le_codfis  ," & _
                "le_telef   ," & _
                "le_faxtlx  ," & _
                "le_cell    ," & _
                "le_email   ," & _
                "le_website ," & _
                "le_agente  ," & _
                "le_codcana  ," & _
                "le_codcptr  ," & _
                "le_codperv  )" & _
                    "VALUES (   " & _
                CStrSQL(nNumero) & "," & _
                CStrSQL(strDitta) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("N") & "," & _
                CStr(1) & "," & _
                CStr(0) & "," & _
                CStrSQL("P") & "," & _
                CStr(1) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_AGENTE"))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_OPERATORE"))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE1").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE2").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("INDIRIZZO").ToString, 70))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CAP").ToString, 9))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CITTA").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("PROVINCIA").ToString, 2))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("COD_NAZIONE").ToString, 3))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("DES_NOTE").ToString.Replace(iBNewline, vbNewLine))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("PARTITA_IVA").ToString, 11))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CODICE_FISCALE").ToString, 16))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("TELEFONO").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("FAX").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CELLULARE").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("EMAIL").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("INTERNET").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_AGENTE_LEAD"))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_CANALE_VENDITA"))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_CONCORRENTE"))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_MODALITA_ACQUISIZIONE"))) & _
                ")"



                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
                strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                                       CStrSQL(strDitta), _
                                       CodiceLead.ToString, _
                                       CStrSQL(dRow("COD_OPERATORE")) _
                                       )
                dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                If dttOut.Rows.Count = 0 Then

                    ' Inserisco i dati nella tabella accessi          
                    strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                                           CStrSQL(strDitta), _
                                           CStrSQL(dRow("COD_OPERATORE")), _
                                           CStrSQL(nNumero) _
                                           )
                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                End If

                CodLead = nNumero
            Else
                Return False
            End If


            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare InsertLeadNoteData")> _
    Public Overridable Function InsertLeadNote(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean

        Dim strSQL As String = ""
        Dim TestoNota As String = ""
        Dim cLead As String = ""
        Dim dttOut As New DataTable

        Try
            cLead = dRow("COD_LEAD").ToString


            strSQL = String.Format("select le_note2 from leads where codditt = {0} and le_codlead = {1}", CStrSQL(strDitta), cLead)
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            If dttOut.Rows.Count = 1 Then
                TestoNota = String.Format("{0}{1}{1}***Nota {4} del {2}***{1}{3}", _
                                          dttOut.Rows(0)("le_note2"), _
                                          vbCrLf, _
                                          dRow("DAT_INSERT").ToString, _
                                          dRow("NOTA").ToString, _
                                          dRow("COD_OPERATORE").ToString _
                                          )
            End If

            strSQL = String.Format("update leads set le_note2 = {0} where codditt = {1} and le_codlead = {2}", _
                                   CStrSQL(TestoNota.Replace(iBNewline, vbNewLine)), _
                                   CStrSQL(strDitta), _
                                   cLead)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            CodLead = CInt(cLead)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateLead(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""
        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Try

            If dRow("cod_lead").ToString <> "" Then
                CodiceLead = CInt(dRow("COD_LEAD"))

                ' Verifico se il codice lead esiste sul db. Se esiste devo fare un update, altrimenti una insert
                strSQL = "select le_codlead from leads where codditt = @ditta and le_codlead = @codlead"
                strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
                strSQL = strSQL.Replace("@codlead", CodiceLead.ToString)

                dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                ' Se sul db c'è un record nel db posso fare l'update , altrimenti faccio l'insert
                If dttOut.Rows.Count = 1 Then
                    strSQL = _
                    "UPDATE leads set " & _
                    " le_descr1 = " & ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE1").ToString, 60))) & "," & _
                    " le_descr2 = " & ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE2").ToString, 50))) & "," & _
                    " le_indir  = " & ConvEmptyToNull(CStrSQL(Left(dRow("INDIRIZZO").ToString, 70))) & "," & _
                    " le_cap    = " & ConvEmptyToNull(CStrSQL(Left(dRow("CAP").ToString, 9))) & "," & _
                    " le_citta  = " & ConvEmptyToNull(CStrSQL(Left(dRow("CITTA").ToString, 50))) & "," & _
                    " le_prov   = " & ConvEmptyToNull(CStrSQL(Left(dRow("PROVINCIA").ToString, 2))) & "," & _
                    " le_stato  = " & ConvEmptyToNull(CStrSQL(Left(dRow("NAZIONE").ToString, 3))) & "," & _
                    " le_note2  = " & ConvEmptyToNull(CStrSQL(dRow("DES_NOTE").ToString.Replace(iBNewline, vbNewLine))) & "," & _
                    " le_pariva = " & ConvEmptyToNull(CStrSQL(Left(dRow("PARTITA_IVA").ToString, 11))) & "," & _
                    " le_codfis = " & ConvEmptyToNull(CStrSQL(Left(dRow("CODICE_FISCALE").ToString, 16))) & "," & _
                    " le_telef  = " & ConvEmptyToNull(CStrSQL(Left(dRow("TELEFONO").ToString, 18))) & "," & _
                    " le_faxtlx = " & ConvEmptyToNull(CStrSQL(Left(dRow("FAX").ToString, 18))) & "," & _
                    " le_cell   = " & ConvEmptyToNull(CStrSQL(Left(dRow("CELLULARE").ToString, 18))) & "," & _
                    " le_email  = " & ConvEmptyToNull(CStrSQL(Left(dRow("EMAIL").ToString, 50))) & "," & _
                    " le_website= " & ConvEmptyToNull(CStrSQL(Left(dRow("INTERNET").ToString, 50))) & "," & _
                    " le_note   = " & ConvEmptyToNull(CStrSQL(Left(dRow("COD_CAMPAGNA").ToString.Replace(iBNewline, vbNewLine), 50))) & _
                    " where codditt = " & CStrSQL(strDitta) & " and le_codlead= " & CStrSQL(CodiceLead)

                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                    CodLead = CodiceLead
                Else
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare InsertCliforTabNoteData")> _
    Public Overridable Function InsertCliforTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String, _
                                                 ByVal strTitolo As String, ByVal strNote As String) As Boolean
        Dim strSQL As String = ""
        Try
            Dim nNumero As Integer = 0
            nNumero = LegNuma(strDitta, "NT", " ", 0, True)
            nNumero = AggNuma(strDitta, "NT", " ", 0, nNumero, False, False, "")

            If strTitolo = "" Then
                strTitolo = "Senza titolo"
            End If

            strSQL = String.Format( _
                          "insert into tabnote (codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot) values ({0}, {1}, {2}, {3}, {4}, {5})", _
                                      CStrSQL(strDitta), _
                                      nNumero, _
                                      CStrSQL(Left(strTitolo, 80).Replace(iBNewline, vbNewLine)), _
                                      CStrSQL("S"), _
                                      strConto, _
                                      CStrSQL(strNote.Replace(iBNewline, vbNewLine)) _
                                  )

            'strSQL = "insert into tabnote (codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot)" & _
            '  " VALUES  ( " & CStrSQL(strDitta) & ", " & nNumero & ", " & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & ", 'S', " & _
            '  strConto & ", " & _
            '  CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & _
            '  " ) "

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare UpdateCliforTabNoteData")> _
    Public Overridable Function UpdateCliforTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String, _
                                          ByVal strTitolo As String, ByVal strNote As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE tabnote" & _
                     " SET tb_desnote=" & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & "," & _
                     " tb_testonot=" & CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & "," & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND tb_codnote =" & strProgressivo & _
                     " AND tb_contonot =" & strConto
            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateClifor(ByVal strDitta As String, ByVal strConto As String, ByVal strDati As String()) As Boolean
        Dim strSQL As String = ""

        Const posTelefono1 As Integer = 3
        'Const posTelefono2 As Integer = 4
        Const posFax As Integer = 5
        Const posCell As Integer = 6
        Const posEmail As Integer = 7
        Const posPiva As Integer = 8
        Const posRagioneSociale As Integer = 9
        Const posAddress As Integer = 10
        Const posCap As Integer = 11
        Const posCity As Integer = 12
        Const posProvincia As Integer = 13
        Const posCodicePagamento As Integer = 14
        Const posCodiceFiscale As Integer = 15

        Try

            strSQL = "UPDATE anagra" & _
                    " SET an_telef=" & CStrSQL(Left(strDati(posTelefono1), 18)) & "," & _
                    " an_faxtlx=" & CStrSQL(Left(strDati(posFax), 18)) & "," & _
                    " an_cell=" & CStrSQL(Left(strDati(posCell), 18)) & "," & _
                    " an_email=" & CStrSQL(Left(strDati(posEmail), 50)) & "," & _
                    " an_pariva=" & CStrSQL(Left(strDati(posPiva), 11)) & "," & _
                    " an_descr1=" & CStrSQL(Left(strDati(posRagioneSociale), 50)) & "," & _
                    " an_indir=" & CStrSQL(Left(strDati(posAddress), 70)) & "," & _
                    " an_cap=" & CStrSQL(Left(strDati(posCap), 5)) & "," & _
                    " an_citta=" & CStrSQL(Left(strDati(posCity), 50)) & "," & _
                    " an_prov=" & CStrSQL(Left(strDati(posProvincia), 2)) & "," & _
                    " an_codpag=" & NTSCInt(strDati(posCodicePagamento)) & "," & _
                    " an_codfis=" & CStrSQL(Left(strDati(posCodiceFiscale), 16)) & "," & _
                    " an_opnome=" & CStrSQL(Left(oApp.User.Nome, 20)) & "," & _
                    " an_ultagg=" & CDataOraSQL(Now) & _
                    " WHERE codditt=" & CStrSQL(strDitta) & _
                    " AND an_conto =" & strConto

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    <Obsolete("Questo metodo è deprecato, usare UpdateCliforNoteData")> _
    Public Overridable Function UpdateCliforNote(ByVal strDitta As String, ByVal strConto As String, ByVal strNote As String, ByVal strTitolo As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE anagra" & _
                     " SET an_note2=" & CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & "," & _
                     " an_opnome=" & CStrSQL(Left(oApp.User.Nome, 20)) & "," & _
                     " an_ultagg=" & CDataOraSQL(Now) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & strConto

            ' " an_note=" & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & "," & _

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function




#End Region

#Region "Export tracciati"

    Public Overridable Function GetAgenti(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetAgenti.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCodpaga(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCodpaga.sql", oApp.NetDir)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetComuni(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetComuni.sql", oApp.NetDir)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetNazioni(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetNazioni.sql", oApp.NetDir)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetInfo(ByRef dttOut As DataTable, ByVal strRelease As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetInfo.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@release", CStrSQL(strRelease))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetClassiSconto(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetClassiSconto.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetPorto(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetPorto.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforCate(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforCate.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCanaliVendita(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCanaliVendita.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetClifor(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strFiltraConAgenti As String, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            If oApp.BUSNET_VERSIONDB < 19 Then
                strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforOLD.sql", oApp.NetDir)
            Else
                strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetClifor.sql", oApp.NetDir)
            End If

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@filtra_con_agenti", CStrSQL(strFiltraConAgenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforBlo(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforBlo.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforNote(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforNote.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforDestdiv(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforDestdiv.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforAge(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforAge.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforDettCon(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforDettCon.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforFatt(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforFatt.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforTestDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                                 Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforTestDoc.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@gg_documenti", CStrSQL(strGiorniStoricoDocumenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            'Dim uc As New UniqueConstraint(dttOut.Columns("xx_numreg"))
            'dttOut.Constraints.Add(uc)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforRighDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                                     Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforRighDoc.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@gg_documenti", CStrSQL(strGiorniStoricoDocumenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforScaDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforScaDoc.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@anni", CStrSQL(2))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetScaDocPush(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetScadocPush.sql", oApp.NetDir)

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArt(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String) As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArt.sql", oApp.NetDir)
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function

    Public Overridable Function GetArtUM(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String) As Boolean

        Dim strSQL As String = ""
        Try

            ' Inutile prevedere una query GetArtUM che sarebbe identica alla GetArt.
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArt.sql", oApp.NetDir)
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function

    Public Overridable Function GetArtLingua(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtLingua.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtGiacenze(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtGiacenze.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtUltVen(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strGiorniUltVen As String = "180") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtUltVen.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@gg_ultacqven", CStrSQL(strGiorniUltVen))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtUltAcq(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strGiorniUltAcq As String = "180") As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtUltAcq.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@gg_ultacqven", CStrSQL(strGiorniUltAcq))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtStoart(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strFiltroGiorniStoArt As String = "180", Optional ByVal strIncludiNonEvasi As String = "1") As Boolean
        'per ogni articolo/cliente prendo l'ultimo ordine
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtStoart.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@giornistorico", CStrSQL(strFiltroGiorniStoArt))
            strSQL = strSQL.Replace("@flg_includi_non_evasi", CStrSQL(strIncludiNonEvasi))



            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtListini(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, ByVal EstraiSoloListiniUMV As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtListini.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@estrai_solo_listini_umv", CStrSQL(EstraiSoloListiniUMV))


            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeads(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                         Optional ByVal strIncludiLeadConClienti As String = "0") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeads.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadConClienti)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCampagne(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCampagne.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadAccessi(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadAccessi.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadAccessiCrm(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadAccessiCrm.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadCate(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadCate.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadConcorrenti(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadConcorrenti.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadDetCon(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                         Optional ByVal strIncludiLeadConClienti As String = "0") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadDetCon.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadConClienti)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetLeadMansioni(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadMansioni.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadModAcquisizione(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadModAcquisizione.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadNote(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadNote.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadTestOff(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String,
                                               Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadTestOff.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadClienti)
            strSQL = strSQL.Replace("@gg_offerte", CStrSQL(strFiltroGiorniOfferte))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadRighOff(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String, _
                                               Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadRighOff.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadClienti)
            strSQL = strSQL.Replace("@gg_offerte", CStrSQL(strFiltroGiorniOfferte))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtSconti(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtSconti.sql", oApp.NetDir)
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtCatalogo(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String) As Boolean
        'nb: escludo gli articoli descrittivi (eccetto 'd'), gli articoli TCO e gli articoli a varianti non movimentabili (la radice della variante)
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtCatalogo.sql", oApp.NetDir)
            ' Se esiste una query personalizzata sovrascrivo quella standard
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetOleObjects(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String) As Boolean
        'nb: escludo gli articoli descrittivi (eccetto 'd'), gli articoli TCO e gli articoli a varianti non movimentabili (la radice della variante)
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetOleObjects.sql", oApp.NetDir)
            ' Se esiste una query personalizzata sovrascrivo quella standard
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

#End Region

End Class
