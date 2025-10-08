using System; // cuidado
using gigu_back_end.Shared.Domain.Model.Entities;

namespace gigu_back_end.Briefcases.Domain.Models.Entities;

public class Project : BaseEntity
{
    public Project() {}
    
    public Project(string title, string description, string price, string time, string gigLink,Briefcase briefcase)
    {
        Title = title;
        Description = description;
        Price = price;
        Time = time;
        GigLink = gigLink;
        Briefcase = briefcase;
        IsActive = true;
        CreatedDate = DateTime.UtcNow;
    }
    
    
    public string Title { get; init; }
    
    
   
    public string Description { get; init; }
    public string Price { get; init; }
    public string Time { get; init; }
    public string GigLink {get; init;}
    
    
    
    public Briefcase Briefcase { get; init; }
}