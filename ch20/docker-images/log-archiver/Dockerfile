FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-alpine AS builder

WORKDIR /src
COPY src/LogArchiver.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out LogArchiver.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5-alpine

WORKDIR /app
ENTRYPOINT ["dotnet", "LogArchiver.dll"]

COPY --from=builder /out/ .