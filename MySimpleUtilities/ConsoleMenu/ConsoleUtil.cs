using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.ConsoleMenu
{
    public static class ConsoleUtil
    {
        public static void WriteColor(string _p, ConsoleColor _color = ConsoleColor.Gray)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = _color;
            Console.Write(_p);
            Console.ForegroundColor = originalColor;
        }

        public static void WriteLineColor(string _p, ConsoleColor _color = ConsoleColor.Gray)
        {
            WriteColor(_p + Environment.NewLine, _color);
        }

        public static T GetInput<T>(string _prompt, string _retry = "Invalid input, try again.")
        {
            bool isBoolean = typeof(T) == typeof(bool);
            if(isBoolean)
                _prompt += " (y/n)";
            Console.Write(_prompt + ": ");
            try
            {
                string input;
                if (isBoolean)
                    input = Console.ReadKey().ToString().Equals("y", StringComparison.OrdinalIgnoreCase) ? "true" : "false";
                else
                    input = Console.ReadLine();

                if(input == null)
                {
                    WriteLineColor("Input canceled.", ConsoleColor.Red);
                    return default(T);
                }

                return (T) Convert.ChangeType(input, typeof(T));
}
            catch (Exception)
            {
                WriteLineColor(_retry, ConsoleColor.Red);
                return GetInput<T>(_prompt, _retry);
            }
        }
    }
}
