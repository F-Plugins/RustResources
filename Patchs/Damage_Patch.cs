using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RustResources.Patchs
{
    [HarmonyPatch(typeof(ResourceManager))]
    [HarmonyPatch(nameof(ResourceManager.damage))]
    public class Damage_Patch
    {
        public delegate void KillResource(Transform resource, Vector3 direction, float damage, float times, float drop, ref EPlayerKill kill, ref uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown, bool trackKill = true);
        public static event KillResource OnKillResource;


        [HarmonyPostfix]
        public static void Damage(Transform resource, Vector3 direction, float damage, float times, float drop, ref EPlayerKill kill, ref uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown, bool trackKill = true)
        {
            if(EPlayerKill.RESOURCE == kill)
            {
                OnKillResource?.Invoke(resource, direction, damage, times, drop, ref kill, ref xp, instigatorSteamID, damageOrigin, trackKill);
            }
        }
    }
}
