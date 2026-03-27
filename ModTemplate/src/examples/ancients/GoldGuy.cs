using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using ModSmith.Models;

class GoldGuy : ModSmithAncientEventModel
{
  public override IReadOnlyList<EventOption> AllPossibleOptions => [
    RelicOption<OldCoin>(),
    RelicOption<GoldenPearl>(),
    RelicOption<GoldenCompass>(),
  ];

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    return AllPossibleOptions;
  }
}
