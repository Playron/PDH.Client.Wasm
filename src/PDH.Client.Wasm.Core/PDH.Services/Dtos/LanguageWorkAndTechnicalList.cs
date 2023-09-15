namespace PDH.Client.Wasm.Core.PDH.Services.Dtos;

public class LanguageWorkAndTechnicalList
{
    public IEnumerable<WorkExperience>? WorkExperiences { get; set; }
    public IEnumerable<TechnicalSkill>? TechnicalSkills { get; set; }
    public IEnumerable<LanguageSkill>? LanguageSkills { get; set; }
}