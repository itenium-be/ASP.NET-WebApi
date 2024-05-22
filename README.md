ASP.NET WebApi
==============

## Prep

- Update to last version of Visual Studio 2022
- Install Docker for Windows
- Install .NET 7 SDK
- Node v16+ (?)
- `docker pull postgres:15-alpine`


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

### Swagger

Tokens -> POST `/api/tokens`  
Tenant: `root`

```json
{
    "email":"admin@root.com",
    "password":"123Pa$$word!"
}
```


### Postman

See `postman` for a postman collection.  
Or go with an open-source & free alternative that looks exactly the same:

- [hoppscotch.io](https://hoppscotch.io/): In browser Postman
    - [Chrome Extension](https://chrome.google.com/webstore/detail/hoppscotch-browser-extens/amknoiejhlmhancpahfcfcfhllgkpbld)
    - [Docker-Compose](https://github.com/hoppscotch/hoppscotch/blob/main/docker-compose.yml)
    - [Github](https://github.com/hoppscotch/hoppscotch)
- [Insomnia](https://github.com/Kong/insomnia)
- [Nightingale](https://github.com/jenius-apps/nightingale-rest-api-client)


In Visual Studio Code:

- [Thunder Client](https://marketplace.visualstudio.com/items?itemName=rangav.vscode-thunder-client): VSCode Plugin
    - [Github](https://github.com/rangav/thunder-client-support)
- [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
    - [Github](https://github.com/Huachao/vscode-restclient)


### cURL

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
- EF Repository Abstraction with [Ardalis.Specification](https://github.com/ardalis/Specification)
- Serilog
- OpenAPI & Swagger
- API Versioning
- Fluent Validations
- Response Caching - Distributed Caching + REDIS


## REST

See `REST-JWT.pptx` for some general REST infos.
Or see our specific [REST-JWT-Postman-DevTools](https://github.com/itenium-be/REST-JWT-DevTools) session.



## Topics

- Filters / custom middleware / global error handling (ActionFilterAttribute)
- Minimal APIs
- Versioning: Create a /v2 where we renamed products -> socks (or just uri-rewriting?)
- Configuration json (secrets.json)
- JSON Serialization (enums) / Zipping
- Caching (Redis)
- Polly (Retries)
- Validation (Attributes and/or FluentValidation)
- Return Http Codes
- Working with files (upload/download)
- Logging?
