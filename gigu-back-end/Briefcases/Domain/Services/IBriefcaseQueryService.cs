using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Briefcases.Domain.Models.Queries;

namespace gigu_back_end.Briefcases.Domain.Services;

public interface IBriefcaseQueryService
{
    Task<IEnumerable<Briefcase>> Handle(GetAllBriefcasesQuery query);
    Task<Briefcase> Handle(GetBriefcaseBySellerIdQuery query); // CAMBIO
}
