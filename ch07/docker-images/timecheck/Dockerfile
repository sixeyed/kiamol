FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101 AS builder

WORKDIR /src
COPY src/TimeCheck.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out TimeCheck.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.1

COPY src/appsettings.json /config/appsettings.json

WORKDIR /app
ENTRYPOINT ["dotnet", "TimeCheck.dll"]

COPY --from=builder /out/ .