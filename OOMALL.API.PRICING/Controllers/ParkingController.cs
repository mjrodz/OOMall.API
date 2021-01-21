using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OOMALL.API.PRICING.Models;
using OOMALL.API.PRICING.Services;
using OOMALL.API.PRICING.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOMALL.API.PRICING.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;
        private readonly IOOParkingLotRepository _ooParkingLotRepository;

        public ParkingController(ILogger<ParkingController> logger, IOOParkingLotRepository ooParkingLotRepository)
        {
            _logger = logger;
            _ooParkingLotRepository = ooParkingLotRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                List<ParkingDistance> parkingDistances = await _ooParkingLotRepository.GetAllDistancesAsync();
                List<EntryPoint> entryPoints = await _ooParkingLotRepository.GetAllEntryPointsAsync();
                List<ParkingSpace> parkingSpaces = await _ooParkingLotRepository.GetAllParkingSpacesAsync();

                object res = from ps in parkingSpaces
                             orderby ps.Code
                             select new
                             {
                                 ParkingSpaceCode = ps.Code,
                                 ps.IsOccupied,
                                 EntryPoints = from pd in parkingDistances
                                               join ep in entryPoints on pd.EntryPointId equals ep.Id
                                               where ps.Id == pd.ParkingSpaceId
                                               orderby ep.Name
                                               select new
                                               {
                                                   EntryPointName = ep.Name,
                                                   pd.DistanceInMeters
                                               }
                             };


                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                return Ok(await _ooParkingLotRepository.GetAllCarCategoriesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }

        [HttpPost("Request")]
        public async Task<IActionResult> ParkAsync(ParkRequest parkRequest)
        {
            try
            {
                return Ok(await _ooParkingLotRepository.GetNearestAvailableParkingAsync(parkRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }


        [HttpPost("Request2")]
        public async Task<IActionResult> Park2Async(ParkRequest parkRequest)
        {
            try
            {
                List<ParkingAvailablity> parkingAvailablities = await _ooParkingLotRepository.GetParkingAvailablitiesByCarCategoryIdAsync(parkRequest.CarCategoryId);

                Guid[] parkingCategoryIds = parkingAvailablities.Select(s => s.ParkingCategoryId).Distinct().ToArray();

                List<ParkingSpace> parkingSpaces = await _ooParkingLotRepository.GetAllParkingSpacesByParkingCategoryIdsAsync(parkingCategoryIds);

                Guid[] parkingSpaceIds = parkingSpaces.Select(s => s.Id).Distinct().ToArray();

                List<ParkingDistance> parkingDistance = await _ooParkingLotRepository.GetDistanceByEntryIdAndSpaceIdsAsync(parkRequest.EntryPointId, parkingSpaceIds);

                List<ParkingCategory> parkingCategories = await _ooParkingLotRepository.GetAllParkingCategoriesAsync();

                object res = (from pa in parkingAvailablities
                              join ps in parkingSpaces on pa.ParkingCategoryId equals ps.ParkingCategoryId
                              join pc in parkingCategories on ps.ParkingCategoryId equals pc.Id
                              join pd in parkingDistance on ps.Id equals pd.ParkingSpaceId
                              orderby pa.Priority, pd.DistanceInMeters
                              select new
                              {
                                  pd.ParkingSpaceId,
                                  pd.DistanceInMeters,
                                  ParkingSpaceCode = ps.Code,
                                  ParkingCategoryCode = pc.Code,
                                  ParkingCategoryDescription = pc.Description,
                                  pc.HourlyRate
                              }).FirstOrDefault();


                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }

        [HttpGet("Available/{carCategoryId}")]
        public async Task<IActionResult> GetAsync(Guid carCategoryId)
        {
            try
            {
                return Ok(await _ooParkingLotRepository.GetParkingAvailablitiesByCarCategoryIdAsync(carCategoryId));
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject(ex));
                return BadRequest();
            }
        }
    }
}
