using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.MainApp
{
    public class OrganisationModel
    {
        [Key]
        public int ORGANISATION_ID { get; set; }
        public string? ORGANISATION_NAME { get; set; }
        public string? EMAIL_ID1 { get; set; }
        public string? EMAIL_ID2 { get; set; }
        public string? CONTACT_NUMBER1 { get; set; }
        public string? CONTACT_NUMBER2 { get; set; }
        public string? CONTACT_NUMBER3 { get; set; }
        public string? ADDRESS_LINE1 { get; set; }
        public string? ADDRESS_LINE2 { get; set; }
        public string? ADDRESS_LINE3 { get; set; }
        public string? CITY { get; set; }
        public string? STATE { get; set; }
        public string? ZIPCODE { get; set; }
        public string? COUNTRY { get; set; }
        public decimal? CREATED_BY { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public decimal? UPDATED_BY { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public decimal ACTIVE_IND { get; set; }
        public int? CURRENT_USAGE { get; set; }
        public int? QR_CODE_ALLOWED { get; set; }
        public decimal STATUS { get; set; }
        public int? SUBSCRIPTION_COUNTER { get; set; }
        public int? COUNTRY_ID { get; set; }
        public string? SIGNED_BY_FIRST_NAME { get; set; }
        public string? SIGNED_BY_LAST_NAME { get; set; }
        public string? BUCKET_NAME { get; set; }
        public string? ORGANISATION_SHORT_NAME { get; set; }
        public int? NAME_COUNTER { get; set; }
        public int? plan_id { get; set; }
        public string? BILLING_COMPANY { get; set; }
        public string? BILLING_ADDRESS { get; set; }
        public int? BILLING_COUNTRY { get; set; }
        public string? BILLING_CITY { get; set; }
        public string? BILLING_STATE { get; set; }
        public string? BILLING_ZIPCODE { get; set; }
        public bool? IsSameBillingDetails { get; set; }
        public int ADDITIONAL_CERTIFICATES { get; set; }
        public string? SQUARE_CUSTOMER_ID { get; set; }
    }
}
