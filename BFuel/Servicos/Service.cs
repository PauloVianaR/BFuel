using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BFuel.Servicos
{
    public abstract class Service
    {
        protected HttpClient _client;
        protected const string BaseApiUrl = "https://betterfuelapi.azurewebsites.net/";
        protected const string BaseApiUrlLocation = "https://atlas.microsoft.com/search/address/json?&subscription-key=0mGNGjfppFNq96Jd9s9RZpEDqlhtYIQfNLhgkLIdZhU&api-version=1.0&language=pt-BR";
        protected const string BaseApiUrlCoords = "https://maps.google.com/maps/api/geocode/json?key=AIzaSyBEtEiQfLpUoXtBIg5U5Ky9fB20_w1bwAk";

        public Service()
        {
            _client = new HttpClient();
        }
    }
}
