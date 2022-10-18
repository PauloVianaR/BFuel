using BFuel.Domain.Models;
using BFuel.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BFuel.Servicos
{
    public class SupplyService : Service
    {
        public async Task<ResponseService<List<Supply>>> GetSupplies(int userId = 0, int numberOfPage = 1)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/Supplies?userid={userId}&numberOfPage={numberOfPage}");

            ResponseService<List<Supply>> responseService = new ResponseService<List<Supply>>
            {
                IsSucess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<List<Supply>>();
            }
            else
            {
                String problemResponse = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ResponseService<List<Supply>>>(problemResponse);

                responseService.Errors = errors.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Supply>> GetSupply(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/Supplies/{id}");

            ResponseService<Supply> responseService = new ResponseService<Supply>
            {
                IsSucess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Supply>();
            }
            else
            {
                String problemResponse = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ResponseService<Supply>>(problemResponse);

                responseService.Errors = errors.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Supply>> AddSupply(Supply sup)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/Supplies", sup);

            ResponseService<Supply> responseService = new ResponseService<Supply>
            {
                IsSucess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Supply>();
            }
            else
            {
                String problemResponse = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ResponseService<Supply>>(problemResponse);

                responseService.Errors = errors.Errors;
            }
            return responseService;
        }

    }
}
