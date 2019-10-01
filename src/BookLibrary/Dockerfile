FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
ARG appInsightsKubeVersion="1.1.1"
ARG libmanBuildVersion="2.0.76"
RUN apt-get update && apt-get install -y apt-transport-https ca-certificates
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get update && apt-get install -y nodejs
WORKDIR /src
COPY ["src/BookLibrary/BookLibrary.csproj", "BookLibrary/"]
RUN dotnet restore BookLibrary/BookLibrary.csproj

COPY ["src/", "."]
WORKDIR /src/BookLibrary
RUN dotnet add BookLibrary.csproj package "Microsoft.ApplicationInsights.Kubernetes.HostingStartup" -v $appInsightsKubeVersion
RUN dotnet add BookLibrary.csproj package "Microsoft.Web.LibraryManager.Build" -v $libmanBuildVersion

RUN dotnet build BookLibrary.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BookLibrary.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.dll"]
