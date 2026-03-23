using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace Cpimhoff.Sts2.ModSmith.Util;

public class InlineLocString : LocString
{
  protected readonly string template;
  public InlineLocString(string template) : base("inline", template)
  {
    this.template = template;
  }

  [HarmonyPatch]
  static class PatchPrivateMembers
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocString), "GetRawText")]
    static bool GetRawText(LocString __instance, ref string __result)
    {
      if (__instance is InlineLocString inlineLocString)
      {
        __result = inlineLocString.template;
        return true;
      }
      return false;
    }
  }
}
