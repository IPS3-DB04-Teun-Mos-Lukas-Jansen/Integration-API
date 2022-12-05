using Integration_API.DataLayer.Internal;
using Integration_API.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Integration_API.LogicLayer
{
    public class IntegrationsHelper : IIntegrationsHelper
    {
        public static string GetAllAvailableIntegrations()
        {
            List<IntegrationInfo> integrations = new List<IntegrationInfo>();

            List<Property> openWeatherMapProps = new List<Property>()
            {
                new Property("Active", "boolean"),
                new Property("City", "string"),
            };


            integrations.Add(new IntegrationInfo(
                "openWeatherMap",
                "open weather map",
                "https://openweathermap.org/img/wn/02d@2x.png",
                openWeatherMapProps,
                new List<string>() { "weather", "forecast" },
                1,
                "Weather data in a fast and easy-to-use way"
                ));


            string value = JsonConvert.SerializeObject(integrations);

            return value;
        }

        private readonly ICredentialsDataAcces _credentialsDataAcces;
        public IntegrationsHelper(ICredentialsDataAcces _credentialsDataAcces)
        {
            this._credentialsDataAcces = _credentialsDataAcces;
        }

        public async Task<string> GetIntegrationCredentials(string userId)
        {
            var creds = await _credentialsDataAcces.GetAllCredentials(userId);

            if (creds == null) { return "[]"; }
            //credt to array

            JObject jObject = JObject.Parse(creds.ToJson());
            List<string> names = jObject.Properties().Select(p => p.Name).ToList();

            JArray credsList = new JArray();
            int i = 0;
            foreach (JProperty value in jObject.Children())
            {
                JObject credential = new JObject();
                credential["name"] = names[i];
                credential.Add("credentials", value.Value);
                credsList.Add(credential);
                i++;
            }

            //JArray to json string
            string json = credsList.ToString();



            return json;
        }

        public async Task<int> RemoveIntegrationCredentials(string userId, string integrationName)
        {
            return await _credentialsDataAcces.RemoveIntegrationCredentials(userId, integrationName);
        }
    }
}
