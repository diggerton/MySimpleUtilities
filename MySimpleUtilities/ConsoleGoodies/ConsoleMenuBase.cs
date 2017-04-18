using MySimpleUtilities.ConsoleExtended;
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
    public class ConsoleMenuBase
    {
        public ConsoleMenuBase(string _cursor = "->")
        { CursorChar = _cursor; }

        private int cursorPosition;
        private bool exitMenu;

        public string CursorChar { get; set; }
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }

        public ConsoleColor ForeColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor HighlightForeColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor HighlightBackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor HeaderColor { get; set; } = ConsoleColor.White;
        public ConsoleColor SubHeaderColor { get; set; } = ConsoleColor.Gray;

        public Action PostActionAction { get; set; }
        public Action OnExitAction { get; set; }
        public Func<IEnumerable<MenuItem>> BuildDynamicMenu { get; set; }

        public bool UseDynamicMenuGeneration { get; set; }
        public bool EscapeClosesMenu { get; set; } = true;
        public bool DrawHeader { get; set; } = true;
        public bool DrawSubHeader { get; set; } = true;

        private Dictionary<int, MenuItem> MenuItems { get; set; } = new Dictionary<int, MenuItem>();

        public virtual void AddMenuItem(MenuItem _item)
        {
            int id = MenuItems.Count == 0 ? 0 : MenuItems.Max(m => m.Key) + 1;
            if (string.IsNullOrWhiteSpace(_item.Text))
                _item.ValidItem = false;
            
            MenuItems.Add(id, _item);
        }
        public virtual void RemoveMenuItem(int _id)
        {
            if (MenuItems.ContainsKey(_id))
                MenuItems.Remove(_id);
        }

        public virtual void ShowMenu()
        {
            exitMenu = false;
            cursorPosition = ResolveCursorPosition(-10);
            while (!exitMenu)
            {
                Console.CursorVisible = false;
                Console.Clear();

                if (DrawHeader)
                    DrawHeaderText();
                if (DrawSubHeader)
                    DrawSubHeaderText();
                if (DrawHeader || DrawSubHeader)
                    Console.WriteLine();

                DrawMenuText();
                UpdateMenu();

                CleanMenuItems();
            }
        }
        public virtual void HideMenu()
        {
            OnExitAction?.Invoke();
            exitMenu = true;
        }
        public virtual void UpdateMenu()
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
                        MenuItems[cursorPosition].Action?.Invoke();
                        PostActionAction?.Invoke();
                        if (MenuItems[cursorPosition].ExitAfterAction)
                            RemoveMenuItem(MenuItems[cursorPosition].Id);
                    }
                    break;
                case ConsoleKey.Escape:
                    {
                        if (EscapeClosesMenu)
                            HideMenu();
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual void DrawHeaderText()
        {
            ConsoleUtil.WriteLineColor(HeaderText, HeaderColor);
        }
        public virtual void DrawSubHeaderText()
        {
            ConsoleUtil.WriteLineColor(SubHeaderText, SubHeaderColor);
        }
        public virtual void DrawMenuText()
        {
            var cursorPadding = (" " + CursorChar + " ").Length;

            foreach (var item in MenuItems)
            {
                if (item.Key == cursorPosition)
                {
                    ConsoleUtil.WriteColor($" { CursorChar } ", HighlightForeColor, BackColor);
                    Console.Write(new string(' ', item.Value.Indent));
                    ConsoleUtil.WriteLineColor(item.Value.Text, item.Value.HighlightForeColor ?? HighlightForeColor, item.Value.HighlightBackColor ?? HighlightBackColor);
                }
                else
                {
                    Console.Write(new string(' ', cursorPadding));
                    Console.Write(new string(' ', item.Value.Indent));
                    ConsoleUtil.WriteLineColor(item.Value.Text, item.Value.ForeColor ?? ForeColor, item.Value.BackColor ?? BackColor);
                }
            }
        }

        private int ResolveCursorPosition(int _movement)
        {
            if (!MenuItems.Any(m => m.Value.ValidItem))
                throw new ArgumentException($"No valid { nameof(MenuItem) } found in { nameof(MenuItems) }.");

            int minValidId = MenuItems.Min(m => m.Key);
            int maxValidId = MenuItems.Max(m => m.Key);

            if (cursorPosition + _movement <= minValidId)
                return minValidId;

            if (cursorPosition + _movement >= maxValidId)
                return maxValidId;

            if (MenuItems.ContainsKey(cursorPosition + _movement) && MenuItems[cursorPosition + _movement].ValidItem)
                return cursorPosition + _movement;
            else
                return ResolveCursorPosition(_movement > 0 ? _movement + 1 : _movement - 1);
        }
        private void CleanMenuItems()
        {
            List<int> toRemove = new List<int>();

            foreach (var menuItem in MenuItems.Values)
            {
                if (menuItem.DeleteSelf)
                    toRemove.Add(menuItem.Id);
            }
            foreach (var id in toRemove)
            {
                RemoveMenuItem(id);
            }

            if (MenuItems.Any(m => m.Value.ValidItem))
                cursorPosition = ResolveCursorPosition(0);
            else
                HideMenu();
        }
    }
}
