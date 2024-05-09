FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ProperTea.Property.Api/ProperTea.Property.Api.csproj", "ProperTea.Property.Api/"]
RUN dotnet restore "ProperTea.Property.Api/ProperTea.Property.Api.csproj"
COPY . .
WORKDIR "/src/ProperTea.Property.Api"
RUN dotnet build "ProperTea.Property.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ProperTea.Property.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProperTea.Property.Api.dll"]
