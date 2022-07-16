FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdLerBackend/AdLerBackend.csproj", "AdLerBackend/"]
RUN dotnet restore "AdLerBackend/AdLerBackend.csproj"
COPY . .
WORKDIR "/src/AdLerBackend"
RUN dotnet build "AdLerBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdLerBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdLerBackend.dll"]
