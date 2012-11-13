using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    public class Cell
    {
        private static readonly Brush _alive = new SolidColorBrush(Colors.Black);
        private static readonly Brush _dead = new SolidColorBrush(Colors.Red);
        private static readonly Brush _none = new SolidColorBrush(Colors.White);

        public readonly Border VisualBox = new Border();
        private int _sinceAlive;
        public bool WasAlive;
        public int NeighboursAlive;

        public void Update(bool alive)
        {
            if (alive)
            {
                if (_sinceAlive != 10)
                {
                    VisualBox.Background = _alive;
                }
                _sinceAlive = 10;
            }
            else
            {
                if (_sinceAlive > 0)
                {
                    if (_sinceAlive == 10)
                    {
                        VisualBox.Background = _none;// _dead;
                    }
                    else if (_sinceAlive == 1)
                    {
                        VisualBox.Background = _none;
                    }
                    _sinceAlive -= 1;
                }
            }
        }

        public void PostUpdate()
        {
            WasAlive = _sinceAlive == 10;
            NeighboursAlive = 0;
        }

        public void Reposition(int x, int y)
        {
            Grid.SetColumn(VisualBox, x);
            Grid.SetRow(VisualBox, y);
        }
    }
}