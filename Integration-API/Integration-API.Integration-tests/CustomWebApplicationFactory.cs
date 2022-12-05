using Integration_API.Auth;
using Integration_API.DataLayer.External;
using Integration_API.DataLayer.Internal;
using Integration_API.LogicLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Integration_API.Integration_tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        //basic dependencies
        private ICredentialsDataAcces _credentialsDataAcces;
        private IAuthorisation _authorisation;
        
        //integrations
        private IOpenWeatherMapCalls? _openWeatherMapCalls ;

        public void MockCredentialsDataAcces(ICredentialsDataAcces _credentialsDataAcces)
        {
            this._credentialsDataAcces = _credentialsDataAcces;
        }

        public void MockOpenWeatherMapCalls(IOpenWeatherMapCalls _openWeatherMapCalls)
        {
            this._openWeatherMapCalls = _openWeatherMapCalls;
        }

        public void MockAuthorisation(IAuthorisation _authorisation)
        {
            this._authorisation = _authorisation;
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var openWeatherMapServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IOpenWeatherMapService));

                services.Remove(openWeatherMapServiceDescriptor);

                var integrationsHelperDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IIntegrationsHelper));

                services.Remove(integrationsHelperDescriptor);

                var authorisationDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IAuthorisation));

                services.Remove(authorisationDescriptor);
                
                services.AddSingleton<IIntegrationsHelper>(new IntegrationsHelper(_credentialsDataAcces));
                
                services.AddSingleton(_authorisation);


                if (_openWeatherMapCalls != null)
                {
                    services.AddSingleton<IOpenWeatherMapService>(new OpenWeatherMapService(_openWeatherMapCalls, _credentialsDataAcces));
                }

            });
        }
    }
}
