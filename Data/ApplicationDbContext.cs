using System;
using Microsoft.EntityFrameworkCore;
using SimpleDotnetMvc.Models;

namespace SimpleDotnetMvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }
    }
}
