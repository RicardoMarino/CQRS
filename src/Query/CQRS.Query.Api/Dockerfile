FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Query/CQRS.Query.Api/CQRS.Query.Api.csproj", "src/Query/CQRS.Query.Api/"]
RUN dotnet restore "src/Query/CQRS.Query.Api/CQRS.Query.Api.csproj"
COPY . .
WORKDIR "/src/src/Query/CQRS.Query.Api"
RUN dotnet build "CQRS.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRS.Query.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CQRS.Query.Api.dll"]
