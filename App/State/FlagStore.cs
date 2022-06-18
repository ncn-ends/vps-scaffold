namespace App.State;

public class FlagStore
{
    public readonly bool AsHttp = false;
    public readonly bool AsHttps = false;
    public readonly bool AsMinimal = false;
    public readonly bool AsNoDomain = false;

    public FlagStore(string[] args)
    {
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


        var hasMinimalFlag = args.Contains("--minimal");
        if (hasMinimalFlag) AsMinimal = true;

        var hasNoDomainFlag = args.Contains("--no-domain");
        if (hasNoDomainFlag) AsNoDomain = true;


        if (hasHttpsOnly && hasNoDomainFlag) throw new ArgumentException("HTTPS requires a proper domain name.");

        AppStore.FlagStore = this;
    }
}