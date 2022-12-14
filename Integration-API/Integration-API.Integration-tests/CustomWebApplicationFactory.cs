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
        private IBronFontysCalls? _bronFontysCalls;

        //basic dependencies
        public void MockCredentialsDataAcces(ICredentialsDataAcces _credentialsDataAcces)
        {
            this._credentialsDataAcces = _credentialsDataAcces;
        }
        public void MockAuthorisation(IAuthorisation _authorisation)
        {
            this._authorisation = _authorisation;
        }

        //integrations
        public void MockOpenWeatherMapCalls(IOpenWeatherMapCalls _openWeatherMapCalls)
        {
            this._openWeatherMapCalls = _openWeatherMapCalls;
        }
        public void MockBronFontysCalls(IBronFontysCalls _bronFontysCalls)
        {
            this._bronFontysCalls = _bronFontysCalls;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //removes OpenWeatherMap Service
                var openWeatherMapServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IOpenWeatherMapService));
                services.Remove(openWeatherMapServiceDescriptor);

                //removes BronFontys Service
                var bronFontysServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IBronFontysService));
                services.Remove(bronFontysServiceDescriptor);

                //removes IntegrationsHelper Service
                var integrationsHelperDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(IIntegrationsHelper));
                services.Remove(integrationsHelperDescriptor);

                //removes Authorisation Service
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

                if (_bronFontysCalls != null)
                {
                    services.AddSingleton<IBronFontysService>(new BronFontysService(_bronFontysCalls, _credentialsDataAcces));
                }

            });
        }
    }
}
