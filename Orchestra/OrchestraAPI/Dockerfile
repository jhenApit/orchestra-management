FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrchestraAPI.csproj", "./"]
RUN dotnet restore "./OrchestraAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OrchestraAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OrchestraAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrchestraAPI.dll"]