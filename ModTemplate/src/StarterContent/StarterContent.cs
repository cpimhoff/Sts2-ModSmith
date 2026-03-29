using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.CardPools;
using ModSmith.Registry;

namespace ModTemplate;

public static class StarterContent
{
  public static void RegisterStarterContent()
  {
    Registry.RegisterPotion<DropOfGold>();
    Registry.RegisterCard<CoinFlip, ColorlessCardPool>();
    Registry.RegisterRelic<GoldArmor>();
    Registry.RegisterPotion<GoldPaint>();
    Registry.RegisterPower<MadeOfGold>();
    Registry.RegisterEvent<TheGoldCoinRoom>();
    Registry.RegisterAncientEvent<GoldGuy, Hive>();
  }
}
