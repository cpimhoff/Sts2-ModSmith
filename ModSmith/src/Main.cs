using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using ModSmith.Util;

namespace ModSmith.Main;

/// <summary>
/// ModSmith's main entry point and shared resources.
/// </summary>
[ModInitializer(nameof(Initialize))]
public static class ModSmithMain
{
  public const string ModId = "ModSmith";

  internal static Harmony Harmony = new(ModId);

  internal static Logger Logger { get; } = new(ModId, LogType.Generic);

  internal static ResourcePaths Res { get; } = new(ModId);

  public static void Initialize()
  {
    Logger.Info("Initializing...");
    Harmony.PatchAll();
    Logger.Info("Initialized.");
  }
}
