using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

using GameOfLife.Patterns.Parser;

namespace GameOfLife
{
    public class Game
    {
        private readonly GameBoard _board;
        private readonly TextBlock _generationDisplay;

        private readonly LifeRules _rules;

        private DispatcherTimer _timer;
        private int _generation;

        private List<Cell> _alives = new List<Cell>();

        public Game(GameBoard board, TextBlock generationDisplay)
            : this(board, generationDisplay, LifeRules.Normal) { }

        public Game(GameBoard board, TextBlock generationDisplay, LifeRules rules)
        {
            _board = board;
            _generationDisplay = generationDisplay;
            _rules = rules;
        }

        public void InitAlive(int x, int y)
        {
            Cell cell = _board.AddCell(x, y);
            _alives.Add(cell);
            cell.Update(true);
            cell.PostUpdate();
        }

        public void Start()
        {
            _board.Draw(_alives);

            _timer = new DispatcherTimer();
            _timer.Tick += FirstTick;
            _timer.Interval = TimeSpan.FromMilliseconds(1500);
            _timer.Start();
        }

        private void FirstTick(object sender, EventArgs e)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(400);
            _timer.Tick -= FirstTick;
            _timer.Tick += Tick;

            Tick(sender, e);
        }

        private void Tick(object sender, EventArgs eventArgs)
        {
            _generation += 1;
            _generationDisplay.Text = _generation.ToString(NumberFormatInfo.InvariantInfo);

            DateTime t0 = DateTime.UtcNow;
            _board.Prepare();
            DateTime t1 = DateTime.UtcNow;
            Update();
            DateTime t4 = DateTime.UtcNow;
            PostUpdate();
            DateTime t5 = DateTime.UtcNow;
            _board.Draw(_alives);
            DateTime t6 = DateTime.UtcNow;
            Debug.WriteLine("1:{0}, 2:{1}, 3:{2}, 4:{3}, 5:{4}, 6:{5}",
                            (int)(t1 - t0).TotalMilliseconds,
                            (int)(t2 - t1).TotalMilliseconds,
                            (int)(t3 - t2).TotalMilliseconds,
                            (int)(t4 - t3).TotalMilliseconds,
                            (int)(t5 - t4).TotalMilliseconds,
                            (int)(t6 - t5).TotalMilliseconds);
        }

        private DateTime t2;
        private DateTime t3;

        public void Update()
        {
            Cell[,] cells = _board.Cells;
            var maybeBorn = new List<Cell>(_alives.Count);
            foreach (Cell cell in _alives)
            {
                AddNeighbourAlive(maybeBorn, cells, cell.X, cell.Y);
            }
            t2 = DateTime.UtcNow;

            var newAlives = new List<Cell>(_alives.Count);
            foreach (Cell cell in _alives)
            {
                if (_rules.SurvivalCounts.Contains(cell.NeighboursAlive))
                {
                    newAlives.Add(cell);
                }
                else
                {
                    cell.Update(false);
                }
            }
            t3 = DateTime.UtcNow;

            foreach (Cell cell in maybeBorn.Where(cell => _rules.BirthCounts.Contains(cell.NeighboursAlive)))
            {
                newAlives.Add(cell);
                cell.Update(true);
            }

            _alives = newAlives;
        }

        private void AddNeighbourAlive(List<Cell> maybeBorn, Cell[,] cells, int x, int y)
        {
            // above
            CountNeighbour(maybeBorn, cells, x - 1, y - 1);
            CountNeighbour(maybeBorn, cells, x + 0, y - 1);
            CountNeighbour(maybeBorn, cells, x + 1, y - 1);
            // sides
            CountNeighbour(maybeBorn, cells, x - 1, y + 0);
            CountNeighbour(maybeBorn, cells, x + 1, y + 0);
            // below
            CountNeighbour(maybeBorn, cells, x - 1, y + 1);
            CountNeighbour(maybeBorn, cells, x + 0, y + 1);
            CountNeighbour(maybeBorn, cells, x + 1, y + 1);
        }

        private void CountNeighbour(List<Cell> maybeBorn, Cell[,] cells, int x, int y)
        {
            Cell c = cells[x, y] ?? _board.AddCell(x, y);
            int n = c.NeighboursAlive += 1;
            if (!c.WasAlive && n == 3)
            {
                maybeBorn.Add(c);
            }
        }

        public void PostUpdate()
        {
            Cell[,] cells = _board.Cells;
            int w = cells.GetLength(0);
            int h = cells.GetLength(1);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Cell cell = cells[x, y];
                    if (cell != null)
                    {
                        cells[x, y].PostUpdate();
                    }
                }
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void SetTickInterval(TimeSpan interval)
        {
            _timer.Interval = interval;
            Debug.WriteLine("Interval: " + _timer.Interval.TotalSeconds);
        }
    }
}