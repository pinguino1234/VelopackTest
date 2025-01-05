using Avalonia.Controls;

namespace VelopackTest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txtVersion.Text = typeof(Program).Assembly.GetName().Version?.ToString();
        }
    }
}