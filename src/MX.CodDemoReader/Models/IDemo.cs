namespace MX.CodDemoReader.Models;

/// <summary>
/// Represents a demo source and metadata.
/// </summary>
public interface IDemo
{
    /// <summary>
    /// Gets the game version of the demo.
    /// </summary>
    GameVersion Version { get; }

    /// <summary>
    /// Gets the demo name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the demo creation time in UTC.
    /// </summary>
    DateTime Created { get; }

    /// <summary>
    /// Gets the map name.
    /// </summary>
    string? Map { get; }

#pragma warning disable CA1716
    /// <summary>
    /// Gets the mod name.
    /// </summary>
    string? Mod { get; }
#pragma warning restore CA1716

    /// <summary>
    /// Gets the game mode.
    /// </summary>
    string? GameMode { get; }

    /// <summary>
    /// Gets the server name.
    /// </summary>
    string? ServerName { get; }

    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    long FileSize { get; }

    /// <summary>
    /// Opens a read stream for the demo.
    /// </summary>
    /// <returns>The opened stream.</returns>
    Stream Open();
}