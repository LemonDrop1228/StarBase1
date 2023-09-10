using System.Windows;
using System.Windows.Input;

namespace StarBase1
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public async Task Minimize()
        {
            Dispatcher.Invoke(() => 
            {
                WindowState = WindowState.Minimized;
            });
        }

        public async Task Maximize() 
        {
            Dispatcher.Invoke(() => 
            {
                WindowState = WindowState.Maximized;
            });
        }

        public async Task Restore() 
        {
            Dispatcher.Invoke(() => 
            {
                WindowState = WindowState.Normal;
            });
        }

        public async Task Exit() 
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }

        public async Task DoDrag(MouseButtonEventArgs mouseEventArgs) 
        {
            if(mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                Dispatcher.Invoke(() => { DragMove(); });
            }
        }

        public async Task ToggleMaximizeRestore() 
        {
            Dispatcher.Invoke(() => 
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            });
        }

        private async void Control_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => await DoDrag(e);

        private async void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) => await ToggleMaximizeRestore();
    }
}
