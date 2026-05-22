FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Microservicios.Atracciones.Catalog.API/Microservicios.Atracciones.Catalog.API.csproj", "Microservicios.Atracciones.Catalog.API/"]
COPY ["Microservicios.Atracciones.Catalog.Business/Microservicios.Atracciones.Catalog.Business.csproj", "Microservicios.Atracciones.Catalog.Business/"]
COPY ["Microservicios.Atracciones.Catalog.DataAccess/Microservicios.Atracciones.Catalog.DataAccess.csproj", "Microservicios.Atracciones.Catalog.DataAccess/"]
COPY ["Microservicios.Atracciones.Catalog.DataManagement/Microservicios.Atracciones.Catalog.DataManagement.csproj", "Microservicios.Atracciones.Catalog.DataManagement/"]

RUN dotnet restore "Microservicios.Atracciones.Catalog.API/Microservicios.Atracciones.Catalog.API.csproj"

COPY . .
WORKDIR "/src/Microservicios.Atracciones.Catalog.API"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Microservicios.Atracciones.Catalog.API.dll"]
