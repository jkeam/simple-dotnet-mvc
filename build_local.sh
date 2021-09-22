#!/bin/bash

docker run --rm --name simple-dotnet-builder -v "$(PWD):/app" -it mcr.microsoft.com/dotnet/sdk:5.0 /app/build.sh
docker build -t simple-dotnet-mvc -f ./Dockerfile_deploy .
docker tag simple-dotnet-mvc jonnyman9/simple-dotnet-mvc
docker push jonnyman9/simple-dotnet-mvc