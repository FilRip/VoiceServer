using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceServer.instances
{
    public class ClassParam
    {
        private static string _nomMachine="";
        private static string _Reponse="";
        private static int _timeToWait;
        private static mainForm _mf;
        private static CB.Threading.ThreadPoolManager _tpm = new CB.Threading.ThreadPoolManager();
        private static bool _kinect;

        public static void log(string texte)
        {
            string dir;
            dir = System.Environment.CurrentDirectory + "\\logs";
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
            try
            {
                System.IO.File.AppendAllText(dir + "\\VoiceServer_" + System.DateTime.Now.Day.ToString("00") + "-" + System.DateTime.Now.Month.ToString("00") + "-" + System.DateTime.Now.Year.ToString("0000") /*+ "_" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString()*/ + ".txt", System.DateTime.Now.Hour.ToString("00") + ":" + System.DateTime.Now.Minute.ToString("00") + ":" + System.DateTime.Now.Second.ToString("00") + " " + texte + "\r\n");
            }
            catch { }
        }

        public static mainForm fenetrePrincipale
        {
            get { return _mf; }
            set { _mf = value; }
        }

        public static string nomMachine
        {
            get { return _nomMachine; }
            set { _nomMachine = value; }
        }

        public static int timeToWait
        {
            get { return _timeToWait; }
            set { _timeToWait = value; }
        }

        public static string reponse
        {
            get { return _Reponse; }
            set { _Reponse = value; }
        }

        public static CB.Threading.ThreadPoolManager tpm
        {
            get { return _tpm; }
            set { _tpm = value; }
        }

        public static bool kinect
        {
            get { return _kinect; }
            set { _kinect = value; }
        }

        public static void lireTempsAttente(string chaine)
        {
            int.TryParse(chaine, out _timeToWait);
        }
    }
}
