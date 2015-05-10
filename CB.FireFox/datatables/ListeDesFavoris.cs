using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.FireFox.datatables
{
    public class ListeDesFavoris
    {
        private static ListeDesFavoris _instance;
        private List<models.unFavoris> _favoris;

        public static ListeDesFavoris getInstance()
        {
            if (_instance == null) _instance = new ListeDesFavoris();
            return _instance;
        }

        public List<models.unFavoris> retourneListe(string profile)
        {
            if (_favoris == null)
            {
                string[] morceau;
                char[] sep = new char[3];
                sep[0] = '}';
                sep[1] = ',';
                sep[2] = '{';

                _favoris = new List<models.unFavoris>();
                models.unProfil profil;
                models.unFavoris favori;
                string[] allLines;
                string html="";

                profil = ListeDesProfils.getInstance().retourneProfil(profile);
                if (profile == null) return null;
                foreach (string fichier in System.IO.Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\" + profil.path + "\\bookmarkbackups\\"))
                {
                    html = "";
                    allLines = System.IO.File.ReadAllLines(fichier);
                    foreach (string ligne in allLines)
                        html = html + ligne;
                    if (html.Trim() != "")
                    {
                        morceau = html.Split(sep);
                        foreach (string fields in morceau)
                        {
                            favori = new models.unFavoris();
                            favori.charge(fields);
                            if ((favori.lien != null) && (favori.lien != "") && (!alreadyPresent(favori.lien)) && (favori.texte!=null) && (favori.texte!="")) _favoris.Add(favori);
                        }
                    }
                }
            }
            return _favoris;
        }

        public bool alreadyPresent(string link)
        {
            if (_favoris == null) return false;
            foreach (models.unFavoris f in _favoris)
                if (f.lien == link) return true;
            return false;
        }
    }
}
