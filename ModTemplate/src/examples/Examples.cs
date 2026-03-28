using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.CardPools;
using ModSmith.Registry;

namespace ModTemplate;

public static class ModTemplateExamples
{
  public static void RegisterAll()
  {
    Registry.RegisterCard<CoinFlip, ColorlessCardPool>();
    Registry.RegisterPotion<DropOfGold>();
    Registry.RegisterRelic<GoldArmor>();
    Registry.RegisterPotion<GoldPaint>();
    Registry.RegisterPower<MadeOfGold>();
    Registry.RegisterEvent<TheGoldCoinRoom>();
    Registry.RegisterAncientEvent<GoldGuy, Hive>();
  }
}
