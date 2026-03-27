using ModSmith.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace ModTemplate;

// An example relic that causes the player to pay some gold per turn to gain block.

public sealed class GoldArmor : ModSmithRelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new GoldVar(5),
    new BlockVar(10m, ValueProp.Unpowered)
  ];

  protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

  public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
  {
    if (side == Owner.Creature.Side && Owner.Gold >= DynamicVars.Gold.IntValue)
    {
      Flash();
      await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);
      await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
    }
  }
}
