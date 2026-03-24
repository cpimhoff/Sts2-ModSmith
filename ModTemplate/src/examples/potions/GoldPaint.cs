using Cpimhoff.Sts2.ModSmith.Models;
using Cpimhoff.Sts2.ModSmith.Registry;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace ModTemplate;

// An example potion that applies the `MadeOfGold` power to an enemy.

public sealed class GoldPaint : ModSmithPotionModel
{
  public override PotionRarity Rarity => PotionRarity.Uncommon;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyEnemy;

  protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MadeOfGold>(5)];

  public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MadeOfGold>()];

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    AssertValidForTargetedPotion(target);
    NCombatRoom.Instance?.PlaySplashVfx(target, StsColors.gold);
    await PowerCmd.Apply<MadeOfGold>(target, DynamicVars[typeof(MadeOfGold).Name].BaseValue, target, null);
  }
}
