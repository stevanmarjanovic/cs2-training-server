# Custom CS2 Server

A reusable Counter-Strike 2 dedicated practice/training server built with [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp).

The initial goal is to create custom practice scenarios directly on standard CS2 maps, starting with manually placed walls/fences. Later, the project can grow into reusable training game modes, custom spawns, planted-bomb scenarios, and other practice tools.

## Planned Features

- [x] CounterStrikeSharp plugin support
- [x] Command for reading player position and rotation
- [x] Spawn custom props at chosen coordinates
- [x] Precache models that are not already available in the current map manifest
- [x] Spawn Dust2 chain-link fence models
- [x] Chain multiple 128-unit fence segments using position, yaw, and amount
- [ ] Add collision to custom walls/fences
- [ ] Store wall definitions per map
- [ ] Commands/tools for easier in-game wall placement
- [ ] Custom player/bot spawn positions
- [ ] Practice scenarios such as planted-bomb retakes
- [ ] Additional reusable training/game modes
- [ ] Simple deployment/setup for other servers

## Current Commands

### `!pos` / `css_pos`

Prints the current player's world position and view rotation.

Useful for gathering coordinates while standing where a custom object should be placed.

Example:

```text
Position: X=-1245.32, Y=833.74, Z=128.03
Rotation: Pitch=4.28, Yaw=-91.37, Roll=0.00
```

### Test fence/prop command

The current prototype can spawn models at hard-coded coordinates and create a chain of fence segments using:

- starting `Vector`
- yaw
- number of segments

The Dust2 fence currently uses two models:

```text
models/props/de_dust/hr_dust/dust_fences/dust_chainlink_fence_001_128.vmdl
models/props/de_dust/hr_dust/dust_fences/dust_chainlink_fence_001_128_links.vmdl
```

The frame and chain-link mesh are spawned at the same position and rotation.

## Prerequisites

### Server

- Linux server
- SteamCMD
- CS2 Dedicated Server files (`appid 730`)
- Metamod:Source
- CounterStrikeSharp
- CounterStrikeSharp runtime package matching the installed CSS version

### Plugin Development

- .NET SDK supported by the current CounterStrikeSharp version
- CounterStrikeSharp API NuGet/package reference
- Any C# IDE/editor, e.g. Rider, Visual Studio, or VS Code

## Basic Setup

### 1. Install CS2 with SteamCMD

Example:

```bash
steamcmd +login anonymous +force_install_dir /mnt/cs2/cs2-server +app_update 730 validate +quit
```

### 2. Install Metamod:Source

Install Metamod into the CS2 `game/csgo` directory and make sure `gameinfo.gi` contains the Metamod search path.

For example:

```text
Game    csgo/addons/metamod
```

Verify from the CS2 server console:

```text
meta list
```

> **Important:** a CS2 update may overwrite `game/csgo/gameinfo.gi`. If `meta` suddenly becomes an unknown command after updating the server, check the Metamod search path again.

### 3. Install CounterStrikeSharp

Copy CounterStrikeSharp and its runtime into:

```text
game/csgo/addons/counterstrikesharp/
```

CounterStrikeSharp is loaded through Metamod.

### 4. Install the plugin

Build the C# plugin and copy its output into:

```text
game/csgo/addons/counterstrikesharp/plugins/<PluginName>/
```

Restart/reload as appropriate.

## Model Precaching

A model existing in a CS2 VPK does **not** necessarily mean it is available to `SetModel()` on the current map.

Models used by this plugin are added during the map precache phase:

```csharp
RegisterListener<Listeners.OnServerPrecacheResources>(manifest =>
{
    manifest.AddResource(FenceFrameModel);
    manifest.AddResource(FenceLinksModel);
});
```

After adding new resources, restart the server or change the map so the precache listener runs.

## Coordinate Notes

For small manual adjustments on a static radar:

```text
North: Y +
South: Y -
East:  X +
West:  X -
Up:    Z +
Down:  Z -
```

Fence orientation uses yaw:

```csharp
new QAngle(0, yaw, 0)
```

The current Dust2 fence is 128 units long, so chained segments are offset by 128 units along the fence's local side axis.

## Development Workflow

1. Join the server.
2. Stand where an object/wall should begin.
3. Use `!pos` to collect coordinates and yaw.
4. Put those values into the wall/fence definition.
5. Spawn a short fence chain and fine-tune X/Y/Z.
6. Once placement is correct, save the values as part of the map's practice configuration.

The next major step is implementing reliable collision independently from the visual fence models.
