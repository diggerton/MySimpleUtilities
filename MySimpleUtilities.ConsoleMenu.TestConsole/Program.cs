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
            //var menu = new ConsoleMenu()
            //{
            //    HeaderText = "header text",
            //    PostActionAction = () => { Console.WriteLine("Press any key to continue..."); Console.ReadKey(); },
            //    SubHeaderText = "sub header text",
            //    EscapeClosesMenu = true
            //};
            //menu.AddMenuItem(new MenuItem
            //{
            //    Text = "MenuItem 1",
            //    Action = () => Console.WriteLine("Menu Item 1 selected"),
            //    ValidItem = true
            //});
            //menu.AddMenuItem(new MenuItem
            //{
            //    Text = "Invalid Menu Item",
            //    ValidItem = false
            //});
            //menu.AddMenuItem(new MenuItem
            //{
            //    Text = "MenuItem 2",
            //    Action = () => Console.WriteLine("Menu Item 2 selected"),
            //    ValidItem = true
            //});
            //menu.AddMenuItem(new MenuItem()
            //{
            //    Text = "MenuItem 3",
            //    Action = () => { Console.WriteLine("Menu Item 3 selected"); menu.DeleteCurrentMenuItem(); },
            //    ValidItem = true
            //});

            //menu.ShowMenu();


            var input = ConsoleUtil.GetInput<bool>("say yes");


        }
    }
}
