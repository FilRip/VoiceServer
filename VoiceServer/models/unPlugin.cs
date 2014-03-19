using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VoiceServer.models
{
    public class unPlugin
    {
        private string _nom;
        private string _fullPath;
        private List<string> _listePhrase;
        private string _reqHttp = "";
        private string _mainClass;
        private string _mainMethod;
        private string _scriptFile = "";
        private List<string> _listeReferences;
        private string _touche ="";
        private string _speak = "";
        private string _execute="";

        public List<string> listeReferences
        {
            get { return _listeReferences; }
            set { _listeReferences = value; }
        }

        public string executeCmd
        {
            get { return _execute; }
            set { _execute = value; }
        }

        public string reqHttp
        {
            get { return _reqHttp;}
            set { _reqHttp = value; }
        }

        public string scriptFile
        {
            get { return _scriptFile; }
            set { _scriptFile = value; }
        }

        public string mainClass
        {
            get { return _mainClass; }
            set { _mainClass = value; }
        }

        public string mainMethod
        {
            get { return _mainMethod; }
            set { _mainMethod = value; }
        }

        public string nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string fullPath
        {
            get { return _fullPath; }
            set { _fullPath = value; }
        }

        public List<string> listePhrase
        {
            get { return _listePhrase; }
            set { _listePhrase = value; }
        }

        public string touche
        {
            get { return _touche; }
            set { _touche = value; }
        }

        public string speak
        {
            get { return _speak; }
            set { _speak = value; }
        }

        public bool charge(string fichier)
        {
            string[] allLines;

            try
            {
                _fullPath = System.IO.Path.GetDirectoryName(fichier);
                allLines = System.IO.File.ReadAllLines(fichier);
                if (allLines != null)
                    foreach (string ligne in allLines)
                    {
                        if (ligne.ToLower().Trim().StartsWith("name="))
                            _nom = ligne.ToLower().Trim().Substring(5);
                        if (ligne.ToLower().Trim().StartsWith("phrase="))
                        {
                            string[] ordre;
                            ordre = ligne.ToLower().Substring(7).Split(',');
                            _listePhrase = new List<string>();
                            foreach (string phrase in ordre)
                                _listePhrase.Add(phrase.ToLower());
                        }
                        else if (ligne.ToLower().Trim().StartsWith("requetehttp="))
                            _reqHttp = ligne.Trim().Substring(12);
                        else if (ligne.ToLower().Trim().StartsWith("mainclass="))
                            _mainClass = ligne.Trim().Substring(10);
                        else if (ligne.ToLower().Trim().StartsWith("mainmethod="))
                            _mainMethod = ligne.Trim().Substring(11);
                        else if (ligne.ToLower().Trim().StartsWith("scriptfile="))
                            _scriptFile = ligne.ToLower().Trim().Substring(11);
                        else if (ligne.ToLower().Trim().StartsWith("referenceassembly="))
                        {
                            string[] ordre;
                            ordre = ligne.Trim().ToLower().Substring(18).Split(',');
                            _listeReferences = new List<string>();
                            foreach (string phrase in ordre)
                                _listeReferences.Add(phrase.Trim().ToLower());
                        }
                        else if (ligne.ToLower().Trim().StartsWith("keypress="))
                            _touche = ligne.Trim().Substring(9).Replace("<enter>", "\r\n");
                        else if (ligne.ToLower().Trim().StartsWith("speak="))
                            _speak = ligne.ToLower().Trim().Substring(6);
                        else if (ligne.ToLower().Trim().StartsWith("execute="))
                            _execute = ligne.ToLower().Trim().Substring(8);
                    }
                if ((_listePhrase == null) || (_listePhrase.Count == 0))
                    instances.ClassParam.log("Plugins " + _nom + " sans phrase");
                return true;
            }
            catch (Exception e)
            {
                instances.ClassParam.log("Erreur pendant chargement d'un plugins\r\n" + _nom + " " + e.Message + "\r\n" + e.StackTrace);
            }
            return false;
        }

        public bool execute(string phrase)
        {
            try
            {
                if (_speak != "") instances.SpeechSystem.getInstance().textToSpeech.Speak(_speak);

                if (_execute != "")
                {
                    string param = "";
                    List<string> _listeParam;
                    _listeParam = CB.GestString.traitement.returnParams(_execute, ' ');
                    if (_listeParam.Count>1)
                        for (int i = 1; i <= _listeParam.Count - 1;i++)
                        {
                            param += _listeParam[i];
                        }
                    System.Diagnostics.Process.Start(_listeParam[0], param);
                }

                if ((_touche == "") && (_reqHttp == "") && (_scriptFile == "")) return false;

                if (_touche != "")
                {
                    instances.ClassParam.log("Envoi le(s) touche(s) : " + _touche);
                    System.Windows.Forms.SendKeys.SendWait(_touche);
                }

                if (_reqHttp!="")
                    if (!CB.Reseaux.GestionHTTP.envoiContenu(_reqHttp))
                    {
                        instances.ClassParam.log("Erreur pendant l'envoi de la requête http");
                        return false;
                    }

                if (_scriptFile == "") return true;

                System.CodeDom.Compiler.CompilerResults cr;
                List<string> listeReference = new List<string>();
                System.Reflection.MethodInfo method;

                listeReference.Add("CB.Reseaux.dll");
                listeReference.Add("CB.Threading.dll");
                listeReference.Add("Microsoft.Speech.dll");
                listeReference.Add("Microsoft.Kinect.dll");

                if ((_listeReferences != null) && (_listeReferences.Count > 0))
                {
                    foreach (string refAssembly in _listeReferences)
                    listeReference.Add(refAssembly);
                }

                if (System.IO.Path.GetExtension(_scriptFile) == ".vb")
                {
                    CB.Interpreters.scriptVBNET a = new CB.Interpreters.scriptVBNET();
                    cr = a.interpreteFichier(_fullPath + "\\" + _scriptFile, listeReference);
                }
                else
                {
                    CB.Interpreters.scriptCsharpNET a = new CB.Interpreters.scriptCsharpNET();
                    cr = a.interpreteFichier(_fullPath + "\\" + _scriptFile, listeReference);
                }

                if (cr.Errors.Count > 0)
                {
                    try
                    {
                        System.IO.File.Delete(_fullPath + "\\" + _scriptFile + ".log");
                    }
                    catch { }
                    foreach (System.CodeDom.Compiler.CompilerError Err in cr.Errors)
                    {
                        System.IO.File.AppendAllText(_fullPath + "\\" + _scriptFile + ".log", "Error number : " + Err.ErrorNumber + "\r\n");
                        System.IO.File.AppendAllText(_fullPath + "\\" + _scriptFile + ".log", "Error text : " + Err.ErrorText + "\r\n");
                        System.IO.File.AppendAllText(_fullPath + "\\" + _scriptFile + ".log", "Line number : " + Err.Line.ToString() + "\r\n");
                        System.IO.File.AppendAllText(_fullPath + "\\" + _scriptFile + ".log", "\r\n");
                        return false;
                    }
                }
                else
                {
                    if (cr != null)
                    {
                        object obj;
                        obj = cr.CompiledAssembly.CreateInstance(_mainClass);
                        method = obj.GetType().GetMethod(_mainMethod);
                        method.Invoke(obj, new object[] { phrase });
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                instances.ClassParam.log("Erreur pendant l'exécution d'un script.\r\n" + _nom + " " + e.Message + "\r\n" + e.StackTrace + "\r\n");
            }
            return false;
        }

        public bool contientPhrase(string phrase)
        {
            if (_listePhrase != null)
                foreach (string p in _listePhrase)
                    if (p.Trim().ToLower() == phrase.Trim().ToLower())
                        return true;
            return false;
        }
    }
}
