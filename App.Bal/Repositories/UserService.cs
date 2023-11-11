using App.Bal.Services;
using App.Dal;
using App.Entity.Dto.MainApp;
using App.Entity.Models;
using App.Entity.Models.MainApp;
using App.Entity.Models.Plan;
using App.Entity.Models.SignUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace App.Bal.Repositories
{
    public class UserService : IUserService
    {

        private readonly AppDbContext appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMainAppService _mainAppService;

        public UserService(AppDbContext appDbContext, IConfiguration configuration, IMainAppService mainAppService)
        {
            _configuration = configuration;
            this.appDbContext = appDbContext;
            _mainAppService = mainAppService;
        }

        public async Task<int> AddOrganisation(OrganisationDeatailsDto deatailsDto)
        {
            return await _mainAppService.AddOrganisation(deatailsDto);
        }

        public async Task<int> CreateAppUserAsync(AppUser appUser)
        {
            await appDbContext.AppUsers.AddAsync(appUser);
            await appDbContext.SaveChangesAsync();
            return appUser.Id;
        }

        public async Task<int> CreateAsync(RegisterUser appUser)
        {
            await appDbContext.RegisterUsers.AddAsync(appUser);
            await appDbContext.SaveChangesAsync();
            return appUser.Id;
        }

        public async Task<int> CreateSubscriptionAsync(Subscription subscription)
        {
            await appDbContext.Subscriptions.AddAsync(subscription);
            await appDbContext.SaveChangesAsync();
            return subscription.Id;
        }

        public async Task<int> CreateTempRegister(string email, string hash, string password)
        {
            RegisterTempMail register = new () { Email = email, Password = password, Hash = hash, CreatedAt = DateTime.UtcNow, IsCompleted = false };
            await appDbContext.RegisterTempMails.AddAsync(register);
            await appDbContext.SaveChangesAsync();
            return register.Id;
        }

        public void DisableOrganisation(string customerId)
        {
            _mainAppService.DisableOrganisation(customerId);
        }

        public async Task<AppUser?> FindAppUserByCustomerId(string customerId)
        {
            return await appDbContext.AppUsers.FirstOrDefaultAsync(e => e.CustomerId == customerId);
        }

        public async Task<AppUser?> FindAppUserByEmailAsync(string email)
        {
            return await appDbContext.AppUsers.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<AppUser?> FindAppUserById(int id)
        {
            return await appDbContext.AppUsers.FindAsync(id);
        }

        public async Task<RegisterUser?> FindByCustomerId(string customerId)
        {
            return await appDbContext.RegisterUsers.Include(e => e.PlanInfo).FirstOrDefaultAsync(e => e.CustomerId == customerId);
        }

        public async Task<RegisterUser?> FindByEmailAsync(string email)
        {
            List<RegisterUser> registerUsers = await appDbContext.RegisterUsers.Where(e => e.Email == email).OrderByDescending(e => e.Id).ToListAsync();
            return await appDbContext.RegisterUsers.FirstOrDefaultAsync(e => e.Email == email);
        }

        public bool FindMainAppUserByEmail(string email)
        {
            return _mainAppService.IsExist(email);
        }

        public async Task<RegisterTempMail?> FindTempRegister(string email)
        {
            return await appDbContext.RegisterTempMails.Where(e => e.Email == email).OrderByDescending(e => e.Id).FirstOrDefaultAsync();   
        }

        public List<CountryModel> GetCountryModels()
        {
            return _mainAppService.GetCountryModels();
        }

        public List<OrganisationPlan> GetOrganisationPlans()
        {
            return _mainAppService.GetOrganisationPlan();
        }

        public async Task<bool> IsUserExist(string email)
        {
            RegisterUser? user = await appDbContext.RegisterUsers.FirstOrDefaultAsync(e => e.Email == email);
            return user != null;
        }

        public async Task<int> UpdateAppUserAsync(AppUser appUser)
        {
            appDbContext.AppUsers.Update(appUser);
            await appDbContext.SaveChangesAsync();
            return appUser.Id;
        }

        public async Task<int> UpdateAsync(RegisterUser appUser)
        {
            appDbContext.RegisterUsers.Update(appUser);
            await appDbContext.SaveChangesAsync();
            return appUser.Id;
        }

        public void UpdateBalance(int balance, string customerId)
        {
            _mainAppService.UpdateBalance(balance, customerId);
        }

        public void UpdateBalanceInBatch(DataTable dataTable, int batchSize, ref string errMsg)
        {
            _mainAppService.UpdateBalnceInBatch(dataTable, batchSize, ref errMsg);
        }

        public void UpdateOrganisationPlan(string customerId, int planId)
        {
            _mainAppService.UpdateOrganisationPlan(customerId, planId);
        }

        public async Task<int> UpdateSubscriptionAsync(Subscription subscription)
        {
            appDbContext.Update(subscription);
            await appDbContext.SaveChangesAsync();
            return subscription.Id;
        }

        public void UpdateTemplateRequest(string customerId, int requestCount)
        {
            _mainAppService.UpdateTemplateCount(customerId, requestCount);
        }

        public async Task<int> UpdateTempRegister(RegisterTempMail tempMail)
        {
            tempMail.IsCompleted = true;
            appDbContext.RegisterTempMails.Update(tempMail);
            await appDbContext.SaveChangesAsync();
            return tempMail.Id;
        }
    }
}
