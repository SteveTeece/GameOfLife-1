using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameOfLife
{
    public class Game
    {
        private readonly GameBoard _board;
        private readonly TextBlock _generationDisplay;
        private DispatcherTimer _timer;
        private int _generation;

        private List<Cell> _alives = new List<Cell>();

        public Game(GameBoard board, TextBlock generationDisplay)
        {
            _board = board;
            _generationDisplay = generationDisplay;
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

            DateTime before = DateTime.UtcNow;
            _board.Prepare();
            Update();
            PostUpdate();
            TimeSpan used = DateTime.UtcNow - before;
            Debug.WriteLine(used.TotalSeconds);
        }

        public void Update()
        {
            Cell[,] cells = _board.Cells;
            var maybeBorn = new List<Cell>(_alives.Count);
            foreach (Cell cell in _alives)
            {
                AddNeighbourAlive(maybeBorn, cells, cell.X, cell.Y);
            }

            var newAlives = new List<Cell>(_alives.Count);
            foreach (Cell cell in _alives)
            {
                if (cell.NeighboursAlive == 2 || cell.NeighboursAlive == 3)
                {
                    newAlives.Add(cell);
                }
                else
                {
                    cell.Update(false);
                }
            }
            foreach (Cell cell in maybeBorn)
            {
                if (cell.NeighboursAlive == 3)
                {
                    newAlives.Add(cell);
                    cell.Update(true);
                }
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
            Cell c = cells[x, y];
            if (c == null)
            {
                c = _board.AddCell(x, y);
            }
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