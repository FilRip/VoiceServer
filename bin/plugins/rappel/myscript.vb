Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Speech
Imports Microsoft.Speech
Imports Microsoft.Kinect

Namespace main

    Public Class mainClass

        Private Shared listeGrammar As New List(Of System.Speech.Recognition.Grammar)
        Private Shared listeKinectGrammar As New List(Of Microsoft.Speech.Recognition.Grammar)

        Public Shared Sub mainSub(whatHeSay As Object)
            If (Not (VoiceServer.instances.ClassParam.kinect)) Then
                Dim gb As System.Speech.Recognition.GrammarBuilder = New System.Speech.Recognition.GrammarBuilder()
                gb.Append(New System.Speech.Recognition.SemanticResultKey("root", "Oui"))
                Dim g3 As System.Speech.Recognition.Grammar = New System.Speech.Recognition.Grammar(gb)
                VoiceServer.instances.SpeechSystem.getInstance().speechMicEngine.LoadGrammar(g3)
                listeGrammar.add(g3)
                AddHandler VoiceServer.instances.SpeechSystem.getInstance().speechMicEngine.SpeechRecognized, AddressOf speechMicEngine_SpeechRecognized
            Else
                Dim gb As Microsoft.Speech.Recognition.GrammarBuilder = New Microsoft.Speech.Recognition.GrammarBuilder()
                gb.Append(New Microsoft.Speech.Recognition.SemanticResultKey("root", "Oui"))
                Dim g3 As Microsoft.Speech.Recognition.Grammar = New Microsoft.Speech.Recognition.Grammar(gb)
                VoiceServer.instances.SpeechSystem.getInstance().speechEngine.LoadGrammar(g3)
                listeKinectGrammar.add(g3)
                AddHandler VoiceServer.instances.SpeechSystem.getInstance().speechEngine.SpeechRecognized, AddressOf SpeechRecognized
            End If
            VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Dites moi oui")
        End Sub

        Public Shared Sub speechMicEngine_SpeechRecognized(sender As Object, e As System.Speech.Recognition.SpeechRecognizedEventArgs)
            Dim ConfidenceThreshold As Double = 0.3
            If (e.Result.Confidence >= ConfidenceThreshold) Then
                traiteEcoute(e.Result.Text)
            End If
        End Sub

        Public Shared Sub SpeechRecognized(sender As Object, e As Microsoft.Speech.Recognition.SpeechRecognizedEventArgs)
            Dim ConfidenceThreshold As Double = 0.3
            If (e.Result.Confidence >= ConfidenceThreshold) Then
                traiteEcoute(e.Result.Text)
            End If
        End Sub

        Public Shared Sub traiteEcoute(phrase As String)
            VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Bien reçu")
        End Sub

    End Class

End Namespace
