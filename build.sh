#!bin/bash
set -e
dotnet restore
cd BookLibrary.Test
dotnet xunit
cd ..
rm -rf $(pwd)/publish/
dotnet publish BookLibrary/BookLibrary.csproj -c release -o $(pwd)/publish
