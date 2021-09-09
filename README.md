# Authentication Service

This is a C# translation of the microservice that handles authentication for the ChatBuds application.

Made in ASP.NET Core 5.0.

The original can be found here: https://github.com/irosvall/chatbuds-auth-service

## <span style="color:red">OBS</span>
This service works fine on its own at the moment, but I have not made it work yet in combination
with the other ChatBuds API's made in node.js.

## API documentation
The documentation of which http requests are possible to make is found at: https://app.swaggerhub.com/apis-docs/chatbuds/auth-service/1.0.0

## System overview
ChatBuds is built as a small microservice-architecture. The consumer/client only communicates with the API Gateway which handles the back-end communication.

The client: https://github.com/irosvall/chatbuds-client

The API Gateway: https://github.com/irosvall/chatbuds-api-gateway

The Resource Service: https://github.com/irosvall/chatbuds-resource-service

The original Auth Service: https://github.com/irosvall/chatbuds-auth-service

![Architecture](.readme/chatbuds-architecture.png)

## Starting up this project locally
To run the Authentication Service locally you will need to add environment variables to the user-secrets.

- PUBLIC_KEY_FILEPATH = the file path to private RS256 key used for JWT encoding
- PRIVATE_KEY_FILEPATH = the file path to public RS256 key used for JWT encoding/decoding
- JWT_EXPIRE_TIME = Specifies the expiration date for the JWT in seconds from it was made.
- DB_CONNECTION_STRING = The database connection string

### How to add environment variables

This article explains how to store environment variables in user-secrets: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows

### Generate RS256 key pairs

The JWTs issued by the service should use RS256 encryption. To achieve this, you need to generate private and public keys.

```bash
$ openssl genrsa -out private.pem 2048
Generating RSA private key, 2048 bit long modulus
.....+++++
...................................+++++
e is 65537 (0x010001)

$ openssl rsa -in private.pem -pubout -out public.pem
writing RSA key

```
