using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1Tasking
{
    public class ConsoleInput
    {
        public bool Cancel { get; set; }

        public virtual void OnCancelKeyPress(object sender, ConsoleCancelEventArgs eventArgs)
        {
            Cancel = true;
        }
    }
}
