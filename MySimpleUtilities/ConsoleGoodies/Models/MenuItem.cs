using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.Models.ConsoleMenu
{
    public class MenuItem
    {
        private Action _action;
        private int _indent;

        public Action PreActionAction { get; set; }
        public Action PostActionAction { get; set; }
        public Action Action
        {
            get
            {
                return () =>
                {
                    this.PreActionAction?.Invoke();
                    this._action.Invoke();
                    this.PostActionAction?.Invoke();
                };
            }
            set { _action = value; }
        }

        public ConsoleColor? ForeColor { get; set; }
        public ConsoleColor? BackColor { get; set; }
        public ConsoleColor? HighlightForeColor { get; set; }
        public ConsoleColor? HighlightBackColor { get; set; }

        public string Text { get; set; }
        public bool ValidItem { get; set; } = true;
        public bool ExitAfterAction { get; set; }
        public int Id { get; set; }
        public bool DeleteSelf { get; set; }
        /// <summary>
        /// Additional MenuItem.Text indent, after cursor is drawn.  Must be >= 0.
        /// </summary>
        public int Indent
        {
            get { return _indent; }
            set
            {
                if (value < 0)
                    _indent = 0;
                _indent = value;
            }
        }


        public static MenuItem Spacer(string _text = "")
        {
            return new MenuItem()
            {
                Text = _text,
                ValidItem = false
            };
        }
    }
}
