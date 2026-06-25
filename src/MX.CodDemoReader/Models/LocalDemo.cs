namespace MX.CodDemoReader.Models;

/// <summary>
/// Represents a local on-disk demo file and its parsed metadata.
/// </summary>
public class LocalDemo : IDemo
{
    private bool IsCorrupted { get; }


    /// <summary>
    ///     Initializes a new instance of the <see cref="LocalDemo" /> class.
    /// </summary>
    /// <param name="path">The path to the demo file.</param>
    /// <param name="version">The version of the demo.</param>
    public LocalDemo(string path, GameVersion version)
    {
        Path = path;
        Version = version;

        try
        {
            using var stream = Open();
            var reader = new DemoReader(stream, Version);

            var config = reader.ReadConfiguration();

            _ = config.TryGetValue("mapname", out var map);
            _ = config.TryGetValue("fs_game", out var mod);
            _ = config.TryGetValue("g_gametype", out var gameType);
            _ = config.TryGetValue("sv_hostname", out var server);
            _ = config.TryGetValue("sv_referencedIwdNames", out var iwdNames);
            _ = config.TryGetValue("sv_referencedFFNames", out var ffNames);

            if (!string.IsNullOrWhiteSpace(mod) && mod.StartsWith("mods/", StringComparison.OrdinalIgnoreCase))
            {
                mod = mod[5..];
            }

            Map = map;
            Mod = mod;
            GameMode = gameType;
            ServerName = server;
            IWDs = iwdNames == null ? [] : iwdNames.Split(' ');
            FFs = ffNames == null
                ? []
                : ffNames.Split(' ').Select(ff =>
                {
                    // Change path of usermaps files to full paths.
                    // e.g. usermaps/mp_caen2_load -> usermaps/mp_caen2/mp_caen2_load

                    if (!ff.StartsWith("usermaps/mp_", StringComparison.Ordinal))
                    {
                        return ff;
                    }

                    var mapName = ff.Split('/').Last();
                    if (mapName.EndsWith("_load", StringComparison.Ordinal))
                    {
                        mapName = mapName[..^5];
                        return $"usermaps/{mapName}/{mapName}_load";
                    }

                    return $"usermaps/{mapName}/{mapName}";
                });
        }
        catch (Exception)
        {
            Map = "???";
            Mod = "???";
            GameMode = "???";
            ServerName = "File corrupted!";
            IWDs = [];
            FFs = [];
            IsCorrupted = true;
        }
    }

    /// <summary>
    ///     Gets the path to the demo file.
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    ///     Gets a collection of IWD files referenced by the demo.
    /// </summary>
    public IEnumerable<string> IWDs { get; }

    /// <summary>
    ///     Gets a collection of FF files referenced by the demo.
    /// </summary>
    public IEnumerable<string> FFs { get; }

    /// <summary>
    ///     Gets a value indicating whether this instance is valid.
    /// </summary>
    public bool IsValid => File.Exists(Path) && !IsCorrupted;

    /// <summary>
    /// Returns the display name for this demo.
    /// </summary>
    /// <returns>The demo name.</returns>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    ///     Deletes this demo file.
    /// </summary>
    public void Delete()
    {
        File.Delete(Path);
    }

    /// <summary>
    ///     Gets the version of this instance.
    /// </summary>
    public GameVersion Version { get; }

    /// <summary>
    ///     Gets or sets the name of this instance.
    /// </summary>
    public string Name
    {
        get => System.IO.Path.GetFileNameWithoutExtension(Path);
        set
        {
            var newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path) ?? throw new InvalidOperationException(),
                $"{value}{System.IO.Path.GetExtension(Path)}");

            File.Move(Path, newPath);

            Path = newPath;
        }
    }

    /// <summary>
    ///     Gets the UTC date this instance was recorded at.
    /// </summary>
    public DateTime Created => File.GetCreationTimeUtc(Path);

    /// <summary>
    ///     Gets the map this instance was recorded in.
    /// </summary>
    public string? Map { get; }

    /// <summary>
    ///     Gets the mod this instance was recorded in.
    /// </summary>
    public string? Mod { get; }

    /// <summary>
    ///     Gets the game type this instance was recorded in.
    /// </summary>
    public string? GameMode { get; }

    /// <summary>
    ///     Gets the server this instance was recorded on.
    /// </summary>
    public string? ServerName { get; }

    /// <summary>
    ///     Gets the size of the file.
    /// </summary>
    public long FileSize => new FileInfo(Path).Length;

    /// <summary>
    ///     Opens a stream of the demo file.
    /// </summary>
    /// <returns>The stream of the demo file.</returns>
    public Stream Open()
    {
        return File.OpenRead(Path);
    }

    /// <summary>
    /// Determines whether this demo equals another demo by path.
    /// </summary>
    /// <param name="other">The other demo to compare.</param>
    /// <returns><see langword="true" /> when both demos refer to the same path; otherwise, <see langword="false" />.</returns>
    protected bool Equals(LocalDemo other)
    {
        return string.Equals(Path, other.Path, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><see langword="true" /> if the objects are equal; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is LocalDemo other && obj.GetType() == GetType() && Equals(other));
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return Path.GetHashCode(StringComparison.Ordinal);
    }
}