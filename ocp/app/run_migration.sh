#!/bin/bash

oc process -f ./app-migration.yaml -p IMAGE_URL="$(oc get is simple-dotnet-mvc -o jsonpath='{.status.dockerImageRepository}'):latest" | oc create -f -
