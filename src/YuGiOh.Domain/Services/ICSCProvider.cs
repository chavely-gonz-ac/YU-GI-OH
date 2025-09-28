using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Models;

namespace YuGiOh.Domain.Services
{
    /// <summary>
    /// Defines the contract for accessing CSC (Country-State-City) data providers.
    /// 
    /// This interface abstracts the logic to fetch geographical information 
    /// such as countries, states, and cities from an external service or data source.
    /// Implementations (e.g., <see cref="CSCProvider"/>) may use caching 
    /// or direct API calls to optimize performance and reduce network usage.
    /// </summary>
    public interface ICSCProvider
    {
        /// <summary>
        /// Retrieves all countries supported by the CSC service.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Country"/> objects representing all available countries.
        /// </returns>
        /// <remarks>
        /// This method should guarantee that the collection is never null, 
        /// though it may be empty if the service has no countries.
        /// </remarks>
        Task<ICollection<Country>> GetAllCountriesAsync();

        /// <summary>
        /// Retrieves all states or provinces that belong to a given country.
        /// </summary>
        /// <param name="countryIso2">
        /// The ISO2 code of the country (e.g., "US" for United States, "MX" for Mexico).
        /// </param>
        /// <returns>
        /// A collection of <see cref="State"/> objects representing the states of the given country.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="countryIso2"/> is null or empty.
        /// </exception>
        Task<ICollection<State>> GetStatesByCountryAsync(string countryIso2);

        /// <summary>
        /// Retrieves all cities belonging to a specific state within a given country.
        /// </summary>
        /// <param name="countryIso2">
        /// The ISO2 code of the country (e.g., "US").
        /// </param>
        /// <param name="stateIso2">
        /// The ISO2 code of the state (e.g., "CA" for California).
        /// </param>
        /// <returns>
        /// A collection of <see cref="City"/> objects representing the cities of the given state.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="countryIso2"/> or <paramref name="stateIso2"/> is null or empty.
        /// </exception>
        Task<ICollection<City>> GetCitiesByStateAsync(string countryIso2, string stateIso2);

        /// <summary>
        /// Retrieves all street types.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="StreetType"/> objects representing the cities of the given state.
        /// </returns>
        Task<ICollection<StreetType>> GetStreetTypesAsync();
    }
}
