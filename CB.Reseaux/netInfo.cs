using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CB.Reseaux
{
    public static class netInfo
    {
        public static List<string> localIPv4Address()
        {
            List<string> retour = new List<string>();
            foreach (System.Net.NetworkInformation.NetworkInterface ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                if (ni.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Down)
                    foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ua in ni.GetIPProperties().UnicastAddresses)
                        if (ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            retour.Add(ua.Address.ToString());
            return retour;
        }
    }
}
