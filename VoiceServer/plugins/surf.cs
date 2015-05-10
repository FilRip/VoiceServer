using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceServer.plugins
{
    class surf
    {
        public const string PHRASE = "site internet";
        private static surf _instance;
        private string _profile;

        public static surf getInstance()
        {
            if (_instance == null) _instance = new surf();
            return _instance;
        }

        public void rafraichir()
        {
            string[] allLines;
            allLines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "\\surf.ini");
            foreach (string line in allLines)
                if (line.Trim().ToLower().StartsWith("profile=")) _profile = line.Trim().ToLower().Replace("profile=", "");
        }

        public void traitement(string phrase)
        {
            string[] mots;
            int nbMot;

            if (phrase == "") return;
            mots = phrase.Replace(PHRASE, "").Split(' ');
            foreach (CB.FireFox.models.unFavoris favori in CB.FireFox.datatables.ListeDesFavoris.getInstance().retourneListe(_profile))
            {
                nbMot = 0;
                foreach (string mot in mots)
                    if (favori.texte.Trim().ToLower().IndexOf(mot.ToLower()) >= 0) nbMot++;
                if (nbMot == mots.Length)
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = favori.lien;
                    proc.Start();
                    break;
                }
            }
        }
    }
}
