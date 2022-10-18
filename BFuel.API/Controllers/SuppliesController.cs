using BFuel.Api.DataBase;
using BFuel.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFuel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesController : ControllerBase
    {
        private int numberOfRegistryByPage = 5;
        private BFuelContext _data;
        public SuppliesController(BFuelContext data)
        {
            _data = data;
        }

        [HttpGet]
        public IEnumerable<Supply> GetSupplies(int userId = 0, int numberOfPage = 1)
        {
            return _data.Supplies.Where(a=> 
                a.UserId == userId)
                .OrderByDescending(a => a.InsertedDate)
                .Skip(numberOfRegistryByPage * (numberOfPage - 1))
                .Take(numberOfRegistryByPage)
                .ToList<Supply>();
        }

        [HttpGet("{id}")]
        public IActionResult GetSupply(int id)
        {
            Supply supDb = _data.Supplies.Find(id);

            if (supDb == null)
                return NotFound();

            return new JsonResult(supDb);
        }

        [HttpPost]
        public IActionResult AddSupply(Supply sup)
        {
            _data.Supplies.Add(sup);
            _data.SaveChanges();

            return CreatedAtAction("GetSupply", new { id = sup.Id }, sup);
        }

    }
}
