FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-alpine3.12 AS builder

WORKDIR /src
COPY src/TimeCheck.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out TimeCheck.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5-alpine3.12

COPY src/appsettings.json /config/appsettings.json

WORKDIR /app
ENTRYPOINT ["dotnet", "TimeCheck.dll"]

COPY --from=builder /out/ .