FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /src
COPY src/TimeCheck.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out TimeCheck.csproj

# app image
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

COPY src/appsettings.json /config/appsettings.json

WORKDIR /app
ENTRYPOINT ["dotnet", "TimeCheck.dll"]

COPY --from=builder /out/ .