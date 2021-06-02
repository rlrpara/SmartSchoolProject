#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SmartSchool.WebApi/SmartSchool.WebApi.csproj", "SmartSchool.WebApi/"]
COPY ["SmartSchool.Service/SmartSchool.Service.csproj", "SmartSchool.Service/"]
COPY ["SmartSchool.Infra.Utilities/SmartSchool.Infra.Utilities.csproj", "SmartSchool.Infra.Utilities/"]
COPY ["SmartSchool.Domain/SmartSchool.Domain.csproj", "SmartSchool.Domain/"]
COPY ["SmartSchool.Infra.Data/SmartSchool.Infra.Data.csproj", "SmartSchool.Infra.Data/"]
RUN dotnet restore "SmartSchool.WebApi/SmartSchool.WebApi.csproj"
COPY . .
WORKDIR "/src/SmartSchool.WebApi"
RUN dotnet build "SmartSchool.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartSchool.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartSchool.WebApi.dll"]