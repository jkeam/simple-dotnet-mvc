# Simple Dotnet MVC
Simple dotnet mvc app.

## OpenShift
```
cd ocp
oc apply -k ./db

# you can check
./db/connect_to_db.sh # then hook up your favorite mssql client

# after db is done
cd app
./create_app.sh

# after app is created run migration
./run_migration.sh

# after migration is done, feel free to delete
oc delete jobs/simple-dotnet-mvc-db-migration
```

### Webhook
These are optional steps to setup a GitHub webhook.
```
# Grab secret, look for triggers -> github -> secret
oc get bc -o yaml

oc describe $(oc get bc -o name)

# Look for Webhook GitHub URL
# In your GitHub repository, select Add Webhook from Settings → Webhooks.  Add this URL to Payload URL.
# Replace <secret> with secret found in first step
# Change the Content Type from GitHub’s default application/x-www-form-urlencoded to application/json
```


## Database
If not using the `./run_migration.sh` then you can run the migrations manually below.  But you should use that script as that creates a Kube Job that does the same thing.  As background, the container image, particularly the `Main` function in `Program.cs` takes in an env variable named `MIGRATION` that either starts the server or runs a migration and terminates.  This is a common .Net practice.

```
# Install tool
dotnet tool install --global dotnet-ef
# Create and name migration
dotnet ef migrations add InitialCreate
# Run migration
dotnet ef database update
```

## OpenShift Resources
The following are the `yaml`s used.  They are used in the `OpenShift` section above.

### Local
oc new-project jon

#### Database
1.  db-pvc.yaml
2.  db-secret.yaml
3.  db-deployment.yaml
4.  db-service.yaml
5.  db-route.yaml - or equivalent

#### App
1.  app-secret.yaml
2.  app-deployment.yaml
3.  app-service.yaml
4.  app-route.yaml
