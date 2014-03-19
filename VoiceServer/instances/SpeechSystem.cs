using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;

namespace VoiceServer.instances
{
    public class SpeechSystem
    {
        private static SpeechSystem _instance;

        private SpeechRecognitionEngine _speechEngine;
        private System.Speech.Synthesis.SpeechSynthesizer _ss;
        private System.Speech.Recognition.SpeechRecognitionEngine _speechMicEngine;
        private int _vitesseSyntheseVocale;
        private int _volumeSyntheseVocale;

        public static SpeechSystem getInstance()
        {
            if (_instance == null)
            {
                _instance = new SpeechSystem();
                _instance._ss = new System.Speech.Synthesis.SpeechSynthesizer();
            }
            return _instance;
        }

        public System.Speech.Recognition.SpeechRecognitionEngine speechMicEngine
        {
            get { return _speechMicEngine; }
            set { _speechMicEngine = value; }
        }

        public SpeechRecognitionEngine speechEngine
        {
            get { return _speechEngine; }
            set { _speechEngine = value; }
        }

        public System.Speech.Synthesis.SpeechSynthesizer textToSpeech
        {
            get { return _ss; }
            set { _ss = value; }
        }

        public int vitesseSyntheseVocale
        {
            get { return _vitesseSyntheseVocale; }
            set { _vitesseSyntheseVocale = value; }
        }

        public int volumeSyntheseVocale
        {
            get { return _volumeSyntheseVocale; }
            set { _volumeSyntheseVocale = value; }
        }
    }
}
