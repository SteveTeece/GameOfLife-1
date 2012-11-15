using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Patterns.Parser
{
    public class LifePatternBlock
    {
        public LifePatternBlock()
        {
            Lines = new List<List<bool>>();
        }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }

        public List<List<bool>> Lines { get; set; }

        public int Width
        {
            get { return Lines.Max(line => line.Count); }
        }

        public int Height
        {
            get { return Lines.Count; }
        }
    }
}