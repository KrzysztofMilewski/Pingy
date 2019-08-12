using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Ping.Behaviors
{
    public class MenuCloseOnLosingFocusBehavior : Behavior<ListBox>
    {
        private ToggleButton _menuButton;

        protected override void OnAttached()
        {
            _menuButton = Application.Current.MainWindow.FindName("MenuButton") as ToggleButton;

            var listBox = AssociatedObject;
            listBox.GotMouseCapture += ListBox_GotMouseCapture;
        }

        private void ListBox_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_menuButton.IsChecked.HasValue && _menuButton.IsChecked.Value)
                _menuButton.IsChecked = false;
        }
    }
}
