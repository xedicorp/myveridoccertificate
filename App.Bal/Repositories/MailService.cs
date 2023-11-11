using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Bal.Services;
using App.Entity.Models;
using Microsoft.Extensions.Configuration;
using App.Entity.Config;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using App.Foundation.Common;
using App.Entity.Models.Mail;
using App.Entity.Dto;

namespace App.Bal.Repositories
{
    public class MailService : IMailService
    {

        private readonly IConfiguration configuration;
        private readonly MailConfig mailConfig;
        private readonly AppConfig appConfig;

        public MailService(IConfiguration configuration)
        {
            this.configuration  = configuration;
            mailConfig = new MailConfig();
            appConfig = new AppConfig();
            configuration.GetSection(MailConfig.Path).Bind(mailConfig);
            configuration.GetSection(AppConfig.Path).Bind(appConfig);
        }


        public void SendContactMail(ContactMail mailRequest)
        {
            SendContact(mailRequest);
        }


        public async void SendCustomPlanQuote(CustomPlan customPlan)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "pricingEnquiry.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("ORGANISATIONNAME", customPlan.OrganisationName);
            fileContent = fileContent.Replace("ORGANISATIONID", customPlan.OrganisationId);
            fileContent = fileContent.Replace("USERNAME", customPlan.UserName);
            fileContent = fileContent.Replace("USEREMAIL", customPlan.UserEmail);
            fileContent = fileContent.Replace("USERID", customPlan.UserId);
            fileContent = fileContent.Replace("CUSTOMERNAME", customPlan.Name);
            fileContent = fileContent.Replace("BUSINESSNAME", customPlan.BusinessName);
            fileContent = fileContent.Replace("CUSTOMERMAIL", customPlan.Email);
            fileContent = fileContent.Replace("PHONENUMBER", customPlan.Phone);
            fileContent = fileContent.Replace("CERTIFICATESREQUIRED", customPlan.Certificates);
            fileContent = fileContent.Replace("REQUESTMESSAGE", customPlan.Request);
            fileContent = fileContent.Replace("PLANDURATION", customPlan.Duration);
            fileContent = fileContent.Replace("CADENCE", customPlan.Cadence);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(mailConfig.Email));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = "Custom Plan Quotation";
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public void SendDemoMail(DemoRequestMail mailRequest)
        {
            SendDemo(mailRequest);
        }

        public async void SendFreeSignUpMail(SignUpMail signUpMail)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "emailsignup.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("VERIFYLINK", signUpMail.VerifyLink);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(signUpMail.Email));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = signUpMail.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendFreeTrial14DayBeforeMail(FreeTrialReminder reminder)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "remindmailday14.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("FULLNAME", reminder.FirstName + " " + reminder.LastName);

            MandrillApi mandrillApi = new(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new()
            {
                new EmailAddress(reminder.Email)
            };

            EmailMessage emailMessage = new()
            {
                FromEmail = mailConfig.Email,
                FromName = mailConfig.FromName,
                To = toEmail,
                Subject = "VeriDoc Certificates Trial Reminder",
                Html = fileContent
            };

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendFreeTrial3DayBeforeMail(FreeTrialReminder reminder)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "remindmailday3.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("FULLNAME", reminder.FirstName + " " + reminder.LastName);
            fileContent = fileContent.Replace("EXPIREDATE", reminder.ExpireDate);

            MandrillApi mandrillApi = new(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new()
            {
                new EmailAddress(reminder.Email)
            };

            EmailMessage emailMessage = new()
            {
                FromEmail = mailConfig.Email,
                FromName = mailConfig.FromName,
                To = toEmail,
                Subject = "VeriDoc Certificates Trial Reminder",
                Html = fileContent
            };

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendFreeTrialEndMail(FreeTrialReminder reminder)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "remindmailday_end.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("FULLNAME", reminder.FirstName + " " + reminder.LastName);

            MandrillApi mandrillApi = new (mailConfig.ApiKey);

            List<EmailAddress> toEmail = new()
            {
                new EmailAddress(reminder.Email)
            };

            EmailMessage emailMessage = new()
            {
                FromEmail = mailConfig.Email,
                FromName = mailConfig.FromName,
                To = toEmail,
                Subject = "VeriDoc Certificates Trial Expire",
                Html = fileContent
            };

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendPaymentFail(SignUpMail signUpMail)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "paymentfailedmail.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("USEREMAIL", signUpMail.Email);
            fileContent = fileContent.Replace("PACKAGENAME", signUpMail.PackName);
            fileContent = fileContent.Replace("PACKAGEAMOUNT", signUpMail.Amount);
            fileContent = fileContent.Replace("REPAYLINK", signUpMail.RepayLink);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(signUpMail.Email));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = signUpMail.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendSignUpMail(SignUpMail signUpMail)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "regmailtemplate.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("FULLNAME", signUpMail.FullName);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(signUpMail.Email));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = signUpMail.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        public async void SendUserCredential(SignUpMail signUpMail)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "senduserpassword.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("FNAME", signUpMail.FirstName);
            fileContent = fileContent.Replace("USEREMAIL", signUpMail.Email);
            fileContent = fileContent.Replace("USERPASSWORD", signUpMail.Password);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(signUpMail.Email));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = signUpMail.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

        private async void SendContact(ContactMail mailRequest)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "admin_contact_mail.html");
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace("FIRSTNAME", mailRequest.FirstName);
            fileContent = fileContent.Replace("LASTNAME", mailRequest.LastName);
            fileContent = fileContent.Replace("EMAILID", mailRequest.Email);
            fileContent = fileContent.Replace("COUNTRYCODE", mailRequest.PhoneCode);
            fileContent = fileContent.Replace("PHONE", mailRequest.PhoneNumber);
            fileContent = fileContent.Replace("SUBJECT", mailRequest.Message);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(mailConfig.Email, "Admin"));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = mailRequest.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));

            path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "user_contact_reply_mail.html");
            fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace("FIRSTNAME", mailRequest.FirstName);
            fileContent = fileContent.Replace("LASTNAME", mailRequest.LastName);
            fileContent = fileContent.Replace("SUBJECT", mailRequest.Message);
            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);

            toEmail.Clear();
            toEmail.Add(new EmailAddress(mailRequest.Email));
            EmailMessage userEmailMessage = new EmailMessage();
            userEmailMessage.FromEmail = mailConfig.Email;
            userEmailMessage.FromName = appConfig.AppName;
            userEmailMessage.To = toEmail;
            userEmailMessage.Subject = mailRequest.Subject;
            userEmailMessage.Html = fileContent;

            List<EmailResult> emailResults1 = await mandrillApi.SendMessage(new SendMessageRequest(userEmailMessage));
        }

        private async void SendDemo(DemoRequestMail mailRequest)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "admin_demo_request_mail.html");
            string fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace("FIRSTNAME", mailRequest.FirstName);
            fileContent = fileContent.Replace("LASTNAME", mailRequest.LastName);
            fileContent = fileContent.Replace("EMAILID", mailRequest.Email);
            fileContent = fileContent.Replace("COUNTRYCODE", mailRequest.PhoneCode);
            fileContent = fileContent.Replace("PHONE", mailRequest.PhoneNumber);
            fileContent = fileContent.Replace("COMPANYNAME", mailRequest.Companyname);
            fileContent = fileContent.Replace("DATESLOT", mailRequest.Date);
            fileContent = fileContent.Replace("TIMESLOT", mailRequest.Time);
            fileContent = fileContent.Replace("COUNTRYNAME", mailRequest.Country);
            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);

            MandrillApi mandrillApi = new MandrillApi(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new List<EmailAddress>();
            toEmail.Add(new EmailAddress(mailConfig.Email, "Admin"));

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.FromEmail = mailConfig.Email;
            emailMessage.FromName = mailConfig.FromName;
            emailMessage.To = toEmail;
            emailMessage.Subject = mailRequest.Subject;
            emailMessage.Html = fileContent;

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));

            path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "user_demo_request_mail.html");
            fileContent = File.ReadAllText(path);
            fileContent = fileContent.Replace("FIRSTNAME", mailRequest.FirstName);
            fileContent = fileContent.Replace("LASTNAME", mailRequest.LastName);
            fileContent = fileContent.Replace("DATESLOT", mailRequest.Date);
            fileContent = fileContent.Replace("TIMESLOT", mailRequest.Time);
            fileContent = fileContent.Replace("COUNTRYNAME", mailRequest.Country);
            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);

            toEmail.Clear();
            toEmail.Add(new EmailAddress(mailRequest.Email));
            EmailMessage userEmailMessage = new EmailMessage();
            userEmailMessage.FromEmail = mailConfig.Email;
            userEmailMessage.FromName = mailConfig.FromName;
            userEmailMessage.To = toEmail;
            userEmailMessage.Subject = mailRequest.Subject;
            userEmailMessage.Html = fileContent;

            List<EmailResult> emailResults1 = await mandrillApi.SendMessage(new SendMessageRequest(userEmailMessage));
        }

        public async void SendPlanCancellation(UserApiDto userApiDto)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "plan_cancellation_mail.html");
            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("APPNAME", appConfig.AppName);
            fileContent = fileContent.Replace("APPURL", appConfig.AppUrl);
            fileContent = fileContent.Replace("APPLOGOURL", appConfig.AppLogoUrl);
            fileContent = fileContent.Replace("ORGANISATIONNAME", userApiDto.OrganisationName);
            fileContent = fileContent.Replace("ORGANISATIONID", userApiDto.OrganisationId);
            fileContent = fileContent.Replace("USERNAME", userApiDto.UserName);
            fileContent = fileContent.Replace("USEREMAIL", userApiDto.UserEmail);
            fileContent = fileContent.Replace("USERID", userApiDto.UserId);
            fileContent = fileContent.Replace("FEEDBACK", userApiDto.Feedback);


            MandrillApi mandrillApi = new(mailConfig.ApiKey);

            List<EmailAddress> toEmail = new()
            {
                new EmailAddress(userApiDto.UserEmail)
            };

            EmailMessage emailMessage = new()
            {
                FromEmail = mailConfig.Email,
                FromName = mailConfig.FromName,
                To = toEmail,
                Subject = "Subscription Cancellation",
                Html = fileContent
            };

            List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));
        }

    }
}
