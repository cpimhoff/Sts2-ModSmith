# Decompiling Slay the Spire II

When writing mods, it is often necessary to read the game's source code to understand how systems work, what hook parameters mean, or which classes to subclass. Slay the Spire II ships as a compiled binary, but it can be decompiled with publicly available tooling.

## Tools

**[GDRETools](https://github.com/GDRETools/gdsdecomp)** (Godot Reverse Engineering Tools) can extract and decompile Godot `.pck` files, including the `.gdscript` and binary resource files bundled with STS2.

For the C# assembly (`sts2.dll`), any standard .NET decompiler works:

- **[ILSpy](https://github.com/icsharpcode/ILSpy)** — cross-platform, free
- **[dnSpy](https://github.com/dnSpy/dnSpy)** — Windows, feature-rich
- **dotPeek** — JetBrains, free

## Decompiling the C# assembly

The compiled C# code for STS2's game logic lives in `sts2.dll`, located in STS2's data directory:

| Platform | Path |
|----------|------|
| Windows  | `{SteamLibrary}/common/Slay the Spire 2/data_sts2_windows_x86_64/sts2.dll` |
| Linux    | `{SteamLibrary}/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll` |
| macOS    | `{SteamLibrary}/common/Slay the Spire 2/SlayTheSpire2.app/Contents/Resources/data_sts2_macos_x86_64/sts2.dll` |

Open this file in ILSpy (or your preferred decompiler) to browse and search the full game source. This is the primary reference for understanding `AbstractModel` hooks, game entities, commands, and more.

## Extracting Godot resources with GDRETools

STS2's Godot resources (scenes, images, GDScript files) are bundled in a `.pck` file alongside the executable. GDRETools can extract these.

### Steps

1. Download the latest release of GDRETools from [github.com/GDRETools/gdsdecomp/releases](https://github.com/GDRETools/gdsdecomp/releases) for your platform.

2. Launch the GDRETools application.

3. Use **Recover Project** (or the equivalent option in your version) and point it at the STS2 `.pck` file. The `.pck` is located in the same directory as the STS2 executable:

   | Platform | `.pck` location |
   |----------|----------------|
   | Windows  | `{SteamLibrary}/common/Slay the Spire 2/SlayTheSpire2.pck` |
   | Linux    | `{SteamLibrary}/common/Slay the Spire 2/SlayTheSpire2.pck` |
   | macOS    | `{SteamLibrary}/common/Slay the Spire 2/SlayTheSpire2.app/Contents/Resources/SlayTheSpire2.pck` |

4. Choose an output directory. GDRETools will extract all resource files to that location.

5. Browse the extracted files. GDScript files, scene files (`.tscn`), and other assets will be readable after extraction.

## What to look for

The most useful files for modders are typically:

- **`sts2.dll`** — the core game logic. Search this for the classes and methods you want to hook.
- **`AbstractModel.cs`** (decompiled from the DLL) — the base class for all game models. See [Event hooks](../concepts/event-hooks.md) for an overview of its hook system.
- **Resource files** — use GDRETools output to find asset paths you can reference via `ResourcePaths.Global(...)`.

> The decompiled source is for reference only. Do not redistribute decompiled game code.
