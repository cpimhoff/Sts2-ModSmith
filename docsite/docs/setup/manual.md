# Manual set up

This page covers the requirements unique to modding Slay the Spire II. It assumes you are comfortable with .NET, C#, and standard tooling.

## Requirements

| Requirement | Notes |
|-------------|-------|
| **.NET 9 SDK** | Target framework is `net9.0` |
| **Slay the Spire II** | Installed via Steam. The build system auto-detects the default Steam library path per platform. |
| **MegaDot** | A custom Godot 4.5.1 build from [megadot.megacrit.com](https://megadot.megacrit.com/). Used to export the mod's `.pck` resource bundle. |
| **ModSmith** | The modding framework. Referenced as a NuGet package; automatically copied to the STS2 mods folder on build. |

## Project template

Use `ModTemplate` from the [ModSmith repository](https://github.com/cpimhoff/Sts2-ModSmith) as your starting point. It includes a pre-configured `.csproj`, mod manifest, and a set of working examples.

> A `dotnet new` template will be added in a future update.

## Build system overview

The `.csproj` handles:

- **Platform detection** — Windows, Linux, and macOS paths are resolved automatically based on the default Steam library location.
- **STS2 assembly references** — `sts2.dll` and `0Harmony.dll` are referenced from the game's data directory.
- **Assembly publicizer** — [`BepInEx.AssemblyPublicizer.MSBuild`](https://github.com/BepInEx/BepInEx.AssemblyPublicizer) is used to expose internal members when needed (opt-in per reference via `<Publicize>true</Publicize>`).
- **Post-build copy** — the mod DLL and manifest are copied to `{Sts2Path}/mods/{ModId}/` after each build.
- **ModSmith copy** — ModSmith's DLL, `.pck`, and manifest are copied to `mods/ModSmith/` automatically.
- **Resource export** — `dotnet publish` invokes MegaDot headless to export the mod's Godot resources to a `.pck` file alongside the DLL.

## Path configuration

Default paths per platform:

| Platform | MegaDot | STS2 data dir |
|----------|---------|---------------|
| Windows  | `C:/megadot/MegaDot_v4.5.1-stable_mono_win64.exe` | `{SteamLibrary}/common/Slay the Spire 2/data_sts2_windows_x86_64` |
| Linux    | `{SteamLibrary}/common/MegaDot/godot.x11.opt.tools.64` | `{SteamLibrary}/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64` |
| macOS    | `~/Applications/MegaDot.app/Contents/MacOS/Godot` | `{SteamLibrary}/common/Slay the Spire 2/SlayTheSpire2.app/Contents/Resources/data_sts2_macos_x86_64` |

Override any path by setting the corresponding MSBuild property before the OS-detection block in your `.csproj`.

## Mod manifest

Each mod requires a `{ModId}.json` file at the project root:

```json
{
  "id": "YourModId",
  "name": "Your Mod Name",
  "author": "your-name",
  "description": "Short description",
  "version": "v1.0.0",
  "has_pck": true,
  "has_dll": true,
  "dependencies": ["ModSmith"],
  "affects_gameplay": true
}
```

The `id` field must match the `ModId` constant in your entry point class and the name of the inner resource folder.

## Entry point

Mods are initialized via `[ModInitializer]`:

```csharp
[ModInitializer(nameof(Initialize))]
public static class YourModMain
{
    public const string ModId = "YourModId";
    public static ResourcePaths Res { get; } = new(ModId);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        // Register your content here
        Registry.RegisterPotion<MyPotion>();
    }
}
```

## Resource directory

Godot resources (images, localization files, etc.) belong in `{ModId}/` at the project root. This directory is exported as a `.pck` on `dotnet publish`. Reference paths via `ResourcePaths` — see [Directory structure](../concepts/directory-structure.md).
