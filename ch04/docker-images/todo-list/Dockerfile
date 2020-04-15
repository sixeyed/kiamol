FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101 AS builder

WORKDIR /src
COPY src/ToDoList.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out ToDoList.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.1

WORKDIR /app
ENTRYPOINT ["dotnet", "ToDoList.dll"]

# elevated for access to Sqlite db
ENV DATA_DIRECTORY="/data"\
    USER="root"

# disable config view
ENV ConfigController__Enabled="false"

VOLUME $DATA_DIRECTORY
USER $USER

COPY --from=builder /out/ .