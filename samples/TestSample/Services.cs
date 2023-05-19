namespace TestSample;

public class ServicesB
{
    private readonly ServicesC _service;

    public ServicesB(ServicesC service)
    {
        _service = service;
    }
    
    public string Get()
    {
        return Name;
    }

    public string Name { get; set; } = Guid.NewGuid().ToString();
}

public class ServicesC
{
    public string Name { get; set; } = "ServicesC";
}