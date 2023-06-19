
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


COPY ["merchant-backend.csproj", "merchant-backend/"]
RUN dotnet restore "merchant-backend/merchant-backend.csproj"

WORKDIR "/src/merchant-backend"
COPY . .

RUN dotnet build "merchant-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "merchant-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "merchant-backend.dll"]

