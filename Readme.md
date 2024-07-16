### Generate cert and configure local machine
```console
dotnet dev-certs https -ep ${HOME}/.aspnet/https/GlobalExceptionHandler.Api.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

> Note: The certificate name, in this case *GlobalExceptionHandler.Api*.pfx must match the project assembly name.

> Note: `<CREDENTIAL_PLACEHOLDER>` is used as a stand-in for a password of your own choosing.

Configure application secrets, for the certificate:

```console

dotnet user-secrets init -p GlobalExceptionHandler.Api/GlobalExceptionHandler.Api.csproj
dotnet user-secrets -p GlobalExceptionHandler.Api/GlobalExceptionHandler.Api.csproj set "Kestrel:Certificates:Development:Password" "<CREDENTIAL_PLACEHOLDER>"
```

> Note: The password must match the password used for the certificate.

### Building and running your application
```console
docker compose up --build -d
```