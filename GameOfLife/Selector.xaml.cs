using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using GameOfLife.Patterns;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : Window
    {
        public Selector()
        {
            InitializeComponent();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            var name = button.Content.ToString();
            var window = new BoardWindow(name);

            var board = BoardFactory.CreateBoard(window, name);

            var game = new Game(board, window.Generation);
            game.PostUpdate();
            game.Start();

            window.Game = game;
            window.Show();
        }

        private void StartRandom(object sender, RoutedEventArgs e)
        {
            var name = "Random";
            var window = new BoardWindow(name);

            int width = int.Parse(RandomWidth.Text, NumberFormatInfo.InvariantInfo);
            int height = int.Parse(RandomHeight.Text, NumberFormatInfo.InvariantInfo);
            double prob = double.Parse(RandomProbability.Text, NumberFormatInfo.InvariantInfo);
            int seed = int.Parse(RandomSeed.Text, NumberFormatInfo.InvariantInfo);
            int[,] b = new int[width, height];
            var rand = new Random(seed);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    b[x, y] = rand.NextDouble() < prob ? 1 : 0;
                }
            }
            var board = BoardFactory.CreateBoard(window, b);

            var game = new Game(board, window.Generation);
            game.PostUpdate();
            game.Start();

            window.Game = game;
            window.Show();
        }
    }
}
