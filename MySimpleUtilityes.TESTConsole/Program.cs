using MySimpleUtilities.ConsoleMenu;
using MySimpleUtilities.Models.ConsoleMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilityes.TESTConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new MyConsoleMenu()
            {
            };
            menu.AddMenuItem(new MenuItem()
            {
                Text = "Test MenuItem",
                Indent = -10
            });
            menu.AddMenuItem(MenuItem.Spacer());
            menu.AddMenuItem(new MenuItem
            {
                Text = "default fore/back color"
            });
            menu.AddMenuItem(new MenuItem
            {
                Text = "alternate fore/back color"
            });

            menu.ShowMenu();
        }
    }
}
