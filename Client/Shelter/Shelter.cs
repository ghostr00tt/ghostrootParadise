using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Shelter
{

    class Shelter
    {
        public string name;
        public Shelter(string name)
        {
            this.name = name;
        }

        public virtual string exc(string[]args) { return ""; }
    }
}
