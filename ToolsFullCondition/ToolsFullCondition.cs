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
            foreach (var gi in UnityEngine.Object.FindObjectsOfType<GearItem>())
            {
                if (FullHPItems.Contains(gi.name))
                {
                    SetItemFullHP(gi);
#if DEBUG
                    MelonLogger.Msg($"[ToolsFullCondition Debug] {gi.name} HP set to 100%");
#endif
                }
            }

            // Container items
            foreach (var container in UnityEngine.Object.FindObjectsOfType<Container>())
            {
                for (int i = 0; i < container.m_GearToInstantiate.Count; i++)
                {
                    var gearObj = container.m_GearToInstantiate[i];
                    if (gearObj == null) continue;

                    var gi = gearObj.GetComponent<GearItem>();
                    if (gi == null) continue;

                    if (FullHPItems.Contains(gi.name))
                    {
                        SetItemFullHP(gi);
#if DEBUG
                        MelonLogger.Msg($"[ToolsFullCondition Debug] {gi.name} HP set to 100% (in container)");
#endif
                    }
                }
            }
        }

        private static void SetItemFullHP(GearItem gi)
        {
            if (gi == null) return;
            gi.SetNormalizedHP(1f, true); // Full Condition
        }
    }
}
