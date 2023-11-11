﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Reflection;

namespace Cofoundry.Domain;

/// <summary>
/// Use this to decorate an (nullable) integer and indicate that it should be the 
/// database id for a Page. If the integer is nullable then this signals
/// that the property is optional.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PageAttribute : RegularExpressionAttribute, IMetadataAttribute, IEntityRelationAttribute
{
    public PageAttribute()
        : base(@"^[1-9]\d*$")
    {
        ErrorMessage = "The {0} field is required";
    }

    public void Process(DisplayMetadataProviderContext context)
    {
        MetaDataAttributePlacementValidator.ValidatePropertyType(this, context, typeof(int), typeof(int?));
        context.DisplayMetadata.TemplateHint = "PageSelector";
    }

    public IEnumerable<EntityDependency> GetRelations(object model, PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyInfo);

        var isRequired = !(model is int?);
        var id = (int?)propertyInfo.GetValue(model);

        if (id.HasValue)
        {
            yield return new EntityDependency(PageEntityDefinition.DefinitionCode, id.Value, isRequired);
        }
    }
}
