using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Permissions;
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
            WriteChatTypes(modelBuilder);
            WritePermissions(modelBuilder);
        }

        private void WritePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatPermission>().HasData(
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.SendTextMessages),
                   Name = "Send text messages",
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.SendPhotos),
                   Name = "Send photos"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.SendVideos),
                   Name = "Send video"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.AddMembers),
                   Name = "Add members"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.DeleteMembers),
                   Name = "Delete members"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.BanMembers),
                   Name = "Ban members"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.ChangeChatInfo),
                   Name = "Change chat info"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(Permission.Administrator),
                   Name = "Administrator"
               }
           );
        }

        private void WriteChatTypes(ModelBuilder modelBuilder)
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
