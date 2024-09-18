FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/", "./src/"]
RUN dotnet restore "src/Presentation/HT.Api/HT.Api.csproj"

WORKDIR "/src/src/Presentation/HT.Api"
RUN dotnet build "HT.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HT.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HT.Api.dll"]
