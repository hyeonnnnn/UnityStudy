using System;
using System.Xml.Serialization;

namespace Sokoban
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }

    class Item
    {
        public int Id;
        public string Name;
        public int Point;
        public int Weight;

        public Item[] itemTable = new Item[3]
        {
            new Item() {Id = 1, Name = "7 points", Point = 7, Weight = 1},
            new Item() {Id = 1, Name = "3 points", Point = 3, Weight = 3},
            new Item() {Id = 1, Name = "1 point", Point = 1, Weight = 6}
        };

        public int GetTotalWeight(Item[] itemTable)
        {
            int result = 0;
            for (int itemId = 0; itemId < itemTable.Length; ++itemId)
            {
                result += itemTable[itemId].Weight;
            }

            return result;
        }

        public Item GetRandomItem(int totalWeight, Item[] itemTable)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, totalWeight + 1);

            Item selectedItem = null;
            int weight = 0;
            for (int itemId = 0; itemId < itemTable.Length; ++itemId)
            {
                weight += itemTable[itemId].Weight;

                if (randomNumber <= weight)
                {
                    selectedItem = itemTable[itemId];
                    break;
                }
            }
            return selectedItem;
        }
    }
}