using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Dto.MainApp;
using Microsoft.Data.SqlClient;
using System.Data;
using App.Entity.Models.MainApp;

namespace App.Dal.AppDb
{
    public class MainAppDb
    {
        private readonly string _dbConnectionString;

        public MainAppDb(string connectionString)
        {
            _dbConnectionString = connectionString;
        }

        public Tuple<int, string> AddOrganisation(OrganisationDeatailsDto organisation)
        {
            string errorMsg = string.Empty;

            DataTable templateIds = new DataTable("IDS");
            templateIds.Columns.Add(new DataColumn("ID"));
            templateIds.Columns["ID"].AllowDBNull = true;

            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter("@OrganisationName", organisation.OrganisationName),
                new SqlParameter("@UserFirstName", organisation.AdminFirstName),
                new SqlParameter("@UserLastName", organisation.AdminLastName),
                new SqlParameter("@UserPassword", organisation.AdminPassword),
                new SqlParameter("@ContactEmail", organisation.ContactEmail),
                new SqlParameter("@CountryCodeId", organisation.CountryCodeId),
                new SqlParameter("@ContactNumber", organisation.ContactNumber),
                new SqlParameter("@SquareCustomerId", organisation.SquareCustomerId),
                new SqlParameter("@CountryName", organisation.CountryName),
                new SqlParameter("@State", organisation.State),
                new SqlParameter("@City", organisation.City),
                new SqlParameter("@Zipcode", organisation.ZipCode),
                new SqlParameter("@BillingCompanyName", organisation.BillingCompanyName),
                new SqlParameter("@BillingCompanyAddress", organisation.BillingCompanyAddress),
                new SqlParameter("@BillingCompanyCountry", organisation.BillingCompanyCountry),
                new SqlParameter("@BillingCompanyState", organisation.BillingCompanyState),
                new SqlParameter("@BillingCompanyCity", organisation.BillingCompanyCity),
                new SqlParameter("@BillingCompanyZip", organisation.BillingCompanyZip),
                new SqlParameter("@OrganisationAddress", organisation.OrganisationAddress),
                new SqlParameter("@SubScriptionCounter", organisation.SubscriptionCounter),
                new SqlParameter("@ShortName", organisation.OrganisationShortName),
                new SqlParameter("@PlanId", organisation.PlanId),
                new SqlParameter("@PositionTitle", organisation.PositionTitle),
                new SqlParameter("@IsBillingSame", organisation.IsBillingSame),
                new SqlParameter("@TemplateIds", templateIds){
                    SqlDbType = SqlDbType.Structured
                },
                new SqlParameter("@CustomTemplateIds",templateIds){
                    SqlDbType = SqlDbType.Structured
                }
            };

            SqlHelper sqlHelper = new ("VD_MasterAdmin_AddOrganisation", connectionString: _dbConnectionString);
            DataTable dataTable = sqlHelper.GetDataTable(sp.ToArray(), ref errorMsg);
            sqlHelper.Dispose();
            if (dataTable.Rows.Count > 0)
            {
                int organisationId = Convert.ToInt32(dataTable.Rows[0]["organisation_id"]);
                if (organisationId != -1)
                {
                    return Tuple.Create(organisationId, Convert.ToString(dataTable.Rows[0]["short_name"]) ?? string.Empty);
                }
            }
            return Tuple.Create(-1, string.Empty);
        }

        public void UpdateBucketName(int organisationId, string bukcetName)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = { new SqlParameter("@OrganisationId", organisationId), new SqlParameter("@BucketName", bukcetName) };
            SqlHelper sqlHelper = new ("VD_MasterAdmin_UpdateBucketName", connectionString: _dbConnectionString);
            sqlHelper.ExecuteNonQuery(parameter, ref errorMsg);
        }

        public List<CountryModel> GetCountryList()
        {
            string errorMsg = string.Empty;
            List<CountryModel> countryModels = new ();

            SqlHelper sqlHelper = new("VD_ContryCodeDetails", connectionString: _dbConnectionString);
            DataTable dataTable = sqlHelper.GetDataTable(ref errorMsg);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CountryModel model = new ();
                    model.COUNTRY_ID = row.Field<int>("COUNTRY_ID");
                    model.NICENAME = row.Field<string>("NICENAME");
                    model.PHONECODE = row.Field<int>("PHONECODE");
                    countryModels.Add(model);   
                }
            }
            return countryModels;
        }

        public List<OrganisationPlan> GetPlans()
        {
            string errorMsg = string.Empty;
            List<OrganisationPlan> organisationPlans = new ();
            SqlHelper sqlHelper = new("VD_Plans", connectionString: _dbConnectionString);
            DataTable dataTable = sqlHelper.GetDataTable(ref errorMsg);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    OrganisationPlan plan = new ();
                    plan.plan_id = row.Field<int>("plan_id");
                    plan.plan_name = row.Field<string>("plan_name");
                    plan.Cadence = row.Field<string>("Cadence");
                    plan.SquarePackageId = row.Field<string>("SquarePackageId");
                    plan.Description = row.Field<string>("Description");
                    plan.Certificates = row.Field<int>("Certificates");
                    plan.Price = row.Field<decimal>("Price");
                    organisationPlans.Add(plan);
                }
            }
            return organisationPlans;
        }

        public bool IsUserExist(string email)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = { new SqlParameter("@Emails", email) };
            SqlHelper sqlHelper = new("VD_CMS_GetUser_Keyword", connectionString: _dbConnectionString);
            DataTable table = sqlHelper.GetDataTable(parameter, ref errorMsg);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void UpdateBalance(int balance, string customerId)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = { new SqlParameter("@SquareCustomerId", customerId), new SqlParameter("@Amount", balance) };
            SqlHelper sqlHelper = new("VD_MasterAdmin_UpdateOrganisationBalance", connectionString: _dbConnectionString);
            sqlHelper.ExecuteNonQuery(parameter, ref errorMsg);
        }

        public void DisableOrganisation(string customerId)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = { new SqlParameter("@SquareCustomerId", customerId) };
            SqlHelper sqlHelper = new("VD_MasterAdmin_UpdateOrganisationInactive", connectionString: _dbConnectionString);
            sqlHelper.ExecuteNonQuery(parameter, ref errorMsg);
        }

        public void UpdateOrganisationPlan(string customerId, int planId)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = {
                new SqlParameter("@SquareCustomerId", customerId), 
                new SqlParameter("@PlanId", planId),  
            };
            SqlHelper sqlHelper = new("VD_MasterAdmin_UpdateOrganisationPlan", connectionString: _dbConnectionString);
            sqlHelper.ExecuteNonQuery(parameter, ref errorMsg);
        }


        public void UpdateTemplteRequest(string customerId, int requestCount)
        {
            string errorMsg = string.Empty;
            SqlParameter[] parameter = {
                new SqlParameter("@SquareCustomerId", customerId),
                new SqlParameter("@Template", requestCount),
            };
            SqlHelper sqlHelper = new("VD_MasterAdmin_UpdateTemplateRequest", connectionString: _dbConnectionString);
            sqlHelper.ExecuteNonQuery(parameter, ref errorMsg);
        }



        public void UpdateBalanceBatch(DataTable table, int batchSize, ref string errMsg)
        {
            try
            {
                string sql = @"UPDATE ORGANISATION  
                                SET SUBSCRIPTION_COUNTER += @QR_CODE,
                                  QR_CODE_ALLOWED += @QR_CODE,
                                  ACTIVE_IND = 1,
                                  STATUS = 1
                                  WHERE SquareCustomerId = @SquareCustomerId";

                using SqlConnection connection = new(_dbConnectionString);
                SqlDataAdapter dataAdapter = new()
                {
                    UpdateBatchSize = batchSize,
                    UpdateCommand = new SqlCommand(sql, connection)
                };
                dataAdapter.UpdateCommand.Parameters.Add(new SqlParameter() { ParameterName = "QR_CODE", SourceColumn = "SubscriptionCounter", SqlDbType = SqlDbType.Int });
                dataAdapter.UpdateCommand.Parameters.Add("@SquareCustomerId", SqlDbType.VarChar, 100, "SquareCustomerId");
                dataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                dataAdapter.Update(table);

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
        }
    }
}
