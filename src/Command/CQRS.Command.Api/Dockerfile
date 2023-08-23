FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Command/CQRS.Command.Api/CQRS.Command.Api.csproj", "src/Command/CQRS.Command.Api/"]
RUN dotnet restore "src/Command/CQRS.Command.Api/CQRS.Command.Api.csproj"
COPY . .
WORKDIR "/src/src/Command/CQRS.Command.Api"
RUN dotnet build "CQRS.Command.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRS.Command.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CQRS.Command.Api.dll"]
