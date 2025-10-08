using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Briefcases.Interfaces.REST.Resources;

namespace gigu_back_end.Briefcases.Interfaces.REST.Transform;

public static class BriefcaseResourceFromEntityAssembler
{
    public static BriefcaseResource ToResourceFromEntity(Briefcase briefcase)
    {
        List<ProjectResource> projects = new List<ProjectResource>();

        foreach (var briefcaseProject in briefcase.Projects)
        {
            projects.Add(new ProjectResource(briefcaseProject.Title, briefcaseProject.Description, briefcaseProject.Price, briefcaseProject.Time, briefcaseProject.GigLink));
        }

        return new BriefcaseResource(briefcase.Id, briefcase.Name, briefcase.Description, briefcase.PublishDate, briefcase.SellerId, projects);
        
        //int Id, string Name, string Description, DateTime PublishDate, int SellerId, List<ProjectResource> Projects
    }
}