

docker build -t merchants-backend -f Dockerfile .

kind load docker-image merchants-backend:latest --name food-delivery-cluster

kubectl delete -f manifest.yml --context kind-food-delivery-cluster
kubectl create -f manifest.yml --context kind-food-delivery-cluster
