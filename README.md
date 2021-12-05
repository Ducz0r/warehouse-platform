# Warehouse Platform

## Description

This solution includes 3 projects that entail the Warehouse Platform. The platform allows tracking of customers' quantities in the centralized warehouse.

## Pre-requisites

In order for the solution to properly work (e.g. to properly execute it in development environment), the following system requirements must be met:

- Windows 10 operating system (haven't tested this on older systems)
- [Visual Studio 2019](https://visualstudio.microsoft.com/) (or newer)
- [.NET SDK 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Node.js](https://nodejs.org/en/)
- [Angular CLI](https://angular.io/cli)

## How it works

#### Server

Server is an ASP.NET Core REST API web service, which runs on an SQLite database behind-the-screens. It acts as a centralized storage point,
as well as a centralized state-holder and center-piece of the entire platform.

#### Administration App

Administration app is an SPA (single-page application) in Angular framework that connects to the server using shared secret credentials (pre-configured bearer token),
and allows display of all warehouse customers and their quantities, to the administrators of the warehouse. Currently, it does not perform any user authentication.

#### Client App

Client application is a desktop WPF application that can be used by customers to login to the platform, and increase their quantities. Customers authenticate into the
application using their name and password combination.

### Run

To run individual project, you can start the project (`Start` run configuration). You can also configure the solution, to start all 3 projects simultaneously by using `<Multiple Startup Projects>` option.

> NOTE: It is important to note that both Administration App and Client App require the Server to be running.

### Run Server

The server should start running locally on `https://localhost:5001`. In development mode, it will also display the [Swagger](https://swagger.io/) API specification, where you can even query the endpoints
(as long as you identify with a customer's credentials).

The server store the data in an [SQLite](https://www.sqlite.org/index.html) file-based database. The database is located under `%ProgramData%\warehouse.db` path. When starting up, server checks whether
the SQLite file already exists, and if not, creates it and runs all the necessary data migrations. These can, however, also be manipulated in the development environment from within Visual Studio using
Package Manager Console commands (e.g. `Update-Database`).

#### Seeding fake data

To seed fake data in development mode, a non-authentication-protected API endpoint `https://localhost:5001/customers/init` (`POST`) can be called to seed some initial data into the database. **Warning!
This will destroy any previous data in the database.**

### Run Administration App

Before running administration app for the first time, it is advised to navigate into `<PLATFORM-PATH>\Warehouse.AdministrationApp\ClientApp` folder in console (this folder is NOT to be confused with the desktop
WPF client application), and perform the following commands there:

```
npm install
```

> NOTE: Sometimes, during development, issues can occur, and in such a scenario, it is sometimes also good to fully remove the `node_modules` folder within `ClientApp`, and re-run `npm install`.

### Run Client Application

The client application can be started from Visual Studio. Since this is a standard WPF desktop application, there is nothing special going on there.

## Remaining work

Oh boy! Since this task was time-limited to 2 days, there are A TON of things left to be desired. The following is a non-complete list of remaining "TODOs":

- Server
  - **Refactor authentication to use OpenID Connect / JWT tokens, perhaps Microsoft Identity framework**
  - Better error-handling on the REST side
  - Setup API versioning (e.g. `/api/v1`)
  - Better REST endpoints Swagger documentation
  - Make database file path configurable (e.g. into `.properties/appsettings` file/s)
  - Make bearer token configurable (e.g. into `.properties/appsettings` file/s)
  - Better seeding/faking of data, not through endpoint
  - Enforce more strict DB constraints & rules (on domain model level, and DB level itself)
  - User MediatR's error handling, instead of relying onto home-grown return types
  - Make properties files for different environments, load them in code at server startup
  - Switch to SQL server, e.g. PostgreSQL
  - Dockerization of server, database
  - Add localization layer
- Angular SPA
  - Make connection with the Server through WebSockets, to have near-real-time updating of table rows, not by using HTTP long polling
  - Implement a smarter method of updating the customers list (not always re-populating the entire list, but only do "smart" changes where data was changed)
  - Pagination of the table display of customers
  - Get rid of warnings on ng-serve (`Unmatched selector: %`, and `WebSocket is closed before the connection is established`)
  - Make API URL, bearer token configurable (e.g. in `environment.ts` file/s, or elsewhere)
  - Better file organization, package different segments of the website into different Angular modules & folders (e.g. services, views, pages, modules, common etc.)
  - Update the Angular dependencies list (the package.json was made from default Microsoft Angular template, which is outdated as of now, and then fixed until it was working, but not without trouble)
  - Add localization layer
- Desktop app
  - **Update the authentication mechanism to a short-lived token-based mechanism (OpenID Connect / JWT), so current user's password is not stored in-memory (albeit it's mostly stored as `SecureString`)**
  - Use MediatR's error handling, instead of relying onto home-grown return types
  - All MediatR handlers should generally have better error handling, and more strict and understandable return types; e.g. `WebRequestResult` is quite ambigous, and it can carry different data types in its Content property, which is not optimal
  - Move server URL etc. to some config file (e.g. `.properties/appsettings`), not have it in `CustomHttpClient` class
  - Add localization layer
  - Style the user interface
  - Use [http client factory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests), instead of having singleton `HttpClient` object