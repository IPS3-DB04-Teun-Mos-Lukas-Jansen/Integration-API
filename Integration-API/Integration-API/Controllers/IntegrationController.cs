using Integration_API.Models.OpenWeatherMap;
using Integration_API.LogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integration_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        private readonly ILogger<IntegrationController> _logger;

        private readonly IIntegrationsHelper _integrationsHelper;
        public IntegrationController(ILogger<IntegrationController> logger, IIntegrationsHelper integrationsHelper)
        {
            _logger = logger;
            _integrationsHelper = integrationsHelper;
        }


        [HttpGet("/all")]
        public async Task<IActionResult> GetAllIntegrations()
        {
            return Ok(IntegrationsHelper.GetAllAvailableIntegrations());
        }

        [HttpGet("/credentials/{id_token}")]
        public async Task<IActionResult> GetIntegrationCredentials(string id_token)
        {
            try
            {
                string userId = await Authorisation.ValidateIdToken(id_token);
                string result = await _integrationsHelper.GetIntegrationCredentials(userId);
                return Ok(result);
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
            catch (NullReferenceException)
            {
                return NotFound("database entry not found");
            }
            
        }



        //remove integration for a user
        [HttpDelete("/credentials/remove/{id_token}/{integration}")]
        public async Task<IActionResult> RemoveIntegrationCredentials(string id_token, string integration)
        {
            try
            {
                string userId = await Authorisation.ValidateIdToken(id_token);
                int rows = await _integrationsHelper.RemoveIntegrationCredentials(userId, integration);
                
                if (rows == 0)
                {
                    return NotFound("database entry not found");
                }
                else
                {
                    return Ok("credentials removed!");
                }
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
            catch (ArgumentNullException)
            {
                return NotFound("database entry not found");
            }
        }




    }
}
