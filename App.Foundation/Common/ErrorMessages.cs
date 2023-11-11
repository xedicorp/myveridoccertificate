using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Foundation.Common
{
    public class ErrorMessages
    {
        public const string InvalidRecaptchaToken = "Invalid reCAPTCHA Token";
        public const string InvalidRecaptchaResponse = "Invalid google reCAPTCHA verification response";
        public const string CaptchaVerificationFailed = "Unable to verify google recaptcha token";
        public const string NotaBoat = "Please verify that you are not bot.";
        public const string _500ServerError = "Something went wrong!";

        public const string UserExists = "User with email already exist.";
        public const string InvalidCustomerID = "Invalid Customer Id.";
        public const string InvalidCardData = "Card verification failed";
        public const string PasswordValidationFailed = "Password validation failed.";
        public const string UserInvalid = "Invalid User.";
        public const string UserCreationFail = "Unable to create user.";
        public const string PlanError = "Please specify valid plan.";
        public const string PlanTimeError = "Please specify valid plan time span.";
        public const string SubscriptionError = "Unable to create subscription.";
        public const string SubscriptionRetrieveError = "Unable to retrieve Subscription.";
        public const string CardUpdateError = "Unable to update card.";
        public const string ServerHasNoPlan = "Server has no configuration";
        public const string InvalidCard = "Invalid card. Please update your card.";
        public const string UpgradePlan = "You are in free plan. Please upgrade your plan.";
        public const string InvalidPrice = "Invalid Certificate Price.";
        public const string InvalidCertificateQty = "Invalid Certificate Quantity.";
        public const string InvalidTemplateQty = "Invalid Template Quantity.";
    }
}
