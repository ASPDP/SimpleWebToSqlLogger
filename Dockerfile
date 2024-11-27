FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleWebToSqlLogger.csproj", "./"]
RUN dotnet restore "SimpleWebToSqlLogger.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SimpleWebToSqlLogger.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleWebToSqlLogger.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#FROM build AS efmigration
#RUN dotnet tool install --global dotnet-ef --version 8.0.11
#ENV PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet ef migrations add lastForgottenMigration
#RUN dotnet ef migrations bundle

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleWebToSqlLogger.dll"]
