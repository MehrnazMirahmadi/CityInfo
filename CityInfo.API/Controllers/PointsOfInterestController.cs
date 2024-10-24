using AutoMapper;
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
            var city = await _cityInfoRepository.CityExistsAsync(cityId);
                //citiesDataStore.Cities
                //.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId, pointOfInterestId);
                //city.PointOfInterest
                //.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        //#region  Post
        //[HttpPost]
        //public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        //  int cityId,
        //  PointOfInterestForCreationDto pointOfInterest
        //  )
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var city = citiesDataStore
        //        .Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var maxpointOfInterestId = citiesDataStore.Cities
        //        .SelectMany(c => c.PointOfInterest)
        //        .Max(p => p.Id);

        //    var createPoint = new PointOfInterestDto()
        //    {
        //        Id = ++maxpointOfInterestId,
        //        Name = pointOfInterest.Name,
        //        Description = pointOfInterest.Description
        //    };

        //    city.PointOfInterest.Add(createPoint);



        //    return CreatedAtAction("GetPointOfInterest",
        //        new
        //        {
        //            cityId = cityId,
        //            pointOfInterestId = createPoint.Id

        //        },
        //        createPoint
        //        );
        //}
        //#endregion

        //#region Edit
        //[HttpPut("{pontiOfInterestId}")]
        //public ActionResult UpdatePointOfInterest(int cityId,
        //    int pontiOfInterestId,
        //    PointOfInterestForUpdateDto pointOfInterest)
        //{
        //    //find  city
        //    var city = citiesDataStore.Cities
        //        .FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //        return NotFound();

        //    // find point of interest
        //    var point = city.PointOfInterest
        //        .FirstOrDefault(p => p.Id == pontiOfInterestId);
        //    if (point == null)
        //        return NotFound();

        //    point.Name = pointOfInterest.Name;
        //    point.Description = pointOfInterest.Description;

        //    return NoContent();

        //}
        //#endregion

        //#region  Edit with patch
        //[HttpPatch("{pontiOfInterestid}")]
        //public ActionResult PartiallyUpdatePointOfOnterest(
        //    int cityId,
        //    int pontiOfInterestid,
        //    JsonPatchDocument<PointOfInterestForUpdateDto>  patchDocument
        //    )
        //{
        //    //find  city
        //    var city = citiesDataStore.Cities
        //        .FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //        return NotFound();

        //    // find point of interest
        //    var pointOfInterestFromStore = city.PointOfInterest
        //        .FirstOrDefault(p => p.Id == pontiOfInterestid);
        //    if (pointOfInterestFromStore == null)
        //        return NotFound();

        //    var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        //    {
        //        Name = pointOfInterestFromStore.Name,
        //        Description=pointOfInterestFromStore.Description
        //    };

        //    patchDocument.ApplyTo(pointOfInterestToPatch,  ModelState);

        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    if(!TryValidateModel(pointOfInterestToPatch))
        //    {
        //        return  BadRequest(modelState: ModelState);
        //    }

        //    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        //    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        //    return NoContent();
        //}

        //#endregion

        //#region Delete
        //[HttpDelete("{pontiOfInterestId}")]
        //public ActionResult DeletePointOfInterest(
        //    int cityId,
        //    int pontiOfInterestId)
        //{
        //    //find  city
        //    var city = citiesDataStore.Cities
        //        .FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //        return NotFound();

        //    // find point of interest
        //    var point = city.PointOfInterest
        //        .FirstOrDefault(p => p.Id == pontiOfInterestId);
        //    if (point == null)
        //        return NotFound();

        //    city.PointOfInterest.Remove(point);

        //    _localMailService
        //        .Send(
        //        "Point Of intrest deleted",
        //        $"Point Of Interest {point.Name}with id {point.Id}"
        //        );

        //    return NoContent();
        //}

        //#endregion
    }
}

