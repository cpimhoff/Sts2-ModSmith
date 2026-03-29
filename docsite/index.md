# ModSmith

**ModSmith** is a modding framework for [Slay the Spire 2](https://store.steampowered.com/app/2069060/Slay_the_Spire_2/) that makes it easy to add new content to the game in C#. It provides base classes, a registration system, and tooling so you can focus on adding cool nonsense into the game.

## Get started

- **Playing mods** — [Install ModSmith](docs/setup/install.md) once to enable any mod that depends on it.
- **Building mods** — New to modding? Start with the [guided setup](docs/setup/guided.md). Experienced C# developer? Jump straight to the [manual](docs/setup/manual.md).

## Supported content

ModSmith supports the following content types out of the box:

- 🃏 **Cards** — New attacks, skills, and powers for any card pool.
- 🧪 **Potions** — Single-use items with arbitrary effects.
- 🏺 **Relics** — Persistent passive items that hook into combat and run events.
- ⚡ **Powers** — Stackable status effects applied to players or enemies.
- 🗺️ **Events** — Multi-page branching narrative encounters on the map.
- 🪙 **Ancients** — Special ancient encounter events with relic rewards.

All content hooks into the same event system the base game uses, giving you full access to everything that happens during a run.
