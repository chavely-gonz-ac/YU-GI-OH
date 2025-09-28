using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Ardalis.Specification;

using YuGiOh.Domain.DTOs;
using YuGiOh.Domain.Models;
using YuGiOh.Domain.Services;
using YuGiOh.Domain.Repositories;

namespace YuGiOh.Infrastructure.CSCService
{
    public class CSCProvider : ICSCProvider
    {
        private readonly CSCLoader _loader;
        private readonly ICachingRepository _cache;
        private readonly TimeSpan _defaultTtl = TimeSpan.FromHours(12);
        private readonly IReadRepositoryBase<StreetType> _streetTypeRepository;

        public CSCProvider(
            ICachingRepository cache, 
            CSCLoader loader,
            IReadRepositoryBase<StreetType> streetTypeRepository)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _streetTypeRepository = streetTypeRepository ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<ICollection<Country>> GetAllCountriesAsync()
        {
            return await _cache.GroupQuery<Country>(
                predicate: c => true,
                loader: () => _loader.GetAllCountriesAsync(),
                ttl: _defaultTtl,
                "countries"
            );
        }

        public async Task<ICollection<State>> GetStatesByCountryAsync(string countryIso2)
        {
            if (string.IsNullOrEmpty(countryIso2)) throw new ArgumentNullException(nameof(countryIso2));

            return await _cache.GroupQuery<State>(
                predicate: s => true,
                loader: () => _loader.GetStatesByCountryAsync(countryIso2),
                ttl: _defaultTtl,
                $"states:{countryIso2}"
            );
        }

        public async Task<ICollection<City>> GetCitiesByStateAsync(string countryIso2, string stateIso2)
        {
            if (string.IsNullOrEmpty(countryIso2)) throw new ArgumentNullException(nameof(countryIso2));
            if (string.IsNullOrEmpty(stateIso2)) throw new ArgumentNullException(nameof(stateIso2));

            return await _cache.GroupQuery<City>(
                predicate: c => true,
                loader: () => _loader.GetCitiesByStateAsync(countryIso2, stateIso2),
                ttl: _defaultTtl,
                $"cities:{countryIso2}:{stateIso2}"
            );
        }

        public async Task<ICollection<StreetType>> GetStreetTypesAsync()
        {
            return (await _cache.GroupQuery<StreetType>(
                predicate: c => true,
                loader: async () => await _streetTypeRepository.ListAsync(),
                ttl: _defaultTtl
            ));
        }
    }
}