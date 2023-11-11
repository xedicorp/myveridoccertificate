using Cofoundry.Domain.Data;
using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Commands;
using Cofoundry.Domain.Domain.SeoTools.Models;
using Cofoundry.Domain.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cofoundry.Domain.Domain.SeoTools.Queries
{
    public class GetSeoToolsDetailsQueryHandler : IQueryHandler<GetSeoToolsDetailsQuery, SeoToolsDetails>
    , IIgnorePermissionCheckHandler
    {
        private readonly CofoundryDbContext _dbContext;
 
        public GetSeoToolsDetailsQueryHandler( CofoundryDbContext dbContext )
        {
            _dbContext = dbContext; 
        }
        public async Task<SeoToolsDetails> ExecuteAsync(GetSeoToolsDetailsQuery query, IExecutionContext executionContext)
        {
            var dbQuery = _dbContext
           .CustomEntityPublishStatusQueries
            .Include(v => v.CustomEntityVersion)
           .Include(v => v.CustomEntity)
           .AsNoTracking()
           .FilterByCustomEntityDefinitionCode("XSCHMA")
           .Where(p => p.PublishStatusQueryId == 1);

            var menuItems = dbQuery.ToList();
            var menuLinks = new List<SchemaItem>();
            foreach (var menuItem in menuItems)
            {
                CustomSchemaItem customHeaderMenuItem = JsonConvert.DeserializeObject<CustomSchemaItem>(menuItem.CustomEntityVersion.SerializedData);
                menuLinks.Add(new SchemaItem
                {
                    Id = customHeaderMenuItem.Id,
                    Name = customHeaderMenuItem.Name,
                    CustomEntityId = menuItem.CustomEntityId

                });
            }

            SeoToolsDetails details = new SeoToolsDetails();
            var SeoTool = await _dbContext.SeoTools.FirstOrDefaultAsync();
            if (SeoTool != null)
            {
                details.ViewPort = SeoTool.ViewPort;
                details.CopyRight = SeoTool.CopyRight;
                details.Author = SeoTool.Author;
                details.ReplyTo = SeoTool.ReplyTo;
                details.Robots = SeoTool.Robots;
                details.ContentLanguage = SeoTool.ContentLanguage;
                details.Audience = SeoTool.Audience;
                details.RevisitAfter = SeoTool.RevisitAfter;
                details.Distribution = SeoTool.Distribution;
                details.AltDistribution = SeoTool.AltDistribution;
                details.Publisher = SeoTool.Publisher;
                details.AltCopyRight = SeoTool.AltCopyRight;
                details.Rel = SeoTool.Rel;
                details.Href = SeoTool.Href;
                details.HrefLang = SeoTool.HrefLang;
                details.GoogleTagManager = SeoTool.GoogleTagManager;
                details.GoogleAnalytics = SeoTool.GoogleAnalytics;
                details.GoogleSiteVerification = SeoTool.GoogleSiteVerification;
                details.BingSiteVerification = SeoTool.BingSiteVerification;

                details.SchemaIds = SeoTool.SchemaIds;
                details.SchemaItems = GetSchemaItems(SeoTool.SchemaIds, menuLinks);
              
            }
            return details;
        }

        private List<CustomSchemaItem> GetSchemaItems(string ids, List<SchemaItem>  allLinks)
        {
            List<CustomSchemaItem> items = new List<CustomSchemaItem>();
            if (!string.IsNullOrEmpty(ids))
            {
                var arrIds = ids.Split(new char[] { ',' }).ToList();
                var q = from p in allLinks
                        where arrIds.Contains(p.CustomEntityId.ToString ())
                        select new CustomSchemaItem
                        {
                              Id = p.Id,
                             Name = p.Name  
                        };
                if(q!=null)
                {
                    items.AddRange(q);
                }
            }
            return items;
        }
    }
}
 
