using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando3.Models
{
    public class UnknownCellTypeException : Exception
    {
        public UnknownCellTypeException(string mess) : base(mess) { }

        public UnknownCellTypeException() { }
    }
}
