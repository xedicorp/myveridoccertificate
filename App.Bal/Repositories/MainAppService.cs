using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Bal.Services;
using App.Entity.Dto.MainApp;
using App.Dal.AppDb;
using App.Entity.Models.MainApp;
using System.Data;
using System.Numerics;

namespace App.Bal.Repositories
{
    public class MainAppService : IMainAppService
    {
        private readonly IStorageService _storageService;


        public MainAppService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<int> AddOrganisation(OrganisationDeatailsDto deatailsDto)
        {
            MainAppDb mainAppDb = new (_storageService.GetConnectionString);
            Tuple<int, string> tuple = mainAppDb.AddOrganisation(deatailsDto);
            if (tuple.Item1 != -1 && !string.IsNullOrEmpty(tuple.Item2))
            {
                string bucketFolderName = _storageService.FolderPrefix + "-" + tuple.Item2;
                bool folderCreated = await _storageService.CreateFolder(bucketFolderName + @"/");
                mainAppDb.UpdateBucketName(tuple.Item1, bucketFolderName);
                return 1;
            }
            else
            {
                return 2; //email is already in used
            }
        }

        public void DisableOrganisation(string customerId)
        {
            MainAppDb mainAppDb = new(_storageService.GetConnectionString);
            mainAppDb.DisableOrganisation(customerId);
        }

        public List<CountryModel> GetCountryModels()
        {
            MainAppDb db = new(_storageService.GetConnectionString);
            return db.GetCountryList();
        }

        public List<OrganisationPlan> GetOrganisationPlan()
        {
            MainAppDb db = new(_storageService.GetConnectionString);
            return db.GetPlans();
        }

        public bool IsExist(string email)
        {
            MainAppDb mainAppDb = new(_storageService.GetConnectionString);
            return mainAppDb.IsUserExist(email);
        }

        public void UpdateBalance(int balance, string customerId)
        {
            MainAppDb mainAppDb = new(_storageService.GetConnectionString);
            mainAppDb.UpdateBalance(balance, customerId);
        }

        public void UpdateBalnceInBatch(DataTable dataTable, int batchSize, ref string errMsg)
        {
            MainAppDb mainApp = new (_storageService.GetConnectionString);
            mainApp.UpdateBalanceBatch(dataTable, batchSize, ref errMsg);
        }

        public void UpdateOrganisationPlan(string customerId, int planid)
        {
            MainAppDb mainApp = new (_storageService.GetConnectionString);
            mainApp.UpdateOrganisationPlan(customerId, planid);
        }

        public void UpdateTemplateCount(string customerId, int requestCount)
        {
            MainAppDb mainApp = new(_storageService.GetConnectionString);
            mainApp.UpdateTemplteRequest(customerId, requestCount);
        }
    }
}
