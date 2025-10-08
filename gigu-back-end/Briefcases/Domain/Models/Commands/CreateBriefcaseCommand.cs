namespace gigu_back_end.Briefcases.Domain.Models.Commands;

/// <summary>
/// Comando para crear un nuevo portafolio (briefcase).
/// </summary>
public record CreateBriefcaseCommand(
    int SellerId,
    string Name,
    string Description,
    DateTime PublishDate,
    List<ProjectCommand> Projects
);
