# cod-demo-reader

.NET library for reading Call of Duty 2, 4, and 5 Huffman-compressed demo files and extracting server configuration metadata. It publishes the `MX.CodDemoReader` NuGet package.

## Locations

- Solution: `src/MX.CodDemoReader.sln`
- Library and package: `src/MX.CodDemoReader`
- Documentation: `docs/`

## Commands

```pwsh
dotnet build src/MX.CodDemoReader.sln
dotnet test src/MX.CodDemoReader.sln
dotnet format src/MX.CodDemoReader.sln --verify-no-changes
```

The repository currently has no test project; the test command is retained for solution and workflow compatibility.

## Constraints

- Preserve bit alignment, little-endian reads, short-read failures, and the distinction between Call of Duty 2 and Call of Duty 4/5 decoding.
- Keep both Huffman frequency tables ordered and exactly 256 entries long.
- Treat `DemoReader`, `DemoMessage`, `LocalDemo`, `IDemo`, and `GameVersion` as public package contracts.
- Keep package identity, target frameworks, and `version.json` behavior unchanged unless explicitly requested.
- Build generates the package; do not publish it during validation.

## Documentation

- [Overview and supported games](docs/overview.md)
- [Development workflows](docs/development-workflows.md)
