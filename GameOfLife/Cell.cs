namespace GameOfLife
{
    public class Cell
    {
        private bool _isAlive;
        public bool WasAlive;
        public int NeighboursAlive;

        public void Update(bool alive)
        {
            _isAlive = alive;
        }

        public void PostUpdate()
        {
            WasAlive = _isAlive;
            NeighboursAlive = 0;
        }

        public void Reposition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}