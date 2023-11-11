using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Dto.MainApp;
using App.Entity.Models.MainApp;

namespace App.Bal.Services
{
    public interface IMainAppService
    {
        public Task<int> AddOrganisation(OrganisationDeatailsDto deatailsDto);
        public bool IsExist(string email);
        public void UpdateBalance(int balance, string customerId);
        public void UpdateOrganisationPlan(string customerId, int planid);
        public void UpdateTemplateCount(string customerId, int requestCount);
        public void DisableOrganisation(string customerId);
        public List<CountryModel> GetCountryModels();
        public List<OrganisationPlan> GetOrganisationPlan();
        public void UpdateBalnceInBatch(DataTable dataTable, int batchSize, ref string errMsg);
    }
}
