using Microsoft.EntityFrameworkCore;
using OOMALL.API.PRICING.Models;
using OOMALL.API.PRICING.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOMALL.API.PRICING.Services
{
    public class OOParkingLotRepository : IOOParkingLotRepository
    {
        private readonly OOParkingLotDbContext _ooParkingLotDbContext;
        public OOParkingLotRepository(OOParkingLotDbContext ooParkingLotDbContext)
        {
            _ooParkingLotDbContext = ooParkingLotDbContext;
        }

        public async Task<List<CarCategory>> GetAllCarCategoriesAsync()
        {
            return await _ooParkingLotDbContext.CarCategories.ToListAsync();
        }
        public async Task<List<CarCategory>> GetCarCategoryByIdAsync(Guid id)
        {
            return await _ooParkingLotDbContext.CarCategories.Where(w => w.Id == id).ToListAsync();
        }
        public async Task<List<ParkingCategory>> GetAllParkingCategoriesAsync()
        {
            return await _ooParkingLotDbContext.ParkingCategories.ToListAsync();
        }
        public async Task<List<EntryPoint>> GetAllEntryPointsAsync()
        {
            return await _ooParkingLotDbContext.EntryPoints.Where(w => w.Status == true).ToListAsync();
        }
        public async Task<List<ParkingSpace>> GetAllParkingSpacesAsync()
        {
            return await _ooParkingLotDbContext.ParkingSpaces.Where(w => w.Status == true).ToListAsync();
        }
        public async Task<List<ParkingSpace>> GetAllParkingSpacesByParkingCategoryIdsAsync(Guid[] parkingCategoryIds)
        {
            return await _ooParkingLotDbContext.ParkingSpaces.Where(w => parkingCategoryIds.Contains(w.ParkingCategoryId) && !w.IsOccupied).ToListAsync();
        }
        public async Task<List<ParkingDistance>> GetAllDistancesAsync()
        {
            return await _ooParkingLotDbContext.ParkingDistances.ToListAsync();
        }
        public async Task<List<ParkingDistance>> GetDistanceByEntryIdAndSpaceIdsAsync(Guid entryPointId, Guid[] parkingSpaceIds)
        {
            return await _ooParkingLotDbContext.ParkingDistances.Where(w => w.EntryPointId == entryPointId && parkingSpaceIds.Contains(w.ParkingSpaceId)).ToListAsync();
        }
        public async Task<List<ParkingDistance>> GetAllDistancesByEntryPointIdAsync(Guid entryPointId)
        {
            return await _ooParkingLotDbContext.ParkingDistances.Where(w => w.EntryPointId == entryPointId).ToListAsync();
        }
        public async Task<List<ParkingAvailablity>> GetParkingAvailablitiesByCarCategoryIdAsync(Guid CarCategoryId)
        {
            return await _ooParkingLotDbContext.ParkingAvailablities.Where(w => w.CarCategoryId == CarCategoryId).ToListAsync();
        }




        public async Task<object> GetNearestAvailableParkingAsync(ParkRequest parkRequest)
        {
            return await (from pa in _ooParkingLotDbContext.ParkingAvailablities
                          where pa.CarCategoryId == parkRequest.CarCategoryId
                          join ps in _ooParkingLotDbContext.ParkingSpaces on pa.ParkingCategoryId equals ps.ParkingCategoryId
                          where ps.IsOccupied == false
                          join pd in _ooParkingLotDbContext.ParkingDistances on ps.Id equals pd.ParkingSpaceId
                          where pd.EntryPointId == parkRequest.EntryPointId
                          join pc in _ooParkingLotDbContext.ParkingCategories on ps.ParkingCategoryId equals pc.Id
                          orderby pa.Priority, pd.DistanceInMeters
                          select new
                          {
                              pd.ParkingSpaceId,
                              pd.DistanceInMeters,
                              ParkingSpaceCode = ps.Code,
                              ParkingCategoryCode = pc.Code,
                              ParkingCategoryDescription = pc.Description,
                              pc.HourlyRate
                          }).FirstOrDefaultAsync();
        }

        public async Task<ParkingSpace> AddParkingTransactionAsync(CarParkRequest carParkRequest)
        {
            if (_ooParkingLotDbContext.ParkingTransactions.Any(o => o.PlateNumber == carParkRequest.PlateNumber && o.ExitDate == null))
            {
                throw new Exception("Vehicle (Plate Number) already exists in the parking!");
            }

            var parkingSpace = await _ooParkingLotDbContext.ParkingSpaces.FirstOrDefaultAsync(f => f.Id == carParkRequest.ParkingSpaceId);
            if (parkingSpace != null)
            {
                DbSet<ParkingTransaction> customers = _ooParkingLotDbContext.Set<ParkingTransaction>();
                customers.Add(new ParkingTransaction
                {
                    Id = Guid.NewGuid(),
                    EntryDate = DateTime.Now,
                    ParkingSpaceId = carParkRequest.ParkingSpaceId,
                    PlateNumber = carParkRequest.PlateNumber,
                    CarCategoryId = carParkRequest.CarCategoryId
                });
                parkingSpace.IsOccupied = true;
            }
            await _ooParkingLotDbContext.SaveChangesAsync();
            return parkingSpace;
        }


        public async Task<ParkingTransaction> UpdateParkingTransactionAsync(CarUnparkRequest carUnparkRequest)
        {
            if (!_ooParkingLotDbContext.ParkingTransactions.Any(o => o.PlateNumber == carUnparkRequest.PlateNumber && o.ExitDate == null))
            {
                throw new Exception("Vehicle (Plate Number) not found in the parking!");
            }

            var previousParkingTransaction = await _ooParkingLotDbContext.ParkingTransactions
                .OrderByDescending(o => o.EntryDate)
                .FirstOrDefaultAsync(f => f.PlateNumber == carUnparkRequest.PlateNumber && f.ExitDate != null);

            var parkingTransaction = await _ooParkingLotDbContext.ParkingTransactions
                .FirstOrDefaultAsync(f => f.PlateNumber == carUnparkRequest.PlateNumber && f.ExitDate == null);

            if (parkingTransaction != null && parkingTransaction.ExitDate == null)
            {
                var dateDiff = carUnparkRequest.ExitDate - parkingTransaction.EntryDate;
                var totalDaysPenalty = (Convert.ToInt32(dateDiff.TotalHours) / 24) * 5000;
                var totalHours = Convert.ToDecimal(Math.Round(dateDiff.TotalHours, 0, MidpointRounding.ToEven));

                bool didReturnLessThanAnHour = false;
                if (previousParkingTransaction != null)
                {
                    didReturnLessThanAnHour = ((DateTime)previousParkingTransaction.ExitDate - parkingTransaction.EntryDate).TotalMinutes < 60;
                }

                var hourlyRate = await (from ps in _ooParkingLotDbContext.ParkingSpaces
                                        where ps.Id == parkingTransaction.ParkingSpaceId
                                        join pc in _ooParkingLotDbContext.ParkingCategories on ps.ParkingCategoryId equals pc.Id
                                        select pc.HourlyRate).FirstOrDefaultAsync();

                var amount = (hourlyRate * Math.Round(didReturnLessThanAnHour ? totalHours : (totalHours > 3 ? totalHours - 3 : 0))) + (didReturnLessThanAnHour ? 0 : 40);

                parkingTransaction.ExitDate = carUnparkRequest.ExitDate;
                parkingTransaction.Amount = amount;
                parkingTransaction.Penalties = totalDaysPenalty;
                parkingTransaction.TotalHours = totalHours;
                parkingTransaction.TotalAmount = amount + totalDaysPenalty;

                var parkingSpace = await _ooParkingLotDbContext.ParkingSpaces.FirstOrDefaultAsync(f => f.Id == parkingTransaction.ParkingSpaceId);
                if (parkingSpace != null)
                {
                    parkingSpace.IsOccupied = false;
                }
            }
            await _ooParkingLotDbContext.SaveChangesAsync();
            return parkingTransaction;
        }

        public async Task<List<ParkingTransaction>> GetAllParkingTransactionsAsync()
        {
            return await _ooParkingLotDbContext.ParkingTransactions.OrderByDescending(w => w.EntryDate).ToListAsync();
        }
    }
}
