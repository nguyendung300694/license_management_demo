﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace LicenseManagement
{
    public static class WebApiConfig
    {
        public class CustomJSONFormatter : JsonMediaTypeFormatter
        {
            public CustomJSONFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            }
            //public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            //{
            //    base.SetDefaultContentHeaders(type, headers, mediaType);
            //    headers.ContentType = new MediaTypeHeaderValue("application/json");
            //}
        }

        public static void Register(HttpConfiguration config)
        {
            //Enable CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Register routes
            config.Routes.MapHttpRoute(
                name: "CustomApi",
                routeTemplate: "licenseapi/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "licenseapi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Add(new CustomJSONFormatter());//config api return JSON format
        }

        public static class CorsSettings
        {
            private static List<string> listOrigins = new List<string>
            {
                //"http://192.168.20.121",
                "http://192.168.21.52/"
            };

            public static string GetListOrigins()
            {
                if (listOrigins.Count > 0)
                {
                    return string.Join(",", listOrigins);//for only defined origins
                }
                else
                {
                    return "*";//for all origins
                }
            }
        }
    }
}
