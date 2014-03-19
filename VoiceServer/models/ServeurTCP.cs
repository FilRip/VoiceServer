using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceServer.models
{
    class ServeurTCP : CB.Reseaux.TcpDialog
    {
        private System.Net.Sockets.TcpClient _client = new System.Net.Sockets.TcpClient();
        private System.Net.Sockets.TcpListener _serveur;

        public void initServeur()
        {
            byte[] ipALecoute;
            List<string> listipv4;

            listipv4 = CB.Reseaux.netInfo.localIPv4Address();

            ipALecoute = listipv4.SelectMany(s => System.Text.Encoding.ASCII.GetBytes(s.ToString())).ToArray();
            _serveur = new System.Net.Sockets.TcpListener(new System.Net.IPAddress(ipALecoute), 4010);
        }
    }
}
