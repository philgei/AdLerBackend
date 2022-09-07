FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdLerBackend.API/AdLerBackend.API.csproj", "AdLerBackend.API/"]
COPY ["AdLerBackend.Application/AdLerBackend.Application.csproj", "AdLerBackend.Application/"]
COPY ["AdLerBackend.Domain/AdLerBackend.Domain.csproj", "AdLerBackend.Domain/"]
COPY ["AdLerBackend.Infrastructure/AdLerBackend.Infrastructure.csproj", "AdLerBackend.Infrastructure/"]

RUN dotnet restore "AdLerBackend.API/AdLerBackend.API.csproj"
COPY . .
WORKDIR "/src/AdLerBackend.API"
RUN dotnet publish "AdLerBackend.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdLerBackend.API.dll"]
