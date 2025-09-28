using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using YuGiOh.Domain.DTOs;

namespace YuGiOh.Infrastructure.CSCService
{
    public class CSCLoader
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _urlBase;
        private readonly HttpClient _http;

        public CSCLoader(IOptions<CSCOptions> options)
        {
            CSCOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _http = new HttpClient();
            _urlBase = $"{_options.Endpoint}countries";
            _http.DefaultRequestHeaders.Add("X-CSCAPI-KEY", _options.APIKey);
        }

        public async Task<ICollection<Country>> GetAllCountriesAsync()
        {
            var countries = await _http.GetFromJsonAsync<List<Country>>(_urlBase, _jsonOptions);
            Console.WriteLine("Fuck");
            return countries ?? new List<Country>();
        }

        public async Task<ICollection<State>> GetStatesByCountryAsync(string countryIso2)
        {
            if (string.IsNullOrEmpty(countryIso2)) throw new ArgumentNullException(nameof(countryIso2));

            var states = await _http.GetFromJsonAsync<List<State>>($"{_urlBase}/{countryIso2}/states", _jsonOptions);
            if (states == null) return new List<State>();

            // Attach country ISO
            foreach (var s in states) s.CountryIso2 = countryIso2;
            return states;
        }

        public async Task<ICollection<City>> GetCitiesByStateAsync(string countryIso2, string stateIso2)
        {
            if (string.IsNullOrEmpty(countryIso2)) throw new ArgumentNullException(nameof(countryIso2));
            if (string.IsNullOrEmpty(stateIso2)) throw new ArgumentNullException(nameof(stateIso2));

            var cities = await _http.GetFromJsonAsync<List<City>>(
                $"{_urlBase}/{countryIso2}/states/{stateIso2}/cities", _jsonOptions
            );
            if (cities == null) return new List<City>();

            foreach (var c in cities)
            {
                c.CountryIso2 = countryIso2;
                c.StateIso2 = stateIso2;
            }

            return cities;
        }
    }
}