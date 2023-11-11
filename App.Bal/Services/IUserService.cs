using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Models;
using App.Entity.Models.SignUp;
using App.Entity.Models.Plan;
using App.Entity.Models.MainApp;
using App.Entity.Dto.MainApp;
using System.Data;

namespace App.Bal.Services
{
    public interface IUserService
    {
        public Task<bool> IsUserExist(string email);
        public Task<int> CreateAsync(RegisterUser appUser);
        public Task<int> CreateAppUserAsync(AppUser appUser);
        public Task<int> UpdateAsync(RegisterUser appUser);
        public Task<int> UpdateAppUserAsync(AppUser appUser);
        public Task<RegisterUser?> FindByEmailAsync(string email);
        public Task<AppUser?> FindAppUserByEmailAsync(string email);
        public bool FindMainAppUserByEmail(string email);
        public Task<int> CreateSubscriptionAsync(Subscription subscription);
        public Task<int> UpdateSubscriptionAsync(Subscription subscription);
        public Task<RegisterUser?> FindByCustomerId(string customerId);
        public Task<AppUser?> FindAppUserByCustomerId(string customerId);
        public Task<AppUser?> FindAppUserById(int id);
        public Task<int> CreateTempRegister(string email, string hash, string password);
        public Task<RegisterTempMail?> FindTempRegister(string email);
        public Task<int> UpdateTempRegister(RegisterTempMail tempMail);


        #region Main app database access
        public List<CountryModel> GetCountryModels();        
        public List<OrganisationPlan> GetOrganisationPlans();
        public Task<int> AddOrganisation(OrganisationDeatailsDto deatailsDto);
        public void UpdateBalance(int balance, string customerId);
        public void UpdateOrganisationPlan(string customerId, int planId);
        public void UpdateTemplateRequest(string customerId, int requestCount);
        public void DisableOrganisation(string customerId);
        public void UpdateBalanceInBatch(DataTable dataTable, int batchSize, ref string errMsg);
        #endregion
    }
}
