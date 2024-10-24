using CityInfo.API.Entities;

namespace CityInfo.API.Repositoties
{
    public interface ICityInfoRepository
    { 
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<bool> CityExistsAsync(int  cityId);
        Task<IEnumerable<PointOfInterest>> 
            GetPointsOfInterestForCityAsync(int  cityId);
        Task<PointOfInterest?> GetPointsOfInterestForCityAsync(int cityId
            ,int pointOfInterestId);   

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
