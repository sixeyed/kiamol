FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301 AS builder

WORKDIR /src
COPY src/entities/ToDoList.Entities.csproj ./entities/
COPY src/messaging/ToDoList.Messaging.csproj ./messaging/
COPY src/model/ToDoList.Model.csproj ./model/
COPY src/save-handler/ToDoList.SaveHandler.csproj ./save-handler/

WORKDIR /src/save-handler
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out ToDoList.SaveHandler.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5

WORKDIR /app
ENTRYPOINT ["dotnet", "ToDoList.SaveHandler.dll"]
ENV Events__events.todo.itemsaved__Publish="true"

COPY --from=builder /out/ .