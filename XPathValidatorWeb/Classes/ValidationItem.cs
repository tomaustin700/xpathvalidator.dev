using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XPathValidatorWeb.Classes
{
    public class ValidationItem : IValidatableObject
    {

        [BindProperty]
        [Required]
        [Display(Name = "XPath Expression")]
        public string XPathExpression { get; set; }

        [BindProperty]
        [Display(Name = "XML")]
        public string XML { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            try
            {
                System.Xml.XPath.XPathExpression.Compile(XPathExpression);
            }
            catch (Exception ex)
            {
                results.Add(new ValidationResult(ex.Message));
            }

            results.Add(new ValidationResult("Validated Successfully"));


            return results;
        }
    }
}
