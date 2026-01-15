using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando3.Models
{
    public class Position
    {
        public int x { get; }
        public int y { get; }

        public Position(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public Position Move(int dx, int dy) // segito funkcio a motornak igy tudja megcsinalni az uj poziciot
        {
            return new Position(x + dx, y + dy);
        }//azert fontos mert igy tudjuk tesztelni a mozgas helyesseget azelott hogy a karakter odalepne
    }
}
