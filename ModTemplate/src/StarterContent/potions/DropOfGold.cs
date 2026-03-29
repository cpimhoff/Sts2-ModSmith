using ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace ModTemplate;

// An example potion that gives the player some gold.

public sealed class DropOfGold : ModSmithPotionModel
{
  public override PotionRarity Rarity => PotionRarity.Common;

  public override PotionUsage Usage => PotionUsage.AnyTime;

  public override TargetType TargetType => TargetType.AnyPlayer;

  protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(100)];

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    AssertValidForTargetedPotion(target);
    NCombatRoom.Instance?.PlaySplashVfx(target, StsColors.gold);
    await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, target.Player!);
  }
}
