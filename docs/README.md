# Payment Gateway

This project requires an appsettings.json file with the following structure at the root of PaymentGateway.API project:

````markdown
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Trace",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PaymentConnectionString": "ConnectionString for Sql Server db"
  },
  "BankAUrl": "UrlOfMockBankA",
  "User": "Joe",
  "Password": "Bloggs",
  "Key": "Must be 16 characters long",
  "Iv": "Must be 16 characters long"
}

````

The API contains the following features:

* Logging using NLog
* AES Encryption for storing sensitive data
* SQL Server Code First Entity Framework for storing payment details
* Http Client for sending requests and Polly for retries of failed requests
* Basic Authentication for authorising requests

## Assumptions

The way the bank would accept and return requests/responses was assumed. A status code rather than a string was returned to the user which again was assumed that they would understand what that would mean.

## Further work

Further extension to the project could be done to add metrics, to time the calls to the endpoints in the API.  Mutliple endpoints could be added to the config and multiple named HttpClients could be added for different endpoints to simulate different banks being requested.