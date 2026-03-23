using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using Cpimhoff.Sts2.ModSmith.Main;

namespace Cpimhoff.Sts2.ModSmith.Patches;

[HarmonyPatch(typeof(LocTable))]
public class MissingLocPatch
{
  [HarmonyPatch(nameof(LocTable.GetLocString))]
  [HarmonyPrefix]
  public static bool Prefix(LocTable __instance, string key, string ____name, ref LocString __result)
  {
    if (__instance.HasEntry(key))
      return true;

    ModSmithMain.Logger.Warn($"GetLocString: Key '{key}' not found in table '{____name}'");
    __result = new LocString(____name, key);
    return false;
  }

  [HarmonyPatch(nameof(LocTable.GetRawText))]
  [HarmonyPrefix]
  public static bool Prefix(LocTable __instance, string key, string ____name, ref string __result)
  {
    if (__instance.HasEntry(key))
      return true;

    ModSmithMain.Logger.Warn($"GetRawText: Key '{key}' not found in table '{____name}'");
    __result = $"{____name}.{key}";
    return false;
  }
}
