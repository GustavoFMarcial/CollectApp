using System.ComponentModel.DataAnnotations;

namespace CollectApp.Attributes
{
    public class FilialValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string[] filial = ["Guaporé Ind.","Gênesis Matriz", "Gênesis Filial", "Luz Ind.", "Luz Rev.",
                "Original", "Exclusiva"];

            if (value is not string strValue || !filial.Contains(strValue))
            {
                return new ValidationResult("Origem coleta inválido");
            }

            return ValidationResult.Success;
        }
    }
}