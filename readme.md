
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
kind load docker-image merchants-backend:latest
```