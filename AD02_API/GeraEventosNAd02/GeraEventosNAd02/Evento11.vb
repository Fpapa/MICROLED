Public Class Evento11
    Public Property tipoOperacao As String
    Public Property idEvento As String
    Public Property dtHrOcorrencia As String
    Public Property dtHrRegistro As String
    Public Property cpfOperOcor As String
    Public Property cpfOperReg As String
    Public Property protocoloEventoRetifCanc As String
    Public Property contingencia As Boolean
    Public Property codRecinto As String
    Public Property numConteiner As String
    Public Property placaSemirreboque As String
    Public Property localizacao As Localizacao
    Public Property posicaoNavio As String
    Public Property conferenciaFisica As Boolean
    Public Property solicitanteFisica As String
    Public Property avaria As Boolean
    Public Property areaConteiner As String
    Public Property listaCameras() As String
End Class
Public Class Localizacao
    Public Property quadra As String
    Public Property pilha As String
    Public Property altura As String
    Public Property nivel As String
End Class
