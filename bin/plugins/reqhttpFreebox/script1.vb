Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Speech
Imports System.Security.Cryptography

Namespace main

    Public Class mainClass

        Public Shared Sub mainSub(whatHeSay As Object)
            Dim constructVar As String
            Dim token As String

            If (Not (System.IO.File.Exists("plugins\\reqhttpfreebox\\token.txt"))) Then
                constructVar = "{""app_id"":""fr.freebox.VoiceServer"",""app_name"":""IGOR"",""app_version"":""3.0.3"",""device_name"":""" & My.Computer.Name & """}"
                token = CB.Reseaux.GestionHTTP.retourneContenuPost("http://mafreebox.freebox.fr/api/v1/login/authorize/", constructVar)
                System.IO.File.AppendAllText("plugins\\reqhttpfreebox\\token.txt", token)
            Else
                token = System.IO.File.ReadAllText("plugins\\reqhttpfreebox\\token.txt")
                Dim result As String
                Dim trackId As String
                Dim challenge As String
                Dim temp As String
                trackId = token.substring(token.lastIndexOf("track_id") + 10).Replace("}", "")
                challenge = CB.Reseaux.GestionHTTP.retourneContenu("http://mafreebox.freebox.fr/api/v1/login/authorize/" & trackId)
                VoiceServer.instances.ClassParam.log(challenge)
                temp = result = CB.Reseaux.GestionHTTP.retourneContenu("http://mafreebox.freebox.fr/api/v1/login/")
                VoiceServer.instances.ClassParam.log("Temp=" & temp)
                result = CB.Reseaux.GestionHTTP.retourneContenuPost("http://mafreebox.freebox.fr/api/v1/login/session/", token)
                VoiceServer.instances.ClassParam.log(result)
            End If
        End Sub

    End Class

End Namespace
