# Event hooks

## What is `AbstractModel`?

Every game entity that can participate in gameplay logic — cards, relics, potions, powers, events, and more — ultimately inherits from `AbstractModel` (defined in `MegaCrit.Sts2.Core.Models`). This base class declares a large collection of `virtual` methods that the game engine calls at specific moments during a run or combat encounter.

When you subclass a ModSmith model (e.g. `ModSmithRelicModel`, `ModSmithPowerModel`), you are ultimately subclassing `AbstractModel`. Overriding its virtual methods is how you give your content behaviors that react to game events.

## How hooks work

Each hook is a `virtual` method that returns `Task.CompletedTask` by default (i.e. does nothing). To respond to an event, override the method in your class:

```csharp
public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
{
    // Runs at the start of the owning player's turn
    await GainGoldCommand.Run(player, 5);
}
```

> Hooks are `async`-compatible. You can `await` game commands inside them. If you don't need to await anything, you can still `override` the method and call `return Task.CompletedTask` (or just return a completed task from whatever you call).

The engine only calls a hook on a model if `ShouldReceiveCombatHooks` returns `true` on that instance. ModSmith's model base classes handle this for you.

## Hook categories

The following is a high-level overview of the hook categories available. For the full, authoritative list, study the decompiled `AbstractModel.cs` — see [Decompiling STS2](../setup/decompile.md).

### Lifecycle & run flow

These hooks fire around major run milestones.

| Method | When it fires |
|--------|--------------|
| `AfterActEntered()` | When the player enters a new Act |
| `BeforeRoomEntered(room)` | Before the player enters a room |
| `AfterRoomEntered(room)` | After the player enters a room |
| `AfterMapGenerated(map, actIndex)` | After the Act map is generated |
| `ModifyGeneratedMap(...)` / `ModifyGeneratedMapLate(...)` | Modify the generated map |

### Combat lifecycle

| Method | When it fires |
|--------|--------------|
| `BeforeCombatStart()` | Before combat begins (setup phase) |
| `BeforeCombatStartLate()` | Late setup, after most initialization |
| `AfterCreatureAddedToCombat(creature)` | When a creature joins the combat |
| `AfterCombatVictoryEarly(room)` | Immediately on victory, before rewards |
| `AfterCombatVictory(room)` | On combat victory |
| `AfterCombatEnd(room)` | After the combat room is fully resolved |

### Turn flow

| Method | When it fires |
|--------|--------------|
| `BeforeSideTurnStart(...)` | Before either side's turn starts |
| `AfterPlayerTurnStartEarly(...)` | Very early in the player's turn start |
| `AfterPlayerTurnStart(...)` | At the start of the player's turn |
| `AfterPlayerTurnStartLate(...)` | Late in the player's turn start |
| `BeforePlayPhaseStart(...)` | Before the player can play cards |
| `BeforeTurnEndVeryEarly(...)` | Very early in turn end processing |
| `BeforeTurnEndEarly(...)` | Early in turn end |
| `BeforeTurnEnd(...)` | Standard turn end hook |
| `AfterTurnEnd(...)` | After turn ends |
| `AfterTurnEndLate(...)` | Late turn end — cleanup |

### Card flow

| Method | When it fires |
|--------|--------------|
| `BeforeHandDraw(...)` / `BeforeHandDrawLate(...)` | Before cards are drawn to hand |
| `AfterCardDrawnEarly(...)` | Early after a card is drawn |
| `AfterCardDrawn(...)` | After a card is drawn |
| `BeforeCardPlayed(cardPlay)` | Before a card is played |
| `AfterCardPlayed(...)` | After a card is played |
| `AfterCardPlayedLate(...)` | Late after a card is played |
| `AfterCardDiscarded(...)` | After a card is discarded |
| `AfterCardExhausted(...)` | After a card is exhausted |
| `AfterCardRetained(...)` | After a card is retained |
| `AfterCardChangedPiles(...)` | Any time a card moves between piles |
| `AfterCardEnteredCombat(...)` | When a card joins this combat |
| `AfterHandEmptied(...)` | After the hand is emptied |
| `AfterShuffle(...)` | After the discard pile is shuffled into the draw pile |
| `BeforeCardRemoved(card)` | Before a card is permanently removed |
| `ModifyCardPlayCount(...)` | Override how many times a card is played |
| `ModifyHandDraw(...)` / `ModifyHandDrawLate(...)` | Adjust the number of cards drawn |
| `TryModifyEnergyCostInCombat(...)` | Override a card's energy cost in combat |
| `TryModifyCardBeingAddedToDeck(...)` | Replace a card being added to the deck |

### Damage & block

| Method | When it fires |
|--------|--------------|
| `BeforeDamageReceived(...)` | Before a creature takes damage |
| `AfterDamageReceived(...)` | After a creature takes damage |
| `AfterDamageReceivedLate(...)` | Late after taking damage |
| `AfterDamageGiven(...)` | After a creature deals damage |
| `BeforeBlockGained(...)` | Before block is applied |
| `AfterBlockGained(...)` | After block is applied |
| `AfterBlockCleared(creature)` | After block is cleared |
| `AfterBlockBroken(creature)` | After block is broken by damage |
| `ModifyDamageAdditive(...)` | Add a flat amount to damage |
| `ModifyDamageMultiplicative(...)` | Multiply damage |
| `ModifyDamageCap(...)` | Cap the maximum damage dealt |
| `ModifyBlockAdditive(...)` | Add to block gained |
| `ModifyBlockMultiplicative(...)` | Multiply block gained |
| `ModifyHpLostBeforeOsty(...)` | Modify HP lost before Osty calculation |
| `ModifyHpLostAfterOsty(...)` | Modify HP lost after Osty calculation |

### Powers & status

| Method | When it fires |
|--------|--------------|
| `BeforePowerAmountChanged(...)` | Before a power's stack count changes |
| `AfterPowerAmountChanged(...)` | After a power's stack count changes |
| `TryModifyPowerAmountReceived(...)` | Intercept and change how much of a power is applied |
| `ModifyPowerAmountGiven(...)` | Modify the amount of a power given by this model |

### Potions

| Method | When it fires |
|--------|--------------|
| `BeforePotionUsed(potion, target)` | Before a potion is used |
| `AfterPotionUsed(potion, target)` | After a potion is used |
| `AfterPotionDiscarded(potion)` | After a potion is discarded |
| `AfterPotionProcured(potion)` | After a potion is obtained |
| `ShouldProcurePotion(potion, player)` | Return `false` to prevent obtaining a potion |

### Economy

| Method | When it fires |
|--------|--------------|
| `AfterGoldGained(player)` | After the player gains gold |
| `ShouldGainGold(amount, player)` | Return `false` to prevent a gold gain |
| `AfterStarsGained(amount, gainer)` | After stars are gained |
| `AfterStarsSpent(amount, spender)` | After stars are spent |

### Rewards & rest sites

| Method | When it fires |
|--------|--------------|
| `BeforeRewardsOffered(player, rewards)` | Before rewards are shown |
| `AfterRewardTaken(player, reward)` | After a reward is taken |
| `TryModifyRewards(...)` / `TryModifyRewardsLate(...)` | Add/remove/replace rewards |
| `TryModifyRestSiteOptions(...)` | Modify available rest site options |
| `TryModifyRestSiteHealRewards(...)` | Modify rest site heal rewards |
| `AfterRestSiteHeal(player, isMimicked)` | After healing at a rest site |

### Guard methods (`Should*`)

These methods return `bool`. Returning `false` from them **prevents** the default behavior.

| Method | What it prevents |
|--------|-----------------|
| `ShouldDie(creature)` | Prevents a creature from dying |
| `ShouldClearBlock(creature)` | Prevents block from being cleared |
| `ShouldDraw(player, fromHandDraw)` | Prevents a card draw |
| `ShouldFlush(player)` | Prevents hand flush |
| `ShouldAddToDeck(card)` | Prevents a card being added to the deck |
| `ShouldPlay(card, autoPlayType)` | Prevents a card from being auto-played |
| `ShouldTakeExtraTurn(player)` | Grants an extra turn (return `true`) |
| `ShouldStopCombatFromEnding()` | Keeps combat alive even after conditions are met |

## Tips

- **`Before*` vs `After*`** — `Before` hooks let you react before the action resolves (sometimes allowing intervention); `After` hooks let you react once the outcome is known.
- **`*Early` / `*Late` variants** — multiple models may hook the same event. Use the timing variant that gives you the ordering you need.
- **`TryModify*` methods** — these use an `out` parameter pattern. Return `true` and set the out value to override the default. Return `false` to leave it unchanged.
- **Modifier methods** — `Modify*` methods receive the current value and return a modified one. They are called for every model that can affect the value, so keep them purely additive/multiplicative rather than setting an absolute value.

For the complete method signatures and parameter types, study the decompiled `AbstractModel.cs` — see [Decompiling STS2](../setup/decompile.md).
