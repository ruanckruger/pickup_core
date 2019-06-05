using System;
using System.Collections.Generic;
using System.Text;
using pickupsv2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace pickupsv2.Data
{
    public class PickupContext : IdentityDbContext
    {
        public PickupContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
    }
}
