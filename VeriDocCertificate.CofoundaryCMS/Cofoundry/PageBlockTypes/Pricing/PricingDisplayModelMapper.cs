using Cofoundry.Core;

namespace VeriDocCertificate.CofoundaryCMS;

public class PricingDisplayModelMapper : IPageBlockTypeDisplayModelMapper<PricingDataModel>
{
    private readonly IContentRepository _contentRepository;

    public PricingDisplayModelMapper(
        IContentRepository contentRepository
        )
    {
        _contentRepository = contentRepository;
    }

    public async Task MapAsync(
        PageBlockTypeDisplayModelMapperContext<PricingDataModel> context,
        PageBlockTypeDisplayModelMapperResult<PricingDataModel> result
        )
    {
        
        // Map display model
        foreach (var items in context.Items)
        {
            var output = new PricingDisplayModel();

            output.Lists = EnumerableHelper
                .Enumerate(items.DataModel.Lists)
                .Select(m => new PricingListDisplayModel()
                {

                    Title = m.Title,
                    MonthlyPlanDescription = m.MonthlyPlanDescription,
                    YearlyPlanDescription = m.YearlyPlanDescription,
                    PlanFeatures = m.PlanFeatures,
                    FreeTrialLink = m.FreeTrialLink,
                    PlanMonthlyLink= m.PlanMonthlyLink,
                    PlanYearlyLink = m.PlanYearlyLink,

                })
                .ToList();

            result.Add(items, output);
        }
    }
}
