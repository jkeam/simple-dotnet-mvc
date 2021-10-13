#!/bin/bash

oc get pods -o name --field-selector status.phase=Running -l app=database-app | xargs -I{} oc port-forward {} 1433
