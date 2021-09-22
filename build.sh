#!/bin/bash

cd /app && dotnet build && dotnet publish --configuration Release
