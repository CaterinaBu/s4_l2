using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s4_l2
{
    public abstract class Product
    {
        public string Name { get; set; }
        public decimal VendorCode { get; set; }
        public decimal Price { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int endX { get; set; }
        public int endY { get; set; }
        public string icon { get; set; }
        protected Product(string name, decimal vendorCode, decimal price, int x, int y,int endX, int endY, string icon)
        {
            Name = name;
            VendorCode = vendorCode;
            this.Price = price;
            this.x = x;
            this.y = y;
            this.endX = endX;
            this.endY = endY;
            this.icon = icon;
        }
    }
}
