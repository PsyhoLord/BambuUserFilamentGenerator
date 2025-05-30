﻿using BambuConfigGenerator.Core.Services.PlatformSpecific;
using BambuConfigGenerator.Services;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.IoC;
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

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            Mvx.IoCProvider?.RegisterSingleton<IFileFolderPickerService>(new FileFolderPickerService());
            // Corrected the registration to match the expected types
            base.InitializeLastChance(iocProvider);
        }

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
