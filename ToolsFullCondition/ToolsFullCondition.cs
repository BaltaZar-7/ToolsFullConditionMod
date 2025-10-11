using System.Collections;
using System.Linq;
using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using UnityEngine;

namespace ToolsFullConditionMod
{
    public class ToolsFullCondition : MelonMod
    {
        private static readonly string[] FullHPItems = new string[]
        {
            "GEAR_PackMatches",
            "GEAR_WoodMatches",
            "GEAR_LampFuelFull",
            "GEAR_LampFuel",
            "GEAR_JerrycanRusty",
            "GEAR_MagnifyingLens",
            "GEAR_BottleHydrogenPeroxide",
            "GEAR_RevolverAmmoBox",
            "GEAR_RifleAmmoBox"
        };

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("ToolsFullCondition loaded.");
        }

        [HarmonyPatch(typeof(QualitySettingsManager), "ApplyCurrentQualitySettings")]
        public class SceneLoadPatch
        {
            private static void Prefix()
            {
                MelonCoroutines.Start(ToolsFullCondition.SetAllGearFullHPCoroutine());
            }
        }

        public static IEnumerator SetAllGearFullHPCoroutine()
        {
            yield return new WaitForSeconds(3f); // 3 secs delay

            // Loose spawned items
            GearItem[] allGearItems = UnityEngine.Object.FindObjectsOfType<GearItem>();
            foreach (GearItem gearItem in allGearItems)
            {
                if (FullHPItems.Contains(gearItem.name))
                {
                    SetItemFullHP(gearItem);
#if DEBUG
            MelonLogger.Msg($"[ToolsFullCondition Debug] {gearItem.name} HP set to 100%");
#endif
                }
            }

            // Container items
            Container[] allContainers = UnityEngine.Object.FindObjectsOfType<Container>();
            foreach (Container container in allContainers)
            {
                for (int i = 0; i < container.m_GearToInstantiate.Count; i++)
                {
                    GameObject gearObject = container.m_GearToInstantiate[i];
                    if (gearObject == null) continue;

                    GearItem gearItem = gearObject.GetComponent<GearItem>();
                    if (gearItem == null) continue;

                    if (FullHPItems.Contains(gearItem.name))
                    {
                        SetItemFullHP(gearItem);
#if DEBUG
                MelonLogger.Msg($"[ToolsFullCondition Debug] {gearItem.name} HP set to 100% (in container)");
#endif
                    }
                }
            }
        }

        private static void SetItemFullHP(GearItem gi)
        {
            if (gi == null) return;
            gi.SetNormalizedHP(1f, true);
        }
    }
}
