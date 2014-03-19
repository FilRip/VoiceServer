using System;
using System.Collections.Generic;
using System.Text;
using System.Speech;
using System.Collections;

namespace main
{
    public class mainClass
    {
        public static void mainSub(object whatHeSay)
        {
            string contenu = "";
            string temps, temperature;
            int start, end;

            contenu = CB.Reseaux.GestionHTTP.retourneContenu("http://fr.meteo.yahoo.com/france/bourgogne/dijon-12723632/");
            if (contenu != "")
            {
                start=contenu.LastIndexOf("day-temp-current temp-c");
                end = contenu.Substring(start + 25).IndexOf("&deg;");
                temperature=contenu.Substring(start + 26, end-1);
                start = contenu.LastIndexOf("condition first");
                end = contenu.Substring(start + 17).TrimStart().IndexOf("</li>");
                temps = contenu.Substring(start + 17, end - 1).Replace("&nbsp", "");
                VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Il fait actuellement " + temperature + " degrée");
                VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Les conditions sont " + temps);
            }
            else
                VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Le service meteo est indisponible");
        }

    }

}
