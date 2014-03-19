using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceServer.instances
{
    public class ListOfPlugins
    {
        private List<models.unPlugin> _listPlugins;
        private static ListOfPlugins _instance;

        public static ListOfPlugins getInstance()
        {
            if (_instance == null)
            {
                _instance = new ListOfPlugins();
                _instance._listPlugins = new List<models.unPlugin>();
            }
            return _instance;
        }

        public int nbPlugins()
        {
            if (_listPlugins == null) return 0;
            return _listPlugins.Count;
        }

        public models.unPlugin returnPluginNom(string nom)
        {
            if (_listPlugins != null)
                foreach (models.unPlugin p in _listPlugins)
                    if (p.nom.Trim().ToLower() == nom.Trim().ToLower()) return p;
            return null;
        }

        public models.unPlugin returnPluginPhrase(string phrase)
        {
            if (_listPlugins != null)
                foreach (models.unPlugin p in _listPlugins)
                    if (p.contientPhrase(phrase.Trim().ToLower())) return p;
            return null;
        }

        public bool addPlugin(models.unPlugin p)
        {
            if (_listPlugins != null)
                foreach (models.unPlugin pp in _listPlugins)
                    if (pp.nom.Trim().ToLower() == p.nom.Trim().ToLower()) return false;
            _listPlugins.Add(p);
            return true;
        }

        public void clearAll()
        {
            _listPlugins.Clear();
        }
    }
}
