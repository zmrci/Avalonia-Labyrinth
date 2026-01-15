using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Beadando3.Views
{
    public class GameCanvas : Control
    {
        public event EventHandler<Key>? KeyPressed;

        public bool HasMotor { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public bool[,]? VisibleCells { get; set; }
        public Point? PlayerPosition { get; set; }
        public Func<int, int, string>? GetCellTypeFunc { get; set; }

        public GameCanvas()
        {
            Focusable = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            KeyPressed?.Invoke(this, e.Key);
            e.Handled = true;
        }

        public void RequestRender()
        {
            InvalidateVisual();
        }

        public void RequestFocus()
        {
            Focus();
        }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);

            var width = Bounds.Width;
            var height = Bounds.Height;

            if (width <= 0 || height <= 0) return;

            if (!HasMotor)
            {
                DrawNoGameMessage(dc, width, height);
                return;
            }

            dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, width, height));

            if (Rows == 0 || Cols == 0) return;

            double cellWidth = width / Cols;
            double cellHeight = height / Rows;

            if (VisibleCells == null || PlayerPosition == null)
                return;

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    if (VisibleCells[y, x])
                    {
                        string cellValue = GetCellTypeFunc?.Invoke(y, x) ?? "";
                        IBrush brush = CellColor(cellValue);
                        var rect = new Rect(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                        dc.DrawRectangle(brush, null, rect);
                    }
                }
            }

            var playerRect = new Rect(PlayerPosition.Value.X * cellWidth, PlayerPosition.Value.Y * cellHeight, cellWidth, cellHeight);
            dc.DrawRectangle(Brushes.Red, null, playerRect);
        }

        private void DrawNoGameMessage(DrawingContext dc, double width, double height)
        {
            dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, width, height));

            var formattedText = new FormattedText(
                "Please select a labyrinth from the menu to start.",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                12,
                Brushes.White
            );

            var location = new Point((width - formattedText.Width) / 2, (height - formattedText.Height) / 2);
            dc.DrawText(formattedText, location);
        }

        private static IBrush CellColor(string? cellType)
        {
            return cellType switch
            {
                "Wall" => Brushes.Gray,
                "Exit" => Brushes.Green,
                "Road" => Brushes.White,
                _ => Brushes.White
            };
        }
    }
}