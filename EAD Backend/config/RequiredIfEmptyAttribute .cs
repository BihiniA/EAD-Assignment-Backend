using System.ComponentModel.DataAnnotations;

namespace EAD_Backend.config;

public class RequiredIfEmptyAttribute : ValidationAttribute
{

    private string DependentProperty { get; }

    public RequiredIfEmptyAttribute(string dependentProperty)
    {
        DependentProperty = dependentProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dependentPropertyValue = validationContext.ObjectInstance.GetType()
            .GetProperty(DependentProperty)
            .GetValue(validationContext.ObjectInstance);

        if (string.IsNullOrWhiteSpace(dependentPropertyValue?.ToString()))
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "This field is required.");
            }
        }

        return ValidationResult.Success;
    }
}