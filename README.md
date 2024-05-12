# MessengerQualif

## Installing entity framework and libs for PostgreSql (.NET CORE 7)
* dotnet tool install --global dotnet-ef --version 7.0.14
* dotnet add package Microsoft.EntityFrameworkCore --version 7.0.14
* dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.14
* dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.14
* dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11


## Entity framework commands
* dotnet ef migrations add "Name" - add migrations
* dotnet ef database update - apply migrations

## Service's Ports
* Database service: 5291
* Authorization service: 5292
* Account Management service: 5293
* Chats service: 5294
* Messages service: 5295
* Roles service: 5296
* Storage service: 5297
* Notification service: 6999
