using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BFuel.Servicos
{
    public abstract class Service
    {
        protected HttpClient _client;
        protected string BaseApiUrl = "https://betterfuelapi.azurewebsites.net/";
        protected string BaseApiUrlLocation = "https://atlas.microsoft.com/search/address/json?&subscription-key=0mGNGjfppFNq96Jd9s9RZpEDqlhtYIQfNLhgkLIdZhU&api-version=1.0&language=pt-BR";

        public Service()
        {
            _client = new HttpClient();
        }
    }
}
