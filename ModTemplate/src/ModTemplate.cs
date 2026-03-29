using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using ModSmith.Util;

namespace ModTemplate;

[ModInitializer(nameof(Initialize))]
public static class ModTemplateMain
{
    public const string ModId = "ModTemplate"; // Must match the id in `ModTemplate.json`

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static ResourcePaths Res { get; } = new(ModId);

    public static void Initialize()
    {
        Logger.Info("Initializing...");
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        // Register your content...
#if (!NoStarterContent)
        StarterContent.RegisterStarterContent();
#endif

        Logger.Info("Initialized.");
    }
}
