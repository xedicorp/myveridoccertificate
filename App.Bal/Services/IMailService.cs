using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Dto;
using App.Entity.Models.Mail;

namespace App.Bal.Services
{
    public interface IMailService
    {
        public void SendContactMail(ContactMail mailRequest);
        public void SendDemoMail(DemoRequestMail mailRequest);
        public void SendFreeSignUpMail(SignUpMail signUpMail);
        public void SendSignUpMail(SignUpMail signUpMail);
        public void SendUserCredential(SignUpMail signUpMail);
        public void SendPaymentFail(SignUpMail signUpMail);
        public void SendCustomPlanQuote(CustomPlan customPlan);
        public void SendPlanCancellation(UserApiDto userApiDto);

        public void SendFreeTrialEndMail(FreeTrialReminder reminder);
        public void SendFreeTrial3DayBeforeMail(FreeTrialReminder reminder);
        public void SendFreeTrial14DayBeforeMail(FreeTrialReminder reminder);
    }
}
