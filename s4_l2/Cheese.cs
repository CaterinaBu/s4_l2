using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s4_l2
{
    public class Cheese : Product 
    {
        public Cheese(string name, decimal vendorCode, decimal price, int x, int y, int endX, int endY /*string icon*/) 
            : base(name, vendorCode, price, x, y, endX, endY, "pacman.png")
        {
        }
    }
}
