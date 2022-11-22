﻿using Integration_API.Models.OpenWeatherMap;
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
        private readonly IOpenWeatherMapService _openWeatherMapService;
        public IntegrationController(ILogger<IntegrationController> logger, IOpenWeatherMapService openWeatherMapService)
        {
            _logger = logger;
            _openWeatherMapService = openWeatherMapService;
        }


        [HttpGet("/all")]
        public async Task<IActionResult> GetAllIntegrations()
        {
            return Ok(IntegrationsHelper.GetAllAvailableIntegrations());
        }

        [HttpGet("/openweathermap/{id_token}")]
        public async Task<IActionResult> GetLocalWeather(string id_token)
        {
            try
            {
                string userId = await Authorisation.ValidateIdToken(id_token);
                OpenWeatherMapResponse result = await _openWeatherMapService.GetLocalWeatherForecast(userId);
                string response = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                return Ok(response);
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
            catch (System.ArgumentNullException)
            {
                return NotFound("bazinga");
            }
        }

        [HttpPost("/openweathermap/{id_token}")]
        public async Task<IActionResult> SetLocalWeatherCredentials(string id_token,OpenWeatherMapCredentials credentials)
        {
            try
            {
                string userId = await Authorisation.ValidateIdToken(id_token);
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