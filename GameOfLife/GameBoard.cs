using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameOfLife
{
    public class GameBoard
    {
        private readonly BoardWindow _window;
        private WriteableBitmap _bitmap;
        private int[] _whiteBitmap;
        public Cell[,] Cells;

        public GameBoard(BoardWindow window, int width, int height)
        {
            _window = window;
            Cells = new Cell[width, height];
            SetDrawingSize(width, height);
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
            int w = Cells.GetLength(0);
            int h = Cells.GetLength(1);
            var newCells = new Cell[newWidth,newHeight];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var cell = Cells[x, y];
                    if (cell != null && cell.WasAlive)
                    {
                        newCells[x + widthOffset, y + heightOffset] = cell;
                        cell.Reposition(x + widthOffset, y + heightOffset);
                    }
                }
            }

            Cells = newCells;
            SetDrawingSize(newWidth, newHeight);
        }

        private void SetDrawingSize(int width, int height)
        {
            _bitmap = _window.SetSize(width, height);
            _whiteBitmap = new int[width * height];
            for (int i = 0; i < _whiteBitmap.Length; i++)
            {
                _whiteBitmap[i] = 0x00ffffff;
            }
        }

        public Cell AddCell(int x, int y)
        {
            var cell = new Cell();
            Cells[x, y] = cell;
            cell.Reposition(x, y);
            return cell;
        }

        public void Draw(List<Cell> alives)
        {
            int width = Cells.GetLength(0);
            int height = Cells.GetLength(1);

            // Reserve the back buffer for updates
            WriteableBitmap bitmap = _bitmap;
            bitmap.Lock();

            // Clear to white
            var rect = new Int32Rect(0, 0, width, height);
            bitmap.WritePixels(rect, _whiteBitmap, bitmap.BackBufferStride, 0);

            unsafe
            {
                // Get a pointer to the back buffer
                int pBackBuffer = (int)bitmap.BackBuffer;
                foreach (Cell cell in alives)
                {
                    // Find the address of the pixel to draw
                    int p = pBackBuffer + (cell.Y * bitmap.BackBufferStride);
                    p += cell.X * 4;
                    *((int*)p) = 0;
                }
            }

            // Specify the area of the bitmap that changed
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));

            // Release the back buffer and make it available for display
            bitmap.Unlock();
        }
    }
}