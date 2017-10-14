#!/bin/bash
set -e

if [[ -z $1 ]] ; then
    echo "Usage: ./build.sh path-to-csproj"
    exit 1
fi

dotnet restore
dotnet build

for dir in $(find -name *.Test -type d); do 
    pushd "$dir"
    dotnet xunit -nobuild
    popd
done

rm -rf $(pwd)/publish/
dotnet publish $1 -c release -o $(pwd)/publish
