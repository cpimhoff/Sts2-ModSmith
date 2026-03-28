# Guided set up

This guide walks you through setting up a mod for Slay the Spire II from scratch. No prior experience with C# or Godot is assumed — we'll explain each step along the way.

## What you'll need

Before writing any code, you need to install a few tools:

1. **Slay the Spire II** — installed via Steam
2. **.NET 9 SDK** — the toolchain used to compile your mod
3. **MegaDot** — a custom build of the Godot engine used by the game
4. **Visual Studio Code** — a free code editor with good C# support

---

## Step 1: Install .NET 9

.NET is the platform that C# mods are compiled and run on. You need version 9.

1. Go to [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
2. Download the **.NET 9.0 SDK** installer for your operating system (not just the Runtime — you need the full SDK)
3. Run the installer

To verify it worked, open a terminal (on macOS: **Terminal** from Spotlight; on Windows: **Command Prompt** or **PowerShell**; on Linux: your terminal of choice) and run:

```
dotnet --version
```

You should see a version number starting with `9.`. If you see an error, restart your terminal and try again.

---

## Step 2: Download MegaDot

MegaDot is a custom build of the Godot game engine that Slay the Spire II is built on. It is used to package your mod's resources.

1. Go to [https://megadot.megacrit.com/](https://megadot.megacrit.com/)
2. Download the version for your operating system

**Installation location by platform:**

| Platform | Expected location |
|----------|------------------|
| macOS    | `~/Applications/MegaDot.app` |
| Linux    | Typically placed in Steam's MegaDot folder automatically |
| Windows  | `C:\megadot\` (the build system expects the `.exe` here by default) |

> **Note:** If you place MegaDot somewhere else, you will need to update the `MegaDotPath` property in your `.csproj` file (covered below).

---

## Step 3: Set up Visual Studio Code

[Visual Studio Code](https://code.visualstudio.com/) (VSCode) is a free code editor that works great for C# development.

1. Download and install VSCode from [https://code.visualstudio.com/](https://code.visualstudio.com/)
2. Open VSCode and go to the **Extensions** panel (the square icon on the left sidebar, or `Cmd+Shift+X` on macOS / `Ctrl+Shift+X` on Windows/Linux)
3. Search for and install these extensions:
   - **C#** (`ms-dotnettools.csharp`) — syntax highlighting, error highlighting, and code navigation for C#
   - **C# Dev Kit** (`ms-dotnettools.csdevkit`) — optional but recommended; adds a solution explorer and richer editor features

---

## Step 4: Get the ModTemplate

The easiest way to start a new mod is to copy `ModTemplate`, which is included in the [ModSmith GitHub repository](https://github.com/cpimhoff/Sts2-ModSmith).

> **Note:** A `dotnet new` template for generating a fresh mod project will be added in a future update. For now, copy the folder manually.

### Downloading the repository

If you are familiar with Git, clone the repository:

```
git clone https://github.com/cpimhoff/Sts2-ModSmith.git
```

If not, you can download it as a ZIP:
1. Go to [https://github.com/cpimhoff/Sts2-ModSmith](https://github.com/cpimhoff/Sts2-ModSmith)
2. Click the green **Code** button, then **Download ZIP**
3. Extract the ZIP somewhere on your computer

### Copy the template

Inside the repository you downloaded, find the `ModTemplate/` folder. Copy the entire folder to wherever you want to keep your mod project — for example, `~/projects/MyMod/`.

---

## Step 5: Customize the template for your mod

Open your copy of `ModTemplate` in VSCode: **File → Open Folder**, then select your mod folder.

You need to update two files to give your mod a unique identity.

### 1. Rename the project files

The files currently named `ModTemplate.csproj` and `ModTemplate.json` should be renamed to match your mod. For example, if your mod is called `AwesomeMod`:

- `ModTemplate.csproj` → `AwesomeMod.csproj`
- `ModTemplate.json` → `AwesomeMod.json`
- The inner resource folder `ModTemplate/` → `AwesomeMod/`

You can do this by right-clicking the files in VSCode's Explorer panel and selecting **Rename**.

### 2. Update `ModTemplate.json` (the manifest)

Open your renamed `AwesomeMod.json`. It looks like this:

```json
{
  "id": "ModTemplate",
  "name": "ModTemplate",
  "author": "your-name",
  "description": "Mod for Slay the Spire II",
  "version": "v0.0.0",
  "has_pck": true,
  "has_dll": true,
  "dependencies": ["ModSmith"],
  "affects_gameplay": true
}
```

Update the fields:
- `"id"` — a unique ID for your mod, no spaces (e.g. `"AwesomeMod"`). This is used internally by the game and ModSmith.
- `"name"` — the human-readable name shown in the mod list
- `"author"` — your name or handle
- `"description"` — a short description of your mod

### 3. Update the mod ID in code

Open `src/ModTemplate.cs` (or rename this file to `src/AwesomeMod.cs`). Find this line:

```csharp
public const string ModId = "ModTemplate"; // Must match the id in `ModTemplate.json`
```

Change it to match the `id` you set in the JSON:

```csharp
public const string ModId = "AwesomeMod"; // Must match the id in `AwesomeMod.json`
```

Also update the `namespace` at the top of the file if you want to use your mod's name as the namespace:

```csharp
namespace AwesomeMod;
```

---

## Step 6: Verify your build paths

Open your `.csproj` file. Near the top you'll see platform-specific sections that tell the build system where to find Slay the Spire II and MegaDot.

**If STS2 is in your default Steam library,** these paths should work automatically.

**If you installed Steam to a non-standard location,** find the section for your OS and update `SteamLibraryPath`. For example on macOS:

```xml
<SteamLibraryPath>$(HOME)/Library/Application Support/Steam/steamapps</SteamLibraryPath>
```

**On Windows**, MegaDot's path is hardcoded:

```xml
<MegaDotPath>C:/megadot/MegaDot_v4.5.1-stable_mono_win64.exe</MegaDotPath>
```

If you installed MegaDot elsewhere, update this path to match.

---

## Step 7: Install ModSmith

ModSmith is the modding framework your mod depends on. It must be present in STS2's `mods/` folder at build time.

The first time you build (next step), ModSmith will be automatically downloaded from NuGet and copied into the mods folder. However, you need to **run STS2 at least once with mods enabled** so the game creates its `mods/` folder.

1. Launch Slay the Spire II from Steam
2. Enable mods from the main menu if prompted
3. Exit the game

---

## Step 8: Build and run your mod

Open a terminal inside your mod folder. In VSCode you can do this with **Terminal → New Terminal**.

Run:

```
dotnet build
```

This compiles your mod, copies the DLL to the game's mods folder, and copies ModSmith there as well. If the build succeeds, you'll see a message like `Build succeeded`.

### Running the game with your mod

To launch Slay the Spire II with your mod active, run:

```
dotnet publish
```

This packages the mod's resources (via MegaDot) and launches the game. Your mod should appear in the mod list.

> **First build taking a while?** That's normal — .NET is downloading dependencies the first time.

---

## Step 9: See it in action

The ModTemplate ships with several example items. One of them, **Drop of Gold**, is a potion that simply grants the player some gold when used. This is a great first thing to look for in-game to confirm everything is working.

Once the game is running with your mod, you can find the potion in a run. Open the console (if available) or simply play until you find a potion reward — Drop of Gold should appear as a possible drop.

The source for this potion is in `src/examples/potions/DropOfGold.cs`. It's a simple, self-contained example that shows the full pattern for adding new content.

---

## Next steps

Now that your mod builds and runs, explore the rest of this documentation:

- **[Concepts](../concepts/index.md)** — understand the high-level systems (event hooks, localization, resource paths)
- **[Examples](../examples/index.md)** — browse fully-worked examples for cards, relics, events, and more
- **[API Reference](https://github.com/cpimhoff/Sts2-ModSmith)** — detailed documentation for every ModSmith class and method

When you're ready to start adding your own content, remove the `ModTemplateExamples.RegisterAll()` call from your main class and replace it with your own registrations.
