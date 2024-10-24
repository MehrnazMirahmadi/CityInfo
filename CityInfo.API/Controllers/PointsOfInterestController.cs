using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Repositoties;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //  /api/cities/3/pointsofinterest
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;


        private readonly CitiesDataStore citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> loger,
            IMailService localMailService,
            CitiesDataStore citiesDataStore
            , ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger  = loger ?? throw new ArgumentNullException(nameof(loger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));

            _cityInfoRepository = cityInfoRepository ??
               throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            this.citiesDataStore = citiesDataStore;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>>
            GetPointsOfInterest(int cityId)
        {

            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"{cityId} Not Found ...");
                return NotFound();
            }

            var pointsOfInterestForCity =  await  _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId
            )
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterest  ==null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        //#region  Post
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
          int cityId,
          PointOfInterestForCreationDto pointOfInterest
          )
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPoint = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(
                cityId, finalPoint);
            await _cityInfoRepository.SaveChangesAsync();

            var createdpoint = _mapper.Map<Models.PointOfInterestDto>(finalPoint);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointOfInterestId = createdpoint.Id
            },createdpoint);
        }
        //#endregion

        #region Edit
        [HttpPut("{pontiOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId,
            int pontiOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
        
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId,pontiOfInterestId);
           
            if(point == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, point);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

        }
        #endregion

        #region  Edit with patch
        [HttpPatch("{pontiOfInterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfOnterest(
            int cityId,
            int pontiOfInterestid,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
            )
        {
           if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointEntity = await _cityInfoRepository
                 .GetPointsOfInterestForCityAsync(cityId, pontiOfInterestid);
            if(pointEntity == null)
            {
                return NotFound();
            }

            var pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>
                (pointEntity);

            patchDocument.ApplyTo(pointToPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(pointToPatch,pointEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Delete
        [HttpDelete("{pontiOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(
            int cityId,
            int pontiOfInterestId)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity =
                await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId, pontiOfInterestId);

            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
           await _cityInfoRepository.SaveChangesAsync();

            _localMailService
                .Send(
                "Point Of intrest deleted",
                $"Point Of Interest {pointOfInterestEntity.Name}with id {pointOfInterestEntity.Id}"
                );

            return NoContent();
        }

        #endregion
    }
}

