namespace PDH.Client.Wasm.Core.Services.Dtos;

public class WorkExperience
{
    public string Role { get; set; } = null!;

    public string WorkPlace { get; set; } = null!;

    public string FromYear { get; set; } = null!;

    public string ToYear { get; set; } = null!;

    public List<WorkItem>? WorkItems { get; set; }
}

public class WorkItem
{
    public string? Item { get; set; }
}