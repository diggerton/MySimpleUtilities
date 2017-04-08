using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.Models.ConsoleMenu
{
    public class MenuItem
    {
        public string Text { get; set; } = "";
        public Action Action { get; set; } = () => throw new NotImplementedException();
        public bool ValidItem { get; set; } = true;

        public MenuItem()
        {

        }
        public MenuItem(string _text, Action _action, bool _validItem = true)
        {
            Text = _text;
            Action = _action;
            ValidItem = _validItem;
        }
    }
}
