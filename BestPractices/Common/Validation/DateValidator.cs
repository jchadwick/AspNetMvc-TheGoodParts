using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class DateValidator
    {
        public static ValidationResult AfterNow(DateTime value)
        {
            if (value.ToUniversalTime() >= DateTime.UtcNow)
                return ValidationResult.Success;

            return new ValidationResult("Value cannot be in the past");
        }
    }
}