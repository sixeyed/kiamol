FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-alpine AS builder

WORKDIR /src
COPY src/WebPingOperator.Model/WebPingOperator.Model.csproj ./WebPingOperator.Model/
COPY src/WebPingOperator.WebPingerArchiveController/WebPingOperator.WebPingerArchiveController.csproj ./WebPingOperator.WebPingerArchiveController/

WORKDIR /src/WebPingOperator.WebPingerArchiveController
RUN dotnet restore

COPY src /src
RUN dotnet publish -c Release -o /out WebPingOperator.WebPingerArchiveController.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5-alpine

WORKDIR /app
ENTRYPOINT ["dotnet", "WebPingOperator.WebPingerArchiveController.dll"]

COPY --from=builder /out/ .