
using ChatsService.ActionAccess.Services;
using ChatsService.Authorization;
using ChatsService.ChatMembers.Services;
using ChatsService.Chats.Services;
using ChatsService.Groups.Services;
using ChatsService.PersonalChats.Services;
using Microsoft.Extensions.FileProviders;

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
            builder.Services.AddScoped<IGroupsInfoService, GroupsInfoService>();
            builder.Services.AddScoped<IChatMembersService, ChatMembersServices>();
            builder.Services.AddScoped<IActionAccessService, ActionAccessService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads"
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads"
            });

            app.MapControllers();

            app.Run();
        }
    }
}
