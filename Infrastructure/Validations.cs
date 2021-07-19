using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Infrastructure
{
    public class FullDateValid : ValidationAttribute, IClientModelValidator
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            // Это конечно смешно! но эта валидация нацелена на JS проверку, на поля 
            // разрешабщие null т.е. ?
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            if(context.Attributes.Any(t=>t.Key == "data-val"))
                context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-FullDateValid", ErrorMessage);
        }
    }
    //Проверяем в ходит ли дата в указанный диапазон дат который строится по дням назат и вперед
    public class MyRangeDays : ValidationAttribute, IClientModelValidator
    {
        public short DaysBack;
        public short DaysForword;
        public new string ErrorMessage { get; set; } = "Дата не попадает в диапазон";

        protected override ValidationResult IsValid (object value, ValidationContext validationContext)
        {
            if(value == null)
                return ValidationResult.Success;
            try
            {
                DateTime valueDate = Convert.ToDateTime(value);
                DateTime startBack = DateTime.Now.AddDays(-DaysBack).Date;
                DateTime startForword = DateTime.Now.AddDays(DaysForword).Date;
                if(startBack <= valueDate && valueDate <= startForword)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage);

            }
            catch {
                return new ValidationResult(ErrorMessage); 
            }
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.Any(t => t.Key == "data-val"))
                context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-MyRangeDays", ErrorMessage);
            context.Attributes.Add("min", DateTime.Now.AddDays(-DaysBack).ToString("yyyy-MM-dd"));
            context.Attributes.Add("max", DateTime.Now.AddDays(DaysForword).ToString("yyyy-MM-dd"));
        }
    }
    //public class ValidateEachItemAttribute : ValidationAttribute
    //{
    //    protected readonly List<ValidationResult> validationResults = new List<ValidationResult>();

    //    public override bool IsValid(object value)
    //    {
    //        var list = value as IEnumerable;
    //        if (list == null) return true;

    //        var isValid = true;

    //        foreach (var item in list)
    //        {
    //            var validationContext = new ValidationContext(item);
    //            var isItemValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
    //            isValid &= isItemValid;
    //        }
    //        return isValid;
    //    }

    //    // I have ommitted error message formatting
    //}
}
