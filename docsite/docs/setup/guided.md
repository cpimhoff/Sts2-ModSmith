# Guided set up

This guide walks you through setting up a mod for Slay the Spire 2 from scratch. No prior experience with C# or Godot is assumed.

> [!NOTE]
> Heads up! Setting up a proper development environment for any software can be time consuming and fickle, but you can do it!

Provided is a guide to getting started, from zero to a fully working (though pretty simple) mod. This document can be followed by people of many skill levels to getting to a stable environment where you can build their first mod.

If you already have a working knowledge of C# build systems or have already built a Slay the Spire 2 mod, you likely can get yourself set up simply through [the manual instructions](./manual.md).

---

## Install .NET 9

.NET is the platform that C# mods are compiled and run on. You'll need version 9.

1. [Download the **.NET 9.0 SDK** installer for your operating system](https://dotnet.microsoft.com/download/dotnet/9.0) from Microsoft. Note, you need the full SDK, not just the runtime.
2. Run the installer

To verify it worked, open a terminal (on macOS: **Terminal** from Spotlight; on Windows: **Command Prompt** or **PowerShell**; on Linux: your terminal of choice) and run:

```bash
dotnet --version
```

You should see a version number starting with `9.`. If you see an error, restart your terminal and try again.

## Download MegaDot

MegaDot is a custom build of the Godot game engine that Slay the Spire 2 is built on. It is used to package your mod's resources.

1. Go to [https://megadot.megacrit.com/](https://megadot.megacrit.com/)
2. Download the version for your operating system

## Install Visual Studio Code

[Visual Studio Code](https://code.visualstudio.com/) (VSCode) is a free code editor that works great for C# development. You can use any other IDE, this is just my suggestion.

1. Download and install VSCode from [https://code.visualstudio.com/](https://code.visualstudio.com/)
2. Open VSCode and go to the **Extensions** panel (the square icon on the left sidebar)
3. Search for and install the C# extension (`ms-dotnettools.csharp`)

## Install ModSmith

ModSmith is the modding framework your mod will depend on. It must be present in STS2's `mods/` folder at build time.

Follow the instructions to [install ModSmith](./install.md).

## Initializing your mod from the template

A Slay the Spire 2 mod needs a fair amount of "boilerplate" (glue code) to get working. A ModSmith mod for Slay the Spire 2 can be quickly generated using a `dotnet` template:

```bash
dotnet new ... --name YourModName
```

> Note that your mod's name must be one word. It is convention to use ["PascalCase"](https://www.tuple.nl/en/knowledge-base/pascal-case) for mod names.

The template, by default, ships with a simple custom potion called "Drop of Gold". This can be disabled for mods if you pass `--no-example-content`.

## Build and run your mod

Open a terminal inside your mod folder. Run:

```bash
dotnet publish
```

This compiles your mod, and copies the result to the game's mods folder. If the build succeeds, you'll see a message like `Build succeeded`.

Once the game is running with your mod, you can find the potion in a run. To test more quickly, open the in-game developer console (using the `~` key) and type:
```bash
potion DROP_OF_GOLD
```

This will grant you the example potion. Drink it, and you'll gain some gold.

> If this didn't work, inspect the logs for your game by opening the developer console and running `open logs`.

Try making changes to `DropOfGold.cs`, and then recompile the mod:
```bash
dotnet publish
```
Restart the game, give yourself the potion again, and see if how your changes affected the potion!

Congrats! You are now a modder.

---

## Next steps

Now that your mod builds and runs, explore the rest of this documentation:

- **[Concepts](../concepts/index.md)** — understand the high-level systems (event hooks, localization, resource paths)
- **[Examples](../examples/index.md)** — browse fully-worked examples for cards, relics, events, and more
- **[API Reference](/api/ModSmith.Models.html)** — detailed documentation for every ModSmith class and method

When you're ready to start adding your own content, remove the `DropOfGold.cs` from your mod, and start building!
