﻿
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
        public DbSet<ChatType> ChatTypes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ChatPermission> ChatPermissions { get; set; }
        public DbSet<RolePermissionRelation> RolePermissionRelations { get; set; }
        public DbSet<UserRoleRelation> UserRoleRelations { get; set; }
    }
}
