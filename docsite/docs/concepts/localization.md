# Localization

All player-visible text in Slay the Spire II is stored in JSON localization files and looked up at runtime by key. Your mod must provide these files for any content it adds.

## File layout

Localization files live inside your mod's resource folder under `localization/eng/`:

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

### Including files in the build

Localization files must be declared as `AdditionalFiles` in your `.csproj` so they are included in the `.pck`:

```xml
<ItemGroup>
    <AdditionalFiles Include="YourMod/localization/**/*.json"/>
</ItemGroup>
```

`ModTemplate` includes this for you.

---

## Key naming conventions

### Cards

```json
{
  "MY_CARD.title": "Card Name",
  "MY_CARD.description": "Card description text.",
  "MY_CARD.someBanter": "Optional banter line."
}
```

### Potions

```json
{
  "MY_POTION.title": "Potion Name",
  "MY_POTION.description": "Potion description text."
}
```

### Relics

```json
{
  "MY_RELIC.title": "Relic Name",
  "MY_RELIC.description": "Relic description text.",
  "MY_RELIC.flavor": "Flavor text shown beneath the description."
}
```

### Powers

```json
{
  "MY_POWER.title": "Power Name",
  "MY_POWER.description": "Power description text."
}
```

### Events

Events have a nested structure reflecting their state machine. Each page and each option within a page gets its own keys:

```json
{
  "MY_EVENT.title": "Event Title",

  "MY_EVENT.pages.INITIAL.description": "Introductory page text.",
  "MY_EVENT.pages.INITIAL.options.CHOICE_A.title": "First choice",
  "MY_EVENT.pages.INITIAL.options.CHOICE_A.description": "What this choice does.",
  "MY_EVENT.pages.INITIAL.options.CHOICE_B.title": "Second choice",
  "MY_EVENT.pages.INITIAL.options.CHOICE_B.description": "What this choice does.",

  "MY_EVENT.pages.OUTCOME_A.description": "Result of choosing A.",
  "MY_EVENT.pages.OUTCOME_B.description": "Result of choosing B."
}
```

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

---

## Text markup

The localization system supports inline markup tags for visual styling:

| Tag | Effect |
|-----|--------|
| `[gold]...[/gold]` | Gold-colored text (for gold/coins) |
| `[blue]...[/blue]` | Blue-colored text (for numbers/values) |
| `[green]...[/green]` | Green-colored text (positive effects) |
| `[red]...[/red]` | Red-colored text (negative effects, damage) |
| `[sine]...[/sine]` | Animated sine-wave text |
| `[jitter]...[/jitter]` | Animated jittering text |

Example:

```json
"MY_POTION.description": "Gain [blue]{Gold}[/blue] [gold]Gold[/gold]."
```

---

## Static hover tips

`static_hover_tips.json` defines reusable tooltip text that can appear when a player hovers over a keyword. Use this for custom keywords or mechanics that need explanation across multiple cards.

```json
{
  "MY_KEYWORD": "Description of what this keyword does."
}
```

---

## Custom keys

Your model can read arbitrary keys from the localization tables beyond the standard ones. The `ModTemplate` card example uses this for banter lines:

```json
{
  "COIN_FLIP.headsBanter": "[green]Heads![/green]",
  "COIN_FLIP.tailsBanter": "[red]Tails.[/red]"
}
```

These are read in code via the mod's localization table. Check the [API reference](https://github.com/cpimhoff/Sts2-ModSmith) and the example implementations in `ModTemplate` for how to retrieve custom keys at runtime.

---

## Missing keys

If a localization key is not found at runtime, ModSmith logs a warning and returns a fallback string in the form `"tableName.key"`. This means missing text will be visible in-game as raw key strings rather than causing a crash, which makes it easy to spot during development.
