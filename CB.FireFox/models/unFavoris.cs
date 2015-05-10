using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.FireFox.models
{
    public class unFavoris
    {
        private int _index;
        private string _texte, _lien;

        public int index
        {
            get { return _index; }
            set { _index = value; }
        }

        public string texte
        {
            get { return _texte; }
            set { _texte = value; }
        }

        public string lien
        {
            get { return _lien; }
            set { _lien = value; }
        }

        public void charge(string field)
        {
                if (field.Trim().ToLower().IndexOf("\"value\"") >= 0)
                {
                    _texte = field.Replace("\"value\":\"", "").Substring(0, field.Replace("\"value\":\"", "").IndexOf("\""));
                }
                else if (field.Trim().ToLower().IndexOf("\"uri\"") >= 0)
                {
                    _lien = field.Replace("\"uri\":\"", "").Substring(0, field.Replace("\"uri\":\"", "").IndexOf("\""));
                }

        }
    }
}
