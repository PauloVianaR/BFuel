﻿using BFuel.Domain.Models;
using BFuel.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BFuel.Servicos
{
    public class Location_Service : Service
    {
        public async Task<ResponseService<Locat>> GetLocationByName(string destiny)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrlLocation}&query={destiny}");

            ResponseService<Locat> responseService = new ResponseService<Locat>
            {
                IsSucess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Locat>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<ResponseService<Locat>>(problemResponse);

                responseService.Errors = errors.Errors;
            }
            return responseService;
        }
    }
}
