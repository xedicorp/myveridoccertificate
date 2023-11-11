﻿namespace Cofoundry.Web;

/// <summary>
/// A factory that creates a collection of ordered IPageActionRoutingStep to
/// be executed in order during the PageController's Page Action. This determines
/// the routing of dynamic page content in the site.
/// </summary>
public class PageActionRoutingStepFactory : IPageActionRoutingStepFactory
{
    private readonly IEnumerable<IPageActionRoutingStep> _routingSteps;

    public PageActionRoutingStepFactory(
        ICheckSiteIsSetupRoutingStep checkSiteIsSetupRoutingStep,
        IInitVisualEditorStateRoutingStep initStateRoutingStep,
        ITryFindPageRoutingInfoRoutingStep tryFindPageRoutingInfoRoutingStep,
        IInitUserContextRoutingStep initUserContextRoutingStep,
        IValidateAccessRulesRoutingStep validateAccessRulesRoutingStep,
        IValidateEntityEditModeRoutingStep validateEntityEditModeRoutingStep,
        IValidateEditPermissionsRoutingStep validateEditPermissionsRoutingStep,
        IValidateDraftVersionRoutingStep validateDraftVersionRoutingStep,
        IValidateSpecificVersionRoutingRoutingStep validateSpecificVersionRoutingRoutingStep,
        IGetNotFoundRouteRoutingStep getNotFoundRouteRoutingStep,
        IGetPageRenderDataRoutingStep getPageRenderDataRoutingStep,
        ISetCachePolicyRoutingStep setCachePolicyRoutingStep,
        IGetFinalResultRoutingStep getFinalResultRoutingStep
        )
    {
        // Here we set the default routing steps, which are run in the order they are 
        // declared. Each step can be overridden using the DI system if you need to debug it
        // in an implemenetation.

        var routingSteps = new List<IPageActionRoutingStep>()
        {
            checkSiteIsSetupRoutingStep,
            initStateRoutingStep,
            tryFindPageRoutingInfoRoutingStep,
            initUserContextRoutingStep,
            validateAccessRulesRoutingStep,
            validateEntityEditModeRoutingStep,
            validateEditPermissionsRoutingStep,
            validateDraftVersionRoutingStep,
            validateSpecificVersionRoutingRoutingStep,
            getNotFoundRouteRoutingStep,
            getPageRenderDataRoutingStep,
            setCachePolicyRoutingStep,
            getFinalResultRoutingStep
        };

        _routingSteps = routingSteps;
    }

    public IEnumerable<IPageActionRoutingStep> Create()
    {
        return _routingSteps;
    }
}
