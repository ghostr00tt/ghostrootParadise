using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Shelter
{
    class BeepBlop : Shelter
    {
        public BeepBlop(string name) : base(name) { }
        public override string exc(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Beep();
                return "";
            }
            else if (args.Length == 1)
                {
                    int i = 0;
                    while (i != Int32.Parse(args.First()))
                    {
                        Console.Beep();
                        i++;
                    }
                return "";
                }
            
            else
            {
                return "Beep requires either 0 or 1 arguments.";
            }
        }
    }
}
