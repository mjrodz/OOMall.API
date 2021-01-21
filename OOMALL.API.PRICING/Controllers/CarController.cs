using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OOMALL.API.PRICING.Services;
using OOMALL.API.PRICING.ViewModels;
using System;
using System.Threading.Tasks;

namespace OOMALL.API.PRICING.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly IOOParkingLotRepository _ooParkingLotRepository;

        public CarController(ILogger<CarController> logger, IOOParkingLotRepository ooParkingLotRepository)
        {
            _logger = logger;
            _ooParkingLotRepository = ooParkingLotRepository;
        }

        [HttpPost("Park")]
        public async Task<IActionResult> PostAsync(CarParkRequest carParkRequest)
        {
            try
            {
                return Ok(await _ooParkingLotRepository.AddParkingTransactionAsync(carParkRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }

        [HttpPut("Unpark")]
        public async Task<IActionResult> PutAsync(CarUnparkRequest carUnparkRequest)
        {
            try
            {
                return Ok(await _ooParkingLotRepository.UpdateParkingTransactionAsync(carUnparkRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }
    }
}
