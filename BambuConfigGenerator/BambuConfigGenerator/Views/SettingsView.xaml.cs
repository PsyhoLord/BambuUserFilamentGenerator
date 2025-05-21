using BambuConfigGenerator.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MvxWpfView<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
