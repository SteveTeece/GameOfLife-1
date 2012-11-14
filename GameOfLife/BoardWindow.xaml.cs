using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow
    {
        private readonly string _name;
        public Game Game;

        public BoardWindow(string name)
        {
            _name = name;
            InitializeComponent();
            Closing += Stop;
            TickInterval.ValueChanged += ChangeTickInterval;
        }

        private void ChangeTickInterval(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Game.SetTickInterval(TimeSpan.FromMilliseconds(TickInterval.Value));
        }

        private void Stop(object sender, CancelEventArgs e)
        {
            Game.Stop();
        }

        public WriteableBitmap SetSize(int width, int height)
        {
            Title = _name + " (" + width + ", " + height + ")";

            var bitmap = new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            image.Source = bitmap;
            return bitmap;
        }
    }
}