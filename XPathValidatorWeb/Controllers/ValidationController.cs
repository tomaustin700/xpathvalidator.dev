using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPathValidatorWeb.Classes;

namespace XPathValidatorWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {

        [HttpPost]
        public IActionResult Validate([FromBody]ValidationItem validationItem)
        {
            if (string.IsNullOrEmpty(validationItem.XPathExpression))
                return BadRequest("XPath Expression is required");

            try
            {
                System.Xml.XPath.XPathExpression.Compile(validationItem.XPathExpression);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}