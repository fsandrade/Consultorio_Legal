#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CL.WebApi/CL.WebApi.csproj", "CL.WebApi/"]
COPY ["CL.Core/CL.Core.csproj", "CL.Core/"]
COPY ["CL.Data/CL.Data.csproj", "CL.Data/"]
RUN dotnet restore "CL.WebApi/CL.WebApi.csproj"
COPY . .
WORKDIR "/src/CL.WebApi"
RUN dotnet build "CL.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CL.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CL.WebApi.dll"]