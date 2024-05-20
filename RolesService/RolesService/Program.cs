
using RolesService.ActionAccess.Services;
using RolesService.Authorization;
using RolesService.Permissions.Services;
using RolesService.Roles.Services;

namespace RolesService
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

            builder.Services.AddScoped(sp => new HttpClient { });
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRolesService, Roles.Services.RolesService>();
            builder.Services.AddScoped<IPermissionsService, PermissionsService>();
            builder.Services.AddScoped<IActionAccessService, ActionAccessService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
