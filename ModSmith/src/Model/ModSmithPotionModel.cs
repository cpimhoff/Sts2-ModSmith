
using ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined potions.
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>potions.json</c> must contain localization keys for the potion:
/// <code>
///   [ID].title       // The name of the potion.
///   [ID].description // The body text of the potion, describing its effects.
/// </code>
/// </remarks>
public abstract class ModSmithPotionModel : PotionModel
{
  /// <summary>
  /// The rarity of the potion, affecting its merchant cost and drop-chance.
  /// </summary>
  public abstract override PotionRarity Rarity { get; }

  /// <summary>
  /// When the potion can be used.
  /// <list type="bullet">
  /// <item>
  ///   <c>CombatOnly</c>: The potion is only usable by the player during combat.
  /// </item>
  /// <item>
  ///   <c>AnyTime</c>: The potion can be used any time by the player.
  /// </item>
  /// <item>
  ///   <c>Automatic</c>: Not used directly by the player; instead the potion triggers itself.
  ///   For example, <c>FairyInABottle</c> is used automatically when the player would die.
  /// </item>
  /// <item>
  ///   <c>None</c>: The potion cannot be used.
  /// </item>
  /// </list>
  /// </summary>
  public abstract override PotionUsage Usage { get; }

  /// <summary>
  /// The target type of the potion.
  /// </summary>
  public abstract override TargetType TargetType { get; }

  /// <summary>
  /// The path to the image for the potion.
  /// Most potions are around <c>(75px x 75px)</c>.
  ///
  /// If not provided, a default image will be used.
  /// </summary>
  protected virtual string PackedImagePath => ModSmithMain.Res.ModSmith("images/potion-default.png");

  /// <summary>
  /// The path to the outline image for the potion.
  ///
  /// If not provided, no outline will used.
  /// </summary>
  protected virtual string PackedOutlinePath => ModSmithMain.Res.ModSmith("images/empty.png");

  /// <summary>
  /// The logic to execute when the potion is used.
  /// </summary>
  protected abstract override Task OnUse(PlayerChoiceContext choiceContext, Creature? target);

  [HarmonyPatch]
  private static class PatchableMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PotionModel), "PackedImagePath", MethodType.Getter)]
    static bool PackedImagePath(PotionModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPotionModel)?.PackedImagePath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PotionModel), "PackedOutlinePath", MethodType.Getter)]
    static bool PackedOutlinePath(PotionModel __instance, ref string __result) =>
        PatchPrivate((__instance as ModSmithPotionModel)?.PackedOutlinePath, ref __result);

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
