using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using ModSmith.Models;

namespace ModTemplate;

class CoinFlip : ModSmithCardModel
{
  public CoinFlip()
    : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new GoldVar(10),
  ];

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    bool heads = RunState?.Rng.Niche.NextBool() ?? Rng.Chaotic.NextBool();
    var goldValue = DynamicVars.Gold.IntValue;
    if (heads)
    {
      TalkCmd.Play(new LocString("cards", $"{base.Id.Entry}.headsBanter"), Owner.Creature, 1.5);
      NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, StsColors.green);
      await PlayerCmd.GainGold(goldValue, Owner);
    }
    else
    {
      TalkCmd.Play(new LocString("cards", $"{base.Id.Entry}.tailsBanter"), Owner.Creature, 1.5);
      NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, StsColors.red);
      goldValue = base.IsUpgraded ? goldValue / 2 : goldValue;
      await PlayerCmd.LoseGold(goldValue, Owner);
    }
  }

  protected override void OnUpgrade()
  {
    base.DynamicVars.Gold.UpgradeValueBy(2);
  }
}
