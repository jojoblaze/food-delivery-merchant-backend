
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src


COPY ["food-delivery.csproj", "food-delivery/"]
RUN dotnet restore "food-delivery/food-delivery.csproj"

WORKDIR "/src/food-delivery"
COPY . .

RUN dotnet build "food-delivery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "food-delivery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "food-delivery.dll"]

