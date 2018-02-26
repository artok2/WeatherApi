using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AzureCoreApi.Models
{
    public class AzureCoreApiContext : DbContext
    {
        public AzureCoreApiContext (DbContextOptions<AzureCoreApiContext> options)
            : base(options)
        {
        }

        public DbSet<AzureCoreApi.Models.Weather> Weather { get; set; }
    }
}
