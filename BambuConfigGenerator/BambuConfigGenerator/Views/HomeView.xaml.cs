using MvvmCross.Platforms.Wpf.Views;
using BambuConfigGenerator.Core.ViewModels;

namespace BambuConfigGenerator.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : MvxWpfView<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}
