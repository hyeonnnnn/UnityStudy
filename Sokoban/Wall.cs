using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class Wall
    {
        public int X { get; }
        public int Y { get; }
        public string Icon { get; }

        public Wall(int x, int y, string icon)
        {
            X = x;
            Y = y;
            Icon = icon;
        }
    }
}
