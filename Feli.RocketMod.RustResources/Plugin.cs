using HarmonyLib;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Feli.RocketMod.RustResources
{
    public class Plugin : RocketPlugin
    {
        private Harmony _harmony;
        
        protected override void Load()
        {
            Logger.Log("Plugin made with love and quality in mind by FPlugins");
            Logger.Log("Do you want more cool plugins or need help? Join FPlugins discord ! https://discord.gg/4FF2548"); 
            
            _harmony = new Harmony("com.fplugins.plugins.rustresources");
            _harmony.PatchAll();
        }

        protected override void Unload()
        {
            _harmony.UnpatchAll();
        }
    }
}