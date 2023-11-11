﻿using Microsoft.AspNetCore.Mvc;

namespace Cofoundry.Web.Admin;

public class DocumentFileTypesApiController : BaseAdminApiController
{
    private readonly IApiResponseHelper _apiResponseHelper;

    public DocumentFileTypesApiController(
        IApiResponseHelper apiResponseHelper
        )
    {
        _apiResponseHelper = apiResponseHelper;
    }

    public async Task<JsonResult> Get()
    {
        return await _apiResponseHelper.RunQueryAsync(new GetAllDocumentAssetFileTypesQuery());
    }
}
