using Cofoundry.Domain.Data;
using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain.Domain.HeaderFooter.Queries
{
    public class GetHeaderFooterDetailsQueryHandler : IQueryHandler<GetHeaderFooterDetailsQuery, HeaderFooterDetails>
    , IIgnorePermissionCheckHandler
    {
        private readonly CofoundryDbContext _dbContext;
 
        public GetHeaderFooterDetailsQueryHandler( CofoundryDbContext dbContext )
        {
            _dbContext = dbContext;
            


        } 
        public async Task<HeaderFooterDetails> ExecuteAsync(GetHeaderFooterDetailsQuery query, IExecutionContext executionContext)
        {
            var dbQuery = _dbContext
           .CustomEntityPublishStatusQueries
            .Include(v => v.CustomEntityVersion)
           .Include(v => v.CustomEntity)
           .AsNoTracking() 
           .FilterByCustomEntityDefinitionCode("HEMENU")
           .Where(p=>p.PublishStatusQueryId==1);
            
            var menuItems= dbQuery.ToList();
            var menuLinks = new List<HeaderMenuItem>();
            foreach(var menuItem in menuItems)
            {
                CustomHeaderMenuItem customHeaderMenuItem = JsonConvert.DeserializeObject< CustomHeaderMenuItem>(menuItem.CustomEntityVersion.SerializedData );
                menuLinks.Add(new HeaderMenuItem
                {
                    Text = menuItem.CustomEntityVersion.Title,
                    Url = customHeaderMenuItem.Url,
                    CustomEntityId= menuItem.CustomEntityId

                });
            }
            var dbQueryS = _dbContext
          .CustomEntityPublishStatusQueries
           .Include(v => v.CustomEntityVersion)
          .Include(v => v.CustomEntity)
          .AsNoTracking()
          .FilterByCustomEntityDefinitionCode("XMEDIA")
          .Where(p => p.PublishStatusQueryId == 1);

            var menuItemsS = dbQueryS.ToList();
            var menuLinksS = new List<HeaderMenuItem>();
            foreach (var menuItem in menuItemsS)
            {
                CustomHeaderMenuItem customHeaderMenuItem = JsonConvert.DeserializeObject<CustomHeaderMenuItem>(menuItem.CustomEntityVersion.SerializedData);
                menuLinksS.Add(new HeaderMenuItem
                {
                    Text = menuItem.CustomEntityVersion.Title,
                    Url = customHeaderMenuItem.Url,
                    CustomEntityId = menuItem.CustomEntityId,
                    ClassName= customHeaderMenuItem.ClassName,

                });
            }
            HeaderFooterDetails details = new HeaderFooterDetails();
           var headerFooterSetting= await _dbContext.HeaderFooterSettings.FirstOrDefaultAsync();
            if (headerFooterSetting != null)
            {
                details.Address = headerFooterSetting.Address;
                details.Email = headerFooterSetting.Email;
                details.Phone = headerFooterSetting.Phone;
                details.HeaderLogoImageId = headerFooterSetting.HeaderLogoImageId ;
                details.FooterLogoImageId = headerFooterSetting.FooterLogoImageId ;
                details.PartnerLogoImageId = headerFooterSetting.PartnerLogoImageId;
                details.HeaderMenuItems = GetLinks(headerFooterSetting.HeaderMenuLinks, menuLinks);
                details.UsefulLinks = GetLinks(headerFooterSetting.UsefulMenuLinks, menuLinks); ;
                details.SocialMediaIcons = GetLinks(headerFooterSetting.SocialMediaLinks, menuLinksS); ;
                details.HeaderMenuItemsIds = headerFooterSetting.HeaderMenuLinks;
                details.UsefulLinksIds = headerFooterSetting.UsefulMenuLinks;
                details.SocialMediaIconsIds = headerFooterSetting.SocialMediaLinks;
            }
            return details;
        }

        private List<HeaderMenuItem> GetLinks(string ids, List<HeaderMenuItem>  allLinks)
        {
            List<HeaderMenuItem> items = new List<HeaderMenuItem>();
            if (!string.IsNullOrEmpty(ids))
            {
                //var arrIds = ids.Split(new char[] { ',' }).ToList();
                //var q = from p in allLinks
                //        where arrIds.Contains(p.CustomEntityId.ToString ())
                //        select p;
                //if(q!=null)
                //{
                //    items.AddRange(q);
                //}
                var ints = ids.Split(",").Select(i => Int32.Parse(i)).ToList();
                foreach (var intId in ints)
                {
                    var item = allLinks.FirstOrDefault(p => p.CustomEntityId == intId);
                    if (item != null)
                    {
                        items.Add(item);
                    }
                }
            }
            return items;
        }
    }
}
 
