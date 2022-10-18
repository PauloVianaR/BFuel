using BFuel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFuel.Api.DataBase
{
    public class BFuelContext : DbContext
    {
        public BFuelContext(DbContextOptions<BFuelContext> options) : base(options)
        {

        }

        //aqui estamos definindo as tabelas do banco SQLite
        public DbSet<User> Users { get; set; }
        public DbSet<Supply> Supplies { get; set; }
    }
}
