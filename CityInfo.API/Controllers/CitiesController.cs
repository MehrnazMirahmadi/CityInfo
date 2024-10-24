using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/Cities")]
    public class CitiesController: ControllerBase
    {
        private readonly CitiesDataStore citiesDataStore;
        public CitiesController(CitiesDataStore citiesDataStore)
        {
            this.citiesDataStore = citiesDataStore;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {

            var cityToReturn = citiesDataStore.Cities
                 .FirstOrDefault(c => c.Id == id);

            if(cityToReturn  == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);

            
        }

     
    }
}
