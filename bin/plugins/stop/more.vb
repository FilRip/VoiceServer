Namespace main
    Public Class mainClass
        Public Static Sub mainSub(phrase As String)
            If (MsgBox("Eteindre l'ordinateur, etes-vous sur ?", vbYesNo) = vbYes) Then
                Dim p As New process
                p.startinfo.filename = "shutdown /s /t 0"
                p.start()
            End If
        End Sub
    End Class
End Namespace
