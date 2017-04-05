using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.ConsoleMenu.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConsoleUtil.GetValidatedEscapableInput<int>("5 < int < 10", x => x > 5 && x < 10, out int value))
            {
                Console.WriteLine("Good! " + value);
            }
            else
                Console.WriteLine("Canceled input");

            Console.ReadKey();
        }
    }
}
