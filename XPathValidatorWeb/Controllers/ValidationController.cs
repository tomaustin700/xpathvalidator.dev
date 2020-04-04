using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPathValidatorWeb.Classes;
using Wmhelp.XPath2;
using System.Xml.Linq;

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
                var xpath = System.Xml.XPath.XPathExpression.Compile(validationItem.XPathExpression);

                if (!string.IsNullOrEmpty(validationItem.XML))
                {
                    using (TextReader sr = new StringReader(validationItem.XML))
                    {
                        XNode node = XDocument.Load(sr);
                        XmlDocument document = new XmlDocument { InnerXml = validationItem.XML };
                        XPathNavigator navigator = document.CreateNavigator();
                        return Ok(Evaluate(xpath, node, navigator));
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Valid XPath");
        }

        private string Evaluate(XPathExpression expression, XNode node, XPathNavigator nav)
        {
            switch (expression.ReturnType)
            {

                case XPathResultType.NodeSet:

                    var nodes = node.XPath2Select(expression.Expression);

                    StringBuilder sb = new StringBuilder();

                    foreach (var single in nodes)
                    {
                        sb.Append(single.ToString());
                        sb.Append(Environment.NewLine);

                    }

                    return sb.ToString();

                default:
                    return nav.XPath2Evaluate(expression.Expression).ToString();
            }
        }
    }
}