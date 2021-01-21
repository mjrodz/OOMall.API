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
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IOOParkingLotRepository _ooParkingLotRepository;

        public TransactionsController(ILogger<TransactionsController> logger, IOOParkingLotRepository ooParkingLotRepository)
        {
            _logger = logger;
            _ooParkingLotRepository = ooParkingLotRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _ooParkingLotRepository.GetAllParkingTransactionsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }
    }
}
