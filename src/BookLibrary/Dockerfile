FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
RUN apt-get update && apt-get install -y apt-transport-https ca-certificates
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get update && apt-get install -y nodejs
WORKDIR /src
COPY ["src/BookLibrary/BookLibrary.csproj", "BookLibrary/"]
RUN dotnet restore BookLibrary/BookLibrary.csproj

COPY ["src/", "."]
WORKDIR /src/BookLibrary
RUN dotnet build BookLibrary.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BookLibrary.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLibrary.dll"]