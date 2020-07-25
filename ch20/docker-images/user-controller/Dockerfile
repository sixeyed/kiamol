FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-alpine AS builder

WORKDIR /src
COPY src/UserController.csproj .
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o /out UserController.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5-alpine

WORKDIR /app
ENTRYPOINT ["dotnet", "UserController.dll"]

COPY --from=builder /out/ .