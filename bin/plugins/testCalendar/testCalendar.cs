using System;
using System.Collections.Generic;
using System.Text;
using System.Speech;
using System.Collections;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace main
{
    public class mainClass
    {
        public static void mainSub(object whatHeSay)
        {
            ArrayList arrayLists = new ArrayList(50);
            ArrayList entryList = new ArrayList(50);
            DateTime nextDate = DateTime.Now;
            bool oneFound = false;
            EventQuery eventQuery = new EventQuery();
            CalendarService calendarService = new CalendarService("VoiceServer");
            calendarService.setUserCredentials("filrip@gmail.com", "password");
            eventQuery.Uri = new Uri(@"https://www.google.com/calendar/feeds/default/private/full");
            eventQuery.StartTime=DateTime.Now;
            eventQuery.EndTime=DateTime.Now.AddDays(7);
            EventFeed eventFeed = calendarService.Query(eventQuery);
            if ((eventFeed != null) && (eventFeed.Entries.Count > 0))
            {
                foreach (EventEntry entry in eventFeed.Entries)
                {
                    entryList.Add(entry);
                    if (entry.Times.Count <= 0)
                    {
                        continue;
                    }
                    foreach (When time in entry.Times)
                    {
                        arrayLists.Add(time.StartTime);
                        if (nextDate.CompareTo(time.StartTime)<0)
                        {
                            nextDate = time.StartTime;
                            oneFound = true;
                        }
                    }
                }
                if (oneFound)
                {
                    VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Votre prochain rendez-vous est le : "+nextDate.ToLongDateString() + nextDate.ToLongTimeString());
                    VoiceServer.instances.ClassParam.log(nextDate.ToString());
                }
            }
            if (!oneFound)
            {
                VoiceServer.instances.SpeechSystem.getInstance().textToSpeech.Speak("Vous n'avez pas de rendez-vous dans les 7 prochains jours");
            }
        }

    }

}
