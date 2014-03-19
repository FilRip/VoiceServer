Public Class nombreEnLettre

    Private mot(25) As String
    Private Resultat As String

    Private Sub Ajoute(ByVal MotSimple As String)
        '--- ajoute un nouveau terme traduit à la chaine résultat
        If (Resultat <> "") Then
            '--- vérifie s'il est nécessaire de coller le nouveau terme au
            '--- précédent dans le cas des "S" à rajouter, ou des tirets
            If ((Resultat.EndsWith("-")) OrElse (MotSimple = "s") OrElse (MotSimple = "-")) Then
                Resultat &= MotSimple
                '--- sinon, ajoute le terme après un espace
            Else
                Resultat &= " " + MotSimple
            End If
        Else
            Resultat = MotSimple
        End If
    End Sub

    Private Function Equivalent(ByVal Valeur As Double) As String
        '--- recherche le mot équivalent à une valeur numérique
        Select Case Valeur
            Case Is < 21
                Return mot(Valeur)
            Case Else
                Return mot(18 + (Valeur / 10))
        End Select
    End Function

    Public Function Nb2Mot(ByVal Valeur As Double, Optional ByVal avecMonnaie As Boolean = False) As String
        Dim a, k As String
        Dim Virgule As Boolean
        Dim N As String
        '--- initialisation du tableau contenant les mots interprétés
        '--- récupération de paramètre passé
        a = Valeur.ToString() + " "
        '--- initialisation des variables de travail
        N = ""
        Virgule = False
        Resultat = ""
        '--- pour toute la longueur de celui-ci
        For B As Integer = 1 To a.Length
            '--- on extrait chacun de ses caractères
            k = a.Substring(B - 1, 1)
            Select Case k
                '--- gère les montants négatifs
                Case "-"
                    Ajoute("moins")
                    '--- si ceux-ci sont numériques, on batit la chaine n
                Case "0" To "9"
                    N = N + k
                    '--- sinon, on teste si on est arrivé à une virgule
                Case Else
                    If Virgule Then
                        '--- les centimes sont comptés sur 2 digits, réajustés de
                        '--- manière inverse aux euros, puisqu'on lit les unités
                        '--- et dizaines de manière inversée (0,2? = 20c et
                        '--- 0,02?=2c)
                        N = Right("000" + Left(N + "000", 2), 2)
                        If Val(N) = 0 Then N = ""
                    End If
                    '--- on traduit le nombre stocké dans n
                    If (N.StartsWith("0")) Then Ajoute("zéro")
                    TraduireEntier(N)
                    If (avecMonnaie) Then
                        If Virgule = 0 And Val(N) > 0 Then
                            Ajoute("euro")
                            '--- et on accorde l'unité avec le nombre
                            If Val(N) > 1 Then Ajoute("s")
                        ElseIf Virgule = 1 And Val(N) > 0 Then
                            Ajoute("centime")
                            '--- en ajoutant un "s" si nécessaire
                            If Val(N) > 1 Then Ajoute("s")
                        End If
                    End If
                    N = ""
                    Select Case k
                        Case Chr(13), Chr(10)
                            Continue For
                        Case Is < " "
                        Case ",", "."
                            Virgule = True
                            '--- si une valeur en euros est exprimée, et que le
                            '--- nombre de centimes est suffisant pour être traité,
                            '--- on lie les 2 par le mot "et"
                            If ((Val(a) <> 0) And (Val("0." + Mid(a, B + 1)) >= 0.01)) Then Ajoute("virgule")
                        Case Else
                    End Select
            End Select
        Next
        Return Resultat
    End Function

    Private Sub TraduireEntier(ByVal NombreATraduire As String)
        '--- convertit un nombre entier contenu dans une chaine de caractères
        '--- en son équivalent ordinal
        Dim nombre, cdu, c, d, u As String
        Dim et, tiret As Boolean
        nombre = NombreATraduire
        If nombre <> "" Then
            '--- si le nombre est 0, on ne perd pas de temps
            If Val(nombre) = 0 Then
                Ajoute("zéro")
            Else
                '--- sinon, on convertit celui-ci en une chaine de caractères
                '--- de longueur multiple de 3, afin de pouvoir la lire par blocs
                '--- de 3 caractères
                nombre = Right("000", -((nombre.Length Mod 3) <> 0) * (3 - (nombre.Length Mod 3))) + nombre
                For longueur As Integer = nombre.Length To 3 Step -3
                    cdu = Left(nombre, 3)
                    nombre = Right(nombre, longueur - 3)
                    '--- on extrait ainsi des ensembles de 3 chiffres, de la
                    '--- gauche vers la droite
                    If cdu <> "000" Then
                        '--- dont on tire une valeur de centaines, dizaines et
                        '--- unités
                        c = Left(cdu, 1)
                        d = Mid(cdu, 2, 1)
                        u = Right(cdu, 1)
                        '--- on convertit les unités non muettes pour les
                        '--- centaines
                        If c >= "2" Then Ajoute(Equivalent(Val(c)))
                        '--- et on traite les 1 muets
                        If c >= "1" Then
                            Ajoute("cent")
                            '--- en appliquant les règles d'accords pour les
                            '--- centaines
                            If Val(nombre) = 0 AndAlso d + u = "00" AndAlso Len(Resultat) > 4 Then Ajoute("s")
                        End If
                        '--- on analyse si le mot ET est nécessaire (21, 31,
                        '--- 41 ...)
                        et = (d >= "2") AndAlso (u = "1")
                        '--- ainsi que les tirets pour certains couples
                        '--- dizaines-unités
                        tiret = ((d >= "2") AndAlso (u > "1") _
                        Or (d >= "1" AndAlso u >= "7")) AndAlso Not et
                        '--- traitement des valeurs 80-99
                        If d >= "8" Then
                            Ajoute("quatre-vingt")
                            et = False
                            '--- retenue nécessaire pour 90 à 99
                            If d = "8" Then d = "0" _
                            Else d = "1" : tiret = True
                            '--- et traitement des unités
                            If u > "0" Then tiret = True Else Ajoute("s")
                            '--- sinon on traite les valeurs 70 à 79
                        ElseIf d = "7" Then
                            Ajoute("soixante")
                            '--- avec une retenue pour les dizaines
                            d = "1"
                            If u <> "1" Then tiret = True
                        End If
                        '--- valeurs entre 10 et 16
                        If (d = "1") AndAlso (u <= "6") Then
                            d = "0"
                            u = "1" + u
                        End If
                        '--- sinon, on gère toutes les autres dizaines
                        If d >= "1" Then
                            '--- gère les tirets pour les dizaines composées
                            If tiret AndAlso d = "1" _
                            And Val(Right(cdu, 2)) > 19 Then
                                Ajoute("-")
                            End If
                            '--- traduction de la dizaine...
                            Ajoute(Equivalent(Val(d + "0")))
                            '--- en accordant l'exception des vingtaines
                            If d + u = "20" AndAlso c <> "0" Then Ajoute("s")
                        End If
                        '--- si le mot Et est nécessaire, on l'ajoute
                        If et Then Ajoute("et")
                        '--- ainsi que le tiret, liant une dizaine et une
                        '--- unité
                        If tiret Then Ajoute("-")
                        '--- puis on traduit l'unité du nombre
                        If Val(u) >= 22 OrElse ((Val(u) >= 1 AndAlso (Val(cdu) > 1 OrElse longueur <> 6))) Then
                            Ajoute(Equivalent(Val(u)))
                        End If
                        '--- enfin, la pondération du nombre est respectée,
                        '--- en ajoutant le multiple nécessaire, et en
                        '--- l'accordant s'il le faut
                        Select Case longueur
                            Case 6 : Ajoute("mille")
                            Case 9 : Ajoute("million")
                                If Val(cdu) > 1 Then Ajoute("s")
                            Case 12
                                Ajoute("milliard")
                                If Val(cdu) > 1 Then Ajoute("s")
                        End Select
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub New()
        mot(1) = "un"
        mot(2) = "deux"
        mot(3) = "trois"
        mot(4) = "quatre"
        mot(5) = "cinq"
        mot(6) = "six"
        mot(7) = "sept"
        mot(8) = "huit"
        mot(9) = "neuf"
        mot(10) = "dix"
        mot(11) = "onze"
        mot(12) = "douze"
        mot(13) = "treize"
        mot(14) = "quatorze"
        mot(15) = "quinze"
        mot(16) = "seize"
        mot(20) = "vingt"
        mot(21) = "trente"
        mot(22) = "quarante"
        mot(23) = "cinquante"
        mot(24) = "soixante"
    End Sub

End Class
