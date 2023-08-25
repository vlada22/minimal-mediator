namespace TestSampleAot;

public class TransientService
{
    public string Id { get; } = Guid.NewGuid().ToString();
}

public class ScopedService
{
    public string Id { get; } = Guid.NewGuid().ToString();
}