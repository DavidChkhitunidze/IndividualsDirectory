using IndividualsDirectory.Entities;
using IndividualsDirectory.Models.Get;
using IndividualsDirectory.Models.Response;
using IndividualsDirectory.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        #region PrivateFields

        private readonly IEntityRepository _repository;

        #endregion

        #region Constructor

        public DataController(IEntityRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region HttpGetMethods

        [HttpGet("cities")]
        public async Task<ActionResult<Response<IQueryable<GetCityViewModel>>>> GetCities([FromQuery] string name)
        {
            var response = new Response<IQueryable<GetCityViewModel>>();

            var citiesResult = await _repository.GetEntitiesAsync<City>();
            if (!citiesResult.Success)
            {
                response.SetErrorMessages(citiesResult.ErrorMessages);
                return NotFound(response);
            }

            var cityEntities = citiesResult.Model;
            if (!string.IsNullOrEmpty(name))
            {
                cityEntities = cityEntities
                    .Where(i => i.Name.ToLowerInvariant().StartsWith(name))
                    .OrderBy(i => i.Name);
            }
            
            var cityViewModels = cityEntities.Select(GetCityViewModel.Projection);

            response.SetSuccess();
            response.SetModel(cityViewModels);

            return Ok(response);
        }

        [HttpGet("city")]
        public async Task<ActionResult<Response<GetCityViewModel>>> GetCity([FromQuery] int id)
        {
            var response = new Response<GetCityViewModel>();

            var cityResult = await _repository.GetEntityAsync<City>(id);
            if (!cityResult.Success)
            {
                response.SetErrorMessages(cityResult.ErrorMessages);
                return NotFound(response);
            }

            var cityEntity = cityResult.Model;
            var cityViewModel = GetCityViewModel.Projection.Compile().Invoke(cityEntity);

            response.SetSuccess();
            response.SetModel(cityViewModel);

            return Ok(response);
        }

        [HttpGet("individuals")]
        public async Task<ActionResult<Response<IQueryable<GetIndividualPreviewViewModel>>>> GetIndividuals([FromQuery] string search)
        {
            var response = new Response<IQueryable<GetIndividualPreviewViewModel>>();

            var individualsResult = await _repository.GetEntitiesAsync<Individual>();
            if (!individualsResult.Success)
            {
                response.SetErrorMessages(individualsResult.ErrorMessages);
                return NotFound(response);
            }

            var individualEntities = individualsResult.Model;
            if (!string.IsNullOrEmpty(search))
            {
                individualEntities = individualEntities.Where(i => i.FirstName.ToLowerInvariant().StartsWith(search)
                                                                || i.LastName.ToLowerInvariant().StartsWith(search)
                                                                || i.PersonalNumber.ToLowerInvariant().StartsWith(search));
            }

            var individualViewModels = individualEntities.Select(GetIndividualPreviewViewModel.Projection);

            response.SetSuccess();
            response.SetModel(individualViewModels);

            return response;
        }

        [HttpGet("individual")]
        public async Task<ActionResult<Response<GetIndividualPreviewViewModel>>> GetIndividual([FromQuery] int id)
        {
            var response = new Response<GetIndividualPreviewViewModel>();

            var individualResult = await _repository.GetEntityAsync<Individual>(id);
            if (!individualResult.Success)
            {
                response.SetErrorMessages(individualResult.ErrorMessages);
                return NotFound(response);
            }

            var individualEntity = individualResult.Model;
            var individualViewModel = GetIndividualPreviewViewModel.Projection.Compile().Invoke(individualEntity);

            response.SetSuccess();
            response.SetModel(individualViewModel);

            return response;
        }

        #endregion
    }
}
