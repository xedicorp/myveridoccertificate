using App.Bal.Services;
using App.Entity.Models.Plan;
using App.Foundation.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Square.Models;
using System.Data;

namespace VeridocApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebjobServiceController : ControllerBase
    {


        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;

        public WebjobServiceController(IUserService userService, IPaymentService paymentService, IMailService mailService)
        {
            _userService = userService;
            _paymentService = paymentService;
            _mailService = mailService;
        }


        [HttpGet("free-trial-reminder")]
        public async Task<IActionResult> FreeTrialReminderMail()
        {
            int batchSize = 1000;
            try
            {
                int totalCount = await _paymentService.GetSubscriptionsTrialRemiderCountAsync();
                bool isModified = false;
                if (totalCount > batchSize)
                {
                    int loopCount = (int)Math.Ceiling(totalCount / (decimal) batchSize);
                    
                    for (int i = 0; i < loopCount; i++)
                    {
                        List<App.Entity.Models.Plan.Subscription> subscriptions = await _paymentService.GetSubscriptionsTrialReminderAsync(batchSize, i);
                        SendTrialMails(subscriptions, ref isModified);
                        if (isModified)
                        {
                            await _paymentService.UpdateSubscriptionsAsync(subscriptions);
                            isModified = false;
                        }
                        subscriptions.Clear();
                    }
                }
                else if (totalCount > 0)
                {
                    List<App.Entity.Models.Plan.Subscription> subscriptions = await _paymentService.GetSubscriptionsTrialReminderAsync(batchSize, 0);
                    SendTrialMails(subscriptions, ref isModified);
                    if (isModified)
                    {
                        await _paymentService.UpdateSubscriptionsAsync(subscriptions);
                        isModified = false;
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("update-balance")]
        public async Task<IActionResult> UpdateBalance()
        {
            string errMsg = string.Empty;
            int batchSize = 1000;
            int totalcount = await _paymentService.GetSubscriptionsCountAsync();

            DataTable dataTable = new();
            
            if (totalcount > batchSize)
            {
                int loopCount = (int)Math.Ceiling(totalcount / (decimal)batchSize);
                for (int i = 0; i < loopCount; i++)
                {
                    List<App.Entity.Models.Plan.Subscription> subscriptions = await _paymentService.GetSubscriptionsAsync(batchSize, i);
                    dataTable.Clear();
                    dataTable.Columns.Add(new DataColumn() { ColumnName = "SubscriptionCounter", DataType = typeof(int) });
                    dataTable.Columns.Add(new DataColumn() { ColumnName = "SquareCustomerId", DataType = typeof(string) });
                    foreach (App.Entity.Models.Plan.Subscription subscription in subscriptions)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["SquareCustomerId"] = subscription.CustomerId;
                        dataRow["SubscriptionCounter"] = subscription.PlanInfo.Certificates;

                        dataTable.Rows.Add(dataRow);
                        subscription.CurrentMonth++;
                    }
                    dataTable.AcceptChanges();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row.SetModified();
                    }
                    _userService.UpdateBalanceInBatch(dataTable, batchSize, ref errMsg);
                    await _paymentService.UpdateSubscriptionsAsync(subscriptions);
                }
            }
            else if(totalcount > 0)
            {
                dataTable.Clear();
                dataTable.Columns.Add(new DataColumn() { ColumnName = "SubscriptionCounter", DataType = typeof(int) });
                dataTable.Columns.Add(new DataColumn() { ColumnName = "SquareCustomerId", DataType = typeof(string) });
                List<App.Entity.Models.Plan.Subscription> subscriptions = await _paymentService.GetSubscriptionsAsync(batchSize, 0);
                foreach (App.Entity.Models.Plan.Subscription subscription in subscriptions)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["SquareCustomerId"] = subscription.CustomerId;
                    dataRow["SubscriptionCounter"] = subscription.PlanInfo.Certificates;

                    dataTable.Rows.Add(dataRow);
                    subscription.CurrentMonth++;
                }
                dataTable.AcceptChanges();
                foreach (DataRow row in dataTable.Rows)
                {
                    row.SetModified();
                }
                _userService.UpdateBalanceInBatch(dataTable, batchSize, ref errMsg);
                await _paymentService.UpdateSubscriptionsAsync(subscriptions);
            }

            return Ok();
        }


        private void SendTrialMails(List<App.Entity.Models.Plan.Subscription> subscriptions, ref bool isModified)
        {
            foreach (var item in subscriptions)
            {
                if (item.StartTime.AddDays(14) < DateTime.UtcNow)
                {
                    _mailService.SendFreeTrial14DayBeforeMail(new App.Entity.Models.Mail.FreeTrialReminder()
                    {
                        Email = item.AppUser?.Email,
                        FirstName = item.AppUser?.FirstName,
                        LastName = item.AppUser?.LastName
                    });
                }
                if (item.StartTime.AddDays(27) < DateTime.UtcNow)
                {
                    _mailService.SendFreeTrial3DayBeforeMail(new App.Entity.Models.Mail.FreeTrialReminder()
                    {
                        Email = item.AppUser?.Email,
                        FirstName = item.AppUser?.FirstName,
                        LastName = item.AppUser?.LastName,
                        ExpireDate = item.EndTime.ToString("dd/MM/yyyy")
                    });
                }
                if (item.EndTime <= DateTime.UtcNow)
                {
                    isModified = true;
                    item.IsActive = false;
                    item.Status = _paymentService.SubscriptionStatusFail;
                    _mailService.SendFreeTrialEndMail(new App.Entity.Models.Mail.FreeTrialReminder()
                    {
                        Email = item.AppUser?.Email,
                        FirstName = item.AppUser?.FirstName,
                        LastName = item.AppUser?.LastName
                    });
                }
            }       
        }
    }
}
