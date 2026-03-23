using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using Cpimhoff.Sts2.ModSmith.Registry;

namespace ModTemplate;

[ModInitializer(nameof(Initialize))]
public static class ModTemplate
{

    public const string ModId = "ModTemplate";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Logger.Info("Initializing...");
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        Logger.Info("Patched all Harmony patches.");

        Registry.RegisterPotion<DropOfGold>();
        Logger.Info("Registered potion.");

        Logger.Info("Initialized.");
    }
}
