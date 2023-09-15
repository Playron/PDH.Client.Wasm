using PDH.Client.Wasm.Core.PDH.Services.Enums;

namespace PDH.Client.Wasm.Core.Services.Dtos;
public class LanguageSkill
{
    public string Name { get; set; } = null!;

    public LanguageLevelEnum Level { get; set; }
}
