using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    public class Cell
    {
        private static readonly Brush _alive = new SolidColorBrush(Colors.Black);
        private static readonly Brush _none = new SolidColorBrush(Colors.White);

        public readonly Border VisualBox = new Border();
        private bool _isAlive;
        public bool WasAlive;
        public int NeighboursAlive;

        public void Update(bool alive)
        {
            if (alive != _isAlive)
            {
                _isAlive = alive;
                VisualBox.Background = alive ? _alive : _none;
            }
        }

        public void PostUpdate()
        {
            WasAlive = _isAlive;
            NeighboursAlive = 0;
        }

        public void Reposition(int x, int y)
        {
            Grid.SetColumn(VisualBox, x);
            Grid.SetRow(VisualBox, y);
        }
    }
}