# Overview

MX.CodDemoReader is a .NET library that extracts configuration metadata from Call of Duty 2/4/5 demo files that are Huffman-compressed. It targets net9.0 and net10.0, shipping as the NuGet package `MX.CodDemoReader`.

## Core concepts
- `DemoReader` decodes a demo stream using Huffman frequency tables (Quake3 table for Call of Duty 2; Call of Duty 4 table reused for Call of Duty 4/5) and pulls out the game-state config block.
- `DemoMessage` wraps bit-level reads for demo payloads, supporting byte, short, int, string, and arbitrary bit reads plus Huffman decode.
- `HuffmanTree`/`HuffmanNode` builds the decode tree from the frequency arrays in `HuffmanFrequencies` (arrays must remain length 256).
- `LocalDemo` implements `IDemo` for on-disk demos, derives map/mod/gametype/server plus referenced IWD/FF files, and normalizes usermaps paths (e.g., `usermaps/mp_caen2_load` => `usermaps/mp_caen2/mp_caen2_load`).

## Supported games
- Call of Duty 2: skips no header bytes and uses Quake3 frequencies.
- Call of Duty 4 and Call of Duty 5: skips the first byte in the stream and uses the Call of Duty 4 frequency table.

## Usage
```csharp
using MX.CodDemoReader;
using MX.CodDemoReader.Models;

var demo = new LocalDemo("/path/to/demo.dm_1", GameVersion.CallOfDuty4);
if (!demo.IsValid)
{
    throw new InvalidOperationException("Demo is missing or corrupted.");
}

var config = new DemoReader(demo.Open(), demo.Version).ReadConfiguration();
var mapName = config.TryGetValue("mapname", out var map) ? map : "unknown";
```

## Package details
- Package Id: `MX.CodDemoReader`
- License: GPL-3.0-only
- Repository: https://github.com/frasermolyneux/cod-demo-reader
