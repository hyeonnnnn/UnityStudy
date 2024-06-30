using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class Box
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Icon { get; }

        public Box(int x, int y, string icon)
        {
            X = x;
            Y = y;
            Icon = icon;
        }
    }
}
