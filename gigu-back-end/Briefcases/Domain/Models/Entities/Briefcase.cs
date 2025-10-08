using System; //cuidado
using System.Collections.Generic; //cuidado
using gigu_back_end.Shared.Domain.Model.Entities;

namespace gigu_back_end.Briefcases.Domain.Models.Entities;

public class Briefcase : BaseEntity
{
    
    
    
    
    
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public int SellerId { get; set; }
    public List<Project> Projects { get; } = new();

    
    public Briefcase(string name, string description, DateTime publishDate, int sellerId)
    {
        Name = name;
        Description = description;
        PublishDate = publishDate;
        SellerId = sellerId;
        IsActive = true;
        CreatedDate = DateTime.UtcNow;
        Projects = new List<Project>();
    }
}