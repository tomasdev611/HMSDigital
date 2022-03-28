#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Patient/Patient.API/", "Patient/Patient.API/"]
COPY ["Patient/Patient.BusinessLayer/", "Patient/Patient.BusinessLayer/"]
COPY ["Patient/Patient.Data/", "Patient/Patient.Data/"]
COPY ["Patient/Patient.ViewModels/", "Patient/Patient.ViewModels/"]

COPY ["Common/Common.ViewModels/", "Common/Common.ViewModels/"]
COPY ["Common/Common.BusinessLayer/", "Common/Common.BusinessLayer/"]
COPY ["Common.Data/", "Common.Data/"]
COPY ["Common/Common.API/", "Common/Common.API/"]

COPY ["Core/Core.Data/", "Core/Core.Data/"]
COPY ["Core/Core.ViewModels/", "Core/Core.ViewModels/"]

WORKDIR "/src/Patient/Patient.API"
RUN dotnet build "Patient.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patient.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient.API.dll"]