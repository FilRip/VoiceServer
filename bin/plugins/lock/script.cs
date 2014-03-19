using System;
using System.Collections.Generic;
using System.Text;
using System.Speech;
using System.Collections;

namespace main
{
    public class mainClass
    {
        public static void mainSub(string phrase)
        {
            System.Diagnostics.Process.Start(@"rundll32.exe", "user32.dll,LockWorkStation");
        }
    }
}