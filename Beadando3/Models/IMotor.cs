using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando3.Models
{
    public interface IMotor
    {
        void Move(int dx, int dy);
        void Start();
        void Pause();
        void Resume();
        void UpdateVisibleCells();
    }
}
