# Payment Gateway

## Requirements to run

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

Sql Server needs to be installed, the connection string placed in appsettings and `update-database` needs to be run on the Package Manager Console, specifying the data project, from Visual Studio to initialise the database.

Both PaymentGateway.API and MockBankA need to be running for the API to function as expected.

## Features

Some of the API features include:

* Logging using NLog
* AES Encryption for storing sensitive data
* SQL Server Code First Entity Framework for storing payment details
* Http Client for sending requests and Polly for retries of failed requests
* Basic Authentication for authorising requests
* Automapper for mapping DTOs 

## Assumptions

* The way the bank would accept and return requests/responses. 
* The information that was required such as name, card number, CVV etc... 
* The format which the payment gateway would receive data, such as strings for card details.

## Further work

* Add metrics, to time the calls to the endpoints in the API.  
* Mutliple bank endpoints could be added to the config and named HttpClients could be added for each endpoint to simulate different banks being requested. 
* Ecryption could be extended to store all data rather than just the Card Number for payment. 
* Extra tests could be added to test different input/output.
* More comprehensive logging could be performed to track all behaviour going in and out of the system. 
* Authentication could be extended to have a store of users and passwords in the database. 
* Authentication could be even further extended to request access tokens and distribute to known users for requests.