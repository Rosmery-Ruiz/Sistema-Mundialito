
# Sistema Mundialito

Proyecto académico desarrollado para poner en práctica mis conocimientos en desarrollo de software, utilizando C#, ASP.NET Core, Next.js y SQL Server.

## Descripción

Sistema para la gestión de un torneo de fútbol, que permite administrar equipos, jugadores, partidos y resultados, además de consultar las posiciones de los equipos.

## Funcionalidades

- Registro y administración de equipos.
- Registro y administración de jugadores.
- Gestión de partidos.
- Registro de resultados.
- Consulta de tabla de posiciones.

## Tecnologías utilizadas

**Backend**
- C#
- ASP.NET Core
- API REST
- Entity Framework Core
- SQL Server

**Frontend**
- Next.js
- React
- JavaScript
- CSS

**Herramientas**
- Visual Studio 2022
- Visual Studio Code
- Git y GitHub

## Estructura del proyecto

El backend se desarrolló utilizando Clean Architecture, separando el código en diferentes capas:

- **Domain:** entidades y reglas del dominio.
- **Application:** lógica de aplicación y casos de uso.
- **Infrastructure:** acceso a datos y repositorios.
- **API:** controladores y endpoints.

El frontend se desarrolló por separado utilizando Next.js.

## Organización

```text
Sistema-Mundialito/
├── backend/
└── frontend/
```

## Base de datos

El proyecto utiliza SQL Server y Entity Framework Core.

Las migraciones se encuentran en `Mundialito.Infrastructure`.

Para crear la base de datos, configurar la cadena de conexión en `appsettings.json` y ejecutar desde la Consola del Administrador de paquetes:

```powershell
Update-Database -Project Mundialito.Infrastructure -StartupProject Mundialito.API
```

El proyecto incluye datos de prueba (Seed) que se cargan automáticamente al iniciar la API, siempre que no existan equipos registrados.

## Sobre el proyecto

Este sistema fue desarrollado como parte de una actividad académica, con el objetivo de aplicar conocimientos de programación, desarrollo de API REST, bases de datos y organización del código.
