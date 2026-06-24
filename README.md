# MX.CodDemoReader
[![Build and Test](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/build-and-test.yml)
[![Code Quality](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/codequality.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/codequality.yml)
[![Copilot Setup Steps](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/copilot-setup-steps.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/copilot-setup-steps.yml)
[![Dependabot Auto-Merge](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/dependabot-automerge.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/dependabot-automerge.yml)
[![PR Verify](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/pr-verify.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/pr-verify.yml)
[![Release - Publish NuGet](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/release-publish-nuget.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/release-publish-nuget.yml)
[![Release - Version and Tag](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/release-version-and-tag.yml/badge.svg)](https://github.com/frasermolyneux/cod-demo-reader/actions/workflows/release-version-and-tag.yml)

## Documentation
* [Overview](/docs/overview.md) - Purpose, core components, and usage tips
* [Development Workflows](/docs/development-workflows.md) - Local build commands, versioning, and CI/CD outline

## Overview
MX.CodDemoReader is a C# library for extracting configuration metadata from Call of Duty 2/4/5 demo files that use Huffman compression. It builds Huffman trees from game-specific frequency tables, decodes the game-state block, and surfaces cvar values like map, mod, gametype, and server. The package targets net9.0 and net10.0 and publishes to NuGet as `MX.CodDemoReader` under GPL-3.0-only.

## NuGet Packages

| Package                                                               | Latest                                                                                                            | Description                                             |
| --------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------- |
| [`MX.CodDemoReader`](https://www.nuget.org/packages/MX.CodDemoReader) | [![NuGet](https://img.shields.io/nuget/v/MX.CodDemoReader.svg)](https://www.nuget.org/packages/MX.CodDemoReader/) | Demo parser for CoD2/CoD4/CoD5 Huffman-compressed files |

## Contributing
Please read the [contributing](CONTRIBUTING.md) guidance; this is a learning and development project.

## Security
Please read the [security](SECURITY.md) guidance; I am always open to security feedback through email or opening an issue.
