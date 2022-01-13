FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine  AS builder

WORKDIR /src
COPY src/KubeExplorer.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out KubeExplorer.csproj

# app image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
ENTRYPOINT ["dotnet", "KubeExplorer.dll"]

COPY --from=builder /out/ .