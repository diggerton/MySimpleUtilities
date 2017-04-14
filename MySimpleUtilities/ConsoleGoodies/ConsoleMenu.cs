using MySimpleUtilities.Models.ConsoleMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Action PreDrawMenuAction { get; set; }
        public Action PostDrawMenuAction { get; set; }
        public Action PostActionAction { get; set; }
        public bool EscapeClosesMenu { get; set; } = true;
        public bool DrawHeader { get; set; } = true;
        public bool DrawSubHeader { get; set; } = true;

        private List<MenuItemEx> MenuItems { get; set; } = new List<MenuItemEx>();
        
        /// <summary>
        /// Adds spacer menu item.
        /// </summary>
        public void AddMenuItem()
        {
            int id = MenuItems.Count == 0 ? 0 : MenuItems.Max(m => m.Id) + 1;

            MenuItems.Add(new MenuItemEx(new MenuItem { Text = "", ValidItem = false }) { Id = id });
        }
        /// <summary>
        /// Adds new menu item.
        /// </summary>
        /// <param name="_item">MenuItem object</param>
        public void AddMenuItem(MenuItem _item)
        {
            int id = MenuItems.Count == 0 ? 0 : MenuItems.Max(m => m.Id) + 1;
            if (string.IsNullOrWhiteSpace(_item.Text))
                _item.ValidItem = false;

            MenuItems.Add(new MenuItemEx(_item) { Id = id });
        }
        
        private void CleanMenuItems()
        {
            MenuItems.RemoveAll(m => m.DeleteSelf);
            cursorPosition = ResolveCursorPosition(0);
        }

        /// <summary>
        /// Deletes currently selected menu item.
        /// </summary>
        public void DeleteCurrentMenuItem()
        {
            var curMenuItem = MenuItems.FirstOrDefault(m => m.Id == cursorPosition);
            if (curMenuItem != null)
                curMenuItem.DeleteSelf = true;
        }

        public void ShowMenu()
        {
            exitMenu = false;
            cursorPosition = ResolveCursorPosition(-10);
            while (!exitMenu)
            {
                Console.CursorVisible = false;
                Console.Clear();

                CleanMenuItems();

                if (DrawHeader)
                    DrawHeaderText();
                if(DrawSubHeader)
                    DrawSubHeaderText();
                if(DrawHeader || DrawSubHeader)
                    Console.WriteLine();

                PreDrawMenuAction?.Invoke();
                DrawMenuText();
                PostDrawMenuAction?.Invoke();
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
                        MenuItems[cursorPosition]?.Action();
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
            if (!MenuItems.Any(m => m.ValidItem))
                throw new ArgumentException($"No valid { nameof(MenuItem) } found in { nameof(MenuItems) }.");

            int minValidId = MenuItems.Min(m => m.Id);
            int maxValidId = MenuItems.Max(m => m.Id);

            if (cursorPosition + _movement <= minValidId)
                return minValidId;

            if (cursorPosition + _movement >= maxValidId)
                return maxValidId;

            if (MenuItems.FirstOrDefault(m=>m.Id==cursorPosition+_movement) != null
                && MenuItems.First(m => m.Id == cursorPosition + _movement).ValidItem)
                return cursorPosition + _movement;
            else
                return ResolveCursorPosition(_movement > 0 ? _movement + 1 : _movement - 1);
        }

        public virtual void DrawMenuText()
        {
            var cursorPadding = (" " + CursorChar + " ").Length;

            foreach (var item in MenuItems)
            {
                if (item.Id == cursorPosition)
                {
                    Console.Write(" ");
                    WriteColor(CursorChar, HighlightColor);
                    Console.Write(" ");
                    WriteLineColor(item.Text, HighlightColor);
                }

                else
                {
                    Console.Write(new string(' ', cursorPadding));
                    WriteLineColor(item.Text);
                }
            }
        }
        public virtual void DrawHeaderText()
        {
            WriteLineColor(HeaderText, HeaderColor);
        }
        public virtual void DrawSubHeaderText()
        {
            WriteLineColor(SubHeaderText, SubHeaderColor);
        }

        public virtual void WriteColor(string _text, ConsoleColor _foreColor = ConsoleColor.Gray)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            Console.ForegroundColor = _foreColor;
            Console.Write(_text);
            Console.ForegroundColor = originalForeColor;
        }
        public virtual void WriteLineColor(string _text, ConsoleColor _foreColor = ConsoleColor.Gray)
        {
            WriteColor(_text + Environment.NewLine, _foreColor);
        }
        
        public virtual string PromptUser(string _prompt, bool _singleCharInput = false)
        {
            Console.Write(_prompt + ": ");
            var result = string.Empty;

            if (_singleCharInput)
                result = Console.ReadKey().ToString();
            else
                result = Console.ReadLine();

            return result;
        }
        public virtual T GetInput<T>(string _prompt, string _retry, bool _singleCharInput = false)
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
        public virtual T ValidateInput<T>(string _prompt, string _retry, Func<T, bool> _comparer, bool _singleCharInput = false)
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

        private class MenuItemEx : MenuItem
        {
            public MenuItemEx(MenuItem _mi)
            {
                Text = _mi.Text;
                Action = _mi.Action;
                ValidItem = _mi.ValidItem;
            }

            public int Id { get; set; }
            public bool DeleteSelf { get; set; } = false;
        }
    }
}
