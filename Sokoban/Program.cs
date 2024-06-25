using System;

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
        static void Main(string[] args)
        {
            Stage stage = new Stage();
            stage.Play();
        }
    }

    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Icon { get; }

        public Player(int x, int y, string icon)
        {
            X = x;
            Y = y;
            Icon = icon;
        }
    }

    class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Icon { get; }

        public Enemy(int x, int y, string icon)
        {
            X = x;
            Y = y;
            Icon = icon;
        }
    }

    class Box
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

    class Wall
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

    class Goal
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Icon { get; }

        public Goal(int x, int y, string icon)
        {
            X = x;
            Y = y;
            Icon = icon;
        }
    }

    class Stage
    {
        Game game;

        static int currStageNumber = 1;
        const int lastStageNumber = 3;

        public Stage()
        {
            game = new Game();
        }

        public void Play()
        {
            while(currStageNumber <= lastStageNumber)
            {
                game.LoadStage(currStageNumber);
                game.Play();
                PrintStageInfo();
                MoveNextStage();

                if (IsClearLastStage())
                {
                    Console.Clear();
                    Console.Write("Game Clear");
                    break;
                }
            }
        }

        void PrintStageInfo()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Stage {currStageNumber}");
        }

        void MoveNextStage()
        {
            if (game.AreAllBoxesOnGoals())
            {
                currStageNumber += 1;
                if (currStageNumber <= lastStageNumber)
                {
                    PrintNextStageConsole();
                }
            }
        }

        void PrintNextStageConsole()
        {
            Console.Clear();
        }

        bool IsClearLastStage()
        {
            if (currStageNumber > lastStageNumber)
            {
                return true;
            }
            return false;
        }
    }

    class Game
    {
        Player player;
        List<Enemy> enemies;
        List<Box> boxes;
        List<Wall> walls;
        List<Goal> goals;

        public Game()
        {
            InitializeGame();
        }

        void InitializeGame()
        {
            // 초기 게임 오브젝트 생성 및 위치 설정
            player = new Player(2, 2, "P");
            boxes = new List<Box>
            {
                new Box(8, 4, "B"),
                new Box(9, 4, "B"),
                new Box(10, 4, "B")
            };

            walls = new List<Wall>
            {
                new Wall(13, 6, "W"),
                new Wall(14, 6, "W"),
                new Wall(15, 6, "W")
            };

            goals = new List<Goal>
            {
                new Goal(11, 9, "G"),
                new Goal(12, 9, "G"),
                new Goal(13, 9, "G")
            };
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
                DrawGameObjects();

                if (AreAllBoxesOnGoals())
                {
                    break;
                }

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
            foreach (var Box in boxes)
            {
                PrintObject(Box.X, Box.Y, Box.Icon);
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

            // 유효한 위치인지 확인
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

            // 유효한 위치인지 확인
            foreach (var box in boxes)
            {
                if(box.X == playerNextX && box.Y == playerNextY)
                {
                    if (IsPositionValid(boxNextX, boxNextY) && !isPositionOccupied(boxNextX, boxNextY))
                    {
                        box.X = boxNextX;
                        box.Y = boxNextY;
                    }
                    else
                    {
                        // 랜덤 요소
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

        public void LoadStage(int stageNumber)
        {
            if (stageNumber == 2)
            {
                player = new Player(2, 2, "P");

                boxes = new List<Box>
                {
                    new Box(6, 11, "B"),
                    new Box(7, 12, "B"),
                    new Box(8, 11, "B"),
                    new Box(8, 10, "B")
                };

                walls = new List<Wall>
                {
                    new Wall(3, 6, "W"),
                    new Wall(4, 6, "W"),
                    new Wall(5, 7, "W"),
                    new Wall(6, 8, "W"),
                };

                goals = new List<Goal>
                {
                    new Goal(13, 7, "G"),
                    new Goal(14, 5, "G"),
                    new Goal(15, 6, "G"),
                    new Goal(14, 8, "G"),
                };
            }

            else if (stageNumber == 3)
            {
                player = new Player(2, 2, "P");

                boxes = new List<Box>
                {
                    new Box(5, 3, "B"),
                    new Box(6, 3, "B"),
                    new Box(8, 4, "B"),
                    new Box(8, 5, "B"),
                    new Box(6, 4, "B")
                };

                walls = new List<Wall>
                {
                    new Wall(5, 8, "W"),
                    new Wall(6, 8, "W"),
                    new Wall(7, 9, "W"),
                    new Wall(8, 9, "W"),
                    new Wall(11, 11, "W")
                };

                goals = new List<Goal>
                {
                    new Goal(8, 13, "G"),
                    new Goal(8, 14, "G"),
                    new Goal(9, 14, "G"),
                    new Goal(10, 14, "G"),
                    new Goal(11, 13, "G")
                };
            }
        }
    }
}