using OOMALL.API.PRICING.Models;
using OOMALL.API.PRICING.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OOMALL.API.PRICING.Services
{
    public interface IOOParkingLotRepository
    {
        Task<List<CarCategory>> GetAllCarCategoriesAsync();
        Task<List<CarCategory>> GetCarCategoryByIdAsync(Guid id);
        Task<List<ParkingCategory>> GetAllParkingCategoriesAsync();
        Task<List<EntryPoint>> GetAllEntryPointsAsync();
        Task<List<ParkingSpace>> GetAllParkingSpacesAsync();
        Task<List<ParkingSpace>> GetAllParkingSpacesByParkingCategoryIdsAsync(Guid[] parkingCategoryIds);
        Task<List<ParkingDistance>> GetAllDistancesAsync();
        Task<List<ParkingDistance>> GetDistanceByEntryIdAndSpaceIdsAsync(Guid entryPointId, Guid[] parkingSpaceIds);
        Task<List<ParkingDistance>> GetAllDistancesByEntryPointIdAsync(Guid entryPointId);
        Task<List<ParkingAvailablity>> GetParkingAvailablitiesByCarCategoryIdAsync(Guid CarCategoryId);

        Task<object> GetNearestAvailableParkingAsync(ParkRequest parkRequest);
        Task<ParkingSpace> AddParkingTransactionAsync(CarParkRequest carParkRequest);
        Task<ParkingTransaction> UpdateParkingTransactionAsync(CarUnparkRequest carUnparkRequest);
        Task<List<ParkingTransaction>> GetAllParkingTransactionsAsync();
    }
}
