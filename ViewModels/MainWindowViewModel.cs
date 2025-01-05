using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Velopack;

namespace VelopackTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand CheckUpdates { get; }
        public ICommand UpdateApp { get; }

        string? _NewVersionText;
        public string? NewVersionText 
        {
            get => _NewVersionText; 
            set => this.RaiseAndSetIfChanged(ref _NewVersionText, value);
        }

        bool _Update = false;
        public bool Update
        {
            get => _Update;
            set => this.RaiseAndSetIfChanged(ref _Update, value);
        } 

        public MainWindowViewModel()
        {
            CheckUpdates = ReactiveCommand.Create(async () =>
            {
                await CheckUpdateApp();
            });

            var CanExecute = this.WhenAnyValue(x => x.Update, (u) => u is true);

            UpdateApp = ReactiveCommand.Create(async () =>
            {
                await UpdateMyApp();
            }, CanExecute);
        }

        private async Task CheckUpdateApp()
        {
            var mgr = new UpdateManager(@"C:\MyApp\Updates\");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();

            if (newVersion != null)
            {
                NewVersionText = "Nueva Version Disponible!";
                Update = true;
            }
        }

        private async Task UpdateMyApp()
        {
            var mgr = new UpdateManager(@"C:\MyApp\Updates\");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return;

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
