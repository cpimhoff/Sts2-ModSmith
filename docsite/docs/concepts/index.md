# Concepts

This section covers the high-level ideas you need to understand to write effective mods for Slay the Spire II with ModSmith.

## [Event hooks](event-hooks.md)

All game entities — cards, relics, potions, powers, and more — inherit from `AbstractModel`, which defines a large set of virtual methods that the engine calls at specific moments during a run or combat. Overriding these methods is the primary way to give your content interesting behaviors.

## [Directory structure](directory-structure.md)

A mod project has two distinct areas: `src/` for C# code and `{ModId}/` for Godot resources like images and localization files. Understanding this structure, and how to reference resources correctly using the `ResourcePaths` helper, is essential for packaging a working mod.

## [Localization](localization.md)

All player-visible text in STS2 is looked up by key from JSON localization files. This page covers the file layout, key naming conventions, dynamic variable interpolation, and text markup supported by the localization system.
