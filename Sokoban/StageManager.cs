using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class StageManager
    {
        public Player Player { get; private set; }
        public List<Box> Boxes { get; private set; }
        public List<Wall> Walls { get; private set; }
        public List<Goal> Goals { get; private set; }

        
        public StageManager()
        {
            Boxes = new List<Box>();
            Walls = new List<Wall>();
            Goals = new List<Goal>();
        }

        public void LoadStage(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char c = lines[y][x];
                    switch (c)
                    {
                        case 'P':
                            Player = new Player(x, y, "P");
                            break;
                        case 'B':
                            Boxes.Add(new Box(x, y, "B"));
                            break;
                        case 'W':
                            Walls.Add(new Wall(x, y, "W"));
                            break;
                        case 'G':
                            Goals.Add(new Goal(x, y, "G"));
                            break;
                    }
                }
            }
        }
    }
}
