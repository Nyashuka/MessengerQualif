using DatabaseService.DataContexts;
using DatabaseService.Services;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // database
            builder.Services.AddDbContext<DatabaseContext>
            (
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            //
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IFriendsService, FriendsService>();
            //
            builder.Services.AddScoped<IUsersService, UsersService>();
            //
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IGroupsService, GroupsService>();
            builder.Services.AddScoped<IChatMembersService, ChatMembersService>();
            //
            builder.Services.AddScoped<IMessageService, MessageService>();
            //
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
            builder.Services.AddScoped<IUserRoleRelationService, UserRoleRelationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
