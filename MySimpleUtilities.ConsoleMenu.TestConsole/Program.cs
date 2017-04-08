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
            var consoleMenu = new ConsoleMenu()
            {
                HeaderText = "Test Header",
                SubHeaderText = "test sub header",
                EscapeClosesMenu = true
            };
            consoleMenu.AddMenuItem(new MenuItem
            {
                Text = "Menu Item 1"
            });
            consoleMenu.AddMenuItem(new MenuItem
            {
                Text = "Menu Item 2"
            });
            consoleMenu.AddMenuItem(new MenuItem
            {
                Text = "Menu Item 3"
            });
            consoleMenu.AddMenuItem(new MenuItem
            {
                Text = "Exit",
                Action = () => consoleMenu.HideMenu()
            });

            consoleMenu.PreDrawMenuAction = () => ConsoleUtil.WriteLineColor("TEST PRE SHOW MENU ACTION", ConsoleColor.Magenta);

            consoleMenu.ShowMenu();

            foreach (var color in Enum.GetNames(typeof(ConsoleColor)))
            {
                ConsoleUtil.WriteLineColor(color.ToString(), (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color));
            }

            Console.ReadKey();
        }
    }
}
