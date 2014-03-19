Imports LuaInterface

Public Class scriptLUA
    Inherits Lua

    Private _script As String = ""

    Private Const FONCTION_PRINCIPALE As String = "main"

    ''' <summary>
    ''' Instancie l'objet et charge + execute le script
    ''' </summary>
    ''' <param name="fichierScript">Nom/emplacement du fichier script a charger tout de suite</param>
    ''' <param name="executeFonctionMain">Execute ou non la fonction "main" du script LUA</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fichierScript As String, Optional ByVal executeFonctionMain As Boolean = True)
        MyBase.New()
        interpreteFichierScript(fichierScript, executeFonctionMain)
    End Sub

    ''' <summary>
    ''' Initialise/ouvre un fichier de script LUA
    ''' </summary>
    ''' <param name="source">Nom/emplacement du fichier script à ouvrir</param>
    ''' <param name="executeFonctionMain">Si True (par défaut), la fonction main() contenu dans le script est immédiatement exécutée</param>
    ''' <returns>True si le fichier a pu être ouvert, sinon False</returns>
    ''' <remarks></remarks>
    Public Function interpreteFichierScript(ByVal source As String, Optional ByVal executeFonctionMain As Boolean = True) As Boolean
        If ((source.StartsWith("\")) Or (source.StartsWith("."))) Then source = My.Application.Info.DirectoryPath() & source
        If (IO.File.Exists(source)) Then
            Try
                DoFile(source)
                _script = source
                If (executeFonctionMain) Then executeFonctionLUA(FONCTION_PRINCIPALE)
                Return True
            Catch ex As Exception
            End Try
        End If
        Return False
    End Function

    ''' <summary>
    ''' Interprete un script LUA contenu dans une variable chaine
    ''' </summary>
    ''' <param name="script">Contenu du script LUA</param>
    ''' <param name="executeFonctionMain">Si True (par défaut), la fonction main() contenu dans le script est immédiatement exécutée</param>
    ''' <returns>True si le script a pu être "parsé", sinon False</returns>
    ''' <remarks></remarks>
    Public Function interpreteChaineLUA(ByVal script As String, Optional ByVal executeFonctionMain As Boolean = True) As Boolean
        Try
            DoString(script)
            _script = script
            If (executeFonctionMain) Then executeFonctionLUA(FONCTION_PRINCIPALE)
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Retourne le contenu chaine d'une variable globale provenant du script LUA
    ''' </summary>
    ''' <param name="nomVariable">Nom de la variable dans LUA</param>
    ''' <returns>Le contenu chaine de la variable LUA ou chaine vide si la variable n'existe pas ou erreur</returns>
    ''' <remarks></remarks>
    Public Function lireChaine(ByVal nomVariable As String) As String
        If (_script = "") Then Return ""
        Try
            Return Me(nomVariable).ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Retourne le contenu entier d'une variable globale provenant du script LUA
    ''' </summary>
    ''' <param name="nomVariable">Nom de la variable dans LUA</param>
    ''' <returns>Retourne le contenu entier de la variable LUA ou zéro si la variable n'existe pas ou erreur à sa conversion en entier</returns>
    ''' <remarks></remarks>
    Public Function lireEntier(ByVal nomVariable As String) As Integer
        If (_script = "") Then Return 0
        Try
            Return CInt(lireChaine(nomVariable))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Retourne le contenu décimal(double) d'une variable globale provenant du script LUA
    ''' </summary>
    ''' <param name="nomVariable">Nom de la variable dans LUA</param>
    ''' <returns>Retourne le contenu double de la variable LUA ou zéro si la variable n'existe pas ou erreur à sa conversion en double</returns>
    ''' <remarks></remarks>
    Public Function lireDecimal(ByVal nomVariable As String) As Double
        If (_script = "") Then Return 0
        Try
            Return CDbl(lireChaine(nomVariable))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Retourne le contenu booléen d'une variable globale provenant du script LUA
    ''' </summary>
    ''' <param name="nomVariable">Nom de la variable dans LUA</param>
    ''' <returns>Retourne le contenu booléen de la variable LUA ou False si la variable n'existe pas ou erreur à sa conversion en booléen</returns>
    ''' <remarks></remarks>
    Public Function lireBooleen(ByVal nomVariable As String) As Boolean
        If (_script = "") Then Return False
        Try
            Return CBool(lireChaine(nomVariable))
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Permet de déclarer une fonction VB.NET comme utilisable dans un script LUA
    ''' </summary>
    ''' <param name="nom">Nom/Alias de la fonction dans le script LUA</param>
    ''' <param name="objet">Objet qui contient la fonction</param>
    ''' <param name="fonction">Pointeur sur la fonction proprement parlée</param>
    ''' <returns>True si la déclaration dans LUA a pu se faire, sinon False</returns>
    ''' <remarks></remarks>
    Public Function ajouteFonctionUtilisable(ByVal nom As String, ByVal objet As Object, ByVal fonction As System.Reflection.MethodInfo) As Boolean
        If (_script = "") Then Return False
        Try
            RegisterFunction(nom, objet, fonction)
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Exécute une fonction contenu dans le script LUA actuellement chargé
    ''' </summary>
    ''' <param name="nomFonction">Nom de la fonction LUA</param>
    ''' <param name="listeParametres">Liste des paramètres requis par la fonction, dans un tableau d'objet</param>
    ''' <returns>Ce que retourne la fonction LUA, ou Nothing</returns>
    ''' <remarks></remarks>
    Public Function executeFonctionLUA(ByVal nomFonction As String, Optional ByVal listeParametres As Object = Nothing) As Object
        If (_script = "") Then Return Nothing
        Try
            Return GetFunction(nomFonction).Call(listeParametres)
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

End Class
