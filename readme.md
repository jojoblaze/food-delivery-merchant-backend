
Build Docker application image

```
docker build -t merchants-backend -f Dockerfile .
```

Run Docker application container locally on port 8080

```
docker run -p 8080:80 -e DOTNET_URLS=http://+:80 -e Logging__Loglevel__Default=Debug -e Logging__Loglevel__Microsoft.AspNetCore=Debug merchants-backend
```

Push Docker application image into Kind cluster
```
kind load docker-image merchants-backend:latest --name <cluster name>
```


Create Kubernetes Deploy
```
kubectl create -f manifest.yml --context kind-<cluster name>

*** --context option wants a cluster name starting with the prefix "kind-" ***
```

To access the application, check <b>NodePort</b> in the manifest.yml
For this application is defined the nodePort as 30008. This means that the service will be exposed on port 30008 of Kubernetes nodes. 
Ensure that you are accessing your web API using the correct host IP address and the node port.

To retrieve the IP address of your Kubernetes nodes when using Kind, you can use the following command:
```
kubectl get nodes -o jsonpath='{ $.items[*].status.addresses[?(@.type=="InternalIP")].address }'
```

Drop Kubernetes Deploy
```
kubectl delete -f manifest.yml
```