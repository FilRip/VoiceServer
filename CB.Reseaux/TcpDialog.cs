using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CB.Reseaux
{
    public class TcpDialog
    {
        private int defaultTimeout = 5000;

        public bool sendData(string adr, int port, byte[] donnees)
        {
            return sendData(adr, port, donnees, defaultTimeout);
        }
        public bool sendData(string adr, int port, byte[] donnees, int timeout)
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            System.Net.Sockets.NetworkStream ns = null;

            try
            {
                client.Connect(adr, port);
                if (!client.Connected) throw new Exception("Pas de connexion");
                client.SendTimeout = timeout;
                ns = client.GetStream();
                ns.Write(donnees, 0, donnees.Length);
                return true;
            }
            catch { }
            finally
            {
                if (ns != null) ns.Close();
                client.Close();
            }
            return false;
        }

        public object sendAndReceived(string adr, int port, byte[] donnees, int timeout)
        {
            return sendAndReceivedData(adr, port, donnees, timeout);
        }
        public object sendAndReceived(string adr, int port, byte[] donnees)
        {
            return sendAndReceivedData(adr, port, donnees, defaultTimeout);
        }

        public void sendAndReceivedAsync(string adr, int port, byte[] donnees, int timeout, System.ComponentModel.RunWorkerCompletedEventHandler callBack)
        {
            System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker();
            Hashtable param = new Hashtable();
            param.Add("adr", adr);
            param.Add("port", port);
            param.Add("donnees", donnees);
            param.Add("timeout", timeout);
            bg.DoWork+=new System.ComponentModel.DoWorkEventHandler(sendAndReceivedAsyncCall);
            bg.RunWorkerCompleted += callBack;
            bg.RunWorkerAsync(param);
        }
        public void sendAndReceivedAsync(string adr, int port, byte[] donnees, System.ComponentModel.RunWorkerCompletedEventHandler callBack)
        {
            sendAndReceivedAsync(adr, port, donnees, defaultTimeout, callBack);
        }
        private void sendAndReceivedAsyncCall(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            e.Result = sendAndReceivedData((string)((Hashtable)(e.Argument))["adr"], (int)((Hashtable)(e.Argument))["port"], (byte[])((Hashtable)(e.Argument))["donnees"], (int)((Hashtable)(e.Argument))["timeout"]);
        }
        private object sendAndReceivedData(string adr, int port, byte[] donnees,int timeout)
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            System.Net.Sockets.NetworkStream ns = null;

            try
            {
                client.Connect(adr, port);
                if (!client.Connected) throw new Exception("Pas de connexion");
                client.SendTimeout = timeout;
                client.ReceiveTimeout = timeout;
                ns = client.GetStream();
                ns.Write(donnees, 0, donnees.Length);
                byte[] data = new byte[client.ReceiveBufferSize];
                return ns.Read(data, 0, client.ReceiveBufferSize);
            }
            catch { }
            finally
            {
                if (ns != null) ns.Close();
                client.Close();
            }
            return null;
        }
    }
}
