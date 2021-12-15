FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as builder

WORKDIR /src
COPY src/whoami.csproj .
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out whoami.csproj

# app image
FROM  mcr.microsoft.com/dotnet/aspnet:6.0-alpine

EXPOSE 80

WORKDIR /app
ENTRYPOINT ["dotnet", "whoami.dll"]

COPY --from=builder /out/ .