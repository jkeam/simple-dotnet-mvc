#!/bin/bash

docker run --name keycloak -e KEYCLOAK_USER=admin -e KEYCLOAK_PASSWORD=admin -p 9990:9990 -p 8080:8080 -p 8443:8443 jboss/keycloak:14.0.0
