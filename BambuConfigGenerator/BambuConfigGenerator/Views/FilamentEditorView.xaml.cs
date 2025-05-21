using MvvmCross.Platforms.Wpf.Views;
using BambuConfigGenerator.Core.ViewModels;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for FilamentEditorView.xaml
    /// </summary>
    public partial class FilamentEditorView : MvxWpfView<FilamentEditorViewModel>
    {
        public FilamentEditorView()
        {
            InitializeComponent();
        }
    }
}
