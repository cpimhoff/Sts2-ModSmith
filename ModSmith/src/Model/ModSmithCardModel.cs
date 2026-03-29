
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using ModSmith.Main;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined cards.
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>cards.json</c> must contain localization keys for the card:
/// <code>
///   [ID].title       // The name of the card.
///   [ID].description // The body text of the card, describing its effects. Does not need to contain keywords.
/// </code>
/// </remarks>
public abstract class ModSmithCardModel : CardModel
{
  /// <summary>
  /// Define core card properties.
  /// </summary>
  /// <param name="canonicalEnergyCost">The energy cost of the card.</param>
  /// <param name="type">The type of the card.</param>
  /// <param name="rarity">The rarity of the card, affecting its merchant cost and drop-chance.</param>
  /// <param name="targetType">The target type of the card, which defines how it is played.</param>
  /// <param name="shouldShowInCardLibrary">Whether the card should be shown in the card library (defaults to true).</param>
  protected ModSmithCardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true)
    : base(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
  {
  }

  /// <summary>
  /// The path to the main portrait art for the card.
  /// Most cards are <c>(250px x 190px)</c>, and ancient cards are <c>(250px x 350px)</c>.
  ///
  /// If not provided, a default image will be used.
  /// </summary>
  public override string PortraitPath => Rarity != CardRarity.Ancient
    ? ModSmithMain.Res.ModSmith("images/card-portrait-default.png")
    : ModSmithMain.Res.ModSmith("images/card-portrait-ancient-default.png");

  /// <summary>
  /// The path to the beta portrait art for the card.
  /// Same dimensions are the main portrait art.
  ///
  /// If not provided, a default image will be used.
  /// </summary>
  /// <remarks>
  /// Beta art for cards is not yet implemented in Slay the Spire 2, so this property
  /// currently does nothing.
  /// </remarks>
  public override string BetaPortraitPath => ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres"); // copied from the base class

  /// <summary>
  /// The path to an overlay scene for the card.
  /// This can be used to add additional visual effects to a card.
  /// </summary>
  /// <remarks>
  /// Currently, the only card in the base game that uses this effect is the "Infection" card.
  /// </remarks>
  protected virtual string? OverlayScenePath => null;
  public sealed override bool HasBuiltInOverlay => OverlayScenePath != null;

  /// <summary>
  /// Handler for when the card is played.
  /// </summary>
  protected abstract override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay);

  /// <summary>
  /// Called when the card is upgraded.
  ///
  /// <para>
  /// Common implementations of this method include:
  /// <list type="bullet">
  /// <item>
  /// Lowering the card's energy cost using <c>EnergyCost.UpgradeBy(-1)</c>.
  /// </item>
  /// <item>
  /// Calling <c>DynamicVars["VAR_NAME"].UpgradeValueBy</c> to change a dynamic variable.
  /// </item>
  /// <item>
  /// <description>Adding or removing a keyword using <c>AddKeyword</c> or <c>RemoveKeyword</c>.</description>
  /// </item>
  /// <item>
  /// <description>Behavior in <c>OnPlay</c> checks <c>IsUpgraded</c> to modify its behavior.</description>
  /// </item>
  /// </list>
  /// </para>
  ///
  /// <para>
  /// The card text is generally automatically updated to reflect the result of an upgrade.
  /// If the card's upgrade behavior needs further customization, the card's <c>description</c>
  /// localization key can be parameterized with the <c>{IfUpgraded}</c> template expression.
  /// </para>
  /// </summary>
  protected abstract override void OnUpgrade();

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
