using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace MyNamespace.DataCollection
{
    public class SportsDataCollector
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseUri;

        // Assuming a base URI for your API; adjust as necessary
        public SportsDataCollector(string baseUri)
        {
            _httpClient = new HttpClient();
            _baseUri = new Uri(baseUri);
        }

        // Fetch all available data for analysis
        public async Task<List<dynamic>> FetchAllDataAsync()
        {
            // List of endpoints can be expanded based on available data types
            var endpoints = new List<string> { "/teams", "/players", "/matches" };
            var allData = new List<dynamic>();

            foreach (var endpoint in endpoints)
            {
                var data = await FetchDataAsync(endpoint);
                allData.AddRange(data);
            }

            return allData;
        }

        // Generic method to fetch data from an API endpoint
        private async Task<List<dynamic>> FetchDataAsync(string endpoint)
        {
            try
            {
                var requestUri = new Uri(_baseUri, endpoint);
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<dynamic>>(jsonString);
                return data ?? new List<dynamic>();
            }
            catch (Exception e)
            {
                // Log the error for later review
                LogError(e);
                return new List<dynamic>(); // Return empty to allow continuation
            }
        }

        // Placeholder for a simple error logging
        private void LogError(Exception e)
        {
            // Implement a more sophisticated logging solution as needed
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
