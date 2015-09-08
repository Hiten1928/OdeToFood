using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

namespace OdeToFood
{
    internal class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
//            var corsAttr = new EnableCorsAttribute("*", "*", "*");
//            configuration.EnableCors(corsAttr);

            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}