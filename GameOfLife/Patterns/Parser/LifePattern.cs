using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Patterns.Parser
{
    public class LifePattern
    {
        public LifePattern()
        {
            Blocks = new List<LifePatternBlock>();
        }

        public string Version { get; set; }

        public string Description { get; set; }

        public LifeRule Rule { get; set; }

        public bool HasRule
        {
            get { return Rule != null; }
        }

        public List<LifePatternBlock> Blocks { get; set; } 

        public int Width
        {
            get { return Blocks.Sum(block => block.Width); }
        }

        public int Height
        {
            get { return Blocks.Sum(block => block.Height); }
        }

        public void AddDescription(string description)
        {
            Description = !string.IsNullOrEmpty(Description) 
                              ? string.Join(Environment.NewLine, Description, description) 
                              : description;
        }
    }
}