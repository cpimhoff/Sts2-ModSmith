using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using ModSmith.Util;

namespace ModSmith.Main;

[ModInitializer(nameof(Initialize))]
public static class ModSmithMain
{
  public const string ModId = "ModSmith";
  internal static Harmony Harmony = new(ModId);

  public static Logger Logger { get; } = new(ModId, LogType.Generic);

  public static ResourcePaths Res { get; } = new(ModId);

  public static void Initialize()
  {
    Logger.Info("Initializing...");
    Harmony.PatchAll();
    Logger.Info("Initialized.");
  }
}
