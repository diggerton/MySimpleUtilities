using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleUtilities.ConsoleGoodies.Models
{
    public class EscapableInputReturnModel<T>
    {
        public bool WasEscaped { get; set; }
        public T Value { get; set; }
    }
}
