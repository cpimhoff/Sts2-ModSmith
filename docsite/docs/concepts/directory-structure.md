# Directory structure

A ModSmith mod project has two distinct areas: C# source code and Godot resources. Understanding their separation is important for the build system to package your mod correctly.

## Layout

```
YourMod/
├── YourMod.csproj          # Build configuration
├── YourMod.json            # Mod manifest
│
├── src/                    # C# source code
│   ├── YourMod.cs          # Entry point ([ModInitializer])
│   └── ...                 # Your cards, relics, potions, etc.
│
└── YourMod/                # Godot resources (same name as mod ID)
    ├── localization/
    │   └── eng/
    │       ├── cards.json
    │       ├── potions.json
    │       └── ...
    └── images/
        └── ...
```

### `src/` — C# code

All C# source files live under `src/`. This is conventional — the build system does not enforce any particular subdirectory structure within `src/`, so you can organize your cards, relics, and other content however you like.

### `{ModId}/` — Godot resources

The folder named after your mod ID (matching the `"id"` in your manifest) contains every resource that needs to be packaged into the mod's `.pck` file. This includes:

- **Localization files** (`localization/eng/*.json`)
- **Custom images** for cards, relics, potions, etc.
- **Any other Godot-compatible asset** your mod references at runtime

The `.pck` is generated when you run `dotnet publish`. It is loaded by the game at startup and makes your resources accessible via Godot's `res://` path system.

## Referencing resources: `ResourcePaths`

Godot uses virtual paths of the form `res://...` to reference resources at runtime. Your mod's `.pck` maps your `{ModId}/` folder to `res://{ModId}/`. To construct these paths without string concatenation, use the `ResourcePaths` helper.

ModSmith models typically expose a static `Res` instance on your main class (set up in `ModTemplate` for you):

```csharp
public static ResourcePaths Res { get; } = new(ModId);
```

### `Res.Mod(path)` — your mod's resources

```csharp
Res.Mod("images/my_card.png")
// → res://YourMod/images/my_card.png
```

Use this for any asset you've placed inside `{ModId}/`.

### `Res.Global(path)` — STS2's built-in resources

```csharp
Res.Global("sts2/images/cards/some_card.png")
// → res://sts2/images/cards/some_card.png
```

Use this to reference assets that ship with Slay the Spire II. You can find available paths by extracting the game's `.pck` — see [Decompiling STS2](../setup/decompile.md).

### `Res.ModSmith(path)` — ModSmith's built-in resources

```csharp
Res.ModSmith("images/placeholders/card_default.png")
// → res://ModSmith/images/placeholders/card_default.png
```

ModSmith ships default placeholder images and other shared resources. A full index is available in the [ModSmith repository](https://github.com/cpimhoff/Sts2-ModSmith/tree/main/ModSmith/ModSmith).

## Example: referencing a custom card image

Assuming you have placed an image at `YourMod/images/my_card.png`:

```csharp
public override string CardArtPath => YourModMain.Res.Mod("images/my_card.png");
```

If you haven't made custom art yet, you can use a ModSmith placeholder:

```csharp
public override string CardArtPath => YourModMain.Res.ModSmith("images/placeholders/card_default.png");
```
