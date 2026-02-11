# Development Workflows

## Local build and test
- Restore, build, and pack in one step: `dotnet build src/MX.CodDemoReader.sln` (multi-targets net9.0 and net10.0 and produces the NuGet package on build).
- Tests are currently absent but `dotnet test src` is kept for workflow compatibility.

## Versioning and releases
- Versioning is managed by Nerdbank.GitVersioning via `version.json`; semantic versions are derived from commit height and tags.
- Release workflows run in two stages: `release-version-and-tag.yml` generates tags and `release-publish-nuget.yml` publishes the package to NuGet using those tags.

## GitHub workflows
- `build-and-test.yml` builds the solution (and will run tests once added).
- `pr-verify.yml` validates pull requests with build/test.
- `codequality.yml` runs static analysis/code-quality checks.
- `copilot-setup-steps.yml` guards changes to workflow files.
- `dependabot-automerge.yml` auto-merges approved Dependabot updates.
- `release-version-and-tag.yml` and `release-publish-nuget.yml` handle version tagging and NuGet publishing.
