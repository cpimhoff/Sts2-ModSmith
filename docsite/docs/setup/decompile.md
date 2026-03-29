# Decompiling Slay the Spire 2

When writing mods, it is often necessary to read the game's source code to understand how systems work, what to patch, or as reference to how to build certain behaviors. Slay the Spire 2 ships as a compiled binary, but it can be decompiled with publicly available tooling.

> [!WARNING]
> Slay the Spire 2 is compiled without obfuscation, allowing anyone who legally owns a copy of the game to inspect its full source code and assets for reference and learning. However, this does not give you permission to _redistribute_ decompiled game code or assets.

## Decompiling with GDRETools

**[GDRETools](https://github.com/GDRETools/gdsdecomp)** (Godot Reverse Engineering Tools) can extract and decompile Godot `.pck` files, including all CS and resource files (images, scenes, etc.) bundled with STS2. This app is cross-platform and available for Windows, macOS, and Linux.

1. Download the latest release of GDRETools from [github.com/GDRETools/gdsdecomp/releases](https://github.com/GDRETools/gdsdecomp/releases) for your platform.

2. Launch the GDRETools application.

3. Use **Recover Project** (or the equivalent option in your version) and point it at the STS2 `.pck` file. The `.pck` is located in the same directory as the STS2 executable:

   | Platform | `.pck` location |
   |----------|----------------|
   | Windows  | C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2\sts2.pck |
   | Linux    | ~/.local/share/Steam/steamapps/common/Slay the Spire 2/sts2.pck |
   | macOS    | ~/Library/Application Support/Steam/steamapps/common/Slay the Spire 2/SlayTheSpire2.app/Contents/Resources/Slay the Spire 2.pck |

4. Choose an output directory. GDRETools will extract all resource files to that location.

> [!WARNING]
> Do not extract the Slay the Spire 2 project into the same repository or folder as your mod. Doing so may cause your mod to bundle content that you do not have permission to redistribute.

## What to look for

The most useful references for modders are typically:

- `src/Core`: All the C# logic for the game.
    - `src/Core/Models/` includes all the base asset types for the game, and all the implementations for cards, potions, relics, etc.
- `localization/eng`: All the localization keys and string values for all game content.
- `images/`: All the sprites for the game.
- `scenes/`: All the Godot scenes for the game.
