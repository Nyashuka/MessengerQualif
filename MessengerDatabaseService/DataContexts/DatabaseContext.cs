
using MessengerDatabaseService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MessengerDatabaseService.DataContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
