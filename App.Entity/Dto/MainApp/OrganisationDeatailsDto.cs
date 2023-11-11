using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Dto.MainApp
{
    public class OrganisationDeatailsDto
    {
        /// <summary>
        /// Gets or sets organisation name
        /// </summary>
        public string? OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets contact email
        /// </summary>
        public string? ContactEmail { get; set; }


        /// <summary>
        /// Gets or sets organisation created date
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Admin First Name
        /// </summary>
        public string? AdminFirstName { get; set; }

        /// <summary>
        /// Admin First Name
        /// </summary>
        public string? AdminName { get; set; }

        /// <summary>
        /// Admin Last Name
        /// </summary>
        public string? AdminLastName { get; set; }

        /// <summary>
        /// Country Code Id
        /// </summary>
        public int? CountryCodeId { get; set; }

        /// <summary>
        /// Contact Number
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// Admin User Password
        /// </summary>
        public string? AdminPassword { get; set; }

        /// <summary>
        /// Organiation Address
        /// </summary>
        public string? OrganisationAddress { get; set; }

        /// <summary>
        /// Subscription Counter
        /// </summary>
        public int SubscriptionCounter { get; set; }

        /// <summary>
        /// Position Title
        /// </summary>
        public string? PositionTitle { get; set; }

        public string? OrganisationShortName { get; set;}
        public string SquareCustomerId { get; set; } = string.Empty;
        public string? CountryName { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? BillingCompanyName { get; set; }
        public string? BillingCompanyAddress { get; set; }
        public string? BillingCompanyState { get; set; }
        public string? BillingCompanyCity { get; set; }
        public string? BillingCompanyZip { get; set; }
        public int? BillingCompanyCountry { get; set; }
        public bool? IsBillingSame { get; set; }
    }
}
