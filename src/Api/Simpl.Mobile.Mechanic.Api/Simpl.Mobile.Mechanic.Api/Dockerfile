FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Simpl.Mobile.Mechanic.Api/Simpl.Mobile.Mechanic.Api.csproj", "Simpl.Mobile.Mechanic.Api/"]
RUN dotnet restore "Simpl.Mobile.Mechanic.Api/Simpl.Mobile.Mechanic.Api.csproj"
COPY . .
WORKDIR "/src/Simpl.Mobile.Mechanic.Api"
RUN dotnet build "./Simpl.Mobile.Mechanic.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Simpl.Mobile.Mechanic.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simpl.Mobile.Mechanic.Api.dll"]
