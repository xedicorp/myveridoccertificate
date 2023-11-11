using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.MainApp
{
    public class MainUsers
    {
        [Key]
        public decimal USER_UUID { get; set; }
        public string? USERID_LOGIN { get; set; }
        public string? PASSWORD { get; set; }
        public string? FIRST_NAME { get; set; }
        public string? MIDDLE_NAME { get; set; }
        public string? LAST_NAME { get; set; }
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
        public decimal? STATUS { get; set; }
        public decimal? SECURITY_QUESTION_UUID { get; set; }
        public string? SECRET_ANSWER { get; set; }
        public decimal? LOGIN_ATTEMPTS { get; set; }
        public decimal? CREATED_BY { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public decimal? UPDATED_BY { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public bool? EMAIL_SUBSCRIPTION { get; set; }
        public decimal ACTIVE_IND { get; set; }
        public bool ADMIN_IND { get; set; }
        public string? SALT { get; set; }
        public DateTime? RESETLINK_DEADLINE { get; set; }
        public string? VERIFICATION_CODE { get; set; }
        public int? ORGANISATION_ID { get; set; }
        public int? COUNTRY_ID { get; set; }
        public string? position_title { get; set; }
        public int? certificates_used { get; set; }
        public string? user_code { get; set; }
        public int? CodeCounter { get; set; }
        public string? map_users { get; set; }
    }
}
