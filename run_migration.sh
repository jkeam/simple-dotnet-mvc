#!/bin/bash

dotnet tool install --global dotnet-ef
dotnet ef database update
echo 'done'
