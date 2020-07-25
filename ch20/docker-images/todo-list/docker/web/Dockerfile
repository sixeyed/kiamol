FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301 AS builder

WORKDIR /src
COPY src/entities/ToDoList.Entities.csproj ./entities/
COPY src/messaging/ToDoList.Messaging.csproj ./messaging/
COPY src/model/ToDoList.Model.csproj ./model/
COPY src/web/ToDoList.csproj ./web/

WORKDIR /src/web
RUN dotnet restore

COPY src/ /src
RUN dotnet publish -c Release -o /out ToDoList.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5

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