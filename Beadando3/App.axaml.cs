using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Beadando3.ViewModels;
using Beadando3.Views;
using System;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Beadando3;

public partial class App : Application
{
    private LabirintusViewModel? _viewModel;
    private MainView? _mainView;
    private GameCanvas? _gameCanvas;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _viewModel = new LabirintusViewModel();
        _mainView = new MainView();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel,
                Content = _mainView
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = _mainView;
            _mainView.DataContext = _viewModel;
        }

        ViewtoViewModel();

        base.OnFrameworkInitializationCompleted();
    }

    private void ViewtoViewModel()
    {
        if (_viewModel == null || _mainView == null) return;

        // Message Box
        _viewModel.GameOverRequested += (s, msg) =>
        {
            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (desktop?.MainWindow != null)
            {
                MessageBoxManager
                    .GetMessageBoxStandard("Game Over", msg, ButtonEnum.Ok)
                    .ShowWindowDialogAsync(desktop.MainWindow);
            }
        };

        _viewModel.RenderRequested += (s, e) =>
        {
            UpdateCanvasFromViewModel();
            _gameCanvas?.RequestRender();
        };

        _viewModel.FocusRequested += (s, e) => _gameCanvas?.RequestFocus();

        _mainView.Loaded += (s, e) =>
        {
            _gameCanvas = _mainView.GetGameCanvas();

            if (_gameCanvas != null)
            {
                UpdateCanvasFromViewModel();
                _gameCanvas.KeyPressed += (sender, key) => _viewModel.HandleKeyPress(key);
            }
        };
    }

    private void UpdateCanvasFromViewModel()
    {
        if (_gameCanvas == null || _viewModel == null) return;

        _gameCanvas.HasMotor = _viewModel.HasMotor;
        _gameCanvas.Rows = _viewModel.Rows;
        _gameCanvas.Cols = _viewModel.Cols;
        _gameCanvas.VisibleCells = _viewModel.VisibleCells;
        _gameCanvas.PlayerPosition = _viewModel.PlayerPosition;
        _gameCanvas.GetCellTypeFunc = _viewModel.GetCellType;
    }
}