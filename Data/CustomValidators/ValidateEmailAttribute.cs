using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MCS.HomeSite.Data.CustomValidators
{
    public class ValidateEmailAttribute : ValidationAttribute
    {        
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var emails = value == null ? string.Empty : value.ToString();

            if (emails.Contains(","))
                return new ValidationResult("Emails must be separated using a semi-colon.");

            var regex = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

            foreach(var email in emails.Split(";", StringSplitOptions.RemoveEmptyEntries))
            {
                if (!regex.IsMatch(email.Trim()))
                    return new ValidationResult($"'{email.Trim()}' is not a valid email address.");
            }

            return ValidationResult.Success;
        }
    }
}
