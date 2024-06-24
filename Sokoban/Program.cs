﻿using System;

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

    internal class Program
    {
        const string playerIcon = "P";
        static int playerX = 1, playerY = 1;

        const string boxIcon = "B";
        static int[] boxX = new int[] { 8, 9, 10 };
        static int[] boxY = new int[] { 3, 3, 3 };

        const string wallIcon = "W";
        static int[] wallX = new int[] { 13, 14, 15 };
        static int[] wallY = new int[] { 6, 6, 6 };

        const string goalIcon = "G";
        static int[] goalX = new int[] { 11, 12, 13 };
        static int[] goalY = new int[] { 8, 8, 8 };

        const int MIN_X = 0, MIN_Y = 0;
        const int MAX_X = 119, MAX_Y = 29;

        delegate bool PositionChecker(int x, int y);

        static void Main(string[] args)
        {
            InitializeConsole();

            while (true)
            {
                Console.Clear();

                DrawGameObjects();

                ConsoleKey key = GetKey();
                Direction movingDirection = DecideDirection(key);

                MovePlayer(ref playerX, ref playerY, movingDirection);

                if (AreAllBoxesOnGoals())
                {
                    Console.Clear();
                    Console.Write("Game Complete");
                    break;
                }
            }
        }

        static void InitializeConsole()
        {
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.Title = "Sokoban";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Clear();
        }

        static void DrawGameObjects()
        {
            PrintObjects(wallX, wallY, wallIcon);
            PrintObjects(goalX, goalY, goalIcon);
            PrintObjects(boxX, boxY, boxIcon);
            PrintObject(playerX, playerY, playerIcon);
        }

        static void PrintObject(int x, int y, string icon)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(icon);
        }

        static void PrintObjects(int[] x, int[] y, string icon)
        {
            int count = x.Length;
            for (int i = 0; i < count; i++)
            {
                PrintObject(x[i], y[i], icon);
            }
        }

        static ConsoleKey GetKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;

            return key;
        }

        static Direction DecideDirection(ConsoleKey key)
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

        static void MovePlayer(ref int playerCurrX, ref int playerCurrY, Direction movingDirection)
        {
            int playerNextX = playerCurrX, playerNextY = playerCurrY;

            switch (movingDirection)
            {
                case Direction.LEFT:
                    playerNextX = playerCurrX - 1;
                    break;
                case Direction.RIGHT:
                    playerNextX = playerCurrX + 1;
                    break;
                case Direction.UP:
                    playerNextY = playerCurrY - 1;
                    break;
                case Direction.DOWN:
                    playerNextY = playerCurrY + 1;
                    break;
            }

            PositionChecker isPositionOccupied = (x, y) => IsObjectAtPosition(x, y, wallX, wallY) || IsObjectAtPosition(x, y, boxX, boxY);

            if (IsPositionValid(playerNextX, playerNextY) && !IsObjectAtPosition(playerNextX, playerNextY, wallX, wallY))
            {
                if (IsObjectAtPosition(playerNextX, playerNextY, boxX, boxY))
                {
                    MoveBox(playerNextX, playerNextY, movingDirection, isPositionOccupied);
                }
                if (!IsObjectAtPosition(playerNextX, playerNextY, boxX, boxY))
                {
                    playerCurrX = playerNextX;
                    playerCurrY = playerNextY;
                }
            }
        }

        static void MoveBox(int playerNextX, int playerNextY, Direction movingDirection, PositionChecker isPositionOccupied)
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

            for (int i = 0; i < boxX.Length; i++)
            {
                if (boxX[i] == playerNextX && boxY[i] == playerNextY)
                {
                    if (IsPositionValid(boxNextX, boxNextY) && !isPositionOccupied(boxNextX, boxNextY))
                    {
                        boxX[i] = boxNextX;
                        boxY[i] = boxNextY;
                    }
                }
            }
        }

        static bool IsPositionValid(int x, int y)
        {
            return x >= MIN_X && x <= MAX_X && y >= MIN_Y && y <= MAX_Y;
        }

        static bool IsObjectAtPosition(int objectX, int objectY, int[] objectPosX, int[] objectPosY)
        {
            for (int i = 0; i < objectPosX.Length; i++)
            {
                if (objectX == objectPosX[i] && objectY == objectPosY[i])
                {
                    return true;
                }
            }
            return false;
        }

        static bool AreAllBoxesOnGoals()
        {
            for (int i = 0; i < boxX.Length; i++)
            {
                bool boxOnGoal = false;
                for (int j = 0; j < goalX.Length; j++)
                {
                    if (boxX[i] == goalX[j] && boxY[i] == goalY[j])
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