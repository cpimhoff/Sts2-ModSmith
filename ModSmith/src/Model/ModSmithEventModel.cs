
using ModSmith.Main;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Events;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined events.
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>events.json</c> must contain localization keys for the event:
/// <code>
///   [ID].title       // The title of the event.
///
///   // The body text of the event when it first appears.
///   [ID].pages.INITIAL.description
///
///   // For each `EventOption` the event may offer, keys must appear of the form:
///   [OPTION_KEY].title  // The title of the option.
///   [OPTION_KEY].description  // The body text of the option, describing its effects.
/// </code>
/// </remarks>
public abstract class ModSmithEventModel : EventModel
{
  /// <summary>
  /// The layout type of the event, which determines the core display behavior for the event.
  /// Almost all events use the <c>Default</c> layout.
  ///
  /// <para>
  /// <c>Default</c> layouts show a background image (See <c>InitialPortraitPath</c>)
  /// and offer the player a choice of options. Options can either end the event or
  /// continue it with a new set of options.
  /// </para>
  ///
  /// <para>
  /// <c>Combat</c> layouts show a combat room and offer the player a choice of options.
  /// Note that this layout type is not required if an event may _trigger_ combat, only if
  /// it _begins_ in a combat room. Any event type can transition into combat using
  /// <c>EnterCombatWithoutExitingEvent</c>.
  /// </para>
  ///
  /// <para>
  /// <c>Custom</c> layouts are an advanced option which provide their own Godot scene
  /// and are responsible for all their own logic and gameplay.
  /// When using a custom layout, you must supply your scene with <c>LayoutScenePath</c>.
  /// </para>
  ///
  /// <para>
  /// <c>Ancient</c> layouts should not be used directly.
  /// Instead, subclass <c>ModSmithAncientEventModel</c>.
  /// </para>
  /// </summary>
  public override EventLayoutType LayoutType => base.LayoutType;

  /// <summary>
  /// When using <c>Custom</c> layouts, this is the path to the layout scene for the event.
  /// Do not override this property for other layout types.
  /// </summary>
  protected virtual string? LayoutScenePath => null;

  /// <summary>
  /// Image background for the event which loads at the start.
  /// Not used for <c>Combat</c> or <c>Custom</c> events.
  /// </summary>
  protected virtual string InitialPortraitPath => ModSmithMain.Res.ModSmith("images/event-portrait-default.png");

  /// <summary>
  /// VFX scene (`.tscn`) which can add additional overlay effects for an event.
  /// Not used for <c>Combat</c> or <c>Custom</c> events.
  /// </summary>
  protected virtual string? VfxPath => null;

  // BackgroundScenePath is not vended here since it only applies to ancients,
  // which have their own subclass.

  /// <summary>
  /// If true, choices in the event are shared between all players.
  /// Otherwise, each player may make their own independent choices in the event.
  /// </summary>
  public override bool IsShared => base.IsShared;

  /// <summary>
  /// Generate the initial set of options the player may choose from when the event first appears.
  ///
  /// Events act as a state machine, where each <c>EventOption</c> may either end the event with
  /// <c>SetEventFinished</c> or transition to the next state with <c>SetEventState</c>.
  /// </summary>
  protected abstract override IReadOnlyList<EventOption> GenerateInitialOptions();

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
