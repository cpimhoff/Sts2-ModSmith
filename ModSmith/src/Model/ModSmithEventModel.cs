
using ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace ModSmith.Models;

public abstract class ModSmithEventModel : EventModel
{
  /// <summary>
  /// You should generally only override this if you are creating an event
  /// which uses a custom layout scene, such as `FakeMerchant`.
  /// </summary>
  protected virtual string? LayoutScenePath => null;

  /// <summary>
  /// Image background for the event which loads at the start.
  /// Not used for combat or custom events.
  /// </summary>
  protected virtual string InitialPortraitPath => ModSmithMain.Res.ModSmith("images/event-portrait-default.png");

  /// <summary>
  /// VFX scene (`.tscn`) for the event which loads at the start.
  /// Not used for combat or custom events.
  /// </summary>
  protected virtual string? VfxPath => null;

  // BackgroundScenePath is not vended here since it only applies to ancients,
  // which have their own subclass.

  // Note that `IsShared` I believe refers to whether the event has a shared
  // multiplayer outcome, *not* whether the event is part of the shared event pool.

  [HarmonyPatch]
  private static class PatchableMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EventModel), "LayoutScenePath", MethodType.Getter)]
    static bool LayoutScenePath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithEventModel)?.LayoutScenePath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EventModel), "InitialPortraitPath", MethodType.Getter)]
    static bool InitialPortraitPath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithEventModel)?.InitialPortraitPath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EventModel), "VfxPath", MethodType.Getter)]
    static bool VfxPath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithEventModel)?.VfxPath, ref __result);

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
