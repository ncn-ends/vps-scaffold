namespace App.State;

public class FlagStore
{
    public bool AsHttp { get; } = false;
    public bool AsHttps { get; } = false;
    public readonly bool AsMinimal = false;

    public FlagStore(string[] args)
    {
        var hasMinimalFlag = args.Contains("--minimal");
        if (hasMinimalFlag) AsMinimal = true;

        var hasHttpOnly = args.Contains("--http-only");
        var hasHttpsOnly = args.Contains("--https-only");
        var conflicting = hasHttpOnly && hasHttpsOnly;
        if (conflicting)
            throw new ArgumentException(
                "Cannot enforce http and https at the same time. Pick one or use neither flag.");
        if (!hasHttpOnly && !hasHttpsOnly)
        {
            AsHttp = true;
            AsHttps = true;
        }
        if (hasHttpOnly) AsHttp = true;
        if (hasHttpsOnly) AsHttps = true;
    }
}