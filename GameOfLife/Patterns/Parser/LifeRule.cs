namespace GameOfLife
{
    public class LifeRule
    {
        public static readonly LifeRule Normal = new LifeRule
            {
                SurvivalCount = new[] { 2, 3 },
                BirthCount = new[] { 3 }
            };

        /// <summary>
        /// The numbers of alive neighbours needed to stay alive.
        /// </summary>
        public int[] SurvivalCount { get; set; }

        /// <summary>
        /// The numbers of alive neighbours needed to be born.
        /// </summary>
        public int[] BirthCount { get; set; }
    }
}