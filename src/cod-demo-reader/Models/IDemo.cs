namespace XtremeIdiots.CallOfDuty.DemoReader.Models
{
    public interface IDemo
    {
        GameType Version { get; }
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