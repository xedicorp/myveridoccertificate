using Cofoundry.Domain.Domain.HeaderFooter.Commands;
using Cofoundry.Domain.Domain.HeaderFooter.Queries;
using Cofoundry.Domain.Domain.SeoTools.Commands;
using Cofoundry.Domain.Domain.SeoTools.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cofoundry.Web.Admin;

public class SeoToolsApiController : BaseAdminApiController
{
    private readonly IApiResponseHelper _apiResponseHelper;
    private readonly IDomainRepository _domainRepository;

    public SeoToolsApiController(
        IDomainRepository domainRepository,
        IApiResponseHelper apiResponseHelper
        )
    {
        _domainRepository = domainRepository;
        _apiResponseHelper = apiResponseHelper;
    }

    public async Task<JsonResult> Get()
    {
        var query = new GetSeoToolsDetailsQuery();
        return await _apiResponseHelper.RunQueryAsync(query);
    } 
    public Task<JsonResult> Post([FromBody] SaveSeoToolsCommand command)
    {
        return _apiResponseHelper.RunCommandAsync(command);
    }
     
   
}
