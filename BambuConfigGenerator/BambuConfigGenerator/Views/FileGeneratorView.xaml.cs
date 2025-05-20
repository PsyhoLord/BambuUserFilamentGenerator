using MvvmCross.Platforms.Wpf.Views;
using BambuConfigGenerator.Core.ViewModels;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for FileGeneratorView.xaml
    /// </summary>
    public partial class FileGeneratorView : MvxWpfView<FileGeneratorViewModel>
    {
        public FileGeneratorView()
        {
            InitializeComponent();
        }
    }
}
