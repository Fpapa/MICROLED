Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class DAO

    Private Shared _ProvedorOrigem As String
    Private Shared _BaseOrigem As String
    Private Shared _ServidorOrigem As String
    Private Shared _SchemaOrigem As String
    Private Shared _UsuarioOrigem As String
    Private Shared _SenhaOrigem As String

    Private Shared _ProvedorDestino As String
    Private Shared _BaseDestino As String
    Private Shared _ServidorDestino As String
    Private Shared _SchemaDestino As String
    Private Shared _UsuarioDestino As String
    Private Shared _SenhaDestino As String

    Private Shared _TabelaDestino As String

    Shared Sub New()

        Dim ConfigurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()

        BaseOrigem = My.Settings.BaseOrigem
        BaseDestino = My.Settings.BaseDestino
        ProvedorOrigem = My.Settings.ProvedorOrigem
        ProvedorDestino = My.Settings.ProvedorDestino
        ServidorOrigem = My.Settings.ServidorOrigem
        ServidorDestino = My.Settings.ServidorDestino
        UsuarioOrigem = My.Settings.UsuarioOrigem
        UsuarioDestino = My.Settings.UsuarioDestino
        SchemaOrigem = My.Settings.SchemaOrigem
        SchemaDestino = My.Settings.SchemaDestino
        SenhaOrigem = My.Settings.SenhaOrigem
        SenhaDestino = My.Settings.SenhaDestino
        TabelaDestino = My.Settings.TabelaDestino

    End Sub

    Public Shared Property BaseOrigem() As String
        Get
            Return _BaseOrigem
        End Get
        Set(ByVal value As String)
            _BaseOrigem = value
        End Set
    End Property
    Public Shared Property BaseDestino() As String
        Get
            Return _BaseDestino
        End Get
        Set(ByVal value As String)
            _BaseDestino = value
        End Set
    End Property

    Public Shared ReadOnly Property StringConexaoOracle(OD$) As String
        Get
            If OD$ = "O" Then
                Return String.Format("Provider={0};Data Source={1};User ID={2};Password={3}", ProvedorOrigem, ServidorOrigem, UsuarioOrigem, SenhaOrigem)
            Else
                Return String.Format("Provider={0};Data Source={1};User ID={2};Password={3}", ProvedorDestino, ServidorDestino, UsuarioDestino, SenhaDestino)
            End If

        End Get
    End Property

    Public Shared ReadOnly Property StringConexaoSQLServer(OD$) As String
        Get
            If OD$ = "O" Then
                Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServidorOrigem, SchemaOrigem, UsuarioOrigem, SenhaOrigem)
            Else
                Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ServidorDestino, SchemaDestino, UsuarioDestino, SenhaDestino)
            End If
        End Get
    End Property

    Public Shared Property ServidorOrigem As String
        Get
            Return _ServidorOrigem
        End Get
        Set(value As String)
            _ServidorOrigem = value
        End Set
    End Property

    Public Shared Property SchemaOrigem As String
        Get
            Return _SchemaOrigem
        End Get
        Set(value As String)
            _SchemaOrigem = value
        End Set
    End Property

    Public Shared Property UsuarioOrigem As String
        Get
            Return _UsuarioOrigem
        End Get
        Set(value As String)
            _UsuarioOrigem = value
        End Set
    End Property

    Public Shared Property SenhaOrigem As String
        Get
            Return _SenhaOrigem
        End Get
        Set(value As String)
            _SenhaOrigem = value
        End Set
    End Property

    Public Shared Property ServidorDestino As String
        Get
            Return _ServidorDestino
        End Get
        Set(value As String)
            _ServidorDestino = value
        End Set
    End Property

    Public Shared Property SchemaDestino As String
        Get
            Return _SchemaDestino
        End Get
        Set(value As String)
            _SchemaDestino = value
        End Set
    End Property

    Public Shared Property UsuarioDestino As String
        Get
            Return _UsuarioDestino
        End Get
        Set(value As String)
            _UsuarioDestino = value
        End Set
    End Property

    Public Shared Property SenhaDestino As String
        Get
            Return _SenhaDestino
        End Get
        Set(value As String)
            _SenhaDestino = value
        End Set
    End Property

    Public Shared Property ProvedorOrigem As String
        Get
            Return _ProvedorOrigem
        End Get
        Set(value As String)
            _ProvedorOrigem = value
        End Set
    End Property

    Public Shared Property ProvedorDestino As String
        Get
            Return _ProvedorDestino
        End Get
        Set(value As String)
            _ProvedorDestino = value
        End Set
    End Property

    Public Shared Property TabelaDestino As String
        Get
            Return _TabelaDestino
        End Get
        Set(value As String)
            _TabelaDestino = value
        End Set
    End Property

    Public Shared Function Execute(OD$, ByVal SQL As String) As Boolean

        Dim Ret As Boolean

        If (OD$ = "O" And DAO.BaseOrigem = "ORACLE") Or OD$ = "D" And DAO.BaseDestino = "ORACLE" Then
            Using Connection As New OleDbConnection(StringConexaoOracle(OD$))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New OleDbCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Command.CommandTimeout = 200
                    Ret = Command.ExecuteNonQuery()
                    Connection.Close()
                End Using
            End Using
        End If

        If (OD = "D" And DAO.BaseOrigem = "MSSQL") Or (OD$ = "D" And DAO.BaseDestino = "MSSQL") Then
            Using Connection As New SqlConnection(StringConexaoSQLServer(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New SqlCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Command.CommandTimeout = 200
                    Ret = Command.ExecuteNonQuery()
                    Connection.Close()
                End Using
            End Using
        End If

        Return Ret

    End Function

    Public Shared Function ExecuteScalar(OD$, ByVal SQL As String) As String

        Dim Ret As String = String.Empty
        If (OD$ = "O" And DAO.BaseOrigem = "ORACLE") Or OD$ = "D" And DAO.BaseDestino = "ORACLE" Then
            Using Connection As New OleDbConnection(StringConexaoOracle(OD$))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New OleDbCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Ret = Command.ExecuteScalar()
                    Connection.Close()
                End Using
            End Using
        End If

        If (OD = "D" And DAO.BaseOrigem = "MSSQL") Or (OD$ = "D" And DAO.BaseDestino = "MSSQL") Then
            Using Connection As New SqlConnection(StringConexaoSQLServer(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New SqlCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Command.CommandTimeout = 200
                    Ret = Command.ExecuteScalar()
                    Connection.Close()
                End Using
            End Using
        End If
        Return Ret

    End Function

    Public Shared Function ExecuteRetornaID(OD As String, ByVal SQL As String, ByVal NomeTabela As String) As String

        Dim Ret As String = String.Empty

        If (OD$ = "O" And DAO.BaseOrigem = "ORACLE") Or OD$ = "D" And DAO.BaseDestino = "ORACLE" Then
            Using Connection As New OleDbConnection(StringConexaoOracle(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New OleDbCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Ret = Command.ExecuteScalar()
                    Connection.Close()
                End Using
            End Using
        End If

        If (OD = "D" And DAO.BaseOrigem = "MSSQL") Or (OD$ = "D" And DAO.BaseDestino = "MSSQL") Then
            Using Connection As New SqlConnection(StringConexaoSQLServer(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New SqlCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Ret = Command.ExecuteNonQuery()
                    Dim meuID As String
                    meuID = "SELECT IDENT_CURRENT('" & NomeTabela & "')"
                    Ret = DAO.ExecuteScalar(OD, meuID)
                    Connection.Close()
                End Using
            End Using

        End If

        Return Ret

    End Function

    Public Shared Function Consultar(OD$, ByVal SQL As String) As DataTable
        Dim Ds As New DataTable
        If (OD$ = "O" And DAO.BaseOrigem = "ORACLE") Or OD$ = "D" And DAO.BaseDestino = "ORACLE" Then
            Using Connection As New OleDbConnection(StringConexaoOracle(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New OleDbCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Dim Rdr As OleDbDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    If Rdr.HasRows Then Ds.Load(Rdr)
                    Connection.Close()
                End Using
            End Using
        End If

        If (OD = "D" And DAO.BaseOrigem = "MSSQL") Or (OD$ = "D" And DAO.BaseDestino = "MSSQL") Then
            Using Connection As New SqlConnection(StringConexaoSQLServer(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Command As New SqlCommand(SQL, Connection)
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Connection.Open()
                    Dim Rdr As SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                    If Rdr.HasRows Then Ds.Load(Rdr)
                    Connection.Close()
                End Using
            End Using
        End If

        Return Ds
        'Dim Ds As New DataTable


    End Function
    Public Shared Function List(OD$, ByVal SQL As String) As DataTable

        Dim Ds As New DataSet()

        If (OD$ = "O" And DAO.BaseOrigem = "ORACLE") Or OD$ = "D" And DAO.BaseDestino = "ORACLE" Then
            Using Con As New OleDbConnection(StringConexaoOracle(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Adp As New OleDbDataAdapter(New OleDbCommand(SQL, Con))
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Try
                        Adp.Fill(Ds)
                    Catch generatedExceptionName As Exception
                        Return Nothing
                    End Try
                    Return Ds.Tables(0)
                End Using
            End Using
        End If


        If (OD = "D" And DAO.BaseOrigem = "MSSQL") Or (OD$ = "D" And DAO.BaseDestino = "MSSQL") Then
            Using Con As New SqlConnection(StringConexaoSQLServer(OD))
#Disable Warning CA2100 ' Review SQL queries for security vulnerabilities
                Using Adp As New SqlDataAdapter(New SqlCommand(SQL, Con))
#Enable Warning CA2100 ' Review SQL queries for security vulnerabilities
                    Try
                        Adp.SelectCommand.CommandTimeout = 200
                        Adp.Fill(Ds)
                    Catch generatedExceptionName As Exception
                        Return Nothing
                    End Try
                    Return Ds.Tables(0)
                End Using
            End Using
        End If

        Return Nothing
    End Function

End Class


