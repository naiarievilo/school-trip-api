using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotNetFiveApiDemo.Application.Common.Validation
{
    public class AtLeastOneOfAttribute : ValidationAttribute
    {
        private readonly string[] _properties;

        public AtLeastOneOfAttribute(params string[] properties)
        {
            _properties = properties;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = _properties.Select(p => validationContext.ObjectType.GetProperty(p))
                .Where(p => p != null)
                .ToList();

            foreach (var property in properties)
            {
                var currentValue = property.GetValue(validationContext.ObjectInstance, null);
                if (currentValue != null && !string.IsNullOrWhiteSpace(currentValue.ToString()))
                    return ValidationResult.Success;
            }

            return new ValidationResult("Email or username is required");
        }
    }
}