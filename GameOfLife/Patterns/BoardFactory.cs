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

        public static GameBoard CreateBoard(BoardWindow window, string name)
        {
            if (stringBoards.ContainsKey(name))
            {
                return CreateBoard(window, stringBoards[name]);
            }
            return CreateBoard(window, intBoards[name]);
        }

        public static GameBoard CreateBoard(BoardWindow window, string[] state)
        {
            int w = state[0].Length;
            int h = state.Length;
            var board = new GameBoard(window, w + 4, h + 4);
            Cell[,] cells = board.Cells;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool alive = state[y][x] == 'x';
                    cells[x + 2, y + 2].Update(alive);
                }
            }

            return board;
        }

        public static GameBoard CreateBoard(BoardWindow window, int[,] state)
        {
            int w = state.GetLength(1);
            int h = state.GetLength(0);
            var board = new GameBoard(window, w + 4, h + 4);
            Cell[,] cells = board.Cells;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool alive = state[y, x] == 1;
                    cells[x + 2, y + 2].Update(alive);
                }
            }

            return board;
        }
    }
}