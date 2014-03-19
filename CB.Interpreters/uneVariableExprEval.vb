''' <summary>
''' Objet représentant une variable dynamique à ajouter à ExprEval
''' </summary>
''' <remarks></remarks>
Public Class uneVariableExprEval
    Private _nom As String
    Private _valeur As Double
    ''' <summary>
    ''' Créer une instance d'une nouvelle variable
    ''' </summary>
    ''' <param name="nom">Nom de la variable (à utiliser dans l'evaluateur d'expression) ne doit contenir QUE des lettres</param>
    ''' <param name="valeur">Valeur de la variable</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal nom As String, ByVal valeur As Double)
        MyBase.New()
        _nom = nom
        _valeur = valeur
    End Sub
    Public Property nom() As String
        Get
            Return _nom
        End Get
        Set(ByVal value As String)
            _nom = value
        End Set
    End Property
    Public Property valeur() As Double
        Get
            Return _valeur
        End Get
        Set(ByVal value As Double)
            _valeur = value
        End Set
    End Property

End Class
