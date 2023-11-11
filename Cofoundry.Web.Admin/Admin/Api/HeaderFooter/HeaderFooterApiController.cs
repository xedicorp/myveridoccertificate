using Cofoundry.Domain.Domain.HeaderFooter.Commands;
using Cofoundry.Domain.Domain.HeaderFooter.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cofoundry.Web.Admin;

public class HeaderFooterApiController : BaseAdminApiController
{
    private readonly IApiResponseHelper _apiResponseHelper;
    private readonly IDomainRepository _domainRepository;

    public HeaderFooterApiController(
        IDomainRepository domainRepository,
        IApiResponseHelper apiResponseHelper
        )
    {
        _domainRepository = domainRepository;
        _apiResponseHelper = apiResponseHelper;
    }

    public async Task<JsonResult> Get()
    {
        var query = new GetHeaderFooterDetailsQuery();
        return await _apiResponseHelper.RunQueryAsync(query);
    } 
    public Task<JsonResult> Post([FromBody] SaveHeaderFooterCommand command)
    {
        return _apiResponseHelper.RunCommandAsync(command);
    }
     
   
}
