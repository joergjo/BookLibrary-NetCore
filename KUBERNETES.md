## Running in Kubernetes
I've tested the included YAML files for Kubernetes with Azure Container Service (AKS). They should work with little or no modification in other Kubernetes environments like minikube or GKE. 

I've included two options to deploy the application in Kubernetes:
- As a `LoadBalancer` service: This will expose a dedicated IP address for the service on AKS's Azure Load Balancer. 
- Using an `Ingress`: This requires you to install an ingress controller before you deploy the app, and exposes this app on an IP address that can be shared with many other services (the way typical shared webhosting sites are built). 

### Deployment
Please make sure to specify a valid MongoDB connection string in `booklibrary-configmap.yaml`. Note that the Kubernetes service definitions do _not_ include a MongoDB pod. You will either need to have a running MongoDB instance outside of your Kubernetes cluster, or add a MongoDB pod to the service definition. 

Then, open a shell that is configured to connect to your Kubernetes cluster and run the following commands depending on how you want to expose the BookLibrary service.

<code>
$ cd /path/to/BookLibrary-NetCore<br />
</code> (Linux, macOS)
or<br />
<code>
$ cd \path\to\BookLibrary-NetCore<br />
</code> (Windows)<br />
<br />

#### LoadBalancer
<code>
$ kubectl apply -f booklibrary-configmap.yaml<br/>
$ kubectl apply -f booklibrary-loadbalancer-svc.yaml<br/>
</code> 

to deploy the `LoadBalancer version`.<br />
You can access the app at `http://<LoadBalancer-IP>/`.

#### Ingress
<code>
$ kubectl apply -f booklibrary-configmap.yaml <br>
$ kubectl apply -f booklibrary-nodeport-svc.yaml <br> 
$ kubectl apply -f booklibrary-ingress.yaml <br> 
</code>

to deploy the `Ingress` version. <br>
You can access the app at `http://<IngressController-IP>/booklibrary`.