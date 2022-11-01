FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .

RUN dotnet restore "MicroServices/DocumentCrudService/DocumentCrudServiceApi/DocumentCrudServiceApi.csproj"
RUN dotnet publish "MicroServices/DocumentCrudService/DocumentCrudServiceApi/DocumentCrudServiceApi.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "DocumentCrudServiceApi.dll"]