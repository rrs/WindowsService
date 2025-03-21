namespace Rrs.WindowsService;

public enum StartMode
{
    Auto,
    AutoDelayed
}

public record WindowsServiceConfiguration(string Name, string DisplayName, string Description, StartMode StartMode = StartMode.Auto, RecoveryOptions? RecoveryOptions = null);
public record RecoveryOptions(int? FirstFailure = 120000, int? SecondFailure = 120000, int? SubsequentFailures = null)
{
    public int Reset { get; init; } = 86400;
}