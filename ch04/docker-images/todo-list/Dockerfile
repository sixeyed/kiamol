FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /src
COPY src/ToDoList.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out ToDoList.csproj

# app image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
ENTRYPOINT ["dotnet", "ToDoList.dll"]

# elevated for access to Sqlite db
ENV DATA_DIRECTORY="/data"\
    USER="root"

ENV ConfigController__Enabled="false" \
    ConnectionStrings__ToDoDb="Filename=/data/todo-list.db" \
    Logging__LogLevel__Default="Error"

VOLUME $DATA_DIRECTORY
USER $USER

COPY --from=builder /out/ .