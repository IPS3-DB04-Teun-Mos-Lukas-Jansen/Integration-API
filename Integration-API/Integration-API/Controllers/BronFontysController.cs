using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Integration_API.Auth;
using Integration_API.LogicLayer;
using Integration_API.Models.BronFontys;

namespace Integration_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BronFontysController : ControllerBase
    {
        private readonly ILogger<BronFontysController> _logger;
        private readonly IBronFontysService _bronFontysService;
        private readonly IAuthorisation _authorisation;
        
        public BronFontysController(ILogger<BronFontysController> logger, IBronFontysService bronFontysService, IAuthorisation authorisation)
        {
            _logger = logger;
            _bronFontysService = bronFontysService;
            _authorisation = authorisation;
        }
        

        [HttpGet("/bronfontys/{id_token}")]
        public async Task<IActionResult> GetFeed(string id_token)
        {
            try
            {
                string userId = await _authorisation.ValidateIdToken(id_token);
                List<BronFontysResponse> response = await _bronFontysService.GetFeed(userId);
                return Ok(response);
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }

        }

        [HttpPost("/bronfontys/{id_token}")]
        public async Task<IActionResult> SetBronFontysCredentials(string id_token, BronFontysCredentials credentials)
        {
            try
            {
                string userId = await _authorisation.ValidateIdToken(id_token);
                await _bronFontysService.SetBronFontysCredentials(userId, credentials);
                return Ok("credentials set!");
            }
            catch (Google.Apis.Auth.InvalidJwtException)
            {
                return BadRequest("invalid token");
            }
        }

    }
}
