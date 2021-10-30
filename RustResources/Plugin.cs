using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("Feli.RustResources", 
    DisplayName = "RustResources",
    Author = "Feli",
    Description = "A plugin that simulates the resources system of rust",
    Website = "https://discord.gg/4FF2548"
)]

namespace RustResources
{
    public class Plugin : OpenModUnturnedPlugin
    {
        private readonly ILogger<Plugin> logger;

        public Plugin(
            ILogger<Plugin> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.logger = logger;
        }

        protected override UniTask OnLoadAsync()
        {
            logger.LogInformation("Plugin made with love and quality in mind by FPlugins");
            logger.LogInformation("Do you want more cool plugins or need help? Join FPlugins discord ! https://discord.gg/4FF2548");  
            return UniTask.CompletedTask;
        }
    }
}
