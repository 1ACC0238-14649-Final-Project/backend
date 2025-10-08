using gigu_back_end.Shared.Domain;
using gigu_back_end.Briefcases.Domain;
using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Briefcases.Domain.Models.Queries;
using gigu_back_end.Briefcases.Domain.Services;

namespace gigu_back_end.Briefcases.Application.QueryServices;

public class BriefcaseQueryService(IBriefcaseRepository briefcaseRepository) : IBriefcaseQueryService
{
    private readonly IBriefcaseRepository _briefcaseRepository = briefcaseRepository ?? throw new ArgumentNullException(nameof(briefcaseRepository));

    public async Task<IEnumerable<Briefcase>> Handle(GetAllBriefcasesQuery query)
    {
        var briefcases = await _briefcaseRepository.GetAllWithProjectsAsync();
        return briefcases?.Where(briefcase => briefcase.IsActive) ?? Enumerable.Empty<Briefcase>();
    }

    public async Task<Briefcase?> Handle(GetBriefcaseBySellerIdQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        return await _briefcaseRepository.FindBySellerIdWithProjectsAsync(query.SellerId);
    }
}