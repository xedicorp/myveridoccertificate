using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Validation
{
    public class PriceValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal price = (decimal)value;
            if (price <= 0)
            {
                return new ValidationResult("Invalid Price!!");
            }
            return ValidationResult.Success;
        }
    }
}
