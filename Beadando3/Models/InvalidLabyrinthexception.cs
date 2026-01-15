using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando3.Models
{
    public class InvalidLabyrinthexception : Exception
    {
        public InvalidLabyrinthexception(string mess) : base(mess) { }

        public InvalidLabyrinthexception() { }
    }
}
