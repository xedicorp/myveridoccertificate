using Cofoundry.Core.Data;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.Data.Internal;
using Cofoundry.Domain.Internal;
 

namespace Cofoundry.Domain.Domain.SeoTools.Commands
{
    public class SaveSeoToolsCommandHandler :   ICommandHandler<SaveSeoToolsCommand>
    , IPermissionRestrictedCommandHandler<SaveSeoToolsCommand>
        //, IValidatableObject
    {
        private readonly CofoundryDbContext _dbContext;
        private readonly IPageCache _pageCache;
        private readonly IMessageAggregator _messageAggregator;
        private readonly IPageStoredProcedures _pageStoredProcedures;
        private readonly ITransactionScopeManager _transactionScopeFactory;
        public SaveSeoToolsCommandHandler(
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
        public async Task ExecuteAsync(SaveSeoToolsCommand command, 
            IExecutionContext executionContext)
        {
            //Normalize(command);
            //await ValidateIsPageUniqueAsync(command, executionContext);

            // var page = await MapPage(command, executionContext);
            var details = await _dbContext.SeoTools.FirstOrDefaultAsync();
            bool isCreateNew = false;
            if (details == null)
            {
                isCreateNew = true;
                details = new Data.SeoTools();
                details.CreateDate = DateTime.UtcNow;
            }
            details.UpdateDate = DateTime.UtcNow;
         
            details.ViewPort = command.ViewPort;
            details.CopyRight = command.CopyRight;
            details.Author = command.Author;
            details.ReplyTo = command.ReplyTo;
            details.Robots = command.Robots;
            details.ContentLanguage = command.ContentLanguage;
            details.Audience = command.Audience;
            details.RevisitAfter = command.RevisitAfter;
            details.Distribution = command.Distribution;
            details.AltDistribution = command.AltDistribution;
            details.Publisher = command.Publisher;
            details.AltCopyRight = command.AltCopyRight;
            details.Rel = command.Rel;
            details.Href = command.Href;
            details.HrefLang = command.HrefLang;
            details.GoogleTagManager = command.GoogleTagManager;
            details.GoogleAnalytics = command.GoogleAnalytics;
            details.GoogleSiteVerification = command.GoogleSiteVerification;
            details.BingSiteVerification= command.BingSiteVerification;

            details.SchemaIds = command.SchemaIds; 
            // using (var scope = _transactionScopeFactory.Create(_dbContext))
            // {
            if(isCreateNew)
            {
                _dbContext.SeoTools.Add(details);
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
        public IEnumerable<IPermissionApplication> GetPermissions(SaveSeoToolsCommand command)
        {
            yield return new PageCreatePermission();

            //if (command.Publish)
            //{
            //    yield return new PagePublishPermission();
            //}
        }
    }
}
