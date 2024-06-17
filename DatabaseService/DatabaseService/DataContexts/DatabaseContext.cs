using DatabaseService.Data;
using DatabaseService.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security;

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
        public DbSet<FriendRelation> FriendRelations { get; set; }
        public DbSet<DefaultRole> DefaultRoles { get; set; }

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
                   Id = Convert.ToInt32(PermissionEnum.SendTextMessages),
                   Name = "Send text messages",
               },
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.SendPhotos),
               //    Name = "Send photos"
               //},
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.SendVideos),
               //    Name = "Send video"
               //},
               new ChatPermission()
               {
                   Id = Convert.ToInt32(PermissionEnum.DeleteMessages),
                   Name = "Delete messages"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(PermissionEnum.AddMembers),
                   Name = "Add members"
               },
               new ChatPermission()
               {
                   Id = Convert.ToInt32(PermissionEnum.DeleteMembers),
                   Name = "Delete members"
               },
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.BanMembers),
               //    Name = "Ban members"
               //},
               new ChatPermission()
               {
                   Id = Convert.ToInt32(PermissionEnum.ChangeChatInfo),
                   Name = "Change chat info"
               },
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.CreateRoles),
               //    Name = "Create roles"
               //},
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.GiveRoles),
               //    Name = "Give roles"
               //},
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.EditRoles),
               //    Name = "Edit roles"
               //},
               //new ChatPermission()
               //{
               //    Id = Convert.ToInt32(PermissionEnum.DeleteRoles),
               //    Name = "Delete roles"
               //},
               new ChatPermission()
               {
                   Id = Convert.ToInt32(PermissionEnum.Administrator),
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
