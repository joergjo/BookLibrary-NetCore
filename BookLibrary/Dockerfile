FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY BookLibrary/BookLibrary.csproj BookLibrary/
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