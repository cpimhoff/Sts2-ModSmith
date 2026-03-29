# Manual set up

This page enumerates the requirements to mod Slay the Spire 2.

## Requirements

| Requirement | Notes |
|-------------|-------|
| **Slay the Spire 2** | Installed via Steam. The build system auto-detects the default Steam library path per platform. |
| **.NET 9 SDK** | Target framework is `net9.0`. Install  |
| **MegaDot** | A custom Godot 4.5.1 build from [megadot.megacrit.com](https://megadot.megacrit.com/). Used to export the mod's `.pck` resource bundle. |
| **ModSmith** | The modding framework. [Install as a regular Slay the Spire II mod](./install.md).  |

## Project template

Use `ModTemplate` from the [ModSmith repository](https://github.com/cpimhoff/Sts2-ModSmith) as your starting point. It includes a pre-configured `.csproj`, mod manifest, and a set of working examples.

> A `dotnet new` template will be added in a future update.
