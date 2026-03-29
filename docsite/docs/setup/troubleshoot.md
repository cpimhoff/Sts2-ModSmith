# Troubleshooting

## Build errors

### `Slay the Spire 2 data not found at path '...'`

The build system could not find STS2's data directory. This means either STS2 is not installed, or it is installed to a non-standard Steam library location.

**Fix:** Open your `.csproj` and set `SteamLibraryPath` explicitly in the section for your OS. For example on macOS:

```xml
<PropertyGroup Condition="'$(IsOSX)' == 'true'">
    <SteamLibraryPath>/Volumes/ExternalDrive/Steam/steamapps</SteamLibraryPath>
    ...
</PropertyGroup>
```

---

### `MegaDot not found at path '...'`

The build system could not find the MegaDot executable. On Windows, the path is hardcoded to `C:/megadot/...` by default. On macOS/Linux there is a default path but it may not match your install.

**Fix:** Update the `MegaDotPath` property in your `.csproj` to point to your actual MegaDot installation:

```xml
<MegaDotPath>/path/to/your/MegaDot</MegaDotPath>
```

Refer to the [Manual set up](manual.md) page for the expected path per platform.

---

### `The type or namespace 'ModSmith' could not be found`

ModSmith's DLL is not present in the STS2 mods folder, so the compiler cannot find it.

**Fix:** Run `dotnet build` once and let it complete — the build process copies ModSmith from NuGet to the mods folder automatically. If the folder doesn't exist yet, launch STS2 at least once so it creates the `mods/` directory, then build again.

---

### `DuplicateModelException` at runtime

Two mod items share the same model ID. Every registered type must have a unique ID derived from its class name and namespace.

**Fix:** Ensure there are no two classes with the same name in the same namespace across all active mods.

---

## Runtime issues

### My mod doesn't appear in the mod list

- Verify that `{ModId}.json` is present in `mods/{ModId}/` alongside the DLL.
- Check that the `"id"` in the JSON matches the folder name exactly (case-sensitive on macOS/Linux).
- Ensure `"has_dll": true` and `"has_pck": true` are set if your mod has both a DLL and a resource pack.

---

### My mod appears but my custom content is missing

- Check the game's log for errors referencing your mod's namespace. A missing localization key or a bad resource path will silently fail or log a warning rather than crash.
- If you are missing a `.pck`, run `dotnet publish` rather than just `dotnet build`. The `.pck` is only generated during publish.
- Confirm that your content is registered in your `Initialize()` method.

---

### Localization text is showing raw key strings (e.g. `"potions.MY_POTION.title"`)

Your localization JSON file is missing the key, has a typo in the key name, or the JSON file is not being included in the build.

**Fix:**
- Double-check the key names in your JSON match exactly what the model expects (case-sensitive).
- Ensure the JSON files are included in your `.csproj` via `<AdditionalFiles Include="{ModId}/localization/**/*.json"/>`.
- Run `dotnet publish` to re-export the `.pck` with the latest JSON changes.
