namespace MX.CodDemoReader.Models
{
    public interface IDemo
    {
        GameVersion Version { get; }
        string? Name { get; }
        DateTime Created { get; }
        string? Map { get; }
        string? Mod { get; }
        string? GameMode { get; }
        string? ServerName { get; }
        long FileSize { get; }
        Stream Open();
    }
}