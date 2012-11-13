using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GameOfLife.Patterns;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _name;
        public Game Game;

        public MainWindow(string name)
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

        public void SetSize(int width, int height)
        {
            Title = _name + " (" + width + ", " + height + ")";
            myGrid.ColumnDefinitions.Clear();
            myGrid.RowDefinitions.Clear();
            for (int i = 0; i < width; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            }
            for (int i = 0; i < height; i++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            }
        }
    }
}