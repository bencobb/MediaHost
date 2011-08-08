using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MediaHost.Helpers;

namespace MediaHost.Controllers
{
    public class BaseController : Controller
    {
        public bool RunValidationForTest = false;

        public bool IsValid(object obj)
        {
            if(RunValidationForTest)
            {
                var validationContext = new ValidationContext(obj, null, null);
                var validationResults = new List<ValidationResult>();

                Validator.TryValidateObject(obj, validationContext, validationResults, true);

                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                }
            }

            return ModelState.IsValid;
        }

        public ContentResult ContentResult<T>(T obj)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return new ContentResult { Content = string.Join("\r\n", errors), ContentType = "text/plain" };
            }
            else
            {
                string json = obj.ToJsonFromObj(); //get some json from your DB
                return new ContentResult { Content = json, ContentType = "application/json" };
            }
        }
    }
}