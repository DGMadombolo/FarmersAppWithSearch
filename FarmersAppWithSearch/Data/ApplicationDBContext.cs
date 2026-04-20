using FarmersAppWithSearch.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FarmersAppWithSearch.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) 
            : base(options) { }

       
        
        public DbSet<Farmer> Farmers { get; set; }
    }
}
