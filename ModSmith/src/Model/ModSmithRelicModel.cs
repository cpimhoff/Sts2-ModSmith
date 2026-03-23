
using Cpimhoff.Sts2.ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Cpimhoff.Sts2.ModSmith.Models;

public abstract class ModSmithRelicModel : RelicModel
{
  public override string PackedIconPath => ModSmithMain.Res.ModSmith("images/relic-default.png");

  protected override string PackedIconOutlinePath => ModSmithMain.Res.ModSmith("images/empty.png");

  protected override string BigIconPath => PackedIconPath;

  /// <summary>
  /// Get the upgraded replacement for the starter relic.
  /// Used by `TouchOfOrobas`, or other mods which provide relic upgrades.
  /// </summary>
  public virtual RelicModel? GetUpgrade() => null;
}


// Patch for `TouchOfOrobas` to allow for a custom character's starter relic to be upgraded
[HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.GetUpgradedStarterRelic))]
class TouchOfOrobasUpgradePatch
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
