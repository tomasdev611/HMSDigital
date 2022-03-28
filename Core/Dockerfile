#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Core/Core.API/", "Core/Core.API/"]
COPY ["Core/Core.BusinessLayer/", "Core/Core.BusinessLayer/"]
COPY ["Core/Core.Data/", "Core/Core.Data/"]
COPY ["Core/Core.ViewModels/", "Core/Core.ViewModels/"]

COPY ["Common/Common.API/", "Common/Common.API/"]
COPY ["Common/Common.ViewModels/", "Common/Common.ViewModels/"]
COPY ["Common/Common.BusinessLayer/", "Common/Common.BusinessLayer/"]
COPY ["Common.Data/", "Common.Data/"]
COPY ["Common/NotificationSDK/", "Common/NotificationSDK/"]
COPY ["Common/NetSuiteSDK", "Common/NetSuiteSDK"]

COPY ["Notification/Notification.ViewModels/", "Notification/Notification.ViewModels/"]

WORKDIR "/src/Core/Core.API"
RUN dotnet build "Core.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.API.dll"]