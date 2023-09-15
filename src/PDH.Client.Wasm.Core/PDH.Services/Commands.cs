using PDH.Client.Wasm.Core.PDH.Services.Dtos;
using PDH.Client.Wasm.Core.PDH.Services.Enums;

namespace PDH.Client.Wasm.Core.PDH.Services;

public record AddNewTechnicalSkillCommand(string Name, TechnicalSkillLevelEnum SkillLevel);

public record SaveSudokuBoardCommand(SudokuGame Game);