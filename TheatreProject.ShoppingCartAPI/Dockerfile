FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TheatreProject.ShoppingCartAPI/TheatreProject.ShoppingCartAPI.csproj", "TheatreProject.ShoppingCartAPI/"]
RUN dotnet restore "TheatreProject.ShoppingCartAPI/TheatreProject.ShoppingCartAPI.csproj"
COPY . .
WORKDIR "/src/TheatreProject.ShoppingCartAPI"
RUN dotnet build "TheatreProject.ShoppingCartAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TheatreProject.ShoppingCartAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheatreProject.ShoppingCartAPI.dll"]
