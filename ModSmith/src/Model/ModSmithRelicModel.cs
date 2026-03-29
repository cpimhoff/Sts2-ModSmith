
using ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined relics.
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>relics.json</c> must contain localization keys for the relic:
/// <code>
///   [ID].title       // The name of the relic.
///   [ID].description // The body text of the relic, describing its effects.
///   [ID].flavor      // Flavor text for the relic.
/// </code>
/// </remarks>
public abstract class ModSmithRelicModel : RelicModel
{
  /// <summary>
  /// The rarity of the relic, affecting its merchant cost and drop-chance.
  /// </summary>
  public abstract override RelicRarity Rarity { get; }

  /// <summary>
  /// The path to the icon for the relic.
  /// Most relics are around <c>(85px x 85px)</c>.
  ///
  /// If not provided, a default image will be used.
  /// </summary>
  public override string PackedIconPath => ModSmithMain.Res.ModSmith("images/relic-default.png");

  /// <summary>
  /// The path to the outline image for the relic.
  ///
  /// If not provided, no outline will used.
  /// </summary>
  protected override string PackedIconOutlinePath => ModSmithMain.Res.ModSmith("images/empty.png");

  /// <summary>
  /// The path to a larger version of the icon for the relic.
  /// This defaults to the same value as <c>PackedIconPath</c>.
  /// </summary>
  protected override string BigIconPath => PackedIconPath;

  /// <summary>
  /// Get the upgraded replacement for a starter relic.
  /// Relics without an upgrade may return <c>null</c>.
  /// </summary>
  ///
  /// <remarks>
  /// This is used by <c>TouchOfOrobas</c> to upgrade a character's starter relic.
  /// Other mods may also provide relic upgrades which use ths method.
  /// </remarks>
  public virtual RelicModel? GetUpgrade() => null;

  // Patch for `TouchOfOrobas` to allow for a custom character's starter relic to be upgraded
  [HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.GetUpgradedStarterRelic))]
  private static class TouchOfOrobasUpgradePatch
  {
    static bool Prefix(RelicModel starterRelic, ref RelicModel? __result)
    {
      if (starterRelic is ModSmithRelicModel modSmithRelic)
      {
        __result = modSmithRelic.GetUpgrade();
        return __result == null;
      }
      return true;
    }
  }
}

