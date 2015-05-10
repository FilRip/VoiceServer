using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB.FireFox.models
{
    public class unProfil
    {
        private string _nom, _path;

        public string nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
