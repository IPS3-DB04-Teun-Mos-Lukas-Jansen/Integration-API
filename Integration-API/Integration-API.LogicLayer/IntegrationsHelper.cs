using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Integration_API.Models.OpenWeatherMap;

namespace Integration_API.LogicLayer
{
    public static class IntegrationsHelper
    {
        public static string GetAllAvailableIntegrations()
        {
            //TBA
            string value = "[{\"Name\":\"OpenWeatherMapCredentials\",\"imageurl\":\"https//boeitniet\",\"properties\":[{\"Name\":\"isActive\",\"type\":\"boolean\"},{\"Name\":\"City\",\"type\":\"string\"}]}]";

            return value;
        }
    }
}
