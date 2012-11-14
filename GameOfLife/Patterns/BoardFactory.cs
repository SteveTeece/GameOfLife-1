using System.Collections.Generic;

namespace GameOfLife.Patterns
{
    public static class BoardFactory
    {
        private static Dictionary<string, string[]> stringBoards =
            new Dictionary<string, string[]>
                {
                    {"Blinker", OscillatorsPatterns.Blinker},
                    {"Pulsar", OscillatorsPatterns.Pulsar},
                    {"Glider", SpaceshipsPatterns.Glider},
                    {"LWSS", SpaceshipsPatterns.Lwss},
                    {"R-pentomino", MethuselahsPatterns.Rpentomino},
                    {"Diehard", MethuselahsPatterns.Diehard},
                    {"Acorn", MethuselahsPatterns.Acorn},
                    {"GosperGliderGun", InfinitePatterns.GosperGliderGun},
                    {"Inf2", InfinitePatterns.Inf2},
                    {"Inf3", InfinitePatterns.Inf3},
                    {"Inf4", InfinitePatterns.Inf4},
                };

        private static Dictionary<string, int[,]> intBoards =
            new Dictionary<string, int[,]>
                {
                    {"Block", StillPatterns.Block},
                    {"Beehive", StillPatterns.Beehive},
                    {"Loaf", StillPatterns.Loaf},
                    {"Boat", StillPatterns.Boat},
                    {"Toad", OscillatorsPatterns.Toad},
                    {"Beacon", OscillatorsPatterns.Beacon},
                };

        public static Game CreateGame(BoardWindow window, string name)
        {
            Game game = stringBoards.ContainsKey(name)
                            ? CreateGame(window, stringBoards[name])
                            : CreateGame(window, intBoards[name]);
            return game;
        }

        public static Game CreateGame(BoardWindow window, string[] state)
        {
            int w = state[0].Length;
            int h = state.Length;
            var board = new GameBoard(window, w + 4, h + 4);
            var game = new Game(board, window.Generation);
            window.Game = game;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool alive = state[y][x] == 'x';
                    if (alive)
                    {
                        game.InitAlive(x + 2, y + 2);
                    }
                }
            }

            return game;
        }

        public static Game CreateGame(BoardWindow window, int[,] state)
        {
            int w = state.GetLength(1);
            int h = state.GetLength(0);
            var board = new GameBoard(window, w + 4, h + 4);
            var game = new Game(board, window.Generation);
            window.Game = game;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool alive = state[y, x] == 1;
                    if (alive)
                    {
                        game.InitAlive(x + 2, y + 2);
                    }
                }
            }

            return game;
        }
    }
}