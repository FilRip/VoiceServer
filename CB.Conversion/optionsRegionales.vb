''' <summary>
''' Retourne les OPTIONS REGIONALES ET LINGUISTIQUES défini dans le Windows sur lequel ce programme tourne
''' </summary>
''' <remarks>Attention, cette classe n'est pas compatible Mono ou tout autre plateforme a part WINDOWS</remarks>
Public Class optionsRegionales

    Private Declare Function GetLocaleInfo Lib "kernel32" Alias "GetLocaleInfoA" (ByVal Locale As Integer, ByVal LCTYPE As Integer, ByVal lpLCData As String, ByVal cchData As Integer) As Integer
    Private Declare Function GetUserDefaultLCID Lib "kernel32" () As Integer

    Public Enum INFO_LOCALE As Integer
        LANGUE_ID = 0
        LANGUE_TEXTE = 1
        LANGUE_ABREGEE = 2
        LANGUE_TEXTE_LOCALE = 3

        PAYS_ID = 4
        PAYS_TEXTE = 5
        PAYS_ABREGE = 6
        PAYS_TEXTE_LOCALE = 7

        CODE_DEFAUT_LANGUE = 8
        CODE_DEFAUT_PAYS = 9
        CODE_PAGE = 10

        SEPARATEUR_DECIMAL = 11
        SEPARATEUR_MILLIERS = 12

        SYMBOL_MONETAIRE_LOCALE = 13
        SYMBOL_MONETAIRE_INTERNATIONAL = 14
        SEPARATEUR_DECIMAL_MONETAIRE = 15
        SEPARATEUR_MILLIERS_MONETAIRE = 16
        CODE_MONETAIRE_LOCALE = 17
        CODE_MONETAIRE_INTERNATIONAL = 18
        FORMAT_MONETAIRE_POSITIF = 19
        FORMAT_MONETAIRE_NEGATIF = 20

        SEPARATEUR_DATE = 21
        SEPARATEUR_HEURE = 22
        FORMAT_DATE_COURTE = 23
        FORMAT_DATE_LONGUE = 24
        CODE_DATE_COURTE = 25
        CODE_DATE_LONGUE = 26
        FORMAT_12_24_HEURE = 27
        FORMAT_SIECLE = 28
        ZERO_POUR_HEURE = 29
        ZERO_POUR_JOUR = 30
        ZERO_POUR_MOIS = 31
        FORMAT_AM = 32
        FORMAT_PM = 33

        LANGUE_ANGLAIS = 34
        PAYS_ANGLAIS = 35

        FORMAT_HEURE = 36

        MONETAIRE_ANGLAIS = 37
        MONETAIRE_TEXTE = 38
    End Enum

    ''' <summary>
    ''' Retourne un double converti depuis le contenu de "texte" qui est en chaine
    ''' En respectant le caractère de séparation défini dans option regionale de Windows
    ''' </summary>
    ''' <param name="texte">Le texte, contenu à traduire en double</param>
    ''' <returns>Un double, peut etre zéro en cas d'erreur</returns>
    ''' <remarks></remarks>
    Public Shared Function convertirEnDouble(ByRef texte As String) As Double
        Dim sep As String = retourneParametre(INFO_LOCALE.SEPARATEUR_DECIMAL)
        If (sep Is Nothing) Then Throw New Exception("Impossible de lire cette option régionale")
        Return Double.Parse(texte.Replace(" ", "").Replace(",", sep).Replace(".", sep))
    End Function

    ''' <summary>
    ''' Retourne sous forme de chaine le parametre 'option régionale' demandé en Enum
    ''' </summary>
    ''' <param name="typeParam">Liste des parametre possibles que l'on peut demander</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function retourneParametre(ByVal typeParam As INFO_LOCALE) As String
        Dim retour As String = ""
        Dim nLength As Integer
        Dim nLocale As Integer
        Dim codeParam As Integer = retourneCodeLocal(typeParam)

        Try
            If (codeParam = &HFFFF) Then Throw New Exception("typeParam demandé inconnu")
            nLocale = GetUserDefaultLCID()
            nLength = GetLocaleInfo(nLocale, codeParam, vbNullString, 0) - 1
            GetLocaleInfo(nLocale, codeParam, retour, nLength)
        Catch ex As Exception
            retour = Nothing
        End Try
        Return retour
    End Function

    Private Shared Function retourneCodeLocal(ByVal infoLocale As INFO_LOCALE) As Integer
        Select Case infoLocale
            Case INFO_LOCALE.LANGUE_ID
                Return &H1
            Case INFO_LOCALE.LANGUE_TEXTE
                Return &H2
            Case INFO_LOCALE.LANGUE_ABREGEE
                Return &H3
            Case INFO_LOCALE.LANGUE_TEXTE_LOCALE
                Return &H4

            Case INFO_LOCALE.PAYS_ID
                Return &H5
            Case INFO_LOCALE.PAYS_TEXTE
                Return &H6
            Case INFO_LOCALE.PAYS_ABREGE
                Return &H7
            Case INFO_LOCALE.PAYS_TEXTE_LOCALE
                Return &H8

            Case INFO_LOCALE.CODE_DEFAUT_LANGUE
                Return &H9
            Case INFO_LOCALE.CODE_DEFAUT_PAYS
                Return &HA
            Case INFO_LOCALE.CODE_PAGE
                Return &HB

            Case INFO_LOCALE.SEPARATEUR_DECIMAL
                Return &HE
            Case INFO_LOCALE.SEPARATEUR_MILLIERS
                Return &HF

            Case INFO_LOCALE.SYMBOL_MONETAIRE_LOCALE
                Return &H14
            Case INFO_LOCALE.SYMBOL_MONETAIRE_INTERNATIONAL
                Return &H15
            Case INFO_LOCALE.SEPARATEUR_DECIMAL_MONETAIRE
                Return &H16
            Case INFO_LOCALE.SEPARATEUR_MILLIERS_MONETAIRE
                Return &H17
            Case INFO_LOCALE.CODE_MONETAIRE_LOCALE
                Return &H19
            Case INFO_LOCALE.CODE_MONETAIRE_INTERNATIONAL
                Return &H1A
            Case INFO_LOCALE.FORMAT_MONETAIRE_POSITIF
                Return &H1B
            Case INFO_LOCALE.FORMAT_MONETAIRE_NEGATIF
                Return &H1C

            Case INFO_LOCALE.SEPARATEUR_DATE
                Return &H1D
            Case INFO_LOCALE.SEPARATEUR_HEURE
                Return &H1E
            Case INFO_LOCALE.FORMAT_DATE_COURTE
                Return &H1F
            Case INFO_LOCALE.FORMAT_DATE_LONGUE
                Return &H20
            Case INFO_LOCALE.CODE_DATE_COURTE
                Return &H21
            Case INFO_LOCALE.CODE_DATE_LONGUE
                Return &H22
            Case INFO_LOCALE.FORMAT_12_24_HEURE
                Return &H23
            Case INFO_LOCALE.FORMAT_SIECLE
                Return &H24
            Case INFO_LOCALE.ZERO_POUR_HEURE
                Return &H25
            Case INFO_LOCALE.ZERO_POUR_JOUR
                Return &H26
            Case INFO_LOCALE.ZERO_POUR_MOIS
                Return &H27
            Case INFO_LOCALE.FORMAT_AM
                Return &H28
            Case INFO_LOCALE.FORMAT_PM
                Return &H29

            Case INFO_LOCALE.LANGUE_ANGLAIS
                Return &H1001
            Case INFO_LOCALE.PAYS_ANGLAIS
                Return &H1002

            Case INFO_LOCALE.FORMAT_HEURE
                Return &H1003

            Case INFO_LOCALE.MONETAIRE_ANGLAIS
                Return &H1007
            Case INFO_LOCALE.MONETAIRE_TEXTE
                Return &H1008
            Case Else
                Return &HFFFF
        End Select
    End Function

End Class
