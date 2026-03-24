using Cpimhoff.Sts2.ModSmith.Registry;

namespace ModTemplate;

public static class ModTemplateExamples
{
  public static void RegisterAll()
  {
    Registry.RegisterPotion<DropOfGold>();
    Registry.RegisterRelic<GoldArmor>();
    Registry.RegisterPotion<GoldPaint>();
    Registry.RegisterPower<MadeOfGold>();
  }
}
