using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    enum Direction
    {
        NONE = 0,
        LEFT = 1,
        RIGHT = 2,
        UP = 3,
        DOWN = 4
    }

    internal class Game
    {
        Player player;
        List<Box> boxes;
        List<Wall> walls;
        List<Goal> goals;
        StageManager stageManager;

        public Game()
        {
            stageManager = new StageManager();
            InitializeGame(stageManager.CurrStage);
        }

        void InitializeGame(int stageNumber)
        {
            stageManager.LoadStage($"stage{stageNumber}.txt");

            player = stageManager.Player;
            boxes = stageManager.Boxes;
            walls = stageManager.Walls;
            goals = stageManager.Goals;
        }

        const int MIN_X = 0, MIN_Y = 1;
        const int MAX_X = 119, MAX_Y = 29;

        delegate bool PositionChecker(int x, int y);

        public void Play()
        {
            InitializeConsole();

            while (true)
            {
                Console.Clear();

                if (AreAllBoxesOnGoals())
                {
                    stageManager.MoveNextStage();
                    if (stageManager.IsClearGame())
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Game Clear");
                        break;
                    }
                    InitializeGame(stageManager.CurrStage);
                    continue;
                }

                DrawGameObjects();

                ConsoleKey key = GetKey();
                Direction movingDirection = DecideDirection(key);

                MovePlayer(movingDirection);
            }
        }

        void InitializeConsole()
        {
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.Title = "Sokoban";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Clear();
        }

        void DrawGameObjects()
        {
            PrintObject(player.X, player.Y, player.Icon);
            foreach (var wall in walls)
            {
                PrintObject(wall.X, wall.Y, wall.Icon);
            }
            foreach (var goal in goals)
            {
                PrintObject(goal.X, goal.Y, goal.Icon);
            }
            foreach (var box in boxes)
            {
                PrintObject(box.X, box.Y, box.Icon);
            }
        }

        void PrintObject(int x, int y, string icon)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(icon);
        }

        ConsoleKey GetKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;

            return key;
        }

        Direction DecideDirection(ConsoleKey key)
        {
            Direction movingDirection = Direction.NONE;

            if (key == ConsoleKey.A)
            {
                movingDirection = Direction.LEFT;
            }
            else if (key == ConsoleKey.D)
            {
                movingDirection = Direction.RIGHT;
            }
            else if (key == ConsoleKey.W)
            {
                movingDirection = Direction.UP;
            }
            else if (key == ConsoleKey.S)
            {
                movingDirection = Direction.DOWN;
            }

            return movingDirection;
        }

        void MovePlayer(Direction movingDirection)
        {
            int playerNextX = player.X, playerNextY = player.Y;

            switch (movingDirection)
            {
                case Direction.LEFT:
                    playerNextX = player.X - 1;
                    break;
                case Direction.RIGHT:
                    playerNextX = player.X + 1;
                    break;
                case Direction.UP:
                    playerNextY = player.Y - 1;
                    break;
                case Direction.DOWN:
                    playerNextY = player.Y + 1;
                    break;
            }

            PositionChecker isPositionOccupied = (x, y) => IsObjectAtPosition(x, y, walls) || IsObjectAtPosition(x, y, boxes);

            // 플레이어가 유효한 위치인지 확인
            if (IsPositionValid(playerNextX, playerNextY) && !IsObjectAtPosition(playerNextX, playerNextY, walls))
            {
                if (IsObjectAtPosition(playerNextX, playerNextY, boxes))
                {
                    MoveBox(playerNextX, playerNextY, movingDirection, isPositionOccupied);
                }
                if (!IsObjectAtPosition(playerNextX, playerNextY, boxes))
                {
                    player.X = playerNextX;
                    player.Y = playerNextY;
                }
            }
        }

        void MoveBox(int playerNextX, int playerNextY, Direction movingDirection, PositionChecker isPositionOccupied)
        {
            int boxNextX = playerNextX, boxNextY = playerNextY;

            switch (movingDirection)
            {
                case Direction.LEFT:
                    boxNextX -= 1;
                    break;
                case Direction.RIGHT:
                    boxNextX += 1;
                    break;
                case Direction.UP:
                    boxNextY -= 1;
                    break;
                case Direction.DOWN:
                    boxNextY += 1;
                    break;
            }

            // 박스가 유효한 위치인지 확인
            foreach (var box in boxes)
            {
                if (box.X == playerNextX && box.Y == playerNextY)
                {
                    if (IsPositionValid(boxNextX, boxNextY) && !isPositionOccupied(boxNextX, boxNextY))
                    {
                        box.X = boxNextX;
                        box.Y = boxNextY;
                    }
                }
            }
        }

        bool IsPositionValid(int x, int y)
        {
            return x >= MIN_X && x <= MAX_X && y >= MIN_Y && y <= MAX_Y;
        }

        bool IsObjectAtPosition(int objectX, int objectY, List<Box> boxes)
        {
            foreach (var box in boxes)
            {
                if (objectX == box.X && objectY == box.Y)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsObjectAtPosition(int objectX, int objectY, List<Wall> walls)
        {
            foreach (var wall in walls)
            {
                if (objectX == wall.X && objectY == wall.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AreAllBoxesOnGoals()
        {
            foreach (var box in boxes)
            {
                bool boxOnGoal = false;
                foreach (var goal in goals)
                {
                    if (box.X == goal.X && box.Y == goal.Y)
                    {
                        boxOnGoal = true;
                        break;
                    }
                }
                if (!boxOnGoal)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
