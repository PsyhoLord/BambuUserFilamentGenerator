using BambuConfigGenerator.Core.Services;
using BambuConfigGenerator.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.IoC;
using BambuConfigGenerator.Core.Services.Interfaces;

namespace BambuConfigGenerator.Core;

public class App : MvxApplication
{
    /// <summary>
    /// Breaking change in v6: This method is called on a background thread. Use
    /// Startup for any UI bound actions
    /// </summary>
    public override void Initialize()
    {
        CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

        // Mvx.IoCProvider?.RegisterSingleton<IMvxTextProvider>(new TextProviderBuilder().TextProvider);
        Mvx.IoCProvider?.RegisterSingleton<IIOService>(new IOService());
        Mvx.IoCProvider?.RegisterSingleton<ITemplateFolderAnalyserService>(new TemplateFolderAnalyserService());
        Mvx.IoCProvider?.RegisterType<IFilamentProfileFileGeneratorService, FilamentProfileFileGeneratorService>();
        Mvx.IoCProvider?.RegisterType<IUserSettingsService, UserSettingsService>();

        RegisterAppStart<HomeViewModel>();
    }

    /// <summary>
    /// Do any UI bound startup actions here
    /// </summary>
    public override Task Startup()
    {
        return base.Startup();
    }

    /// <summary>
    /// If the application is restarted (eg primary activity on Android
    /// can be restarted) this method will be called before Startup
    /// is called again
    /// </summary>
    public override void Reset()
    {
        base.Reset();
    }
}