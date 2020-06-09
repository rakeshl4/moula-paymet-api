# moula-paymet-api

The RESTful API is built with ASP.NET Core 3.1 using a decoupled, maintainable architecture.

## Frameworks and Libraries
- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-3.1?view=aspnetcore-3.1);
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (for data access); 
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) (API documentation).

## How to execute the program

First, install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1). 
Then, open the terminal or command prompt at the root path of the project (```~/Payment.Api```) and run the following commands, in sequence:

```
dotnet restore
dotnet run
```

Navigate to ```https://localhost:5001/swagger``` to check the API documentation.

Navigate to ```https://localhost:5001/api/customer/eb07ea19-38cc-4579-892c-510da1eca613``` to check if the API is working. 

To test all the scenarios, you can use the following curl commands

The database is preloaded with the following customers - 

| Customer Id                            | Full Name      |
| -------------------------------------- | -------------- |
| `eb07ea19-38cc-4579-892c-510da1eca613` | David Snedakar |
| `639485dc-edb9-4e0d-abc5-b164db1aa497` | Curtis Peltz   |

### As a user I want to see my balance and a list of payments. 

```
curl -k https://localhost:5001/api/customer/eb07ea19-38cc-4579-892c-510da1eca613
``` 

```
curl -k https://localhost:5001/api/customer/639485dc-edb9-4e0d-abc5-b164db1aa497
```

### As a user I want to create a payment request

```
curl -i -k -X POST "https://localhost:5001/api/payment/create" -H "Content-Type: application/json" --data "{\"customerId\":\"eb07ea19-38cc-4579-892c-510da1eca613\", \"amount\":435.6, \"transactionDate\":\"2015-01-01T15:23:42\"}"

```

### As a user I want to cancel a payment request 

The **PAYMENT_ID** is obtained from the response in the previous step of creating a new payment.

```
curl -i -k -X POST "https://localhost:5000/api/payment/cancel" -H "Content-Type: application/json" --data "{\"transactionId\":\"[REPLACE WITH PAYMENT_ID]\", \"comments\":\"abort transaction\"}"

```

### As a user I want to process a payment request

The **PAYMENT_ID** is obtained from the response in the previous step of creating a new payment.

```
curl -i -k -X POST "https://localhost:44335/api/payment/approve" -H "Content-Type: application/json" --data "{\"transactionId\":\"[REPLACE WITH PAYMENT_ID]\"}"

```

## How to run unit tests

First, install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1). 
Then, open the terminal or command prompt at the root path of the project (```~/Payment.Api.UnitTest```) and run the following commands, in sequence:

```
dotnet restore
dotnet test
```

## How to run e2e tests

First, install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1). 
Then, open the terminal or command prompt at the root path of the project (```~/Payment.Api.FunctionalTest```) and run the following commands, in sequence:

```
dotnet restore
dotnet test
```
