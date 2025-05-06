FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DotNetFiveApiDemo/DotNetFiveApiDemo.csproj", "DotNetFiveApiDemo/"]
RUN dotnet restore "DotNetFiveApiDemo/DotNetFiveApiDemo.csproj"
COPY . .
WORKDIR "/src/DotNetFiveApiDemo"
RUN dotnet build "./DotNetFiveApiDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotNetFiveApiDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetFiveApiDemo.dll"]
