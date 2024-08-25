## Handling Exceptions in ASP.NET Core with IExceptionHandler

.NET 8 introduces a streamlined approach for global exception handling, making your applications more resilient and easier to maintain. Here’s how it works:

🚀 Efficient Error Handling: The new IExceptionHandler makes managing exceptions cleaner and easier, boosting code maintainability.

🔍 Clear Error Messages: ProblemDetails ensures clients get clear, helpful error messages.

🛠️ Logging: Integrated logging speeds up issue diagnosis. (Adjust your logger to prevent double-logging with DiagnosticsTelemetry.)

### Implementation:
- Create & Register: Implement your handler and register it with services.AddExceptionHandler<ExceptionHandler>().

- Standardize: Register ProblemDetails for RFC 7807-compliant error responses.

- Configure: Add UseExceptionHandler() early in the pipeline to catch exceptions.

### Keep in mind:
- You can enrich the ProblemDetails object with additional information using the Extensions property for even more detailed error reporting.

- Be careful not to disclose sensitive internal information, such as stack traces and exception data, to Web API clients, as this could pose security risks.


### Run application 

#### Generate cert and configure local machine
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

#### Building and running your application
```console
docker compose up --build -d
```