using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
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

        private bool _isFullScreen;

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

        private void OnImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFullScreen)
            {
                ExitFullScreen();
            }
            else if (e.ClickCount == 2)
            {
                EnterFullScreen();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_isFullScreen)
            {
                if (e.Key == Key.Escape)
                {
                    ExitFullScreen();
                }
            }
            else
            {
                if (e.Key == Key.F11 ||
                    (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Alt) != 0))
                {
                    EnterFullScreen();
                }
            }
        }

        private void EnterFullScreen()
        {
            _isFullScreen = true;
            Footer.Height = new GridLength(0);
            WindowStyle = WindowStyle.None;
            Topmost = true;
            WindowState = WindowState.Maximized;
        }

        private void ExitFullScreen()
        {
            _isFullScreen = false;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Topmost = false;
            WindowState = WindowState.Normal;
            Footer.Height = GridLength.Auto;
        }
    }
}