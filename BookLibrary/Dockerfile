FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
RUN apt-get update && apt-get install -y apt-transport-https ca-certificates
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get update && apt-get install -y nodejs
WORKDIR /src
COPY ["BookLibrary/BookLibrary.csproj", "BookLibrary/"]"
RUN dotnet restore BookLibrary/BookLibrary.csproj

COPY . .
WORKDIR /src/BookLibrary
RUN dotnet build BookLibrary.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BookLibrary.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLibrary.dll"]