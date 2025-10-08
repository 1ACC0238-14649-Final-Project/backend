namespace gigu_back_end.Briefcases.Interfaces.REST.Resources;

/// <summary>
/// Recurso que representa un proyecto dentro de un portafolio (briefcase).
/// </summary>
/// <param name="Title">Título del proyecto.</param>
/// <param name="Description">Descripción detallada del proyecto.</param>
/// <param name="Price">Precio estimado del proyecto (formato string, por ejemplo: "$100").</param>
/// <param name="Time">Tiempo estimado para completar el proyecto (formato string, por ejemplo: "2 semanas").</param>
/// <param name="GigLink">Enlace al gig o publicación asociada al proyecto.</param>
public record ProjectResource(
    string Title,
    string Description,
    string Price,
    string Time,
    string GigLink
);
