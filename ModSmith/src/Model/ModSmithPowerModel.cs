
using Cpimhoff.Sts2.ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace Cpimhoff.Sts2.ModSmith.Models;

public abstract class ModSmithPowerModel : PowerModel
{
  protected new virtual string PackedIconPath => ModSmithMain.Res.ModSmith("images/power-default.png");

  protected virtual string BigIconPath => ModSmithMain.Res.ModSmith("images/power-default.png");

  /// Make `PackedImagePath` virtual and `BigIconPath` overridable
  /// despite being private or non-virtual in the base class.
  [HarmonyPatch]
  private static class PatchableMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PowerModel), "PackedIconPath", MethodType.Getter)]
    static bool PackedIconPath(PowerModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPowerModel)?.PackedIconPath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PowerModel), "BigIconPath", MethodType.Getter)]
    static bool BigIconPath(PowerModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPowerModel)?.BigIconPath, ref __result);

    static bool PatchPrivate(string? customPath, ref string __result)
    {
      if (customPath is string path)
      {
        __result = path;
        return false;
      }
      else return true;
    }
  }
}
