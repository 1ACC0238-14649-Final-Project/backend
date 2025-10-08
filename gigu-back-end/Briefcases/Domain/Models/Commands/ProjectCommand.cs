namespace gigu_back_end.Briefcases.Domain.Models.Commands;

/// <summary>
/// Comando que representa un proyecto a ser incluido dentro de un portafolio (briefcase).
/// </summary>
/// <param name="Title">Título del proyecto.</param>
/// <param name="Description">Descripción del proyecto.</param>
/// <param name="Price">Precio estimado del proyecto (por ejemplo, "$250").</param>
/// <param name="Time">Duración estimada del proyecto (por ejemplo, "3 semanas").</param>
/// <param name="GigLink">Enlace al gig asociado o página externa del proyecto.</param>
public record ProjectCommand(
    string Title,
    string Description,
    string Price,
    string Time,
    string GigLink
);
