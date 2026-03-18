FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Cover.Shared/Cover.Shared.csproj Cover.Shared/
COPY src/Cover.Web/Cover.Web.csproj Cover.Web/
RUN dotnet restore Cover.Web/Cover.Web.csproj

COPY src/Cover.Shared/ Cover.Shared/
COPY src/Cover.Web/ Cover.Web/
RUN dotnet publish Cover.Web/Cover.Web.csproj -c Release -o /app

FROM nginx:alpine
COPY --from=build /app/wwwroot /usr/share/nginx/html
COPY src/Cover.Web/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
