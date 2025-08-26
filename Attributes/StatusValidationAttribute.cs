using System.ComponentModel.DataAnnotations;

namespace CollectApp.Attributes
{
    public class StatusValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string[] status = ["Coletado", "Deletado"];

            if (value is not string strValue || !status.Contains(strValue))
            {
                return new ValidationResult("Status inválido");
            }

            return ValidationResult.Success;
        }
    }
}