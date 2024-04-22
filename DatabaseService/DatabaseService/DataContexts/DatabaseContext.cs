﻿using DatabaseService.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseService.DataContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatType> ChatTypes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<GroupChatInfo> GroupChatInfos { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ChatPermission> ChatPermissions { get; set; }
        public DbSet<RolePermissionRelation> RolePermissionRelations { get; set; }
        public DbSet<UserRoleRelation> UserRoleRelations { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<FriendRelation> FriendRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatType>().HasData(
                new ChatType()
                {
                    Id = 0,
                    Name = "personal",
                },
                new ChatType()
                {
                    Id = 1,
                    Name = "group"
                }
            );
        }
    }
}
