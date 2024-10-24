using CityInfo.API.Entities;

namespace CityInfo.API.Repositoties
{
    public interface ICityInfoRepository
    { 
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> 
            GetPointsOfInterestForCityAsync(int  cityId);
        Task<PointOfInterest?> GetPointOfInterestForCity(int cityId
            ,int pointOfInterestId);   
    }
}
