## Deploying on Kubernetes
I've tested the included YAML files for Azure Kubernetes Service (AKS). They should work as is in other Kubernetes environments like minikube or GKE. 

I've included two options to deploy the application in Kubernetes and expose the app to the internet:
- Using a `LoadBalancer` service: This will expose a dedicated IP address for the service on AKS's Azure Load Balancer. 
- Using an `Ingress`: This requires you to install an ingress controller before deploying the app, and exposes this app on an IP address that can be shared with many other services (the way typical shared webhosting sites are built). You can use [Helm](https://helm.sh/) to install an ingress controller on [AKS](https://docs.microsoft.com/en-us/azure/aks/kubernetes-helm).

### Deployment
Please make sure to specify a valid MongoDB connection string in `secrets.yaml`. Note that the Kubernetes service definitions do _not_ include a MongoDB deployment. You will either need to have a running MongoDB instance outside of your Kubernetes cluster, or [deploy MongoDB to your cluster](https://www.mongodb.com/blog/post/introducing-mongodb-enterprise-operator-for-kubernetes-openshift). 

Next, open a shell that is configured to connect to your Kubernetes cluster and run the following commands depending on how you want to expose the BookLibrary application and change directory to the solution's root folder.

### Windows 10 
```
$ cd \path\to\BookLibrary-NetCore
```

### Linux, macOS
```
$ cd /path/to/BookLibrary-NetCore
``` 

#### LoadBalancer
From the solution's root folder, run
```
$ kubectl apply -f manifests/configmap.yaml
$ kubectl apply -f manifests/secrets.yaml
$ kubectl apply -f manifests/loadbalancer-deployment.yaml
```

to deploy the `LoadBalancer version`.<br />
You can access the app at `http://<LoadBalancer-IP>/`.

#### Ingress
Alternatively, run
```
$ kubectl apply -f manifests/configmap.yaml
$ kubectl apply -f manifests/secrets.yaml
$ kubectl apply -f manifests/clusterip-deployment.yaml 
$ kubectl apply -f manifests/ingress.yaml 
```

to deploy the `Ingress` version. <br>
You can access the app at `http://<IngressController-IP>/booklibrary`.
