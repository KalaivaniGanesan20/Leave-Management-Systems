using System.ComponentModel.DataAnnotations;
using leave.Models.Domain;
#nullable disable
namespace CustomValidation
{
    //CustomValidator class to validate From date and To date
public class FromDateToToDateAttribute : ValidationAttribute
{
   protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (Leaves)validationContext.ObjectInstance;
        if (model.leaveFrom > model.leaveTo)
        {
            return new ValidationResult("From date should be less than to date.");
        }

        return ValidationResult.Success;
    }
}
} 