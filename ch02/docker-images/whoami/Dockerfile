FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102 as builder

WORKDIR /src
COPY src/whoami.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out whoami.csproj

# app image
FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

EXPOSE 80

WORKDIR /app
ENTRYPOINT ["dotnet", "whoami.dll"]

COPY --from=builder /out/ .