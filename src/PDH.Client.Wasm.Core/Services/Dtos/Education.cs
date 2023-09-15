namespace PDH.Client.Wasm.Core.Services.Dtos;

public class Education
{
    public string StudyName { get; set; } = null!;

    public string University { get; set; } = null!;

    public int FromYear { get; set; }

    public int ToYear { get; set; }
}
