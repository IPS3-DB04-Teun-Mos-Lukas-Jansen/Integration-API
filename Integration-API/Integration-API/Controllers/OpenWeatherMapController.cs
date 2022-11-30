using Integration_API.Auth;
using Integration_API.LogicLayer;
using Integration_API.Models.OpenWeatherMap;
using Microsoft.AspNetCore.Mvc;

namespace Integration_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenWeatherMapController : ControllerBase
    {
        private readonly ILogger<OpenWeatherMapController> _logger;
        private readonly IOpenWeatherMapService _openWeatherMapService;
        private readonly IAuthorisation _authorisation;
        public OpenWeatherMapController(ILogger<OpenWeatherMapController> logger, IOpenWeatherMapService openWeatherMapService, IAuthorisation authorisation)
        {
            _logger = logger;
            _openWeatherMapService = openWeatherMapService;
            _authorisation = authorisation;
        }

        [HttpGet("/openweathermap/{id_token}")]
        public async Task<IActionResult> GetLocalWeather(string id_token)
        {
            try
            {
                string userId = await _authorisation.ValidateIdToken(id_token);
                OpenWeatherMapResponse result = await _openWeatherMapService.GetLocalWeatherForecast(userId);
                string response = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return Ok(response);
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
            catch (ArgumentNullException)
            {
                return NotFound("bazinga");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("city not found");
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return StatusCode(406, "integration disabled");
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest("No config");
                }
                else
                {
                    return StatusCode(500, "Whoopsie something went wrong https://http.cat/500");
                }
            }
            

        }

        [HttpPost("/openweathermap/{id_token}")]
        public async Task<IActionResult> SetLocalWeatherCredentials(string id_token, OpenWeatherMapCredentials credentials)
        {
            try
            {
                string userId = await _authorisation.ValidateIdToken(id_token);
                await _openWeatherMapService.SetLocalWeatherForecastCredentials(userId, credentials);
                return Ok("credentials set!");
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
        }
    }
}
