using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Beadando3.Views
{
    public partial class MainView : UserControl
    {
        private GameCanvas? _gameCanvas;


        public MainView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            _gameCanvas = this.FindControl<GameCanvas>("GameCanvasControl"); // azert kell mert legyen mar meg az ablak csak utana tudja megtalalni benne a canvast
        } // megkeressuk a game canvas nevu elemet es elmentjuk

        public GameCanvas? GetGameCanvas() => _gameCanvas; //getter az appnak
    }
}