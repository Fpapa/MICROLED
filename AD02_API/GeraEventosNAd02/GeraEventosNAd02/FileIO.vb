Public Class FileIO
    Public Shared Sub WriteToFile(strToWrite As String)
        Dim Stream As IO.StreamWriter = Nothing
        Try
            Dim NomeStream As String
            NomeStream = Microsoft.VisualBasic.Format(Now, "yyyyMMdd_HH_") & "Ad02.log"
            Stream = New IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory & "\log\" & NomeStream, True)
            Stream.Write(strToWrite)
            Stream.Flush()
            Stream.Close()

        Catch ex As Exception

        End Try
    End Sub

End Class
