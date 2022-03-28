#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Notification/Notification.API/", "Notification/Notification.API/"]
COPY ["Notification/Notification.BusinessLayer/", "Notification/Notification.BusinessLayer/"]
COPY ["Notification/Notification.ViewModels/", "Notification/Notification.ViewModels/"]

COPY ["Common/Common.API/", "Common/Common.API/"]
COPY ["Common/Common.BusinessLayer/", "Common/Common.BusinessLayer/"]
COPY ["Common/Common.ViewModels/", "Common/Common.ViewModels/"]
COPY ["Common.Data/", "Common.Data/"]
COPY ["Core/Core.Data/", "Core/Core.Data/"]

WORKDIR "/src/Notification/Notification.API"
RUN dotnet build "Notification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"]