
Public Class Service1

    Dim tmr As Timers.Timer
    Protected Overrides Sub OnStart(ByVal args() As String)

        FlagExecutando = False
        tmr = New Timers.Timer()

        tmr.Interval = 1000 * 60 * My.Settings.IntervaloMin

        AddHandler tmr.Elapsed, AddressOf TimerTick
        tmr.Enabled = True

    End Sub

    Protected Overrides Sub OnStop()

    End Sub

    Private Sub TimerTick(obj As Object, e As EventArgs)
        If Not FlagExecutando Then
            Inicio.Integra()
        End If
    End Sub

End Class
