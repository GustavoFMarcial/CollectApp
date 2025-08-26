using System.ComponentModel.DataAnnotations;

namespace CollectApp.Attributes
{
    public class SupplierValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string[] company = [
                "GMS", "RT Colors", "Potência", "Kaitos", "Papel Safra", "AL Industria",
                "C&A", "Glasspack", "UHP", "Floresta",
                ];

            if (value is not string strValue || !company.Contains(strValue))
            {
                return new ValidationResult("Fornecedor inválido");
            }

            return ValidationResult.Success;
        }
    }
}