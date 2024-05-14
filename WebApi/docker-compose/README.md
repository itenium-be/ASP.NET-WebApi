# Working with Docker-Compose

There are some prerequisites for using the included docker-compose.yml files:

1) Make sure you have docker installed (on windows install docker desktop)

2) Create and install an https certificate:

    ```
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cert.pfx -p password!
    ```

3) It's possible that the above step gives you an `A valid HTTPS certificate is already present` error.
   In that case you will have to run the following command, and then  `Re-Run Step 2`

    ```
     dotnet dev-certs https --clean
    ```

4) Trust the certificate

    ```
     dotnet dev-certs https --trust
    ```


## Docker-Compose Commands

WebAPI + PostgreSQL

```sh
docker-compose -f docker-compose.postgresql.yml up -d
docker-compose -f docker-compose.postgresql.yml down
```

Your API should be available at `https://localhost:5100/swagger` and `http://localhost:5010/swagger`

## Specifications

Let's first examine the Environment Variables passed into the dotnet-webapi container.

- ASPNETCORE_ENVIRONMENT : Custom Environment Name.
- ASPNETCORE_URLS : Enter in the Port list.
- ASPNETCORE_HTTPS_PORT : Custom SSL Port.
- DatabaseSettings__ConnectionString : Valid Connection String.
- HangfireSettings__Storage__ConnectionString : Valid Connection String.
- DatabaseSettings__DBProvider : This will the database engine.
- HangfireSettings__Storage__StorageProvider : This will the database engine.

Each of the docker-compose will have the same exact variables with values suited to the context.

Note that the default Docker Image that will be pulled is `iammukeshm/dotnet-webapi:latest`. This is my public Image Repository.
