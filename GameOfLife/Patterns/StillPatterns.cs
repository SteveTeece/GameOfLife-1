namespace GameOfLife.Patterns
{
    public static class StillPatterns
    {
        public static readonly int[,] Block =
            new[,]
                {
                    {1, 1},
                    {1, 1},
                };

        public static readonly int[,] Beehive =
            new[,]
                {
                    {0, 1, 1, 0},
                    {1, 0, 0, 1},
                    {0, 1, 1, 0},
                };

        public static readonly int[,] Loaf =
            new[,]
                {
                    {0, 1, 1, 0},
                    {1, 0, 0, 1},
                    {0, 1, 0, 1},
                    {0, 0, 1, 0},
                };

        public static readonly int[,] Boat =
            new[,]
                {
                    {1, 1, 0},
                    {1, 0, 1},
                    {0, 1, 0},
                };
    }
}