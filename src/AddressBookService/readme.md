# Address Book Service

## Introduction

Project purpose is to demonstrate use of Dapper and MSSQL in a docker container.
MSSQL volumes are mounted into .ctn folder (added to .gitignore).
When running service as standalone container, remember to add database connection string to appsettings.json or environment variables.

## Docker commands

Build:
>`docker compose build`

Run:
> `docker compose up -d`

Stop
> `docker compose down`

## Env file

Expected values in environment file:

```yaml
AB_SERVICE_PORT=8080
AB_DATABASE_SETUP=true
ASP_ENV=Development
MSSQL_CONNECTION='Server=mssql;Initial Catalog=AddressBookDb;User Id=sa;Password=<db password>;TrustServerCertificate=true'
MSSQL_PORT=1433
MSSQL_SAPW=<db password>
```

Place this .env in same folder as docker-compose.yml (root folder)