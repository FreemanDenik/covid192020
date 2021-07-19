using covid192020.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Infrastructure
{
    //public class NotFullDateDeleteAttribute : Attribute, IActionFilter
    //{
    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {
    //    }

    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        var model = context.ActionArguments["model"] as TreatmentViewModel;

    //    }
    //}
    public class TestT : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {

        }
    }
    public class TreatmentChange : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments["model"] as TreatmentViewModel;
            // При привязки данных (стандартная отправка форма post) IdentViruses имеет значение то добовляется ложная строка, вот ее надо удалять
            if (model.IdentViruses != null && model.IdentViruses[model.IdentViruses.Count - 1].CreateDate == new DateTime(0001, 01, 01))
            {
                model.IdentViruses.RemoveAt(model.IdentViruses.Count-1);
            }
           
            if (model.Dinamics != null)
            {
                List<DinamicViewModel> dinamics = model.Dinamics.ToList();
                foreach (var i in dinamics)
                {
                    if(!CustomHelper.ValidStringOnNullAndSpace(i.PaO2FiO2) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.SpO2) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.CHDD) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.PKT) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.CRB) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.WBC) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.PLT) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.D_Dim) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Ob_bel) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Ob_bil) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Moch) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Kreat) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.F_gen) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.ALT) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.ACT) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.LDG) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Lymphocytes) &&
                       !CustomHelper.ValidStringOnNullAndSpace(i.Ferritin))
                    {
                        model.Dinamics.Remove(i);
                    }
                }
            }
            //context.ModelState.Clear();

        }

        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var model1 = context.ActionArguments["model"] as TreatmentViewModel;
        //    model1.PatientId = 100;
        //    context.Result = new BadRequestObjectResult(model1);
        //    await next();
        //}

    }
}
