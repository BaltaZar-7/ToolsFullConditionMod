#nullable disable

using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;

namespace ToolsFullConditionMod
{
    [HarmonyPatch(typeof(GearItem), "ManualStart")]
    public static class Patch_GearItem_ManualStart
    {
        private static readonly string[] FullHPItems = new string[]
        {
            "GEAR_WoodMatches",
            "GEAR_PackMatches",
            "GEAR_LampFuelFull",
            "GEAR_LampFuel",
            "GEAR_JerrycanRusty",
            "GEAR_MagnifyingLens",
            "GEAR_BottleHydrogenPeroxide",
            "GEAR_RevolverAmmoBox",
            "GEAR_RifleAmmoBox"
        };

        static void Postfix(GearItem __instance)
        {
            if (__instance == null) return;
            if (FullHPItems.Contains(__instance.name))
            {
                __instance.SetNormalizedHP(1f, true);
#if DEBUG
                MelonLogger.Msg($"[ContainerItemFullHP] {__instance.name} HP set to 100%");
#endif
            }
        }
    }
}
