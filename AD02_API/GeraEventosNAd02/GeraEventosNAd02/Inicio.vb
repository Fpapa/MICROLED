Imports System.IO
Imports System.Text
Imports Newtonsoft.Json

Public Class Inicio
    Public Shared Sub Integra()
        Try
            FileIO.WriteToFile(Now & " Inicio da Integração" & vbNewLine)
            Dim tbEve As New DataTable




            If My.Settings.Eve_11 = True Then

                Sql = $"Select
                            V.AUTONUM, V.AUTONUMCNTR, V.CNPJRECINTO, 
                            V.DATA, V.FLAG_DESOVA, V.FLAG_HISTORICO, 
                            V.ID, V.ID_CONTEINER, V.ISOCODE, 
                            V.ORIGEM, V.PATIO, V.PESO, 
                            V.RECINTO, V.SISTEMA, V.TAMANHO, 
                            V.VALIDA, V.VIAGEM, V.YARD,V.CODERECINTO,
                            V.Avaria,V.ConferenciaFisica,V.Contingencia,
                            V.ListaCamera,CPF
                            FROM VW_AD02N_EVE11 V WHERE ROWNUM<=10"
                tbEve = DAO.Consultar("O", Sql)
                For I = 0 To tbEve.Rows.Count - 1
                    Dim Eve As New Evento11
                    With Eve
                        .areaConteiner = tbEve.Rows(I)("YARD")
                        .avaria = IIf(tbEve.Rows(I)("AVARIA") = 0, False, True)
                        .codRecinto = tbEve.Rows(I)("CODERECINTO")
                        .conferenciaFisica = IIf(tbEve.Rows(I)("ConferenciaFisica") = 0, False, True)
                        .contingencia = IIf(tbEve.Rows(I)("Contingencia") = 0, False, True)
                        .cpfOperOcor = tbEve.Rows(I)("CPF").REPLACE("-", "").REPLACE(".", "")
                        .cpfOperReg = tbEve.Rows(I)("CPF").REPLACE("-", "").REPLACE(".", "")
                        .dtHrOcorrencia = tbEve.Rows(I)("DATA")
                        .dtHrRegistro = tbEve.Rows(I)("DATA")
                        .idEvento = Microsoft.VisualBasic.Right("000000000000" & DAO.ExecuteScalar("O", "SELECT OPERADOR.SEQ_EVE_AD02_NEW.NEXTVAL FROM DUAL"), 12)
                        .listaCameras = tbEve.Rows(I)("ListaCamera")
                        Dim Loc As New Localizacao
                        Dim ValidaNavis As Boolean
                        ValidaNavis = False
                        If tbEve.Rows(I)("patio") = 1 And Len(tbEve.Rows(I)("YARD").ToString) = 6 Then
                            If Microsoft.VisualBasic.Asc(Microsoft.VisualBasic.Mid(tbEve.Rows(I)("yard").ToString, 5, 1)) >= 65 And Microsoft.VisualBasic.Asc(Microsoft.VisualBasic.Mid(tbEve.Rows(I)("yard").ToString, 5, 1)) <= 90 Then
                                If IsNumeric(Microsoft.VisualBasic.Right(tbEve.Rows(I)("yard").ToString, 1)) Then
                                    ValidaNavis = True
                                End If
                            End If
                        End If

                        If tbEve.Rows(I)("valida") = 1 Then
                            Loc.altura = Microsoft.VisualBasic.Right(tbEve.Rows(I)("YARD").ToString, 1)
                        ElseIf ValidaNavis Then
                            Loc.altura = Microsoft.VisualBasic.Right(tbEve.Rows(I)("YARD").ToString, 1)
                        Else
                            Loc.altura = ""
                        End If

                        If tbEve.Rows(I)("valida") = 1 Then
                            If Len(tbEve.Rows(I)("YARD").ToString) = 7 Then
                                Loc.nivel = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 5, 2)
                            Else
                                Loc.nivel = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 4, 2)
                            End If
                        ElseIf ValidaNavis Then
                            Loc.nivel = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 5, 1)
                        Else
                            Loc.nivel = ""
                        End If

                        If tbEve.Rows(I)("valida") = 1 Then
                            If Len(tbEve.Rows(I)("YARD").ToString) = 7 Then
                                Loc.pilha = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 3, 2)
                                Loc.quadra = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 1, 2)
                            Else
                                Loc.pilha = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 2, 2)
                                Loc.quadra = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 1, 1)
                            End If
                        ElseIf ValidaNavis Then
                            Loc.pilha = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 3, 2)
                            Loc.quadra = Microsoft.VisualBasic.Mid(tbEve.Rows(I)("YARD").ToString, 1, 2)
                        Else
                            Loc.pilha = ""
                            Loc.quadra = ""
                        End If

                        .localizacao = Loc
                        .numConteiner = tbEve.Rows(I)("ID_CONTEINER").REPLACE("-", "")
                        .placaSemirreboque = ""
                        .posicaoNavio = ""
                        .protocoloEventoRetifCanc = ""
                        .solicitanteFisica = "1"
                        .tipoOperacao = "I"
                    End With

                    Dim jsonEve = JsonConvert.SerializeObject(Eve)

                    If My.Settings.BaseDestino = "MSSQL" Then
                        Sql = $"INSERT INTO [{My.Settings.SchemaDestino}].[dbo].[{My.Settings.TabelaDestino}]
                        ([Evento]
                        ,[Envio_JSON]
                        )
                        VALUES
                        (11
                        ,'{jsonEve.ToString}'
                        )
                        "
                        DAO.Execute("D", Sql)
                    Else
                        Sql = $"INSERT INTO {My.Settings.SchemaDestino}.{My.Settings.TabelaDestino}
                        (id,
                        Evento
                        ,Envio_JSON
                        )
                        VALUES
                        (SEQ{My.Settings.TabelaDestino}
                        ,11
                        ,'{jsonEve.ToString}'
                        )
                        "
                        DAO.Execute("D", Sql)
                    End If

                Next I

            End If
            FileIO.WriteToFile(Now & " Fim da Integração" & vbNewLine)


        Catch ex As Exception

        End Try
    End Sub
End Class
