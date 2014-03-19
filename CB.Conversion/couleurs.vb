Public Class couleurs

    ''' <summary>
    ''' Retourne une couleur, codée en net framework d'après le contenu d'une chaine de caractère (Type : Rouge,Vert,Bleu)
    ''' </summary>
    ''' <param name="coulChaine">La chaine de caractère contenant la couleur (type R,B,V)</param>
    ''' <returns>Un code couleur</returns>
    ''' <remarks></remarks>
    Public Shared Function chaineVersCouleurs(ByVal coulChaine As String) As System.Drawing.Color
        Try
            Dim code As String()
            code = coulChaine.Split(",")
            Return System.Drawing.Color.FromArgb(code(0), code(1), code(2))
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Retourne, sous forme d'une chaine "Rouge,Vert,Bleu", le code couleur envoyé
    ''' </summary>
    ''' <param name="coul">Le code couleur</param>
    ''' <returns>Une chaine de caractère</returns>
    ''' <remarks></remarks>
    Public Shared Function couleursVersChaine(ByVal coul As System.Drawing.Color) As String
        Try
            Return coul.R.ToString & "," & coul.G.ToString & "," & coul.B.ToString
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

End Class
