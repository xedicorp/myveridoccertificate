using Cofoundry.Core.Data;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.Data.Internal;
using Cofoundry.Domain.Internal;
 

namespace Cofoundry.Domain.Domain.HeaderFooter.Commands
{
    public class SaveHeaderFooterCommandHandler :   ICommandHandler<SaveHeaderFooterCommand>
    , IPermissionRestrictedCommandHandler<SaveHeaderFooterCommand>
        //, IValidatableObject
    {
        private readonly CofoundryDbContext _dbContext;
        private readonly IPageCache _pageCache;
        private readonly IMessageAggregator _messageAggregator;
        private readonly IPageStoredProcedures _pageStoredProcedures;
        private readonly ITransactionScopeManager _transactionScopeFactory;
        public SaveHeaderFooterCommandHandler(
       CofoundryDbContext dbContext,
       IPageCache pageCache,
       IMessageAggregator messageAggregator,
       IPageStoredProcedures pageStoredProcedures,
       ITransactionScopeManager transactionScopeFactory
       )
        {
            _dbContext = dbContext;
            _pageCache = pageCache;
            _messageAggregator = messageAggregator;
            _pageStoredProcedures = pageStoredProcedures;
            _transactionScopeFactory = transactionScopeFactory;
        }
        public async Task ExecuteAsync(SaveHeaderFooterCommand command, 
            IExecutionContext executionContext)
        {
            //Normalize(command);
            //await ValidateIsPageUniqueAsync(command, executionContext);

            // var page = await MapPage(command, executionContext);
            HeaderFooterSetting defaultSetting= await _dbContext.HeaderFooterSettings.FirstOrDefaultAsync();
            bool isNewRecord = false;
            if(defaultSetting==null)
            {
                isNewRecord = true;
                defaultSetting = new HeaderFooterSetting();
            }
            defaultSetting.UpdateDate = DateTime.UtcNow;
            defaultSetting.Address = command.Address;
            defaultSetting.Phone = command.Phone;
            defaultSetting.Email = command.Email;
            defaultSetting.HeaderLogoImageId = command.HeaderLogoImageId;
            defaultSetting.FooterLogoImageId = command.FooterLogoImageId;
            defaultSetting.PartnerLogoImageId = command.PartnerLogoImageId;

            defaultSetting.HeaderMenuLinks = command.HIds;
            defaultSetting.UsefulMenuLinks = command.UIds;
            defaultSetting.SocialMediaLinks = command.SIds;
            // using (var scope = _transactionScopeFactory.Create(_dbContext))
            // {

            if(isNewRecord)
            {
                _dbContext.HeaderFooterSettings.Add(defaultSetting);
            }
            await _dbContext.SaveChangesAsync();
               // await _pageStoredProcedures.UpdatePublishStatusQueryLookupAsync(page.PageId);

             //   scope.QueueCompletionTask(() => OnTransactionComplete(command, page));

             //   await scope.CompleteAsync();
           // }

            // Set Ouput
           // command.OutputPageId = page.PageId;
        }
        private void Normalize(AddPageCommand command)
        {
            command.UrlPath = command.UrlPath?.ToLowerInvariant();
            command.Title = command.Title.Trim();
            command.MetaDescription = command.MetaDescription?.Trim() ?? string.Empty;
            command.MetaTitle = command.MetaTitle?.Trim() ?? string.Empty;
            command.OpenGraphTitle = command.OpenGraphTitle?.Trim();
            command.OpenGraphDescription = command.OpenGraphDescription?.Trim();
        }
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    throw new NotImplementedException();
        //}
        public IEnumerable<IPermissionApplication> GetPermissions(SaveHeaderFooterCommand command)
        {
            yield return new PageCreatePermission();

            //if (command.Publish)
            //{
            //    yield return new PagePublishPermission();
            //}
        }
    }
}
