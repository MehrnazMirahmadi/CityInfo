using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositoties;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/Cities")]
    public class CitiesController: ControllerBase
    {
       private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            var Cities =await _cityInfoRepository.GetCitiesAsync();

            return Ok(
                _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(Cities)
                );
    
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {

            return Ok();
   
        }

    }
}
