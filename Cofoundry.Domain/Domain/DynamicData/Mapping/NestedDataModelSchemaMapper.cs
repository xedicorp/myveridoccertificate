﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cofoundry.Domain.Internal;

public class NestedDataModelSchemaMapper : INestedDataModelSchemaMapper
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IDynamicDataModelSchemaMapper _dynamicDataModelSchemaMapper;

    public NestedDataModelSchemaMapper(
        IModelMetadataProvider modelMetadataProvider,
        IDynamicDataModelSchemaMapper dynamicDataModelSchemaMapper
        )
    {
        _modelMetadataProvider = modelMetadataProvider;
        _dynamicDataModelSchemaMapper = dynamicDataModelSchemaMapper;
    }

    public NestedDataModelSchema Map(Type modelType)
    {
        ArgumentNullException.ThrowIfNull(modelType);

        var schema = new NestedDataModelSchema();
        var dataModelMetaData = _modelMetadataProvider.GetMetadataForType(modelType);

        schema.TypeName = modelType.Name;
        schema.Description = dataModelMetaData.Description;
        schema.DisplayName = dataModelMetaData.DisplayName;

        if (string.IsNullOrEmpty(schema.DisplayName))
        {
            var modelName = StringHelper.RemoveSuffix(modelType.Name, "DataModel");
            schema.DisplayName = TextFormatter.PascalCaseToSentence(modelName);
        }

        _dynamicDataModelSchemaMapper.Map(schema, modelType);

        return schema;
    }
}
