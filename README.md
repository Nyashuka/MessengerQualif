# MessengerQualif

## Installing entity framework and libs for PostgreSql (.NET CORE 7)
* dotnet add package Microsoft.EntityFrameworkCore --version 7.0.14
* dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.14
* dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.14
* dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11

## Entity framework commands
* dotnet ef migrations add "Name" - add migrations
* dotnet ef database update - apply migrations
