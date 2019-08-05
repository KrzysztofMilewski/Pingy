using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Ping.Utilities
{
    public class ApplicationShutdownBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            var button = AssociatedObject;
            button.Click += CloseApplication;
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
