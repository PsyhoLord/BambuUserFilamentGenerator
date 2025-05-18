using BambuConfigGenerator.Core.Services;
using BambuConfigGenerator.Services;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Localization;
using MvvmCross.Platforms.Wpf.Views;

namespace BambuConfigGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
        }
    }
}
