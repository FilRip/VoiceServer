using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows;
using StartupEventArgs = Microsoft.VisualBasic.ApplicationServices.StartupEventArgs;
using System.Runtime.InteropServices;

namespace CSharp1SeuleInstance.Winform
{
    sealed class SingleInstanceManagerWinForm : WindowsFormsApplicationBase
    {
        private SingleInstanceManagerWinForm()
            : base(AuthenticationMode.Windows)
        {
            //C'est ce qui va permettre 1 seule instance à la fois!
            this.IsSingleInstance = true;
        }

        [STAThread]
        internal static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            SingleInstanceManagerWinForm instance = new SingleInstanceManagerWinForm();
            instance.MainForm = new VoiceServer.mainForm();
            instance.Run(args);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Exception.Message);
        }
    }
}
