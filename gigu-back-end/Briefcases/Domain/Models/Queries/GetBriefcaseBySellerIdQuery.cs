namespace gigu_back_end.Briefcases.Domain.Models.Queries;

/// <summary>
/// Query para obtener un briefcase por el ID del vendedor (SellerId).
/// </summary>
public record GetBriefcaseBySellerIdQuery
{
    public GetBriefcaseBySellerIdQuery(int sellerId)
    {
        SellerId = sellerId;
    }

    public int SellerId { get; init; }
}