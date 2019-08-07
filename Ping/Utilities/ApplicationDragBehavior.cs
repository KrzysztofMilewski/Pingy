using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ping.Utilities
{
    public class ApplicationDragBehavior : Behavior<Card>
    {
        private bool _dragging = false;
        private Window _mainWindow;
        private Point _startingPoint;

        protected override void OnAttached()
        {
            var card = AssociatedObject;

            card.MouseDown += Card_MouseDown;
            card.MouseUp += Card_MouseUp;
            card.MouseMove += Card_MouseMove;
            card.MouseLeave += Card_MouseLeave;

            _mainWindow = Application.Current.MainWindow;
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                _dragging = true;

                var shift = e.GetPosition(_mainWindow) - _startingPoint;

                _mainWindow.Left += shift.X;
                _mainWindow.Top += shift.Y;
            }
        }

        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                var shift = e.GetPosition(_mainWindow) - _startingPoint;

                _mainWindow.Left += shift.X;
                _mainWindow.Top += shift.Y;
            }
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _dragging = true;
                _startingPoint = e.GetPosition(_mainWindow);
            }
        }

        private void Card_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragging)
                _dragging = false;
        }

    }
}
