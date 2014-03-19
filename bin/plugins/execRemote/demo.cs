using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testWMI
{
    public partial class Form1 : Form
    {
        private const string COMPUTER_NAME = "FILRIP_CENTRAL";
        private const string DOMAIN_NAME = "FILRIP_CENTRAL"; // Same as computer if there is no domain controller
        private const string USERNAME = "";
        private const string PASSWORD = "";

        private void mainSub(string phrase)
        {
            try
            {
                System.Management.ConnectionOptions co = new System.Management.ConnectionOptions();
                co.Username = USERNAME;
                co.Password = PASSWORD;
                co.Authority = "ntlmdomain:" + DOMAIN_NAME;
                System.Management.ManagementScope ms = new System.Management.ManagementScope("\\\\" + COMPUTER_NAME + "\\root\\cimv2", co);
                ms.Connect();
                System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_Process");
                mc.Scope = ms;

                System.Management.ManagementClass procStartInfo = new System.Management.ManagementClass("Win32_ProcessStartup");
                System.Management.ManagementObject psi = procStartInfo.CreateInstance();
                psi["ShowWindow"] = 1;

                System.Management.ManagementBaseObject mbo = mc.GetMethodParameters("Create");
                mbo["CommandLine"] = "notepad.exe";
                mbo["ProcessStartupInformation"] = psi;
                System.Management.ManagementBaseObject mbo2 = mc.InvokeMethod("Create", mbo, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
