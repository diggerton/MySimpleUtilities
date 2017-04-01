using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.ConsoleMenu
{
    public class ConsoleMenu
    {
        private ConsoleMenu() { }
        public ConsoleMenu(string _cursor = "->")
        { CursorChar = _cursor; }

        private int cursorPosition;
        private bool exitMenu;

        public string CursorChar { get; set; }
        public ConsoleColor ForeColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor HighlightColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.White;
        public ConsoleColor SubHeaderColor { get; set; } = ConsoleColor.Gray;
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        /// <summary>
        /// An additional Action performed after MenuItem.Action
        /// </summary>
        public Action PostActionAction { get; set; }
        public bool EscapeClosesMenu { get; set; } = true;
        public bool DrawHeader { get; set; } = true;
        public bool DrawSubHeader { get; set; } = true;

        private Dictionary<int, MenuItem> MenuDict { get; set; }

        public void AddMenuItem(MenuItem _item)
        {
            if (MenuDict == null)
                MenuDict = new Dictionary<int, MenuItem>();

            int id = MenuDict.Count == 0 ? 0 : MenuDict.Max(m => m.Key) + 1;

            if (!MenuDict.Any(m => m.Value.Text.Equals(_item.Text, StringComparison.OrdinalIgnoreCase)))
                MenuDict.Add(id, _item);
            else
                Debug.WriteLine("Duplicate MenuItem was not added to the collection.");
        }

        public void ShowMenu()
        {
            exitMenu = false;
            cursorPosition = ResolveCursorPosition(-10);
            while (!exitMenu)
            {
                Console.CursorVisible = false;
                Console.Clear();

                if (DrawHeader)
                    DrawHeaderText();
                if(DrawSubHeader)
                    DrawSubHeaderText();
                if(DrawHeader || DrawSubHeader)
                    Console.WriteLine();

                DrawMenuText();
                UpdateMenu();
            }
        }

        public void HideMenu()
        {
            exitMenu = true;
        }

        public void UpdateMenu()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        cursorPosition = ResolveCursorPosition(-1);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    {
                        cursorPosition = ResolveCursorPosition(1);
                    }
                    break;
                case ConsoleKey.Enter:
                    {
                        Console.CursorVisible = true;
                        MenuDict[cursorPosition]?.Action();
                        PostActionAction?.Invoke();
                    }
                    break;
                case ConsoleKey.Escape:
                    {
                        if(EscapeClosesMenu)
                            exitMenu = true;
                    }
                    break;
                default:
                    break;
            }
        }

        public int ResolveCursorPosition(int _movement)
        {
            if (!MenuDict.Any(m => m.Value.ValidItem))
                throw new ArgumentException($"No valid { nameof(MenuItem) } found in { nameof(MenuDict) }.");

            int minValidId = MenuDict.Where(m => m.Value.ValidItem).Min(m => m.Key);
            int maxValidId = MenuDict.Where(m => m.Value.ValidItem).Max(m => m.Key);

            if (cursorPosition + _movement <= minValidId)
                return minValidId;

            if (cursorPosition + _movement >= maxValidId)
                return maxValidId;

            if (MenuDict.ContainsKey(cursorPosition + _movement) && MenuDict[cursorPosition + _movement].ValidItem)
                return cursorPosition + _movement;
            else
                return ResolveCursorPosition(_movement > 0 ? _movement + 1 : _movement - 1);
        }

        public void DrawMenuText()
        {
            var cursorPadding = (" " + CursorChar + " ").Length;
            for (int i = 0; i < MenuDict.Count; i++)
            {
                if(i == cursorPosition)
                {
                    Console.Write(" ");
                    WriteColor(CursorChar, HighlightColor);
                    Console.Write(" ");
                    WriteLineColor(MenuDict[i].Text, HighlightColor);
                }
                else
                {
                    Console.Write(new string(' ', cursorPadding));
                    WriteLineColor(MenuDict[i].Text);
                }
            }
        }
        public void DrawHeaderText()
        {
            WriteLineColor(HeaderText, HeaderColor);
        }
        public void DrawSubHeaderText()
        {
            WriteLineColor(SubHeaderText, SubHeaderColor);
        }

        public void WriteColor(string _text, ConsoleColor _foreColor = ConsoleColor.Gray)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            Console.ForegroundColor = _foreColor;
            Console.Write(_text);
            Console.ForegroundColor = originalForeColor;
        }
        public void WriteLineColor(string _text, ConsoleColor _foreColor = ConsoleColor.Gray)
        {
            WriteColor(_text + Environment.NewLine, _foreColor);
        }
        
        public string PromptUser(string _prompt, bool _singleCharInput = false)
        {
            Console.Write(_prompt + ": ");
            var result = string.Empty;

            if (_singleCharInput)
                result = Console.ReadKey().ToString();
            else
                result = Console.ReadLine();

            return result;
        }
        public T GetInput<T>(string _prompt, string _retry, bool _singleCharInput = false)
        {
            try
            {
                return (T)Convert.ChangeType(PromptUser(_prompt, _singleCharInput), typeof(T));
            }
            catch (Exception)
            {
                Console.WriteLine(_retry);
                return GetInput<T>(_prompt, _retry, _singleCharInput);
            }
        }
        public T ValidateInput<T>(string _prompt, string _retry, Func<T, bool> _comparer, bool _singleCharInput = false)
        {
            T input;
            var valid = false;

            do
            {
                input = GetInput<T>(_prompt, _retry, _singleCharInput);
                valid = _comparer(input);
            } while (!valid);

            return input;
        }
    }
}
