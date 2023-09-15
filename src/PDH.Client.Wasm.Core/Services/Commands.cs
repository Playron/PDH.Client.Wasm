using PDH.Client.Wasm.Core.PDH.Services.Enums;
using PDH.Client.Wasm.Core.Services.Dtos;

namespace PDH.Client.Wasm.Core.PDH.Services;

public record AddNewTechnicalSkillCommand(string Name, TechnicalSkillLevelEnum SkillLevel);

public record SaveSudokuBoardCommand(SudokuGame Game);