FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301 AS builder

WORKDIR /src
COPY src/Pi.Math/Pi.Math.csproj ./Pi.Math/
COPY src/Pi.Runtime/Pi.Runtime.csproj ./Pi.Runtime/
COPY src/Pi.Web/Pi.Web.csproj ./Pi.Web/

WORKDIR /src/Pi.Web
RUN dotnet restore

COPY src/Pi.Math/ /src/Pi.Math/
COPY src/Pi.Runtime /src/Pi.Runtime/
COPY src/Pi.Web /src/Pi.Web/
RUN dotnet publish -c Release -o /out Pi.Web.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5

EXPOSE 80
ENTRYPOINT ["dotnet", "Pi.Web.dll"]
CMD ["-m", "console", "-dp", "6"]

WORKDIR /app
COPY --from=builder /out/ .