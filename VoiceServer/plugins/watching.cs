using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceServer.plugins
{
    class watching
    {
        private List<string> fichiers;
        private static watching _instance;
        public const string PHRASE="regarder film";

        public static watching getInstance()
        {
            if (_instance == null) _instance = new watching();
            return _instance;
        }

        public void rafraichir()
        {
            string[] allLines;
            instances.ListeDesRepertoires.getInstance().clearListe();
            allLines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "\\watching.ini");
            foreach (string line in allLines)
                instances.ListeDesRepertoires.getInstance().addItem(line);
            if (fichiers == null) fichiers = new List<string>();
            fichiers.Clear();
            foreach (string rep in instances.ListeDesRepertoires.getInstance().retourneListe())
                if (System.IO.Directory.Exists(rep)) boucleSousRepertoire(rep); else System.Windows.Forms.MessageBox.Show("Le répertoire\r\n" + rep + "\r\nn'exite pas/plus.");
        }

        private void boucleSousRepertoire(string rep)
        {
            try
            {
                foreach (string fichier in System.IO.Directory.GetFiles(rep))
                {
                    fichiers.Add(fichier);
                }
                foreach (string eachRep in System.IO.Directory.GetDirectories(rep))
                    boucleSousRepertoire(eachRep);
            }
            catch { }
        }

        public void traitement(string phrase)
        {
            string[] mots;
            int nbMot;

            mots = phrase.Replace(PHRASE, "").Split(' ');
            foreach (string fichier in fichiers)
            {
                nbMot=0;
                foreach (string mot in mots)
                    if (System.IO.Path.GetFileNameWithoutExtension(fichier).Trim().ToLower().IndexOf(mot.ToLower()) >= 0) nbMot++;
                if (nbMot == mots.Length)
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = fichier;
                    proc.Start();
                    break;
                }
            }
        }
    }
}
