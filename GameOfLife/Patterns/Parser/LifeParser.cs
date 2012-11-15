using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GameOfLife.Patterns.Parser
{
    public static class LifeParser
    {
        public static LifePattern ParseFile(string path)
        {
            if (!File.Exists(path)) throw new ArgumentException("Could not find file.", "path");

            var pattern = new LifePattern();

            using (var reader = new StreamReader(path))
            {
                LifePatternBlock currentPatternBlock = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');

                    switch (parts[0])
                    {
                        case "#Life":
                            pattern.Version = parts[1];
                            break;
                        case "#D":
                            if (parts.Length > 1) pattern.AddDescription(parts[1]);
                            break;
                        case "#N":
                            pattern.Rules = LifeRules.Normal;
                            break;
                        case "#R":
                            pattern.Rules = CreateRule(parts[1]);
                            break;
                        case "#P":
                            if (currentPatternBlock != null) pattern.Blocks.Add(currentPatternBlock);
                            currentPatternBlock = CreatePatternBlock(parts[1], parts[2]);
                            break;
                        default:
                            AddBlockLine(parts[0], currentPatternBlock);
                            break;
                    }
                }

                if (currentPatternBlock != null) pattern.Blocks.Add(currentPatternBlock);
            }

            NormalizeBlockLines(pattern);
            NormalizeBlockOffsets(pattern);

            return pattern;
        }

        private static void NormalizeBlockOffsets(LifePattern pattern)
        {
            var centerX = pattern.Width / 2;
            var centerY = pattern.Height / 2;

            foreach (var block in pattern.Blocks)
            {
                block.OffsetX += centerX;
                block.OffsetY += centerY;
            }
        }

        private static void NormalizeBlockLines(LifePattern pattern)
        {
            // Get the longest line and add dead cells to the ones that are too short.
            var maxLineLength = pattern.Blocks.Max(block => block.Lines.Max(line => line.Count));
            foreach (var block in pattern.Blocks)
            {
                foreach (var line in block.Lines)
                {
                    var missingCellCount = maxLineLength - line.Count;
                    for (var i = 0; i < missingCellCount; i++)
                    {
                        line.Add(false);
                    }
                }
            }
        }

        private static void AddBlockLine(string line, LifePatternBlock currentPatternBlock)
        {
            if (currentPatternBlock == null) throw new InvalidDataException("Invalid Life format.");
            currentPatternBlock.Lines.Add(ParseBlockLine(line));
        }

        private static List<bool> ParseBlockLine(string blockLine)
        {
            return blockLine.Select(character => character == '*').ToList();
        }

        private static LifePatternBlock CreatePatternBlock(string offsetX, string offsetY)
        {
            return new LifePatternBlock { OffsetX = int.Parse(offsetX), OffsetY = int.Parse(offsetY) };
        }

        private static LifeRules CreateRule(string ruleString)
        {
            var rules = ruleString.Split('/');
            return new LifeRules { SurvivalCounts = GetCounts(rules[0]), BirthCounts = GetCounts(rules[1]) };
        }

        private static List<int> GetCounts(string countString)
        {
            return countString.Select(count => int.Parse(count.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}