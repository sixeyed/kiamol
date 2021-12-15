FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /src
COPY src/Numbers.Web/Numbers.Web.csproj .
RUN dotnet restore

COPY src/Numbers.Web/ .
RUN dotnet publish -c Release -o /out Numbers.Web.csproj

# app image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

ENV RngApi__Url=http://numbers-api/sixeyed/kiamol/master/ch03/numbers/rng \
    RngApi__RetryHost=raw.githubusercontent.com

ENTRYPOINT ["dotnet", "/app/Numbers.Web.dll"]

WORKDIR /app
COPY --from=builder /out/ .