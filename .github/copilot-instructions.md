# Copilot Instructions

This repository publishes `MX.CodDemoReader`, a library for decoding Call of Duty 2, 4, and 5 demo streams and extracting configuration metadata.

## Runtime and layout

- SDK: `10.0.301` from `global.json`; the package targets `net9.0` and `net10.0`.
- Solution: `src/MX.CodDemoReader.slnx`.
- Implementation and package project: `src/MX.CodDemoReader`.
- There is currently no test project.

## Repository rules

- Call of Duty 2 uses the Quake 3 frequency table without the Call of Duty 4/5 leading-byte skip.
- Call of Duty 4 and 5 use the Call of Duty 4 frequency table and skip the leading byte before decoding.
- Preserve bit-position accounting in `DemoMessage`, little-endian integer reads, clear short-read exceptions, and the exact order and length of both 256-entry frequency arrays.
- Preserve `LocalDemo` path normalization and corrupted-file behavior unless the task explicitly changes those contracts.
- Public readers and models are NuGet consumer contracts.
- Package ID, target frameworks, package metadata, and NBGV configuration in `version.json` are release boundaries.

## Validation

```pwsh
dotnet build src/MX.CodDemoReader.slnx
dotnet test src/MX.CodDemoReader.slnx
dotnet format src/MX.CodDemoReader.slnx --verify-no-changes
```

See `docs/overview.md` for the decoding model and supported games.
