namespace GameOfLife.Patterns
{
    public static class OscillatorsPatterns
    {
        public static readonly string[] Blinker =
            new[]
                {
                    " x ",
                    " x ",
                    " x ",
                };

        public static readonly int[,] Toad =
            new[,]
                {
                    {0, 0, 1, 0},
                    {1, 0, 0, 1},
                    {1, 0, 0, 1},
                    {0, 1, 0, 0},
                };

        public static readonly int[,] Beacon =
            new[,]
                {
                    {1, 1, 0, 0},
                    {1, 1, 0, 0},
                    {0, 0, 1, 1},
                    {0, 0, 1, 1},
                };

        public static readonly string[] Pulsar =
        new[]
                {
                    "    x     x    ",
                    "    x     x    ",
                    "    xx   xx    ",
                    "               ",
                    "xxx  xx xx  xxx",
                    "  x x x x x x  ",
                    "    xx   xx    ",
                    "               ",
                    "    xx   xx    ",
                    "  x x x x x x  ",
                    "xxx  xx xx  xxx",
                    "               ",
                    "    xx   xx    ",
                    "    x     x    ",
                    "    x     x    ",
                };

    }
}