﻿using Cofoundry.Domain.Extendable;

namespace Cofoundry.Domain.Internal;

public class ContentRepositoryImageAssetRepository
        : IContentRepositoryImageAssetRepository
        , IExtendableContentRepositoryPart
{
    public ContentRepositoryImageAssetRepository(
        IExtendableContentRepository contentRepository
        )
    {
        ExtendableContentRepository = contentRepository;
    }

    public IExtendableContentRepository ExtendableContentRepository { get; }

    public IContentRepositoryImageAssetByIdQueryBuilder GetById(int imageAssetId)
    {
        return new ContentRepositoryImageAssetByIdQueryBuilder(ExtendableContentRepository, imageAssetId);
    }

    public IContentRepositoryImageAssetByIdRangeQueryBuilder GetByIdRange(IEnumerable<int> imageAssetIds)
    {
        return new ContentRepositoryImageAssetByIdRangeQueryBuilder(ExtendableContentRepository, imageAssetIds);
    }

    public IContentRepositoryImageAssetSearchQueryBuilder Search()
    {
        return new ContentRepositoryImageAssetSearchQueryBuilder(ExtendableContentRepository);
    }

    public Task AddAsync(AddImageAssetCommand command)
    {
        return ExtendableContentRepository.ExecuteCommandAsync(command);
    }

    public Task UpdateAsync(UpdateImageAssetCommand command)
    {
        return ExtendableContentRepository.ExecuteCommandAsync(command);
    }

    public Task DeleteAsync(int imageAssetId)
    {
        var command = new DeleteImageAssetCommand()
        {
            ImageAssetId = imageAssetId
        };

        return ExtendableContentRepository.ExecuteCommandAsync(command);
    }
}