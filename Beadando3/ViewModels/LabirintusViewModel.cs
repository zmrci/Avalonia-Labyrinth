using Avalonia.Threading;
using Beadando3.Models;
using Beadando3.Persistence;
using System;
using Avalonia.Input;
using Avalonia;

namespace Beadando3.ViewModels
{
    public class LabirintusViewModel : ViewModelBase
    {
        private Motor? _motor;
        private DispatcherTimer _timer;
        private int _elapsedTime;
        private string _pauseButtonText = "Pause Game";

        public string ElapsedTimeText => $"Elapsed time: {_elapsedTime}";

        public DelegateCommand LoadLab1Command { get; }
        public DelegateCommand LoadLab2Command { get; }
        public DelegateCommand LoadLab3Command { get; }
        public DelegateCommand PauseCommand { get; }


        public DelegateCommand MoveCommand { get; }

        public event EventHandler<string>? GameOverRequested;
        public event EventHandler? RenderRequested;
        public event EventHandler? FocusRequested;

        public LabirintusViewModel()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;

            LoadLab1Command = new DelegateCommand(() => LoadLabyrinth("Assets/labyrinth1.txt"));
            LoadLab2Command = new DelegateCommand(() => LoadLabyrinth("Assets/labyrinth2.txt"));
            LoadLab3Command = new DelegateCommand(() => LoadLabyrinth("Assets/labyrinth3.txt"));

            PauseCommand = new DelegateCommand(obj => PauseResume(), obj => Motor != null);


            MoveCommand = new DelegateCommand(param =>
            {
                if (param is string direction)
                {
                    switch (direction)
                    {
                        case "Up": HandleKeyPress(Key.Up); break;
                        case "Down": HandleKeyPress(Key.Down); break;
                        case "Left": HandleKeyPress(Key.Left); break;
                        case "Right": HandleKeyPress(Key.Right); break;
                    }
                }
            });
        }

        public Motor? Motor
        {
            get => _motor;
            set
            {
                if (_motor != null)
                {
                    _motor.GameChange -= OnGameChange;
                    _motor.GameOver -= OnGameOver;
                }

                SetProperty(ref _motor, value);

                if (_motor != null)
                {
                    _motor.GameChange += OnGameChange;
                    _motor.GameOver += OnGameOver;
                }

                OnPropertyChanged(nameof(IsGameLoaded));
                OnPropertyChanged(nameof(HasMotor));
                OnPropertyChanged(nameof(Rows));
                OnPropertyChanged(nameof(Cols));
                OnPropertyChanged(nameof(VisibleCells));
                OnPropertyChanged(nameof(PlayerPosition));
                OnPropertyChanged(nameof(IsActive));
                PauseCommand.RaiseCanExecuteChanged();
            }
        }

        public bool HasMotor => Motor != null;
        public int Rows => Motor?.labyrinth.GetLength(0) ?? 0;
        public int Cols => Motor?.labyrinth.GetLength(1) ?? 0;
        public bool[,]? VisibleCells => Motor?.VisibleCells;

        public string GetCellType(int x, int y)
        {
            if (Motor?.labyrinth == null) return "";
            return Motor.labyrinth[x, y].ToString();
        }

        public Point? PlayerPosition
        {
            get
            {
                if (Motor?.playerPosition == null) return null;
                return new Point(Motor.playerPosition.x, Motor.playerPosition.y);
            }
        }

        public bool IsActive => Motor?.isActive == true;

        public string PauseButtonText
        {
            get => _pauseButtonText;
            set => SetProperty(ref _pauseButtonText, value);
        }

        public bool IsGameLoaded => Motor != null;

        public void HandleKeyPress(Key key)
        {
            if (!IsActive) return;

            int dx = 0, dy = 0;
            bool validKey = false;

            switch (key)
            {
                case Key.Up: dy = -1; validKey = true; break;
                case Key.Down: dy = 1; validKey = true; break;
                case Key.Left: dx = -1; validKey = true; break;
                case Key.Right: dx = 1; validKey = true; break;
            }

            if (validKey)
            {
                Motor?.Move(dx, dy);
            }
        }

        private void LoadLabyrinth(string path)
        {
            try
            {
                var map = LabyrinthLoader.Load(path);
                Motor = new Motor(map);
                _elapsedTime = 0;
                PauseButtonText = "Pause Game";
                _timer.Start();
                Motor.Start();

                RenderRequested?.Invoke(this, EventArgs.Empty);
                FocusRequested?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        private void PauseResume()
        {
            if (Motor == null) return;

            if (Motor.isActive)
            {
                Motor.Pause();
                PauseButtonText = "Resume Game";
            }
            else
            {
                Motor.Resume();
                PauseButtonText = "Pause Game";
            }

            OnPropertyChanged(nameof(IsActive));
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Motor?.isActive == true)
            {
                _elapsedTime++;
                OnPropertyChanged(nameof(ElapsedTimeText));
            }
        }

        private void OnGameChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Motor));
            OnPropertyChanged(nameof(VisibleCells));
            OnPropertyChanged(nameof(PlayerPosition));
            RenderRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnGameOver(object? sender, EventArgs e)
        {
            _timer.Stop();
            GameOverRequested?.Invoke(this, $"Congratulations, you made it out! Your time: {_elapsedTime} seconds.");
            Motor = null;
            _elapsedTime = 0;
            OnPropertyChanged(nameof(ElapsedTimeText));
            PauseButtonText = "Pause Game";
            RenderRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}