#!/bin/bash

ports=(5291 5292 5293 5294 5295 5296 6999)

for port in "${ports[@]}"
do
    echo "Searching for processes on port $port..."
    pid=$(sudo lsof -t -i:$port)

    if [ -z "$pid" ]; then
        echo "No process found on port $port."
    else
        echo "Killing process $pid listening on port $port..."
        sudo kill -9 $pid
        echo "Process killed successfully."
    fi
done

dotnet run --project DatabaseService/DatabaseService/DatabaseService.csproj &
dotnet run --project AuthorizationService/AuthorizationService/AuthorizationService.csproj &
dotnet run --project AccountManagementService/AccountManagementService/AccountManagementService.csproj &
dotnet run --project ChatsService/ChatsService/ChatsService.csproj &
dotnet run --project RolesService/RolesService/RolesService.csproj &
dotnet run --project MessagesService/MessagesService/MessagesService.csproj &
dotnet run --project NotificationService/NotificationService/NotificationService.csproj

