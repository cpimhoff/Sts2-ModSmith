using ModSmith.Models;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using ModSmith.Main;

namespace ModSmith.Registry;

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

  // We cannot call `ModelDb.Get<TEvent>` during registration since the ModelDb
  // is not yet initialized. So we store types instead of instances for later patching.
  private static List<Type> _globalEventTypes = [];
  private static Dictionary<Type, List<Type>> _actToEventTypes = [];

  /// <summary>
  /// Registers a new `ModSmithEventModel` to the global event pool, making it
  /// encounter-able in any act (further limited by its `IsAllowed` method).
  /// </summary>
  public static void RegisterEvent<TEvent>() where TEvent : ModSmithEventModel
  {
    _globalEventTypes.Add(typeof(TEvent));
  }

  /// <summary>
  /// Registers a new `ModSmithEventModel` to the specified act's event pool, making it
  /// encounter-able in that act.
  /// </summary>
  public static void RegisterEvent<TEvent, TAct>() where TEvent : ModSmithEventModel where TAct : ActModel
  {
    var act = typeof(TAct);
    if (act.IsAbstract)
    {
      throw new InvalidOperationException(
        $"Cannot register event {typeof(TEvent).Name} to abstract act type: {act.Name}. "
      + $"If you intend to register it globally, use `RegisterEvent<{typeof(TEvent).Name}>()` instead.");
    }

    if (!_actToEventTypes.TryGetValue(act, out var list))
    {
      list = [];
      _actToEventTypes[act] = list;
    }
    list.Add(typeof(TEvent));
  }

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

  private static Dictionary<Type, List<Type>> _actToAncientEventTypes = [];
  private static bool _hasInstalledAncientEventPatches = false;

  /// <summary>
  /// Registers a new `ModSmithAncientEventModel` to a specific act, making it
  /// encounter-able at the start of that act.
  /// </summary>
  public static void RegisterAncientEvent<TAncient, TAct>() where TAncient : ModSmithAncientEventModel where TAct : ActModel
  {
    var act = typeof(TAct);
    if (act.IsAbstract)
    {
      throw new InvalidOperationException(
        $"Cannot register ancient event {typeof(TAncient).Name} to abstract act type: {act.Name}. "
      + $"You must specify concrete act type per call, such as `RegisterAncientEvent<{typeof(TAncient).Name}, Glory>()`.");
    }

    if (!_actToAncientEventTypes.TryGetValue(act, out var list))
    {
      list = [];
      _actToAncientEventTypes[act] = list;
    }
    list.Add(typeof(TAncient));

    if (!_hasInstalledAncientEventPatches)
    {
      foreach (var actType in typeof(ActModel).Assembly
        .GetTypes()
        .Where(t => t.IsSubclassOf(typeof(ActModel)) && !t.IsAbstract))
      {
        var method = AccessTools.PropertyGetter(actType, "AllAncients")
          ?? throw new Exception($"{actType.Name}.AllAncients is not a property. Cannot patch ancient events.");
        ModSmithMain.Harmony.Patch(method, postfix: new HarmonyMethod(
          typeof(PatchRegisteredAncientEvents),
          nameof(PatchRegisteredAncientEvents.AllAncientsAppendRegistered)
        ));
      }
      _hasInstalledAncientEventPatches = true;
    }
  }
  // When we add support for custom acts, we will likely need to revisit this to
  // allow patching all `ModSmitActModel`s based on when they can appear in the
  // game (likely via an "act number" field).

  private static class PatchRegisteredAncientEvents
  {
    public static void AllAncientsAppendRegistered(ActModel __instance, ref IEnumerable<AncientEventModel> __result)
    {
      if (_actToAncientEventTypes.TryGetValue(__instance.GetType(), out var actAncientTypes))
      {
        var actAncients = actAncientTypes.Select(t => ModelDb.GetById<AncientEventModel>(ModelDb.GetId(t)));
        __result = __result.Concat(actAncients).Distinct();
      }
    }
  }
}
