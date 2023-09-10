using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Components.Web;

namespace StarBase1.Services
{
    public interface IHostWindowService
    {
        Task Minimize();
        Task Maximize();
        Task Restore();
        Task Exit();
        Task ToggleMaximizeRestore();
    }

    public class HostWindowService : IHostWindowService
    {
        private readonly MainWindow _mainWindow;

        public HostWindowService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async Task Minimize() =>
            await _mainWindow.Dispatcher.InvokeAsync(() => 
                _mainWindow.Minimize());

        public async Task Maximize() =>
            await _mainWindow.Dispatcher.InvokeAsync(() => 
                _mainWindow.Maximize());

        public async Task Restore() =>
            await _mainWindow.Dispatcher.InvokeAsync(() => 
                _mainWindow.Restore());

        public async Task Exit() =>
            await _mainWindow.Dispatcher.InvokeAsync(() => 
                _mainWindow.Exit());

        public async Task ToggleMaximizeRestore() =>
            await _mainWindow.Dispatcher.InvokeAsync(() =>
             _mainWindow.ToggleMaximizeRestore());
    }
}
