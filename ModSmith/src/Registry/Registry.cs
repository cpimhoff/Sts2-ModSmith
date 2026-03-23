using Cpimhoff.Sts2.ModSmith.Models;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace Cpimhoff.Sts2.ModSmith.Registry;

public static class Registry
{
  /// <summary>
  /// Registers a new `ModSmithPotionModel` to the `SharedPotionPool`.
  /// </summary>
  public static void RegisterPotion<TPotion>() where TPotion : ModSmithPotionModel
  {
    ModHelper.AddModelToPool<SharedPotionPool, TPotion>();
  }

  /// <summary>
  /// Registers a new `ModSmithPotionModel` to a specified `PotionPoolModel`.
  /// </summary>
  public static void RegisterPotion<TPotion, TPotionPool>() where TPotion : ModSmithPotionModel where TPotionPool : PotionPoolModel
  {
    ModHelper.AddModelToPool<TPotionPool, TPotion>();
  }
}
