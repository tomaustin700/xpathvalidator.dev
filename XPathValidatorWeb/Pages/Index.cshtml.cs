using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using XPathValidatorWeb.Classes;
using XPathValidatorWeb.Extensions;

namespace XPathValidatorWeb.Pages
{
    [BindProperties()]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public ValidationItem Main { get; set; }


        public void OnGet()
        {
        }


        public void OnPost()
        {
            if (ModelState.IsValid)
            {

            }

            //OnGet();

            //return OnGet();
        }
    }
}
