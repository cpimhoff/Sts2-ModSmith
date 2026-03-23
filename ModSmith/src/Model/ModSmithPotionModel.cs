
using Cpimhoff.Sts2.ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Cpimhoff.Sts2.ModSmith.Models;

public abstract class ModSmithPotionModel : PotionModel
{
  protected virtual string PackedImagePath { get; } = ModSmithMain.Res.ModSmith("images/potion-default.png");

  protected virtual string PackedOutlinePath { get; } = ModSmithMain.Res.Global("images/potions/atlases/potion_outline_atlas.sprites/gamblers_brew.tres");

  protected override Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    return Task.CompletedTask;
  }

  /// Make `PackedImagePath` and `PackedOutlinePath` overridable
  /// despite being private in the base class.
  [HarmonyPatch]
  private static class PatchablePrivateMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PotionModel), "PackedImagePath", MethodType.Getter)]
    static bool PackedImagePath(PotionModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPotionModel)?.PackedImagePath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PotionModel), "PackedOutlinePath", MethodType.Getter)]
    static bool PackedOutlinePath(PotionModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPotionModel)?.PackedOutlinePath, ref __result);

    static bool PatchPrivate(string? customPath, ref string result)
    {
      if (customPath is string path)
      {
        result = path;
        return false;
      }
      else return true;
    }
  }
}
