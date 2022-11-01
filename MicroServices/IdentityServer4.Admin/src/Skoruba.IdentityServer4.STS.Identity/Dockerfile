# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.STS.Identity/Skoruba.IdentityServer4.STS.Identity.csproj", "src/Skoruba.IdentityServer4.STS.Identity/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Shared.Configuration/Skoruba.IdentityServer4.Shared.Configuration.csproj", "src/Skoruba.IdentityServer4.Shared.Configuration/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.EntityFramework.Shared/Skoruba.IdentityServer4.Admin.EntityFramework.Shared.csproj", "src/Skoruba.IdentityServer4.Admin.EntityFramework.Shared/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.EntityFramework.Identity/Skoruba.IdentityServer4.Admin.EntityFramework.Identity.csproj", "src/Skoruba.IdentityServer4.Admin.EntityFramework.Identity/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.EntityFramework/Skoruba.IdentityServer4.Admin.EntityFramework.csproj", "src/Skoruba.IdentityServer4.Admin.EntityFramework/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.EntityFramework.Extensions/Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.csproj", "src/Skoruba.IdentityServer4.Admin.EntityFramework.Extensions/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.EntityFramework.Configuration/Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.csproj", "src/Skoruba.IdentityServer4.Admin.EntityFramework.Configuration/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Shared/Skoruba.IdentityServer4.Shared.csproj", "src/Skoruba.IdentityServer4.Shared/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.BusinessLogic.Identity/Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.csproj", "src/Skoruba.IdentityServer4.Admin.BusinessLogic.Identity/"]
# COPY ["IdentityServer4.Admin/src/Skoruba.IdentityServer4.Admin.BusinessLogic.Shared/Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.csproj", "src/Skoruba.IdentityServer4.Admin.BusinessLogic.Shared/"]
# RUN dotnet restore "src/Skoruba.IdentityServer4.STS.Identity/Skoruba.IdentityServer4.STS.Identity.csproj"
# COPY . .
# WORKDIR "/src/src/Skoruba.IdentityServer4.STS.Identity"
# RUN dotnet build "Skoruba.IdentityServer4.STS.Identity.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "Skoruba.IdentityServer4.STS.Identity.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
# ENTRYPOINT ["dotnet", "Skoruba.IdentityServer4.STS.Identity.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "MicroServices/IdentityServer4.Admin/src/Skoruba.IdentityServer4.STS.Identity/Skoruba.IdentityServer4.STS.Identity.csproj"
RUN dotnet publish "MicroServices/IdentityServer4.Admin/src/Skoruba.IdentityServer4.STS.Identity/Skoruba.IdentityServer4.STS.Identity.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Skoruba.IdentityServer4.STS.Identity.dll"]