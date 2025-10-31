# Estructura del Proyecto

---

## Estructura Global

---

* documentation - proyecto con toda la documentación referente al proyecto, orientación, configuración, diseño de la solución.
* src - contiente todo el código de la solución
  * YuGiOh.Domain - proyecto de la capa de dominio
    * Enums
    * Models
    * Services
    * Interfaces
    * Exceptions
    * ValueObjects
    * DataToObjects
  * YuGiOh.Application - proyecto de la capa de aplicación
    * Behaviors
    * Features
      * Auth
      * TournamentManagement
      * Statistics
  * YuGiOh.Infrastructure
    * Migrations
    * Persistence
      * Configurations
      * Repositories
      * DbContext.cs
    * Caching
    * CSC
    * Emailing
    * Archetypes
    * Identity
      * Services
    * Seeding
  * YuGiOh.WebAPI
    * Controllers
    * Middleware
    * Program.cs
  * yu-gi-oh.presentation
    * src
      * auth
        * register
          * account
          * player
          * staff
          * deck
        * login
      * tournament_management
        * create
        * enroll
        * rounds
          * generate
          * view
        * matches
          * update
          * view
      * statistics
        * stat_name
    * tests
    * e2e
  * YuGiOh.UnitTests
    * Domain
      * Exceptions
    * Application
      * Behaviors
      * Features
        * Auth
        * TournamentManagement
        * Statistics
    * Infrastructure
      * Persistence
      * Caching
      * CSC
      * Emailing
      * Archetypes
      * Identity
      * Seeding
    * WebAPI
      * Controllers
      * Middlewares
  * YuGiOh.IntegrationTests
    * WebAPI
      * Controllers
      * Middlewares

---

## Capa de Dominio

---

### Creación

```bash
dotnet new classlib
```

---

## Capa de Aplicación

---

### Creación

```bash
dotnet new classlib
dotnet add reference ../YuGiOh.Domain/YuGiOh.Domain.csproj
dotnet add package MediatR
dotnet add package FluentValidator
dotnet add package AutoMapper
```

---

## Capa de Infraestructura

---

### Creación

```bash
dotnet new classlib
dotnet add reference ../YuGiOh.Domain/YuGiOh.Domain.csproj
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package MailKit
dotnet add package Ardalis.Specification.EntityFrameworkCore
dotnet add package HtmlAgilityPack
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Newtonsoft.Json
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package StackExchange.Redis
dotnet add package System.IdentityModel.Tokens.Jwt
```

---

## Capa de Presentación de la API

---

### Creación

```bash
dotnet new webapi
dotnet add reference ../YuGiOh.Application/YuGiOh.Application.csproj
dotnet add reference ../YuGiOh.Infrastructure/YuGiOh.Infrastructure.csproj
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Correr el proyecto

```bash
dotnet ef migrations add InitialCreate --project YuGiOh.Infrastructure --startup-project YuGiOh.WebAPI
dotnet ef database update --project YuGiOh.Infrastructure --startup-project YuGiOh.WebAPI

dotnet clean
dotnet build
dotnet run
```

---

## Capa de Presentación

---

### Creación

Se crea el proyecto mediante el siguente comando.

```bash
npx create-next-app@14.2.13 yu-gi-oh.presentation --typescript --eslint --use-npm
```

### Correr

Se corre el proyecto mediante el siguiente comando.

```bash
npm run dev
```

---

## Tests Unitarios

---

### Creación

```bash
dotnet new xunit
dotnet add reference ../YuGiOh.WebAPI/YuGiOh.WebAPI.csproj
```
