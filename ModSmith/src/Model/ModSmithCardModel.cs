
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using ModSmith.Main;
using MegaCrit.Sts2.Core.Helpers;

namespace ModSmith.Models;

public abstract class ModSmithCardModel : CardModel
{
  protected ModSmithCardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true)
    : base(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
  {
  }

  // The image paths for cards are already set up for overrides as, just need to provide a static default.
  public override string PortraitPath => Rarity != CardRarity.Ancient
    ? ModSmithMain.Res.ModSmith("images/card-portrait-default.png")
    : ModSmithMain.Res.ModSmith("images/card-portrait-ancient-default.png");
  public override string BetaPortraitPath => ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres"); // copied from the base class

  /// <summary>
  /// The path to an overlay scene for the card.
  /// Automatically derives `HasBuiltInOverlay` from this value.
  /// </summary>
  protected virtual string? OverlayScenePath => null;
  public sealed override bool HasBuiltInOverlay => OverlayScenePath != null;

  [HarmonyPatch]
  private static class PatchableMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CardModel), "OverlayPath", MethodType.Getter)]
    static bool OverlayPath(CardModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithCardModel)?.OverlayScenePath, ref __result);

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
