
using ChatsService.Authorization;
using ChatsService.Chats.Services;
using ChatsService.Groups.Services;
using ChatsService.PersonalChats.Services;

namespace ChatsService
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
            builder.Services.AddScoped<IChatsService, Chats.Services.ChatsService>();
            builder.Services.AddScoped<IPersonalChatsService, PersonalChatsService>();
            builder.Services.AddScoped<IGroupsService, GroupsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
