# MX.CodDemoReader

A .NET library for reading metadata from Call of Duty 2, 4, and 5 Huffman-encoded demo files. Extracts game configuration, map names, server details, and other metadata without full demo playback.

## Installation

```shell
dotnet add package MX.CodDemoReader
```

## Quick Start

### Reading Demo Configuration

```csharp
using MX.CodDemoReader;

await using var stream = File.OpenRead("demo0001.dm_1");
var reader = new DemoReader(stream, GameVersion.CallOfDuty4);

IDictionary<string, string> config = reader.ReadConfiguration();

Console.WriteLine($"Map: {config["mapname"]}");
Console.WriteLine($"Server: {config["sv_hostname"]}");
Console.WriteLine($"Game Mode: {config["g_gametype"]}");
```

### Using LocalDemo for File-Based Access

```csharp
using MX.CodDemoReader;

var demo = new LocalDemo("path/to/demo0001.dm_1", GameVersion.CallOfDuty4);

Console.WriteLine($"Name: {demo.Name}");
Console.WriteLine($"Map: {demo.Map}");
Console.WriteLine($"Server: {demo.ServerName}");
Console.WriteLine($"Game Mode: {demo.GameMode}");
Console.WriteLine($"Created: {demo.Created}");
Console.WriteLine($"File Size: {demo.FileSize}");
Console.WriteLine($"Valid: {demo.IsValid}");

// Open the demo stream for further processing
await using var stream = demo.Open();
```

## Supported Games

| Game | `GameVersion` Value | Demo Extension |
|------|-------------------|----------------|
| Call of Duty 2 | `GameVersion.CallOfDuty2` | `.dm_1` |
| Call of Duty 4: Modern Warfare | `GameVersion.CallOfDuty4` | `.dm_1` |
| Call of Duty: World at War | `GameVersion.CallOfDuty5` | `.dm_1` |

## Key Types

| Type | Description |
|------|-------------|
| `DemoReader` | Parses demo file streams to extract configuration |
| `LocalDemo` | File-based `IDemo` implementation with metadata extraction |
| `IDemo` | Interface for demo file abstraction |
| `GameVersion` | Enum: `Unknown`, `CallOfDuty2`, `CallOfDuty4`, `CallOfDuty5` |

## License

This project is licensed under the [GPL-3.0-only](https://spdx.org/licenses/GPL-3.0-only.html) license.
