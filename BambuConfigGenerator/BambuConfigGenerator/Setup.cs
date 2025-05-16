using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;

namespace BambuConfigGenerator
{
    internal class Setup : MvxWpfSetup<Core.App>
    {
        //protected override ILoggerProvider CreateLogProvider()
        //{
        //    return new SerilogLoggerProvider();
        //}

        //protected override ILoggerFactory CreateLogFactory()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Verbose()
        //        .WriteTo.Debug()
        //        .CreateLogger();

        //    return new SerilogLoggerFactory();
        //}

        protected override ILoggerFactory? CreateLogFactory() => default!;

        protected override ILoggerProvider? CreateLogProvider() => default!;

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            //pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Messenger.Plugin>();
            //pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
        }
    }
}
