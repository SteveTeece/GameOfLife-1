using System;
using System.Windows.Controls;

namespace GameOfLife
{
    public class GameBoard
    {
        private readonly MainWindow _window;
        public Cell[,] Cells;

        public GameBoard(MainWindow window, int width, int height)
        {
            _window = window;
            _window.SetSize(width, height);
            Cells = new Cell[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = new Cell();
                    cell.Reposition(x, y);
                    Cells[x, y] = cell;
                    _window.myGrid.Children.Add(cell.VisualBox);
                }
            }
        }

        public void Prepare()
        {
            int w = Cells.GetLength(0);
            int h = Cells.GetLength(1);
            int wo = 0;
            int ho = 0;
            int wd = 0;
            int hd = 0;
            // check top row - increase height
            for (int x = 0; x < w; x++)
            {
                if (Cells[x, 1].WasAlive)
                {
                    ho = CalculateDelta(h);
                    hd += ho;
                    break;
                }
            }
            // check bottom row - increase height
            for (int x = 0; x < w; x++)
            {
                if (Cells[x, h - 2].WasAlive)
                {
                    hd += CalculateDelta(h);
                    break;
                }
            }
            // check left column - increase width
            for (int y = 0; y < h; y++)
            {
                if (Cells[1, y].WasAlive)
                {
                    wo = CalculateDelta(w);
                    wd += wo;
                    break;
                }
            }
            // check right column - increase width
            for (int y = 0; y < h; y++)
            {
                if (Cells[w - 2, y].WasAlive)
                {
                    wd += CalculateDelta(w);
                    break;
                }
            }

            if (wd != 0 || hd != 0)
            {
                Resize(w + wd, wo, h + hd, ho);
            }
        }

        private int CalculateDelta(int length)
        {
            int delta = Math.Max(1, (int)Math.Ceiling(0.2* length));
            return delta;
        }

        private void Resize(int newWidth, int widthOffset, int newHeight, int heightOffset)
        {
            _window.SetSize(newWidth, newHeight);

            int w = Cells.GetLength(0);
            int h = Cells.GetLength(1);
            var newCells = new Cell[newWidth,newHeight];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var cell = Cells[x, y];
                    newCells[x + widthOffset, y + heightOffset] = cell;
                    cell.Reposition(x + widthOffset, y + heightOffset);
                }
            }

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    if (newCells[x, y] == null)
                    {
                        var cell = new Cell();
                        cell.Reposition(x, y);
                        newCells[x, y] = cell;
                        _window.myGrid.Children.Add(cell.VisualBox);
                    }
                }
            }

            Cells = newCells;
        }
    }
}