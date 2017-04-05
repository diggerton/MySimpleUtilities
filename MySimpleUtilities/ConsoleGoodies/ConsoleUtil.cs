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

        public static T GetInput<T>(string _prompt, string _retry = "Invalid Input")
        {
            T value = default(T);
            var isBool = typeof(T) == typeof(bool);
            string input = string.Empty;
            var prompt = _prompt + (isBool ? " (y/n)" : "");

            try
            {
                Console.Write($"{ prompt }: ");
                if (isBool)
                {
                    input = Console.ReadKey().KeyChar.ToString().Equals("y", StringComparison.OrdinalIgnoreCase) ? "true" : "false";
                }
                else
                {
                    input = Console.ReadLine();
                }
                Console.WriteLine();
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception)
            {
                Console.WriteLine(_retry);
                GetInput<T>(_prompt, _retry);
            }

            return value;
        }
        public static T GetValidatedInput<T>(string _prompt, Func<T, bool> _validator, string _retry = "Invalid Input")
        {
            T value = default(T);
            var isBool = typeof(T) == typeof(bool);
            bool isValid = false;

            try
            {
                do
                {
                    Console.Write($"{ _prompt }: ");
                    value = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                    isValid = _validator(value);
                    if(!isValid)
                        Console.WriteLine(_retry);
                } while (!isValid);
            }
            catch (Exception)
            {
                Console.WriteLine(_retry);
                GetValidatedInput<T>(_prompt, _validator, _retry);
            }
            return value;
        }
        
        public static bool GetEscapableInput<T>(string _prompt, out T _value, string _retry = "Invalid Input")
        {
            string inputString;
            _value = default(T);
            bool isEmptyInput = false;
            bool isBool = typeof(T) == typeof(bool);

            try
            {
                Console.Write($"{ _prompt }");
                if (isBool)
                {
                    Console.Write($" (y/n): ");
                    inputString = Console.ReadKey().Key.ToString();
                    if (string.IsNullOrEmpty(inputString))
                    {
                        inputString = string.Empty;
                        isEmptyInput = true;
                    }
                    else if (inputString.Equals("y", StringComparison.OrdinalIgnoreCase))
                        inputString = true.ToString();
                    else if (inputString.Equals("n", StringComparison.OrdinalIgnoreCase))
                        inputString = false.ToString();
                }
                else
                {
                    Console.Write(": ");
                    inputString = Console.ReadLine();
                }

                if (!string.IsNullOrWhiteSpace(inputString))
                    _value = (T)Convert.ChangeType(inputString, typeof(T));
                else
                    isEmptyInput = true;
            }
            catch (Exception)
            {
                Console.WriteLine(_retry);
                return GetEscapableInput<T>(_prompt, out _value, _retry);
            }

            return !isEmptyInput;
        }
        public static bool GetValidatedEscapableInput<T>(string _prompt, Func<T, bool> _validator, out T _value, string _retry = "Invalid Input")
        {
            bool isValid = false;
            bool notEscaped = false;
            do
            {
                notEscaped = GetEscapableInput<T>(_prompt, out _value, _retry);
                if (notEscaped)
                    isValid = _validator(_value);

                if (!isValid && notEscaped)
                    Console.WriteLine(_retry);

            } while (!isValid && notEscaped);

            return notEscaped;
        }
    }
}
