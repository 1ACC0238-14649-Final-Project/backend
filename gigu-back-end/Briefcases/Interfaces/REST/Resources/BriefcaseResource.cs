namespace gigu_back_end.Briefcases.Interfaces.REST.Resources;

/// <summary>
/// Recurso que representa un portafolio (briefcase) con sus proyectos relacionados.
/// </summary>
/// <param name="Id">Identificador único del portafolio.</param>
/// <param name="Name">Nombre del portafolio.</param>
/// <param name="Description">Descripción del portafolio.</param>
/// <param name="PublishDate">Fecha de publicación del portafolio.</param>
/// <param name="SellerId">Identificador del vendedor o usuario dueño del portafolio.</param>
/// <param name="Projects">Lista de proyectos contenidos en el portafolio.</param>
public record BriefcaseResource(
    int Id,
    string Name,
    string Description,
    DateTime PublishDate,
    int SellerId,
    List<ProjectResource> Projects
);
