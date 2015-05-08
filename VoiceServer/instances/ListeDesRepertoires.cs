using System;
using System.Collections.Generic;
using System.Text;

namespace MyFolder.instances
{
    class ListeDesRepertoires
    {
        private static ListeDesRepertoires _instance;
        private List<string> _liste;

        public static ListeDesRepertoires getInstance()
        {
            if (_instance == null) _instance = new ListeDesRepertoires();
            return _instance;
        }

        public List<string> retourneListe()
        {
            return _liste;
        }

        public void clearListe()
        {
            _liste = new List<string>();
        }

        public void addItem(string item)
        {
            _liste.Add(item);
        }
    }
}
