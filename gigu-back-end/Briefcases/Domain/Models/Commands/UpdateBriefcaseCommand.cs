namespace gigu_back_end.Briefcases.Domain.Models.Commands;

/// <summary>
/// Comando para actualizar los datos de un portafolio (briefcase) existente.
/// </summary>
public record UpdateBriefcaseCommand(
    int Id,
    string Name,
    string Description,
    DateTime PublishDate
);
