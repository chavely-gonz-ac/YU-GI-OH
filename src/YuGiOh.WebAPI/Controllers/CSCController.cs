using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Models;
using YuGiOh.Domain.Services;

namespace YuGiOh.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CSCController : APIControllerBase
    {
        private readonly ICSCProvider _cscProvider;

        public CSCController(
            IMediator mediator, 
            ICSCProvider cscProvider) : base(mediator)
        {
            _cscProvider = cscProvider;
        }

        /// <summary>
        /// Gets all countries supported by the CSC service.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<Country>), 200)]
        public async Task<IActionResult> GetCountries()
        {
            ICollection<Country> result = await _cscProvider.GetAllCountriesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Gets all states belonging to a specific country.
        /// </summary>
        [HttpGet("{countryIso2}")]
        [ProducesResponseType(typeof(ICollection<State>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetStates(string countryIso2)
        {
            if (string.IsNullOrWhiteSpace(countryIso2))
                return BadRequest("Country ISO2 code is required.");

            var result = await _cscProvider.GetStatesByCountryAsync(countryIso2);
            foreach (State item in result)
            {
                Console.WriteLine(item);
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets all cities belonging to a specific state within a country.
        /// </summary>
        [HttpGet("{countryIso2}/{stateIso2}")]
        [ProducesResponseType(typeof(ICollection<City>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCities(string countryIso2, string stateIso2)
        {
            if (string.IsNullOrWhiteSpace(countryIso2) || string.IsNullOrWhiteSpace(stateIso2))
                return BadRequest("Both country and state ISO2 codes are required.");

            var result = await _cscProvider.GetCitiesByStateAsync(countryIso2, stateIso2);
            return Ok(result);
        }

        /// <summary>
        /// Gets all street types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<StreetType>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetStreetTypes()
        {
            var result = await _cscProvider.GetStreetTypesAsync();
            return Ok(result);
        }
    }
}
