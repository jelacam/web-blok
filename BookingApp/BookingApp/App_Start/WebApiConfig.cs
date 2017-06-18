using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using BookingApp.Models;
using System.Web.Http.OData.Extensions;
using System.Net.Http.Headers;

namespace BookingApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {	
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            //config.EnableCors(cors);


            //za odata

            //ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<Accommodation>("AccommodationsPagingController");
            //config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes             // podrazumevano vrati Json rezultat umesto XML-a
                             .Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
