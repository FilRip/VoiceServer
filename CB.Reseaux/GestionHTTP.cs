using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace CB.Reseaux
{
    public class GestionHTTP
    {
        public static string retourneContenuPost(string adresse, string data)
        {
            string retour = "";
            try
            {
                HttpWebRequest oWRequest = (HttpWebRequest)WebRequest.Create(adresse);
                oWRequest.Method = "POST";
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
                oWRequest.ContentType = "application/x-www-form-urlencoded";
                oWRequest.ContentLength = byteArray.Length;
                Stream dataStream = oWRequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            
                HttpWebResponse oWResponse = (HttpWebResponse)oWRequest.GetResponse();
                Stream oS = oWResponse.GetResponseStream();
                StreamReader oSReader = new StreamReader(oS, System.Text.Encoding.ASCII);
                retour = oSReader.ReadToEnd();
                oSReader.Close();
                oS.Close();
            }
            catch (Exception e)
            {
                retour = e.Message;
            }
            return retour;
        }

        public static string retourneContenu(string adresse)
        {
            string retour = "";
            try
            {
                HttpWebRequest oWRequest = (HttpWebRequest)WebRequest.Create(adresse);
                HttpWebResponse oWResponse = (HttpWebResponse)oWRequest.GetResponse();
                Stream oS = oWResponse.GetResponseStream();
                StreamReader oSReader = new StreamReader(oS, System.Text.Encoding.ASCII);
                retour = oSReader.ReadToEnd();
                oSReader.Close();
                oS.Close();
            }
            catch { }
            return retour;
        }

        public static bool envoiContenu(string adresse)
        {
            string retour = "";
            try
            {
                HttpWebRequest oWRequest = (HttpWebRequest)WebRequest.Create(adresse);
                HttpWebResponse oWResponse = (HttpWebResponse)oWRequest.GetResponse();
                Stream oS = oWResponse.GetResponseStream();
                StreamReader oSReader = new StreamReader(oS, System.Text.Encoding.ASCII);
                retour = oSReader.ReadToEnd();
                oSReader.Close();
                oS.Close();
                return true;
            }
            catch { }
            return false;
        }

    }
}
