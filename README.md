# MX.CodDemoReader

Call of Duty 2/4/5 Huffman demo reader that extracts config metadata for downstream tooling. Packaged as `MX.CodDemoReader` on NuGet.

## Build & test
- `dotnet build src/MX.CodDemoReader.sln`
- `dotnet test src` (no tests yet; keeps workflow compatibility)

## Releases
- Versioning uses Nerdbank.GitVersioning (`version.json`) with tags `v<semver>`.
- CI/CD aligns with `api-client-abstractions`: feature/bugfix/hotfix pushes run Build and Test; PRs run PR Verify; main pushes run Release - Version and Tag, followed by Release - Publish NuGet.

## Package info
- Package Id: `MX.CodDemoReader`
- License: GPL-3.0-only
- Repository: https://github.com/frasermolyneux/cod-demo-reader
