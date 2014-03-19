Imports Microsoft.Scripting.Hosting
Imports IronPython.Hosting

Public Class scriptPython

    Public Function interpreteFichier(ByVal fileName As String, ByVal autoexec As Boolean) As ScriptScope
        Try
            Dim engine As ScriptEngine = Python.CreateEngine()
            Dim source As ScriptSource = engine.CreateScriptSourceFromFile(fileName)
            Dim scope As ScriptScope = engine.CreateScope()

            Dim op As ObjectOperations = engine.Operations
            source.Execute(scope)
            If (autoexec) Then
                Dim pyClass As Object = scope.GetVariable("autoexec")
                Dim instance As Object = op.CreateInstance(pyClass)
                op.InvokeMember(instance, "autoexec")
            End If
            Return scope
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Public Function interpreteTexte(ByVal script As String) As Object
        Try
            Dim engine As ScriptEngine = Python.CreateEngine()
            Dim source As ScriptSource = engine.CreateScriptSourceFromString(script)
            Dim scope As ScriptScope = engine.CreateScope()

            Return source.Execute(scope)
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

End Class
