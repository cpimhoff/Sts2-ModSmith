using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;

namespace ModTemplate;

[ModInitializer(nameof(Initialize))]
public static class ModTemplate
{

    public const string ModId = "ModTemplate";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}
