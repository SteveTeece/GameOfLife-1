using System;
using System.Windows.Controls;

namespace GameOfLife
{
    public class GameBoard
    {
        private readonly BoardWindow _window;
        public Cell[,] Cells;

        public GameBoard(BoardWindow window, int width, int height)
        {
            _window = window;
            _window.SetSize(width, height);
            Cells = new Cell[width,height];
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
                Cell cell = Cells[x, 1];
                if (cell != null && cell.WasAlive)
                {
                    ho = CalculateDelta(h);
                    hd += ho;
                    break;
                }
            }
            // check bottom row - increase height
            for (int x = 0; x < w; x++)
            {
                Cell cell = Cells[x, h - 2];
                if (cell != null && cell.WasAlive)
                {
                    hd += CalculateDelta(h);
                    break;
                }
            }
            // check left column - increase width
            for (int y = 0; y < h; y++)
            {
                Cell cell = Cells[1, y];
                if (cell != null && cell.WasAlive)
                {
                    wo = CalculateDelta(w);
                    wd += wo;
                    break;
                }
            }
            // check right column - increase width
            for (int y = 0; y < h; y++)
            {
                Cell cell = Cells[w - 2, y];
                if (cell != null && cell.WasAlive)
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
            _window.myGrid.Children.Clear();
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var cell = Cells[x, y];
                    if (cell != null)
                    {
                        if (cell.WasAlive)
                        {
                            newCells[x + widthOffset, y + heightOffset] = cell;
                            cell.Reposition(x + widthOffset, y + heightOffset);
                            _window.myGrid.Children.Add(cell.VisualBox);
                        }
                    }
                }
            }

            Cells = newCells;
        }

        public Cell AddCell(int x, int y)
        {
            var cell = new Cell();
            Cells[x, y] = cell;
            cell.Reposition(x, y);
            _window.myGrid.Children.Add(cell.VisualBox);
            return cell;
        }
    }
}