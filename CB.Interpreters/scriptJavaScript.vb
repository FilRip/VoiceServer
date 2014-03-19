'Imports Jint

'    Public Class scriptJavaScript
'        Inherits JintEngine

'        ''' <summary>
'        ''' Interprete une chaine, ou un programme entier, en javascript
'        ''' Cette(ces) ligne(s) ainsi envoyées sont concatennée à celle déjà envoyée à cet objet "scriptJavaScript"
'        ''' Si bien que l'on peut demander une réinterprétation d'une fonction javascript déjà envoyée par exemple, même avec d'autres parametres
'        ''' </summary>
'        ''' <param name="chaine">La chaine à interpréter en Javascript</param>
'        ''' <returns>Ce que le javascript peut retourner dans son type natif, si par exemple on demande l'interprétation d'une fonction javascript qui retourne un résultat</returns>
'        ''' <remarks></remarks>
'        Public Function interprete(ByVal chaine As String) As Object
'            Dim result As Object
'            Try
'                result = Run(chaine)
'            Catch ex As Exception
'                result = Nothing
'            End Try
'            Return result
'        End Function

'    End Class
