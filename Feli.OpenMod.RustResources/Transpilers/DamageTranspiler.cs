using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Feli.OpenMod.RustResources.Transpilers
{
    [HarmonyPatch(typeof(ResourceManager))]
    public static class DamageTranspiler
    {
        [HarmonyPatch("damage"), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> damage(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            object instigator = null;

            for (int i = 0; i < codes.Count; i++)
            {
                var instruction = codes[i];

                if (instruction.opcode == OpCodes.Ldarg_S && instruction.operand.ToString() == "7")
                    instigator = instruction.operand;

                if (instruction.opcode == OpCodes.Call && instruction.Calls(typeof(ItemManager).GetMethod("dropItem")))
                {
                    codes.Insert(i - 1, new CodeInstruction(OpCodes.Ldarg_S, instigator));
                    instruction.operand = AccessTools.Method(typeof(DamageTranspiler), nameof(dropItem));
                    i++;
                }
            }

            return codes.AsEnumerable();
        }

        public static void dropItem(Item item, Vector3 point, bool playEffect, bool isDropped, CSteamID steamId, bool wideSpread)
        {
            if(steamId != CSteamID.Nil)
            {
                var player = PlayerTool.getPlayer(steamId);

                if(!player.inventory.tryAddItemAuto(item, true, true, true, playEffect))
                {
                    ItemManager.dropItem(item, point, playEffect, isDropped, wideSpread);
                }
            }
            else
            {
                ItemManager.dropItem(item, point, playEffect, isDropped, wideSpread);
            }
        }
    }
}
