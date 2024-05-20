# Working with Docker-Compose

There are some prerequisites for using the included docker-compose.yml files:

```
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cert.pfx -p password!
dotnet dev-certs https --trust
docker compose up -d
```

`A valid HTTPS certificate is already present`:

```
dotnet dev-certs https --clean
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cert.pfx -p password!
```
