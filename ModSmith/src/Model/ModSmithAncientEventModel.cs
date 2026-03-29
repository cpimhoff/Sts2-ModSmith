using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Ancients;
using ModSmith.Main;
using MegaCrit.Sts2.Core.Nodes.Events;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Events;

namespace ModSmith.Models;

/// <summary>
/// Base class for mod-defined ancient events.
/// </summary>
///
/// <remarks>
/// ## Localization
/// <c>ancients.json</c> must contain localization keys for the ancient:
/// <code>
///   [ID].title     // The name of the ancient.
///   [ID].epithet   // A subtitle for the ancient.
/// </code>
/// </remarks>
public abstract class ModSmithAncientEventModel : AncientEventModel
{
  /// <summary>
  /// The background scene for the event.
  /// This can be a `.tscn` file or an image file.
  /// If an image is provided, ModSmith will generate a simple scene which
  /// displays the image centered and scaled to fit.
  /// </summary>
  protected virtual string BackgroundScenePath => ModSmithMain.Res.ModSmith("images/ancient-bg-default.png");

  private bool IsBackgroundSceneAnImage => !BackgroundScenePath.EndsWith(".tscn");
  private static string BasicAncientBackgroundScenePath => ModSmithMain.Res.ModSmith("scenes/basic_ancient_background.tscn");

  /// <summary>
  /// The path to the map icon for the ancient.
  ///
  /// If not provided, a default icon will be used.
  /// </summary>
  protected virtual string MapIconPath => ModSmithMain.Res.ModSmith("images/ancient-map-icon-default.png");

  /// <summary>
  /// The path to the map icon outline for the ancient.
  ///
  /// If not provided, a default icon will be used.
  /// </summary>
  protected virtual string MapIconOutlinePath => ModSmithMain.Res.ModSmith("images/ancient-map-icon-outline-default.png");

  /// <summary>
  /// The path to the run history icon for the ancient.
  /// This is also used as the icon for when the ancient is the speaker in dialogue.
  ///
  /// If not provided, a default icon will be used.
  /// </summary>
  protected virtual string RunHistoryIconPath => ModSmithMain.Res.ModSmith("images/ancient-icon-default.png");

  /// <summary>
  /// The path to the run history icon outline for the ancient.
  ///
  /// If not provided, a default icon will be used.
  /// </summary>
  protected virtual string RunHistoryIconOutlinePath => ModSmithMain.Res.ModSmith("images/ancient-icon-outline-default.png");

  /// <summary>
  /// All possible options that the ancient can offer.
  ///
  /// This is used to populate the ancient's unique relics and cards into the compendium,
  /// and is not generally used during a run.
  /// </summary>
  public abstract override IReadOnlyList<EventOption> AllPossibleOptions { get; }

  /// <summary>
  /// Generate the options for the event, such as the relics or boons the player
  /// can gain. Generally, ancients offer three options.
  ///
  /// Most ancients in the base game define 3 or more "pools" of options, and then
  /// use `Rng.NextItem` to select one option from each pool.
  /// </summary>
  ///
  /// <remarks>
  /// The term "Initial" here is a bit misleading. Whatever you return here will be the
  /// choices the player has when they encounter the ancient. The term "Initial" is reused
  /// from the base "EventModel" class, in which event options can change over time.
  /// This is not the case for ancients.
  /// </remarks>
  protected abstract override IReadOnlyList<EventOption> GenerateInitialOptions();

  /// <summary>
  /// Generate the dialogue sets for this ancient.
  ///
  /// If not provided, a default generic dialogue set will be used.
  /// </summary>
  protected override AncientDialogueSet DefineDialogues()
  {
    return new ModSmithDefaultAncientDialogueSet()
    {
      FirstVisitEverDialogue = null,
      CharacterDialogues = [],
      AgnosticDialogues = [
        new AncientDialogue("")
      ],
    };
  }

  [HarmonyPatch]
  private static class PatchableMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EventModel), "BackgroundScenePath", MethodType.Getter)]
    static bool BackgroundScenePath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithAncientEventModel)?.BackgroundScenePath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AncientEventModel), "MapIconPath", MethodType.Getter)]
    static bool MapIconPath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithAncientEventModel)?.MapIconPath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AncientEventModel), "MapIconOutlinePath", MethodType.Getter)]
    static bool MapIconOutlinePath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithAncientEventModel)?.MapIconOutlinePath, ref __result);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AncientEventModel), "RunHistoryIcon", MethodType.Getter)]
    static bool RunHistoryIcon(AncientEventModel __instance, ref Texture2D __result)
    {
      if (__instance is ModSmithAncientEventModel modSmithAncient)
      {
        __result = PreloadManager.Cache.GetCompressedTexture2D(modSmithAncient.RunHistoryIconPath);
        return false;
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AncientEventModel), "RunHistoryIconOutlinePath", MethodType.Getter)]
    static bool RunHistoryIconOutlinePath(EventModel __instance, ref string __result) =>
      PatchPrivate((__instance as ModSmithAncientEventModel)?.RunHistoryIconOutlinePath, ref __result);

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

  // There are some reads against the ancient's run history icon that are unfortunately not DRY in the source
  // code, so need to be patched manually at the point of use, namely inside of `ImageHelper.GetRoomIcon*Path`.
  [HarmonyPatch]
  private static class PatchRunHistoryIcon
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ImageHelper), "GetRoomIconPath", MethodType.Normal)]
    static bool GetRoomIconPath(MapPointType mapPointType, RoomType roomType, ModelId? modelId, ref string? __result)
    {
      if (mapPointType == MapPointType.Ancient && roomType == RoomType.Event && modelId != null)
      {
        var model = ModelDb.GetByIdOrNull<AncientEventModel>(modelId);
        if (model is ModSmithAncientEventModel modSmithAncient)
        {
          __result = modSmithAncient.RunHistoryIconPath;
          return false;
        }
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ImageHelper), "GetRoomIconOutlinePath", MethodType.Normal)]
    static bool GetRoomIconOutlinePath(MapPointType mapPointType, RoomType roomType, ModelId? modelId, ref string? __result)
    {
      if (mapPointType == MapPointType.Ancient && roomType == RoomType.Event && modelId != null)
      {
        var model = ModelDb.GetByIdOrNull<AncientEventModel>(modelId);
        if (model is ModSmithAncientEventModel modSmithAncient)
        {
          __result = modSmithAncient.RunHistoryIconOutlinePath;
          return false;
        }
      }
      return true;
    }
  }

  // We want to provide a simple way for users to provide an image as a background scene
  // for the ancient; without needing to create a custom scene. To do so, we patch `CreateBackgroundScene`
  // to return a simple scene with an unset image if the background scene would be an image. We then also patch
  // the node to correctly initialize the image just in time.
  [HarmonyPatch]
  private static class PatchBackgroundSceneImageInitialization
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EventModel), "CreateBackgroundScene", MethodType.Normal)]
    static bool CreateBackgroundScenePatchImage(EventModel __instance, ref PackedScene __result)
    {
      if (__instance is ModSmithAncientEventModel modSmithAncient && modSmithAncient.IsBackgroundSceneAnImage)
      {
        __result = PreloadManager.Cache.GetScene(BasicAncientBackgroundScenePath);
        return false;
      }
      return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NAncientEventLayout), "InitializeVisuals", MethodType.Normal)]
    static void InitializeVisualsPatchImage(NAncientEventLayout __instance)
    {
      var rawAncient = AccessTools.Field(typeof(NEventLayout), "_event").GetValue(__instance);
      if (rawAncient is not ModSmithAncientEventModel modSmithAncient) return;

      var basicTextureRect = __instance.GetNode("%AncientBgContainer").GetNodeOrNull<TextureRect>("Ancient/TextureRect");
      if (basicTextureRect is null) return;

      var imagePath = modSmithAncient.BackgroundScenePath;
      var image = PreloadManager.Cache.GetTexture2D(imagePath);
      basicTextureRect.Texture = image;
    }
  }

  // The dialogue sets are a pretty complex bit to program, and so we want to
  // provide a simple fallback implementation. Unfortunately, the base class always
  // looks for localization keys using the `base.Id.Entry` as the ancient entry,
  // which won't work for a common implementation. So instead, we provide an implementation
  // which returns a special marker type, and then patch the localization to substitute
  // a static key if this is used. Consumers are still free to override this method to
  // fully customize their dialogue set.
  private sealed class ModSmithDefaultAncientDialogueSet : AncientDialogueSet
  { }

  [HarmonyPatch]
  private static class ModSmithDefaultAncientDialogueSetPatch
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AncientDialogueSet), "PopulateLocKeys", MethodType.Normal)]
    static void PopulateLocKeys(AncientDialogueSet __instance, ref string ancientEntry)
    {
      if (__instance is ModSmithDefaultAncientDialogueSet)
      {
        ancientEntry = "MODSMITH_DEFAULT";
      }
    }
  }
}



