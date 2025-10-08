using gigu_back_end.Shared.Domain.Models.Commands; // cuidado
using gigu_back_end.Briefcases.Domain.Models.Commands;
using gigu_back_end.Briefcases.Domain.Models.Entities;

namespace gigu_back_end.Briefcases.Domain.Services;

public interface IBriefcaseCommandService
{
    Task<Briefcase> Handle(CreateBriefcaseCommand command);
    Task<bool> Handle(DeleteBriefcaseCommand command);
    Task<bool> Handle(UpdateBriefcaseCommand command, int id);   
}
