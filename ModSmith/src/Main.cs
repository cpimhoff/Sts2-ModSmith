using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;

namespace Cpimhoff.Sts2.ModSmith.Main;

[ModInitializer(nameof(Initialize))]
public static class ModSmithMain
{

  public const string ModId = "ModSmith";

  public static Logger Logger { get; } = new(ModId, LogType.Generic);

  public static void Initialize()
  {
    Logger.Info("Initializing...");

    Harmony harmony = new(ModId);
    harmony.PatchAll();

    Logger.Info("Initialized.");
  }
}
