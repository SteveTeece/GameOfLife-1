using System.Collections.Generic;

namespace GameOfLife.Patterns.Parser
{
    public class LifeRules
    {
        public static readonly LifeRules Normal = new LifeRules
            {
                SurvivalCounts = new List<int> { 2, 3 },
                BirthCounts = new List<int> { 3 }
            };

        /// <summary>
        /// The numbers of alive neighbours needed to stay alive.
        /// </summary>
        public List<int> SurvivalCounts { get; set; }

        /// <summary>
        /// The numbers of alive neighbours needed to be born.
        /// </summary>
        public List<int> BirthCounts { get; set; }
    }
}