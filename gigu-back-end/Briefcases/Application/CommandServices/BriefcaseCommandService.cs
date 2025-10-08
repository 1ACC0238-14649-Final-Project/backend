using System.Data;
using FluentValidation;




using gigu_back_end.Shared.Domain;

using gigu_back_end.Shared.Domain.Models.Commands; // cuidado
using gigu_back_end.Briefcases.Domain.Services; 

using gigu_back_end.Briefcases.Domain;
using gigu_back_end.Briefcases.Domain.Models.Commands;


using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Briefcases.Domain.Models.Exceptions;


//using gigu_back_end.Briefcases.Domain.Models.Validators; cuidado
using NuGet.Packaging.Licenses; // cuidado

namespace gigu_back_end.Briefcases.Application.CommandServices;

public class BriefcaseCommandService (
    IBriefcaseRepository briefcaseRepository, 
    IUnitOfWork unitOfWork,
    IValidator<CreateBriefcaseCommand> validator) : IBriefcaseCommandService 
{
    private readonly IBriefcaseRepository _briefcaseRepository =
        briefcaseRepository ?? throw new ArgumentNullException(nameof(briefcaseRepository));    

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IValidator<CreateBriefcaseCommand> _validator =
        validator ?? throw new ArgumentNullException(nameof(validator));   
    
    public async Task<Briefcase> Handle(CreateBriefcaseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        //ValidateBriefcase(command);
        /*var briefcases = await _briefcaseRepository.ListAsync(); // get all rows  SELECT* FROM BRIEFCASES
        if (briefcases.Any(briefcase => briefcase.Name == command.Name))
            throw new DuplicateNameException($"A briefcase with the name '{command.Name}' already exists.");*/

        //!command.Projects.Any()  command.Projects.Count > 0

        var existingBriefcase =
            await _briefcaseRepository.GetByNameAsync(command.Name); //  SELECT* FROM BRIEFCASES WHERE name = {command.Name}}
        if (existingBriefcase != null)
            throw new DuplicateNameException($"A briefcase with the name '{command.Name}' already exists.");

        if (command.Projects == null || !command.Projects.Any())
            throw new NotProjectFoundException();


        var briefcase = new Briefcase(command.Name, command.Description, command.PublishDate, command.SellerId)
        {
            UserId = 1
        };

        command.Projects.ForEach(project =>
        {
            briefcase.Projects.Add(new Project(project.Title, project.Description, project.Price, project.Time,  project.GigLink, briefcase));
        });

        //cfg.CreateMap<Projects, ProjectsCommand>()
        // public Project(string title, string description, string price, string time, string gigLink,Briefcase briefcase)

        await _briefcaseRepository.AddAsync(briefcase);
        await _unitOfWork.CompleteAsync();

        return briefcase;
        
    }
    
    private void ValidateBook(CreateBriefcaseCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrEmpty(command.Name))
            throw new ArgumentException("Briefcase name is required");

        if (command.Name.Length > 20)
            throw new ArgumentException("Briefcase name MAX LENGHT 20  CHARACTERS");

        if (command.Description.Length > 100)
            throw new ArgumentException("Briefcase description MAX LENGHT 100  CHARACTERS");

        if (command.Description.Length < 10)
            throw new ArgumentException("Briefcase description Min LENGHT 10  CHARACTERS");
    }
    
    
    public async Task<bool> Handle(DeleteBriefcaseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var briefcase = await _briefcaseRepository.FindByIdAsync(command.Id);
        if (briefcase is null) return false;

        briefcase.IsActive = false;
        briefcase.ModifiedDate = DateTime.UtcNow;
        briefcase.UpdatedUserId = 87; // Placeholder for dynamic user ID.

        _briefcaseRepository.Update(briefcase);
        await _unitOfWork.CompleteAsync();

        return true;
    }
    
    public async Task<bool> Handle(UpdateBriefcaseCommand command, int Id)
    {
        var briefcase = await _briefcaseRepository.FindByIdAsync(Id);
        if (briefcase is null) throw new DataException("Briefcase not found.");


        briefcase.Name = command.Name;
        briefcase.Description = command.Description;
       
        briefcase.ModifiedDate = DateTime.UtcNow;
        briefcase.UpdatedUserId = 87;

        _briefcaseRepository.Update(briefcase);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}