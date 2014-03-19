Public Class traitementBDD

    Public Shared Function lireChampSansDBNull(ByVal ligne As DataRow, ByVal nomChamp As String) As Object
        Dim val As Object = Nothing

        If (ligne.Item(nomChamp) Is Nothing) Then Throw New Exception("Nom de champs inexistant")
        Try
            val = ligne.Item(nomChamp)
        Catch ex As Exception
        End Try
        If (val Is DBNull.Value) Then
            If (ligne.Table.Columns.Item(nomChamp).DataType Is GetType(System.String)) Then
                val = ""
            ElseIf (ligne.Table.Columns.Item(nomChamp).DataType Is GetType(System.DateTime)) Then
                val = Nothing
            ElseIf (ligne.Table.Columns.Item(nomChamp).DataType Is GetType(System.Boolean)) Then
                val = False
            Else
                val = 0
            End If
        End If
        Return val
    End Function

End Class
