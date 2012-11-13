using System;
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

        public Game(GameBoard board, TextBlock generationDisplay)
        {
            _board = board;
            _generationDisplay = generationDisplay;
        }

        public void Start()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += FirstTick;
            _timer.Interval = TimeSpan.FromMilliseconds(2000);
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

            _board.Prepare();
            Update();
            PostUpdate();
        }

        public void Update()
        {
            Cell[,] cells = _board.Cells;
            int w = cells.GetLength(0);
            int h = cells.GetLength(1);

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (cells[x, y].WasAlive)
                    {
                        UpdateNeighborCount(cells, x, y);
                    }
                }
            }

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    Cell cell = cells[x, y];
                    bool alive = cell.NeighboursAlive == 3 || (cell.WasAlive && cell.NeighboursAlive == 2);
                    cell.Update(alive);
                }
            }
        }

        private void UpdateNeighborCount(Cell[,] cells, int x, int y)
        {
            // above
            cells[x - 1, y - 1].NeighboursAlive += 1;
            cells[x + 0, y - 1].NeighboursAlive += 1;
            cells[x + 1, y - 1].NeighboursAlive += 1;
            // sides
            cells[x - 1, y + 0].NeighboursAlive += 1;
            cells[x + 1, y + 0].NeighboursAlive += 1;
            // below
            cells[x - 1, y + 1].NeighboursAlive += 1;
            cells[x + 0, y + 1].NeighboursAlive += 1;
            cells[x + 1, y + 1].NeighboursAlive += 1;
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
                    cells[x, y].PostUpdate();
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