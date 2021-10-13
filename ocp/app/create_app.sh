#!/bin/bash

# 0. add secrets
cat ./app-secret.yaml | sed "s/CURRENT_PROJECT/$(oc project -q)/" | oc create -f -

# 1. create app
oc new-app dotnet:5.0-ubi8~https://github.com/jkeam/simple-dotnet-mvc.git --strategy=source \
  --env AD_DOMAIN="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.AD_DOMAIN}' | base64 --decode)" \
  --env AD_TENANT_ID="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.AD_TENANT_ID}' | base64 --decode)" \
  --env AD_CLIENT_ID="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.AD_CLIENT_ID}' | base64 --decode)" \
  --env AD_CLIENT_SECRET="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.AD_CLIENT_SECRET}' | base64 --decode)" \
  --env AD_CALLBACK_PATH="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.AD_CALLBACK_PATH}' | base64 --decode)" \
  --env DB_CONNECTION_URL="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.DB_CONNECTION_URL}' | base64 --decode)" \
  --env DB_USER="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.DB_USER}' | base64 --decode)" \
  --env DB_PASS="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.DB_PASS}' | base64 --decode)" \
  --env DB_HOST="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.DB_HOST}' | base64 --decode)" \
  --env ASPNETCORE_ENVIRONMENT="$(oc get secret simple-dotnet-mvc-secret -o jsonpath='{.data.ASPNETCORE_ENVIRONMENT}' | base64 --decode)" \
  --labels 'app.kubernetes.io/part-of=simple-dotnet-mvc,app.openshift.io/connects-to=database-app,app.openshift.io/runtime=dotnet'

# 2. add topology annotations
oc patch deployment simple-dotnet-mvc --patch "$(cat app-annotation-patch.yaml)"

# 3. update exposed port
oc patch service simple-dotnet-mvc --patch "$(cat app-service-patch.yaml)"

# 4. get certs
oc get secrets -n openshift-ingress -o name | grep ingress-certs | xargs -I{} oc get {} -n openshift-ingress -o jsonpath='{.data.tls\.crt}' | base64 --decode > /tmp/ca.crt

oc get secrets -n openshift-ingress -o name | grep ingress-certs | xargs -I{} oc get {} -n openshift-ingress -o jsonpath='{.data.tls\.key}' | base64 --decode > /tmp/ca.key

# 5. create route
oc create route edge --service=simple-dotnet-mvc --cert=/tmp/ca.crt --key=/tmp/ca.key
echo "https://$(oc get route simple-dotnet-mvc -o jsonpath='{.spec.host}')"
rm -rf /tmp/ca.crt /tmp/ca.key

echo 'Update redirect URI in Azure AD and migrate database'
