using PDH.Client.Wasm.Core.PDH.Services.Enums;

namespace PDH.Client.Wasm.Core.Services.Dtos;

public class TechnicalSkill
{
    public string Skill { get; init; } = null!;

    public TechnicalSkillLevelEnum TechnicalSkillLevel { get; init; }
}