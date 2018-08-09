## Deploying as Service Fabric Mesh application
You can deploy the Book Library API as a microservice in Service Fabric Mesh. Since Mesh makes use of Docker containers, you will need to build a Docker container first or use my [prebuilt image](https://hub.docker.com/r/joergjo/booklibrary-netcore/). The provided ARM template references my prebuilt image.

> Note that Service Fabric Mesh is currently in preview and only available in a few Azure regions, like East US or West Europe.

### Deployment
Please make sure to specify a valid MongoDB connection string as an inline JSON parameter in the command shown below. Note that the Service Fabric Mesh resource does _not_ include a MongoDB service. You will either need to have a running MongoDB instance outside of Service Fabric Mesh, or add a MongoDB service to the resource definition. 

Next, follow [these steps](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-cli) to set up Azure CLI 2.0 and the Mesh extension on your system. Then, open a shell and run the following commands. 

### Windows 10 
```
$ cd \path\to\BookLibrary-NetCore
az group create -g <ResourceGroup> -l <Region> 
az mesh deployment create -g <ResourceGroup> --template-file .\mesh_rp.linux.json --parameters "{\"mongoUrl\": {\"value\": \"mongodb://<User>:<Password>@<MongoHostFqdn>/library_database\"}}"
```

### Linux, macOS
```
$ cd /path/to/BookLibrary-NetCore
az group create -g <ResourceGroup> -l <Region> 
az mesh deployment create -g <ResourceGroup> --template-file .\mesh_rp.linux.json --parameters "{\"mongoUrl\": {\"value\": \"mongodb://<User>:<Password>@<MongoHostFqdn>/library_database\"}}"
``` 

The last command will display the public IP address of the application endpoint. Access the app at `http://<PublicIp>/`.
