using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando3.Persistence
{
    internal class LabyrinthSizeException : Exception
    {
        public LabyrinthSizeException() { }
        public LabyrinthSizeException(string message) : base(message) { }
    }
}
