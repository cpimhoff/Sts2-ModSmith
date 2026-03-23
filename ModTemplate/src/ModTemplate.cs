using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using Cpimhoff.Sts2.ModSmith.Util;

namespace ModTemplate;

[ModInitializer(nameof(Initialize))]
public static class ModTemplate
{
    public const string ModId = "ModTemplate"; // Must match the id in `ModTemplate.json`

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static ResourcePaths Res { get; } = new(ModId);

    public static void Initialize()
    {
        Logger.Info("Initializing...");
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        Logger.Info("Patched all Harmony patches.");

        // ========================================================
        // Register example content.
        // You should remove this and replace with your own content.
        ModTemplateExamples.RegisterAll();
        // ========================================================

        Logger.Info("Initialized.");
    }
}
