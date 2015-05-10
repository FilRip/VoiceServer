Public Class gestionFichierConfig

    ' Les lignes commencant par ";" (sans les guillemets) sont considérées comme des commentaires et sont donc ignorées par le système
    Private Const COMMENTAIRES As String = ";"

    Private _EntreeFichier As System.IO.FileStream
    Private _FichierConfig As System.IO.StreamReader
    Private _nomencours As String
    Private _section As String()
    Private _nomsection As String = ""

    ''' <summary>
    ''' Initialise le système de lecture de fichier
    ''' </summary>
    ''' <param name="nomfichier">Nom/emplacement du fichier INI</param>
    ''' <returns>True si le fichier à pu être ouvert, sinon False (fichier inexistant?)</returns>
    ''' <remarks>ATTENTION DEADLOCK POSSIBLE, LIRE INFO DANS LA FONCTION</remarks>
    Public Function FichierIniOuvrir(ByVal nomfichier As String) As Boolean
        ' INFO : Attention deadlock possible :
        ' Si le fichier fait zéro octet, il l'ouvre mais impossible de lire, et refuse de le refermer
        If (nomfichier = "") Then Return False
        Try
            Dim taille As Long = New IO.FileInfo(nomfichier).Length
            _EntreeFichier = New System.IO.FileStream(nomfichier, IO.FileMode.Open)
            _FichierConfig = New System.IO.StreamReader(_EntreeFichier, System.Text.Encoding.UTF8, False, taille)
            _nomencours = nomfichier
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Retourne en chaine le contenu d'une variable du .INI actuellement ouvert
    ''' </summary>
    ''' <param name="nomparam">Nom de la variable</param>
    ''' <returns>Contenu chaine de la variable ou chaine vide si non trouvée</returns>
    ''' <remarks></remarks>
    Public Function FichierIniLireChaine(ByVal nomparam As String) As String
        If (_nomencours = "") Then Return ""
        _FichierConfig.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        Dim ligne As String
        Dim boucle As Boolean

        boucle = True
        While (boucle)
            ligne = _FichierConfig.ReadLine()
            boucle = (Not (ligne Is Nothing))
            ligne = Trim(ligne)
            If (ligne.ToLower.StartsWith(nomparam.ToLower & "=")) Then
                Return ligne.Substring(ligne.IndexOf("=") + 1)
            End If
        End While
        Return ""
    End Function

    ''' <summary>
    ''' Referme le fichier actuellement ouvert
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FichierIniFermer()
        Try
            _FichierConfig.Close()
            _EntreeFichier.Close()
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Lire la valeur booléenne d'une variable du fichier ini actuellement ouvert
    ''' </summary>
    ''' <param name="nomparam">Nom de la variable</param>
    ''' <returns>La valeur booléenne de la variable, ou False si la variable n'existe pas dans ce fichier</returns>
    ''' <remarks></remarks>
    Public Function FichierIniLireBooleen(ByVal nomparam As String) As Boolean
        Dim retour As String
        If (_nomencours = "") Then Return False
        retour = FichierIniLireChaine(nomparam)
        If (retour Is Nothing) Then Return False
        If ((retour.ToLower = "vrai") Or (retour.ToLower = "true") Or (retour.ToLower = "oui")) Then Return True Else Return False
    End Function

    ''' <summary>
    ''' Lire la valeur entière d'une variable du fichier ini actuellement ouvert
    ''' </summary>
    ''' <param name="nomparam">Nom de la variable</param>
    ''' <returns>La valeur entière de la variable, ou zéro si la variable n'existe pas dans ce fichier ou erreur pendant sa convertion en entier</returns>
    ''' <remarks></remarks>
    Public Function FichierIniLireEntier(ByVal nomparam As String) As Integer
        Dim Retour As String
        If (_nomencours = "") Then Return 0
        Retour = FichierIniLireChaine(nomparam)
        Try
            Return CInt(Retour)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Lire la valeur décimale(double) d'une variable du fichier ini actuellement ouvert
    ''' </summary>
    ''' <param name="nomparam">Nom de la variable</param>
    ''' <returns>La valeur décimale de la variable, ou zéro si la variable n'existe pas dans ce fichier ou erreur pendant sa convertion en double</returns>
    ''' <remarks></remarks>
    Public Function FichierIniLireDouble(ByVal nomparam As String) As Double
        Dim Retour As String
        If (_nomencours = "") Then Return 0
        Retour = FichierIniLireChaine(nomparam)
        Try
            Return CDbl(Retour.Replace(",", "."))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Teste si une variable est présente dans le fichier actuellement ouvert
    ''' </summary>
    ''' <param name="nomparam">Nom de la variable à chercher</param>
    ''' <returns>True si la variable existe, sinon False</returns>
    ''' <remarks></remarks>
    Public Function FichierIniParamExist(ByVal nomparam As String) As Boolean
        If (_nomencours = "") Then Return False
        _FichierConfig.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        Dim ligne As String
        Dim boucle As Boolean

        boucle = True
        While (boucle)
            ligne = _FichierConfig.ReadLine()
            boucle = (Not (ligne Is Nothing))
            ligne = Trim(ligne).ToLower
            If (ligne.StartsWith(nomparam.ToLower & "=")) Then
                Return True
            End If
        End While
        Return False
    End Function

    ''' <summary>
    ''' Retourne le nom/emplacement du fichier ini actuellement ouvert par le système
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FichierIniRetourneNomFichier() As String
        Return _nomencours
    End Function

    Private Function FichierIniLireDebut(ByVal nomparam As String) As String
        If (_nomencours = "") Then Return ""
        _FichierConfig.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        Dim ligne As String
        Dim boucle As Boolean

        boucle = True
        While (boucle)
            ligne = _FichierConfig.ReadLine()
            boucle = (Not (ligne Is Nothing))
            ligne = Trim(ligne)
            If (ligne.ToLower.StartsWith(nomparam.ToLower)) Then
                Return ligne
            End If
        End While
        Return ""
    End Function

    ''' <summary>
    ''' Lire le contenu d'une variable dans une section particulière du fichier INI
    ''' Exemple dans le .INI :
    ''' [Section]
    ''' var1=test
    ''' var2=test2
    ''' </summary>
    ''' <param name="nomsection">Nom de la section</param>
    ''' <param name="cle">Nom de la variable</param>
    ''' <returns>Le contenu, en chaine, de la variable</returns>
    ''' <remarks></remarks>
    Public Function FichierIniLireSection(ByVal nomsection As String, ByVal cle As String) As String
        Dim retour As String = ""
        ' on regarde si ce n'est pas la derniere section que l'on viens de charger précédement
        ' gain de vitesse, on ne va pas la recharger, on l'a déjà
        If (_nomsection <> nomsection.ToLower) Then
            _section = Nothing
            _nomsection = ""
            Dim ligne, ligneencours As String
            Dim boucle As Boolean = True
            ligne = FichierIniLireDebut("[" + nomsection + "]")
            ' test si section inexistante
            If ((ligne Is Nothing) Or (ligne = "")) Then Return ""
            ligne = ""
            While (boucle)
                ligneencours = _FichierConfig.ReadLine
                ' on arrete de lire tous les parametres dès que l'on trouve une ligne qui commence par '[' qui signale donc le début d'une nouvelle section
                If (ligneencours Is Nothing) Then
                    boucle = False
                Else
                    If (ligneencours.StartsWith("[")) Then
                        boucle = False
                    Else
                        If (Not (ligneencours.StartsWith(COMMENTAIRES))) Then
                            If (ligne <> "") Then ligne &= Chr(255)
                            ligne &= ligneencours
                        End If
                    End If
                End If
            End While
            _section = ligne.Split(Chr(255))
            _nomsection = nomsection.ToLower
        End If
        Dim i As Integer
        If (_section IsNot Nothing) Then
            If (_section.Length > 0) Then
                For i = 0 To _section.Length - 1
                    If (Trim(_section(i)).ToLower.StartsWith(Trim(cle).ToLower & "=")) Then
                        retour = _section(i).Substring(_section(i).IndexOf("=") + 1)
                        Exit For
                    End If
                Next
            End If
        End If
        Return retour
    End Function

    ''' <summary>
    ''' Retourne le contenu de la section du .INI actuellement en précache
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function retourneSection() As String()
        Return _section
    End Function

    ''' <summary>
    ''' Retourne le nom de la section du .INI actuellement en précache, nom en lower case
    ''' </summary>
    ''' <returns>Le nom de la section, type String</returns>
    ''' <remarks></remarks>
    Public Function retourneNomSectionEnCours() As String
        Return _nomsection
    End Function

End Class
