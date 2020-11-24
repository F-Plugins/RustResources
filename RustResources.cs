using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using OpenMod.API.Ioc;
using RustResources.Patchs;
using SDG.Unturned;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;
using OpenMod.Core.Helpers;
using System.Collections.Generic;

[assembly: PluginMetadata("F.RustResources", DisplayName = "RustResources")]
namespace RustResources
{
    public class RustResources : OpenModUnturnedPlugin
    {
        private readonly IConfiguration m_Configuration;
        private readonly IUserManager userManager;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ILogger<RustResources> m_Logger;
        public static List<ItemData> itemDatas = new List<ItemData>();

        public RustResources(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<RustResources> logger,
            IUserManager userManager2,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            userManager = userManager2;
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            m_Logger.LogInformation("Rust Resources Loaded");


            Damage_Patch.OnKillResource += Damage_Patch_OnKillResource;
        }

        private void Damage_Patch_OnKillResource(UnityEngine.Transform resource, UnityEngine.Vector3 direction, float damage, float times, float drop, ref SDG.Unturned.EPlayerKill kill, ref uint xp, Steamworks.CSteamID instigatorSteamID = default, SDG.Unturned.EDamageOrigin damageOrigin = SDG.Unturned.EDamageOrigin.Unknown, bool trackKill = true)
        {
            AsyncHelper.RunSync(async () =>
            {
                var pl = await userManager.FindUserAsync(KnownActorTypes.Player, $"{instigatorSteamID}", UserSearchMode.FindById);
                var player = (UnturnedUser)pl;

                List<RegionCoordinate> regionCoordinates = new List<RegionCoordinate>();
                regionCoordinates.Add(new RegionCoordinate(player.Player.Player.movement.region_x, player.Player.Player.movement.region_y));
                List<InteractableItem> interactableItems = new List<InteractableItem>();
                ItemManager.getItemsInRadius(resource.position, 400f, regionCoordinates, interactableItems);
                string items = m_Configuration.GetSection("plugin_configuration:resourcesIds").Get<string>();
                string[] itemsid = items.Split(',');
                foreach (var item in itemDatas)
                {
                    foreach(var ids in itemsid)
                    {
                        if (item.item.id == Convert.ToUInt16(ids))
                        {
                            ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
                            if (player.Player.Player.inventory.tryFindSpace(itemAsset.size_x, itemAsset.size_y, out byte page, out byte x, out byte y, out byte rot))
                                ItemManager.instance.askTakeItem(instigatorSteamID, player.Player.Player.movement.region_x, player.Player.Player.movement.region_y, item.instanceID, x, y, rot, page);
                        }
                    }
                }
            });
        }

        protected override async UniTask OnUnloadAsync()
        {
            m_Logger.LogInformation("Rust Resources Unloaded");
        }
    }
}
