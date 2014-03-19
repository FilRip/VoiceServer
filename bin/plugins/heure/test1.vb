Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Speech

Namespace main

    Public Class mainClass

        Public Shared Sub mainSub(whatHeSay As Object)
            VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Il est " & System.DateTime.Now.Hour.ToString() & " heure " & System.DateTime.Now.Minute.ToString())
        End Sub

    End Class

End Namespace
