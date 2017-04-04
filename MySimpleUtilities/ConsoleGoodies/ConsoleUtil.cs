using MySimpleUtilities.ConsoleGoodies.Models;
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

        public static EscapableInputReturnModel<T> GetValidatedEscapableInput<T>(string prompt, Func<T, bool> validator, string retry = "Invalid Input")
        {
            T value;
            var returnModel = new EscapableInputReturnModel<T>();
            bool isValid;

            try
            {
                do
                {
                    var result = GetEscapableInput<T>(prompt, retry);
                    if (result.WasEscaped)
                    {
                        returnModel.WasEscaped = result.WasEscaped;
                        isValid = true;
                        return returnModel;
                    }
                    else
                    {
                        value = (T)Convert.ChangeType(result.Value, typeof(T));
                        isValid = validator(value);
                    }

                    if (!isValid)
                        Console.WriteLine(retry);

                } while (!isValid);
                returnModel.Value = value;
            }
            catch (Exception)
            {
                Console.WriteLine(retry);
                GetValidatedEscapableInput(prompt, validator, retry);
            }

            return returnModel;
        }
        public static EscapableInputReturnModel<T> GetEscapableInput<T>(string _prompt, string _retry = "Invalid Input")
        {
            Console.Write($"{ _prompt }: ");
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            var sb = new StringBuilder();
            var returnModel = new EscapableInputReturnModel<T>();
            bool escChar;
            T value = default(T);

            try
            {
                do
                {
                    keyInfo = Console.ReadKey();
                    sb.Append(keyInfo.KeyChar);
                    escChar = keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.Escape;
                } while (!escChar);
                if (keyInfo.Key != ConsoleKey.Escape)
                    value = (T)Convert.ChangeType(sb.ToString(), typeof(T));
                else
                    returnModel.WasEscaped = true;
            }
            catch (Exception)
            {
                Console.WriteLine(_retry);
                GetEscapableInput<T>(_prompt, _retry);
            }

            returnModel.Value = value;
            Console.Write(Environment.NewLine);

            return returnModel;
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

        public static bool GetValidatedEscapableInput<T>(string _prompt, Func<T, bool> _validator, out T _value, string _retry = "Invalid Input")
        {
            var result = GetValidatedEscapableInput<T>(_prompt, _validator, _retry);
            _value = result.Value;
            return !result.WasEscaped;
        }
        public static bool GetEscapableInput<T>(string _prompt, out T _value, string _retry = "Invalid Input")
        {
            var result = GetEscapableInput<T>(_prompt, _retry);
            _value = result.Value;
            return result.WasEscaped;
        }
    }
}
