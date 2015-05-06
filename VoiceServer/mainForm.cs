using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using System.IO;
using System.Net.Sockets;

namespace VoiceServer
{
    public partial class mainForm : Form
    {
        public KinectSensor sensor;
        private bool _jecoute;
        private int _numThreadListening;
        private bool _quitter;

        public mainForm()
        {
            InitializeComponent();
            instances.ClassParam.fenetrePrincipale = this;
        }

        public void init()
        {
            instances.ClassParam.timeToWait = 5000;
            instances.SpeechSystem.getInstance().vitesseSyntheseVocale = 1;
            instances.SpeechSystem.getInstance().volumeSyntheseVocale = 100;
            try
            {
                foreach (var potentialSensor in KinectSensor.KinectSensors)
                {
                    if (potentialSensor.Status == KinectStatus.Connected)
                    {
                        sensor = potentialSensor;
                        break;
                    }
                    else
                        throw new Exception("Etat du Kinect non pret : " + potentialSensor.Status.ToString());
                }

                if (sensor != null)
                {
                    try
                    {
                        // Start the sensor!
                        sensor.Start();
                    }
                    catch (IOException)
                    {
                        invokeAddLog("Some others applications is streaming from the same Kinect sensor");
                        sensor = null;
                    }
                }
            }
            catch (Exception ex)
            {
                invokeAddLog("Pas de Kinect?");
                invokeAddLog(ex.Message);
            }

            if (sensor == null)
            {
                if (KinectSensor.KinectSensors.Count==0)
                    invokeAddLog("Aucun Kinect branché");
                invokeAddLog("Pas de Kinect");
                invokeAddLog("On passe en mode micro");
                instances.SpeechSystem.getInstance().speechMicEngine = new System.Speech.Recognition.SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"));
                try
                {
                    instances.SpeechSystem.getInstance().speechMicEngine.SetInputToDefaultAudioDevice();
                }
                catch (Exception e)
                {
                    invokeAddLog("Pas de micro par défaut non plus...");
                    invokeAddLog(e.Message);
                }
            }
            else
            {
                instances.ClassParam.kinect = true;
                invokeAddLog("Kinect activé");
                RecognizerInfo ri = GetKinectRecognizer();

                if (ri != null)
                {
                    instances.SpeechSystem.getInstance().speechEngine = new SpeechRecognitionEngine(ri.Id);
                    instances.SpeechSystem.getInstance().speechEngine.SetInputToAudioStream(sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                }
                else
                {
                    invokeAddLog("Pas de système de reconnaissance vocale");
                }
            }

            if (!enableRecognition()) return;
            invokeAddLog("Charge les plugins");
            loadPlugins();

            if (sensor != null)
                instances.SpeechSystem.getInstance().speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            else
                instances.SpeechSystem.getInstance().speechMicEngine.RecognizeAsync(System.Speech.Recognition.RecognizeMode.Multiple);

            instances.SpeechSystem.getInstance().textToSpeech.Rate = instances.SpeechSystem.getInstance().vitesseSyntheseVocale;
            instances.SpeechSystem.getInstance().textToSpeech.Volume = instances.SpeechSystem.getInstance().volumeSyntheseVocale;
            instances.SpeechSystem.getInstance().textToSpeech.Speak("Bonjour");

            invokeAddLog("Pret");

            //initServeur();
        }

        public void initServeur()
        {
            models.ServeurTCP s = new models.ServeurTCP();
            s.initServeur();
        }

        public void setNPlugins(int nbPlugins)
        {
            txtNbPlugins.Text = nbPlugins.ToString();
        }

        public bool enableRecognition()
        {
            if ((sensor == null) && (instances.SpeechSystem.getInstance().speechMicEngine != null))
            {
                instances.SpeechSystem.getInstance().speechMicEngine.SpeechRecognized += speechMicEngine_SpeechRecognized;
                instances.SpeechSystem.getInstance().speechMicEngine.SpeechRecognitionRejected += speechMicEngine_SpeechRecognitionRejected;
            }
            else if (instances.SpeechSystem.getInstance().speechEngine != null)
            {
                instances.SpeechSystem.getInstance().speechEngine.SpeechRecognized += SpeechRecognized;
                instances.SpeechSystem.getInstance().speechEngine.SpeechRecognitionRejected += SpeechRecognitionRejected;
            }
            else
                return false;
            return true;
        }

        private void speechMicEngine_SpeechRecognitionRejected(object sender, System.Speech.Recognition.SpeechRecognitionRejectedEventArgs e)
        {
            invokeAddLog("Texte non reconnu : " + e.Result.Text);
        }

        public void disableRecognition()
        {
            if (sensor == null)
                instances.SpeechSystem.getInstance().speechMicEngine.SpeechRecognized -= speechMicEngine_SpeechRecognized;
            else
                instances.SpeechSystem.getInstance().speechEngine.SpeechRecognized -= SpeechRecognized;
        }

        private void speechMicEngine_SpeechRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            invokeAddLog("J'ai entendu : " + e.Result.Text);
            const double ConfidenceThreshold = 0.3;
            if (e.Result.Confidence >= ConfidenceThreshold)
                traiteEcoute(e.Result.Text);
        }

        private void SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            invokeAddLog("Texte non reconnu : " + e.Result.Text);
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            invokeAddLog("J'ai entendu : " + e.Result.Text);
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;
            if (e.Result.Confidence >= ConfidenceThreshold)
                traiteEcoute(e.Result.Text);
        }

        private void traiteEcoute(string textComplet)
        {
            if ((!_jecoute) && (textComplet.ToLower().Trim() == instances.ClassParam.nomMachine.Trim().ToLower()))
            {
                instances.SpeechSystem.getInstance().textToSpeech.SpeakAsync(instances.ClassParam.reponse);
                instances.ClassParam.log("J'écoute tes ordres");
                instances.ClassParam.tpm.stopJob(_numThreadListening);
                _numThreadListening = instances.ClassParam.tpm.addJob(_coupe_Tick, null, instances.ClassParam.timeToWait);
                _jecoute = true;
            }
            else if (_jecoute)
            {
                models.unPlugin p;
                p = instances.ListOfPlugins.getInstance().returnPluginPhrase(textComplet);
                if (p != null)
                {
                    instances.ClassParam.tpm.stopJob(_numThreadListening);
                    instances.ClassParam.tpm.addJob(_coupe_Tick, null, instances.ClassParam.timeToWait);
                    _jecoute = true;
                    if (!p.execute(textComplet.Trim().ToLower()))
                        invokeAddLog("Erreur pendant l'exécution du script");
                }
            }
        }

        private void _coupe_Tick(object param)
        {
            invokeAddLog("Je n'écoute plus tes ordres");
            _jecoute = false;
        }

        private void loadPlugins()
        {
            models.unPlugin p;
            string[] allLines;

            try
            {
                instances.ListOfPlugins.getInstance().clearAll();
                foreach (string rep in System.IO.Directory.GetDirectories(System.Environment.CurrentDirectory + "\\plugins"))
                {
                    foreach (string fichier in System.IO.Directory.GetFiles(rep, "*.ini"))
                    {
                        p = new models.unPlugin();
                        if (!p.charge(fichier))
                            throw new Exception("Impossible de lire le ini du plugin");
                        if (!instances.ListOfPlugins.getInstance().addPlugin(p))
                            throw new Exception("Impossible d'ajouter le plugin " + p.nom + " à la liste");
                        if (p.listePhrase != null)
                            foreach (string ligne in p.listePhrase)
                            {
                                if (sensor == null)
                                {
                                    System.Speech.Recognition.GrammarBuilder gb = new System.Speech.Recognition.GrammarBuilder();
                                    gb.Append(new System.Speech.Recognition.SemanticResultKey("root", ligne));
                                    System.Speech.Recognition.Grammar g3 = new System.Speech.Recognition.Grammar(gb);
                                    instances.SpeechSystem.getInstance().speechMicEngine.LoadGrammar(g3);
                                }
                                else
                                {
                                    Microsoft.Speech.Recognition.GrammarBuilder gb = new Microsoft.Speech.Recognition.GrammarBuilder();
                                    gb.Append(new Microsoft.Speech.Recognition.SemanticResultKey("root", ligne));
                                    Microsoft.Speech.Recognition.Grammar g3 = new Microsoft.Speech.Recognition.Grammar(gb);
                                    instances.SpeechSystem.getInstance().speechEngine.LoadGrammar(g3);
                                }
                            }
                    }
                }

                // Add dictation engine
                if (sensor == null)
                {
                    System.Speech.Recognition.GrammarBuilder gb = new System.Speech.Recognition.GrammarBuilder();
                    gb.AppendDictation();
                    System.Speech.Recognition.Grammar g3 = new System.Speech.Recognition.Grammar(gb);
                    instances.SpeechSystem.getInstance().speechMicEngine.LoadGrammar(g3);
                }
                else
                {
                    Microsoft.Speech.Recognition.GrammarBuilder gb = new Microsoft.Speech.Recognition.GrammarBuilder();
                    gb.AppendDictation();
                    Microsoft.Speech.Recognition.Grammar g3 = new Microsoft.Speech.Recognition.Grammar(gb);
                    instances.SpeechSystem.getInstance().speechEngine.LoadGrammar(g3);
                }

                setNPlugins(instances.ListOfPlugins.getInstance().nbPlugins());
                allLines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "\\config.ini");
                if (allLines != null)
                    foreach (string ligne in allLines)
                    {
                        if (ligne.ToLower().StartsWith("name="))
                        {
                            instances.ClassParam.nomMachine = ligne.Substring(5);
                            if (sensor == null)
                            {
                                System.Speech.Recognition.GrammarBuilder gb = new System.Speech.Recognition.GrammarBuilder();
                                gb.Append(new System.Speech.Recognition.SemanticResultKey("root", instances.ClassParam.nomMachine));
                                System.Speech.Recognition.Grammar g3 = new System.Speech.Recognition.Grammar(gb);
                                instances.SpeechSystem.getInstance().speechMicEngine.LoadGrammar(g3);
                                //instances.SpeechSystem.getInstance().speechMicEngine.LoadGrammar(new System.Speech.Recognition.DictationGrammar());
                            }
                            else
                            {
                                Microsoft.Speech.Recognition.GrammarBuilder gb = new Microsoft.Speech.Recognition.GrammarBuilder();
                                gb.Append(new Microsoft.Speech.Recognition.SemanticResultKey("root", instances.ClassParam.nomMachine));
                                Microsoft.Speech.Recognition.Grammar g3 = new Microsoft.Speech.Recognition.Grammar(gb);
                                instances.SpeechSystem.getInstance().speechEngine.LoadGrammar(g3);
                            }
                        }
                        else if (ligne.ToLower().StartsWith("answer="))
                            instances.ClassParam.reponse = ligne.Substring(7);
                        else if (ligne.ToLower().StartsWith("timetolistening="))
                            instances.ClassParam.lireTempsAttente(ligne.Trim().Substring(16));
                        else if (ligne.ToLower().StartsWith("vitessesynthesevocale="))
                            instances.SpeechSystem.getInstance().vitesseSyntheseVocale = int.Parse(ligne.Substring(22));
                        else if (ligne.ToLower().StartsWith("volumesynthesevocale="))
                            instances.SpeechSystem.getInstance().volumeSyntheseVocale = int.Parse(ligne.Substring(21));
                    }
            }
            catch (Exception e)
            {
                invokeAddLog(e.Message + "\r\n" + e.StackTrace + "\r\n");
            }
        }

        private RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "fr-FR".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        public delegate void delegateAddLog(string texte);
        private void addLog(string texte)
        {
            rapport.AppendText(texte + "\r\n");
            instances.ClassParam.log(texte);
        }

        public void invokeAddLog(string texte)
        {
            if (this.InvokeRequired)
                this.Invoke(new delegateAddLog(addLog), texte);
            else
                addLog(texte);
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            sysTray.Visible = true;
            init();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            disableRecognition();
            invokeAddLog("Désactive reconnaissance vocale");
            instances.SpeechSystem.getInstance().textToSpeech.Speak("Désactive reconnaissance vocale");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (enableRecognition())
            {
                invokeAddLog("Active reconnaissance vocale");
                instances.SpeechSystem.getInstance().textToSpeech.Speak("Active reconnaissance vocale");
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (sensor == null)
            {
                instances.SpeechSystem.getInstance().speechMicEngine.UnloadAllGrammars();
                loadPlugins();
            }
            else
            {
                instances.SpeechSystem.getInstance().speechEngine.UnloadAllGrammars();
                loadPlugins();
            }
            invokeAddLog("Rechargement ok");
            instances.SpeechSystem.getInstance().textToSpeech.Speak("Rechargement effectué");
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_quitter)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            _quitter = true;
            Close();
        }

        private void sysTray_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }
    }
}
