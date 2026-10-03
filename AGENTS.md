# cod-demo-reader

.NET library for reading Call of Duty 2, 4, and 5 Huffman-compressed demo files and extracting server configuration metadata. It publishes the `MX.CodDemoReader` NuGet package.

## Locations

- Solution: `src/MX.CodDemoReader.slnx`
- Library and package: `src/MX.CodDemoReader`
- Tests: `src/MX.CodDemoReader.Tests`
- Documentation: `docs/`

## Commands

```pwsh
dotnet build src/MX.CodDemoReader.slnx
dotnet test src/MX.CodDemoReader.slnx
dotnet format src/MX.CodDemoReader.slnx --verify-no-changes
```

## Constraints

- Preserve bit alignment, little-endian reads, short-read failures, and the distinction between Call of Duty 2 and Call of Duty 4/5 decoding.
- Keep both Huffman frequency tables ordered and exactly 256 entries long.
- Keep the frequency-table hashes and generated demo-stream regression tests current when intentionally changing the protocol decoder.
- Treat `DemoReader`, `DemoMessage`, `LocalDemo`, `IDemo`, and `GameVersion` as public package contracts.
- Keep package identity, target frameworks, and `version.json` behavior unchanged unless explicitly requested.
- Build generates the package; do not publish it during validation.

## Documentation

- [Overview and supported games](docs/overview.md)
- [Development workflows](docs/development-workflows.md)
