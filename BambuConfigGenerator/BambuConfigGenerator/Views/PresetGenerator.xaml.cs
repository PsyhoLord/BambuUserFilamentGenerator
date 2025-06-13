using BambuConfigGenerator.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for PresetGenerator.xaml
    /// </summary>
    public partial class PresetGenerator : MvxWpfView<PresetGeneratorViewModel>
    {
        public PresetGenerator()
        {
            InitializeComponent();
        }
    }
}
