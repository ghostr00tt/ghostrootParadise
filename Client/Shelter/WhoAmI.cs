using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Shelter
{
    class WhoAmI : Shelter
    {

        public WhoAmI(string name) : base(name) { }
        public override string exc(string[] args)
        {
            if (args.Length != 0) return "Invalid arguments for WhoAmI,expected 0";
            return "Username : "+Environment.UserName +" | "+"Machine Name : "+ Environment.MachineName + " | "+"Domain Name : "+Environment.UserDomainName ;
        }
    }
}
