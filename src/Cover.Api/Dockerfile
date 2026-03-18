FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Cover.Shared/Cover.Shared.csproj Cover.Shared/
COPY src/Cover.Api/Cover.Api.csproj Cover.Api/
RUN dotnet restore Cover.Api/Cover.Api.csproj

COPY src/Cover.Shared/ Cover.Shared/
COPY src/Cover.Api/ Cover.Api/
RUN dotnet publish Cover.Api/Cover.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
HEALTHCHECK --interval=10s --timeout=3s CMD curl -f http://localhost:8080/api/setup/status || exit 1
ENTRYPOINT ["dotnet", "Cover.Api.dll"]
