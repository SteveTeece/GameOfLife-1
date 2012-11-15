using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GameOfLife.Patterns;
using GameOfLife.Patterns.Parser;

using Microsoft.Win32;

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

            Game game = BoardFactory.CreateGame(window, name);
            game.Start();

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
            var game = BoardFactory.CreateGame(window, b);
            game.Start();

            window.Show();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = ".LIF",
                    Filter = "Game of Life Patterns (.LIF)|*.LIF",
                    Multiselect = true
                };

            var result = openFileDialog.ShowDialog(this);
            if (result != true) return;

            Parallel.ForEach(openFileDialog.FileNames, path =>
                {
                    var lifePattern = LifeParser.ParseFile(path);

                    var name = Path.GetFileNameWithoutExtension(path);

                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var window = new BoardWindow(name);
                            BoardFactory.CreateGame(window, lifePattern).Start();

                            window.Show();
                        }));
                });
        }
    }
}
