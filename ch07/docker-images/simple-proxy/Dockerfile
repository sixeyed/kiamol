FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /src
COPY src/SimpleProxy.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out SimpleProxy.csproj

# app image
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

EXPOSE 1080
WORKDIR /app
ENTRYPOINT ["dotnet", "SimpleProxy.dll"]

COPY --from=builder /out/ .