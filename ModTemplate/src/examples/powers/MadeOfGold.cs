using Cpimhoff.Sts2.ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ModTemplate;

// An example power where the attacking player gains gold whenever the creature is hit by an attack.
// Applied via the `GoldPaint` potion.

public sealed class MadeOfGold : ModSmithPowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target == Owner && dealer?.Player != null && result.UnblockedDamage > 0 && props.IsPoweredAttack())
    {
      Flash();
      await PlayerCmd.GainGold(Amount, dealer.Player);
    }
  }
}
