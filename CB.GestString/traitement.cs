using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CB.GestString
{
    public class traitement
    {
        public static List<string> returnParams(string text, char separator)
        {
            return returnParams(text, separator, '"');
        }

        public static List<string> returnParams(string text, char separator, char textSeparator)
        {
            List<string> ret = new List<string>();
            string currentString = "";
            string curApos = "";
            bool add = true;
            while (text.LastIndexOf(separator.ToString() + separator.ToString())>=0)
                text = text.Replace(separator.ToString() + separator.ToString(), separator.ToString());
            foreach (char t in text)
            {
                add = true;
                if (t == textSeparator)
                    if (curApos == "")
                    {
                        curApos = textSeparator.ToString();
                        add = false;
                    }
                    else if (curApos == textSeparator.ToString())
                    {
                        curApos = "";
                        add = false;
                    }
                if ((t == separator) && (curApos == ""))
                {
                    ret.Add(currentString);
                    currentString = "";
                }
                else
                    if (add) currentString = currentString + t;
            }

            if (currentString != "") ret.Add(currentString);
            return ret;
        }

        public static int numberOfString(string text, string toSearch)
        {
            return numbersOfString(text, toSearch).Count;
        }

        public static List<int> numbersOfString(string text, string toSearch)
        {
            List<int> i = new List<int>();
            int start = 0;
            int lastFound;

            if ((text != null) && (text != ""))
                while (true)
                {
                    if (start >= text.Length) break;
                    lastFound = text.IndexOf(toSearch, start);
                    if (lastFound >= 0)
                    {
                        i.Add(lastFound);
                        start = lastFound + 1;
                    }
                    else
                        break;
                }

            return i;
        }

        public static string returnParam(string text, char separator, string param)
        {
            List<string> ret;
            bool returnNext = false;

            ret = returnParams(text, separator);
            if (ret!=null)
                foreach (string t in ret)
                {
                    if (returnNext) return t;
                    if (t.Trim().ToLower().StartsWith(param.Trim().ToLower()))
                    {
                        if (t.Trim().Length > param.Trim().Length)
                            return t.Substring(param.Trim().Length);
                        else
                            returnNext = true;
                    }
                }
            return null;
        }
    }
}
