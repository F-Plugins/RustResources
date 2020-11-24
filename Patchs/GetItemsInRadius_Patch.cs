using HarmonyLib;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RustResources.Patchs
{
    [HarmonyPatch(typeof(ItemManager))]
    [HarmonyPatch(nameof(ItemManager.getItemsInRadius))]
    class GetItemsInRadius_Patch
    {
        [HarmonyPrefix]
        public static bool getItemsgetItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<InteractableItem> result)
        {
            for (int i = 0; i < search.Count; i++)
            {
                RegionCoordinate regionCoordinate = search[i];
                if (ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
                {
                    for (int a = 0; a < ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].items.Count; a++)
                    {
                        var itemData = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].items[a];
                        if ((itemData.point - center).sqrMagnitude < sqrRadius)
                        {
                            RustResources.itemDatas.Add(itemData);
                        }
                    }
                }
            }
            return true;
        }
    }
}
