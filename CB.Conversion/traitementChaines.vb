Public Class traitementChaines

    ''' <summary>
    ''' Converti une chaine unicode en chaine UTF8
    ''' </summary>
    ''' <param name="txtUnicode">La chaine unicode à convertir en UTF8</param>
    ''' <returns>Une chaine au format UTF8</returns>
    ''' <remarks></remarks>
    Public Shared Function unicodeToUTF8(ByVal txtUnicode As String) As String
        Dim src() As Byte
        src = Text.Encoding.Unicode.GetBytes(txtUnicode)
        Return Text.Encoding.UTF8.GetString(src)
    End Function

End Class
