using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Wmhelp.XPath2;
using XPathValidatorAPI.Classes;

namespace XPathValidatorAPI
{
    public static class API
    {
        [Function("Validate")]
        public static async Task<HttpResponseData> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var validationItem = await JsonSerializer.DeserializeAsync<ValidationItem>(req.Body, serializeOptions);

            if (string.IsNullOrEmpty(validationItem.XPathExpression))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.WriteString("XPath Expression is required");
                return badResponse;

            }

            try
            {
                var xpath = XPathExpression.Compile(validationItem.XPathExpression);

                if (!string.IsNullOrEmpty(validationItem.XML))
                {
                    using (TextReader sr = new StringReader(validationItem.XML))
                    {
                        XNode node = XDocument.Load(sr);
                        XmlDocument document = new XmlDocument { InnerXml = validationItem.XML };
                        XPathNavigator navigator = document.CreateNavigator();

                        var okResponse = req.CreateResponse(HttpStatusCode.OK);
                        okResponse.WriteString(Evaluate(xpath, node, navigator));
                        return okResponse;

                    }

                }
            }
            catch (Exception ex)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.WriteString(ex.Message);
                return badResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString("Valid XPath");
            return response;
        }

        private static string Evaluate(XPathExpression expression, XNode node, XPathNavigator nav)
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
