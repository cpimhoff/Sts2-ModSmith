using Cpimhoff.Sts2.ModSmith.Models;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace Cpimhoff.Sts2.ModSmith.Registry;

public static class Registry
{
  /// <summary>
  /// Registers a new `ModSmithPotionModel` to the `SharedPotionPool`.
  /// </summary>
  public static void RegisterPotion<TPotion>() where TPotion : ModSmithPotionModel
  {
    RegisterPotion<TPotion, SharedPotionPool>();
  }

  /// <summary>
  /// Registers a new `ModSmithPotionModel` to a specified `PotionPoolModel`.
  /// </summary>
  public static void RegisterPotion<TPotion, TPotionPool>() where TPotion : ModSmithPotionModel where TPotionPool : PotionPoolModel
  {
    ModHelper.AddModelToPool<TPotionPool, TPotion>();
  }

  /// <summary>
  /// Registers a new `ModSmithRelicModel` to the `SharedRelicPool`.
  /// </summary>
  public static void RegisterRelic<TRelic>() where TRelic : ModSmithRelicModel
  {
    RegisterRelic<TRelic, SharedRelicPool>();
  }

  /// <summary>
  /// Registers a new `ModSmithRelicModel` to a specified `RelicPoolModel`.
  /// </summary>
  public static void RegisterRelic<TRelic, TRelicPool>() where TRelic : ModSmithRelicModel where TRelicPool : RelicPoolModel
  {
    ModHelper.AddModelToPool<TRelicPool, TRelic>();
  }

  /// <summary>
  /// Registers a new `ModSmithPowerModel`.
  /// </summary>
  public static void RegisterPower<TPower>() where TPower : ModSmithPowerModel
  {
    // nothing actually needed here -- will keep this around for consistency
    // and future-proofing
  }

  private static List<Type> _globalEventTypes = [];

  /// <summary>
  /// Registers a new `ModSmithEventModel` to the global event pool, making it
  /// encounter-able in any act (further limited by its `IsAllowed` method).
  /// </summary>
  public static void RegisterEvent<TEvent>() where TEvent : ModSmithEventModel
  {
    // we cannot call `ModelDb.Get<TEvent>` here since the model db is not yet initialized
    // so we store the type for later patching
    _globalEventTypes.Add(typeof(TEvent));
  }
  // Patching events into a specific act is not yet supported

  [HarmonyPatch]
  private static class PatchRegisteredEvents
  {
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ModelDb), "AllSharedEvents", MethodType.Getter)]
    static void AllSharedEventsAppendGlobal(ref IEnumerable<EventModel> __result)
    {
      var globalEvents = _globalEventTypes.Select(t => ModelDb.GetById<EventModel>(ModelDb.GetId(t)));
      __result = __result
        .Concat(globalEvents)
        .Distinct();
    }
  }
}
