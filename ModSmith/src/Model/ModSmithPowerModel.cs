
using ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined powers.
/// <para>
/// Note that "power" does not exclusively refer to "Power" cards in the game.
/// Powers are any status effect applied to a creature during combat,
/// including debuffs like "Vulnerable".
/// </para>
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>powers.json</c> must contain localization keys for the power:
/// <code>
///   [ID].title            // The name of the power.
///   [ID].description      // The body text of the power when it is not applied to a target. Used for tooltips in cards and potions.
///   [ID].smartDescription // The body text of the power when it is applied to a target. Can reference dynamic variables.
/// </code>
/// </remarks>
public abstract class ModSmithPowerModel : PowerModel
{
  /// <summary>
  /// Whether the power is a buff or debuff.
  /// Used by artifact and similar effects to determine whether to negate or cleanse a power.
  /// </summary>
  public abstract override PowerType Type { get; }

  /// <summary>
  /// How the power stacks.
  /// <list type="bullet">
  /// <item><c>PowerStackType.Counter</c>:
  ///   The power can stack, summing the <c>Amount</c> of the power,
  ///   and potentially increasing its effect or duration.</item>
  /// <item><c>PowerStackType.Single</c>:
  ///   The power does not stack.
  ///   Gaining multiple instances of the power does nothing.</item>
  /// </list>
  /// </summary>
  public abstract override PowerStackType StackType { get; }

  /// <summary>
  /// The path to the icon for the power.
  /// Most powers are around <c>(64px x 64px)</c>.
  ///
  /// If not provided, a default image will be used.
  /// </summary>
  protected new virtual string PackedIconPath => ModSmithMain.Res.ModSmith("images/power-default.png");

  /// <summary>
  /// The path to a larger version of the icon for the power.
  /// This defaults to the same value as <c>PackedIconPath</c>.
  /// </summary>
  protected virtual string BigIconPath => PackedIconPath;

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
