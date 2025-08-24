using System.ComponentModel.DataAnnotations;

namespace CollectApp.Attributes
{
    public class StatusValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string[] status = ["collected", "deleted"];

            if (value is not string strValue || !status.Contains(strValue))
            {
                return new ValidationResult("Invalid status");
            }

            return ValidationResult.Success;
        }
    }
}