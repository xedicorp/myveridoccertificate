﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Reflection;

namespace Cofoundry.Domain;

/// <summary>
/// This can be used to decorate an integer id property that links to another entity. The entity
/// must have a definition that implements IDependableEntityDefinition.  Defining relations allow the system to
/// detect and prevent entities used in required fields from being removed.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EntityDependencyAttribute : Attribute, IEntityRelationAttribute, IMetadataAttribute
{
    public EntityDependencyAttribute(string entityDefinitionCode)
    {
        ArgumentNullException.ThrowIfNull(entityDefinitionCode);

        if (entityDefinitionCode.Length != 6)
        {
            throw new ArgumentException(nameof(entityDefinitionCode) + " must be 6 characters in length.", nameof(entityDefinitionCode));
        }

        EntityDefinitionCode = entityDefinitionCode;
    }

    public IEnumerable<EntityDependency> GetRelations(object model, PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyInfo);

        var isRequired = !(model is int?);
        var id = (int?)propertyInfo.GetValue(model);

        if (id.HasValue)
        {
            yield return new EntityDependency(EntityDefinitionCode, id.Value, isRequired);
        }
    }

    public void Process(DisplayMetadataProviderContext context)
    {
        MetaDataAttributePlacementValidator.ValidatePropertyType(this, context, typeof(int), typeof(int?));
    }

    public string EntityDefinitionCode { get; set; }
}