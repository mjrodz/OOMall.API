using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OOMALL.API.PRICING.Services;
using System;
using System.Threading.Tasks;

namespace OOMALL.API.PRICING.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryPointsController : ControllerBase
    {
        private readonly ILogger<EntryPointsController> _logger;
        private readonly IOOParkingLotRepository _ooParkingLotRepository;

        public EntryPointsController(ILogger<EntryPointsController> logger, IOOParkingLotRepository ooParkingLotRepository)
        {
            _logger = logger;
            _ooParkingLotRepository = ooParkingLotRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _ooParkingLotRepository.GetAllEntryPointsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }
    }
}
