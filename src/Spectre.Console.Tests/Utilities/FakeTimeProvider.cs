namespace Spectre.Console.Tests;

internal sealed class FakeTimeProvider : TimeProvider
{
    private DateTime _utcNow;

    public FakeTimeProvider(DateTime startTime)
    {
        _utcNow = startTime;
    }

    public override DateTimeOffset GetUtcNow() => new DateTimeOffset(_utcNow, TimeSpan.Zero);
    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;

    public void Advance(TimeSpan timeSpan) => _utcNow += timeSpan;
}
