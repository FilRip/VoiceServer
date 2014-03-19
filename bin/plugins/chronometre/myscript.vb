Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Speech
Imports Microsoft.Speech
Imports Microsoft.Kinect

Namespace main

    Public Class mainClass

        Public Shared Sub mainSub(whatHeSay As Object)
            Dim temps As String
            Dim duree As Integer
            temps = whatHeSay.ToString().Replace("rappel moi dans ", "").Replace("minutes", "").Trim()
            Select Case temps
                Case "une"
                    duree = 1
                Case "deux"
                    duree = 2
                Case "trois"
                    duree = 3
                Case "quatre"
                    duree = 4
                Case "cinq"
                    duree = 5
                Case "six"
                    duree = 6
                Case "sept"
                    duree = 7
                Case "huit"
                    duree = 8
                Case "neuf"
                    duree = 9
                Case "dix"
                    duree = 10
                Case "onze"
                    duree = 11
                Case "douze"
                    duree = 12
                Case "treize"
                    duree = 13
                Case "quatorze"
                    duree = 14
                Case "quinze"
                    duree = 15
            End Select
            VoiceServer.Program.tpm.addJob(AddressOf retour, Nothing, duree * 60 * 1000)
        End Sub

        Public Shared Sub retour(param As Object)
            VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Je dois vous rappeler. Le temps est écoulé.")
        End Sub

    End Class

End Namespace
