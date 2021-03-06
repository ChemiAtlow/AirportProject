# Airport Project - Server 🐕‍🦺

An ASP.NET Core 5.0 project that is the API server that runs the whole project.

## Table of Contents

- [1. Motivation](#1-motivation)
- [2. Tech & frameworks used](#2-tech--frameworks-used)
- [3. Structure](#3-structure)
  - [3.1. API controller](#31-api-controller)
  - [3.2. Open API & Swagger](#32-open-api--swagger)
  - [3.3. SignalR hub](#33-signalr-hub)
- [4. Installation](#4-installation)

## 1. Motivation

The Server project is the API which is open with a restricted CORS policy, and enables flight creating and getting control tower data.

In addition, the API has a SignalR hub that updates on changes in airports in real-time.

## 2. Tech & frameworks used

-   [C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9) and [.Net 5.](https://github.com/dotnet/core/tree/master/release-notes/5.0)
-   [Entity Framework Core](https://github.com/dotnet/efcore)
-   [SQLite](https://www.sqlite.org/index.html)
-   [SignalR](https://github.com/dotnet/aspnetcore/tree/master/src/SignalR).

## 3. Structure

The project is built of three building blocks:

-   [API controller](#31-api-controller).
-   [OpenAPI & Swagger](#32-open-api--swagger)
-   [SignalR hub](#33-signalr-hub).

### 3.1. API controller

A Simple .Net controller which is an API controller and exposes the following endpoints:
-   Get the data of all the airplanes availabale for flights.
-   Get the history of the flights which have been to a specific station.
-   Get a snapshot of the data of the requested control tower.
-   Adding a new flight which should land / takeoff in the future.

### 3.2. Open API & Swagger

A Swagger UI to show the existing endpoints, with the required request bodies, and the possible API responses.

### 3.3. SignalR hub

A SignalR hub that is incharge of real-time updates to the UI regarding flights moving through out the airport and flights which were added and will be landing / taking off in the future.

## 4. Installation

There are a few available methods to run the API Server of this project.

1.  Go to the [releases page](https://github.com/ChemiAtlow/AirportProject/releases/latest) and download the latest Server asset for your OS.
2.  Clone the project and build the ASP.NET server project to fire-up the API server.
3.  Clone the project, and run docker compose to start a Nginx server containig the Server and the UI.
    ```bash
    docker-compose up --build
    ```
