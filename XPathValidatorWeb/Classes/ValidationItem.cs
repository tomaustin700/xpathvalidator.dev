using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XPathValidatorWeb.Classes
{
    public class ValidationItem
    {

        public string XPathExpression { get; set; }


        public string XML { get; set; }


    }
}
