FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301 AS builder

WORKDIR /src
COPY src/SimpleProxy.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out SimpleProxy.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5

EXPOSE 1080
WORKDIR /app
ENTRYPOINT ["dotnet", "SimpleProxy.dll"]

COPY --from=builder /out/ .