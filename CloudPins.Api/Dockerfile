FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CloudPins.Domain ./CloudPins.Domain
COPY CloudPins.Application ./CloudPins.Application
COPY CloudPins.Infrastructure ./CloudPins.Infrastructure
COPY CloudPins.Api ./CloudPins.Api

RUN dotnet restore CloudPins.Api/CloudPins.Api.csproj
RUN dotnet publish CloudPins.Api/CloudPins.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT [ "dotnet", "CloudPins.Api.dll" ]
