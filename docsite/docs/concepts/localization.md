# Localization Keys

All player-visible text in Slay the Spire 2 is stored in JSON localization files and looked up at runtime by key. Your mod must provide these files for any content it adds.

## File layout

Localization files live inside your mod's resource folder under `localization/<lang>/`:

```
YourMod/
└── localization/
    └── eng/
        ├── cards.json
        ├── potions.json
        ├── relics.json
        ├── powers.json
        ├── events.json
        ├── ancients.json
        ├── cards_keywords.json
        └── static_hover_tips.json
```

Each file is a flat JSON object mapping string keys to string values. You only need to include the files relevant to your mod's content.

> Unlike other resources included in your mod's resource directory, which are copied in directly, Slay the Spire 2 has a special case for localization files, where all mod's localization files are _merged together_ during mod initialization.

## Required keys

Most content will implicitly look for keys based on its derived ID. For example, a card class named `MyCard` will look for its name at the key `MY_CARD.title`. ModSmith base classes document what keys will be required.

If a localization key is not found at runtime, ModSmith logs a warning and returns a fallback string in the form `"tableName.key"`. This means missing text will be visible in-game as raw key strings rather than causing a crash. This makes it easier to spot during development.

---

## Dynamic variable interpolation

Descriptions can embed dynamic values using curly-brace syntax. The variable name must match a `DynamicVariable` registered on your model:

```json
"MY_CARD.description": "Deal {Damage} damage and gain {Block} block."
```

These values are resolved at display time from the model's registered dynamic variables, so they automatically update when the card is upgraded or when other effects modify the values.

### Conditional text

You can show different text depending on whether the item is upgraded:

```json
"MY_CARD.description": "Deal {Damage} damage.{IfUpgraded:show:  Upgraded bonus.|}"
```

- `{IfUpgraded:show: text|}` — shows ` text` if upgraded, shows nothing otherwise
- The `|` separates the "true" case from the "false" case (empty = nothing shown)

## Text markup

The localization system supports inline markup tags for visual styling, such as:

| Tag | Effect |
|-----|--------|
| `[gold]...[/gold]` | Gold-colored text (for keywords) |
| `[blue]...[/blue]` | Blue-colored text (for numbers/values) |
| `[green]...[/green]` | Green-colored text (positive effects) |
| `[red]...[/red]` | Red-colored text (negative effects, damage) |
| `[sine]...[/sine]` | Animated sine-wave text |
| `[jitter]...[/jitter]` | Animated jittering text |

Example:

```json
"DROP_OF_GOLD.description": "Gain [blue]{Gold}[/blue] [gold]gold[/gold]."
```
