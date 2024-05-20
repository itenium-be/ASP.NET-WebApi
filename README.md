ASP.NET WebApi
==============

## Prep

- Update to last version of Visual Studio 2022
- Install Docker for Windows
- Install .NET 7 SDK
- Node v16+ (?)


## postgres

Login: postgres / admin  
Port: 5433  
Database: socks

```sh
cd WebApi/docker-compose
docker compose up -d
```

## WebApi

`/WebApi`: Swagger @ `localhost:5001/swagger`

[Template: FullStackHero](https://github.com/fullstackhero/dotnet-webapi-starter-kit)
  - [FSH Getting started video](https://www.youtube.com/watch?v=a1mWRLQf9hY)
  - [Docs](https://fullstackhero.net)
  - [Getting Started Docs](https://fullstackhero.net/dotnet-webapi-boilerplate/general/getting-started/)

### Get a Token

Swagger: Tokens -> POST `/api/tokens`  
Tenant: `root`

```json
{
    "email":"admin@root.com",
    "password":"123Pa$$word!"
}
```

CURL command for getting a token:

```curl
curl -X POST \
  'https://localhost:5001/api/tokens' \
  --header 'Accept: */*' \
  --header 'tenant: root' \
  --header 'Accept-Language: en-US' \
  --header 'Content-Type: application/json' \
  --data-raw '{
  "email": "admin@root.com",
  "password": "123Pa$$word!"
}'
```

### Features

- Multi Tenancy Support with Finbuckle
- Serilog
- OpenAPI & Swagger
- API Versioning
- Fluent Validations
- Response Caching - Distributed Caching + REDIS




## Topics

- CI / IOC
- Filters / custom middleware / global error handling
- Minimal APIs
- Model binding
- Configuration json (secrets.json)
- JSON Serialization (enums) / Zipping
- REST
- Caching
- Poly (Retries)
- Validation
- Return Http Codes
- Working with files (upload/download)
- Logging?
