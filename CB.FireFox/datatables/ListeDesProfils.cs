using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.FireFox.datatables
{
    public class ListeDesProfils
    {
        private static ListeDesProfils _instance;
        private List<models.unProfil> _profiles;

        public static ListeDesProfils getInstance()
        {
            if (_instance == null) _instance = new ListeDesProfils();
            return _instance;
        }

        public void chargeProfils()
        {
            try
            {
                int num = -1;
                bool repeat = true;
                models.unProfil p;

                CB.FichiersINI.gestionFichierConfig fichierini = new FichiersINI.gestionFichierConfig();
                fichierini.FichierIniOuvrir(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\profiles.ini");

                while (repeat)
                {
                    num++;
                    if (fichierini.FichierIniLireSection("Profile" + num.ToString(), "name") != "")
                    {
                        p = new models.unProfil();
                        p.nom = fichierini.FichierIniLireSection("Profile" + num.ToString(), "name");
                        p.path = fichierini.FichierIniLireSection("Profile" + num.ToString(), "path");
                        if (_profiles == null) _profiles = new List<models.unProfil>();
                        _profiles.Add(p);
                    }
                    else
                        repeat = false;
                }
            }
            catch { }
        }

        public models.unProfil retourneProfil(string name)
        {
            if ((_profiles == null) || (_profiles.Count==0)) chargeProfils();
            if (_profiles != null)
            {
                foreach (models.unProfil p in _profiles)
                    if (p.nom == name) return p;
            }
            return null;
        }
    }
}
