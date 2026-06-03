# Sports Ground Booking API

![.NET](https://img.shields.io/badge/.NET-10-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![License](https://img.shields.io/badge/license-MIT-green)

A backend API for multi-sport ground booking management built with ASP.NET Core Web API and Microsoft SQL Server.

## Features

- User registration and JWT login
- Ground CRUD management
- Sport-specific ground cataloging
- Slot CRUD management
- Booking creation, retrieval, cancellation
- Role-based admin authorization
- Swagger OpenAPI documentation
- Automatic EF Core migrations on startup
- Docker-ready production container

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication
- Swagger / OpenAPI

## Project Progress

### Completed

- User registration and JWT login
- Ground CRUD endpoints
- Slot CRUD endpoints
- Booking creation, retrieval, cancellation
- Role-based admin authorization
- Swagger documentation
- Automatic database migrations on startup
- Docker production container support
- Payment APIs
- Addon management APIs
- Enhanced admin dashboard endpoints

### In Progress

- Booking availability / calendar filtering

### Planned

- Ground image upload system
- Invoice and receipt generation
- Tournament / multi-day booking support
- Pricing engine and rate calculation
- Full booking history export
- Email notification system
- Advanced admin reporting
- Revenue analytics and charts
- Booking reminder notifications

## Requirements

- .NET 10 SDK
- SQL Server instance or container
- Docker (optional)

## Problems Faced

### 1. Circular JSON serialization with EF Core navigation properties

- Problem: EF Core entities with navigation properties created recursive object graphs and caused `System.Text.Json.JsonException`.
- Fix: Added `ReferenceHandler.IgnoreCycles` in `Program.cs`.
- Reference: https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-preserve-references

### 2. Production configuration loading during local development

- Problem: Running without `ASPNETCORE_ENVIRONMENT=Development` caused production settings to load and the wrong connection string to be used.
- Fix: Explicitly set environment variables when running locally.
- Reference: https://learn.microsoft.com/aspnet/core/fundamentals/environments

### 3. Docker container cannot connect to SQL Server using `localhost`

- Problem: Inside a container, `localhost` resolves to the container itself, not the host or another container.
- Fix: Use the SQL Server container hostname or a shared Docker network alias.
- Reference: https://docs.docker.com/network/

### 4. Swagger not reachable externally

- Problem: The app only listened on `localhost` by default.
- Fix: Set `ASPNETCORE_URLS="http://0.0.0.0:5107"` and map the port in Docker.
- Reference: https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel

### 5. Static addon pricing issue

- If addon prices change later, old bookings should still preserve original pricing.
- Fix: BookingAddon stores:
    - item name
    - unit price
    - quantity
    - total price
    - complimentary status

    This creates immutable booking history and proper invoice support.

- Reference: This follows the snapshot pricing pattern commonly used in ecommerce and booking systems, where transactional records preserve historical pricing data even if master records change later.

## Local Development

1. Configure your local database connection in `appsettings.Development.json` or via `DB_CONNECTION_STRING`.

2. Run the API:

- For development server with development settings:

```bash
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS="http://0.0.0.0:5106" dotnet run --no-launch-profile
```

3. Open Swagger at:

```text
http://localhost:5106/swagger
```

## Docker Deployment

Build the image:

```bash
docker build -t sports-ground-booking-api .
```

Run the container:

```bash
docker run -e DB_CONNECTION_STRING="Server=your-sql-server;Database=SportsGroundBookingDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;Encrypt=False" -p 5107:5107 sports-ground-booking-api
```

> If SQL Server runs in another container, use the database container hostname instead of `localhost`.

### Docker Network Example

```bash
docker network create sports-network
```

Run SQL Server and API containers on the same network:

```bash
docker run --network sports-network ...
```

This allows the API container to resolve:

```bash
sports-db
```

as the SQL Server hostname.

## Configuration

### appsettings.json

Shared settings, including JWT configuration and logging.

### appsettings.Development.json

Local development configuration, including the SQL Server connection string.

### appsettings.Production.json

Production deployment configuration. The app supports overriding the database connection with the environment variable:

```bash
DB_CONNECTION_STRING
```

## API Endpoints

### OpenAPI / Swagger

- `openapi: 3.0.1`
- Title: `Sports Ground Booking API`
- Version: `v1`
- Swagger UI is available at `/swagger` when the API is running.

### Authentication

- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`

### Public API

- `GET /api/v1/grounds`
- `GET /api/v1/grounds/{id}`
- `GET /api/v1/slots`
- `GET /api/v1/slots/ground/{groundId}`
- `GET /api/v1/slots/{id}`

Ground payloads support a `sportType` field so each venue can be categorized as `Cricket`, `Football`, `Badminton`, `Tennis`, `Multi-Sport`, or any custom label you use in your admin panel.

### Addon APIs

- `GET /api/v1/addons`
- `GET /api/v1/addons?sportType=Football`
- `POST /api/v1/addons`

Addon payloads also support a `sportType` field. When `sportType` is passed to the list endpoint, the API returns addons for that sport plus shared `Multi-Sport` addons.

### Admin APIs

Requires `Authorization: Bearer <token>` and role `Admin`.

- `POST /api/v1/grounds`
- `PUT /api/v1/grounds/{id}`
- `DELETE /api/v1/grounds/{id}`
- `POST /api/v1/slots`
- `PUT /api/v1/slots/{id}`
- `DELETE /api/v1/slots/{id}`
- `GET /api/v1/admin/dashboard`

### Booking APIs

Requires authenticated user.

- `POST /api/v1/Booking`
- `GET /api/v1/Booking/my`
- `GET /api/v1/Booking/{id}`
- `DELETE /api/v1/Booking/{id}`
- `GET /api/v1/Booking/availability?groundId={groundId}&date={yyyy-MM-dd}`

### Feedback APIs

- `POST /api/v1/feedback`
- `GET /api/v1/feedback/ground/{groundId}`

### Payment APIs

Requires authenticated user.

- `POST /api/v1/payments/pay/{bookingId}`

### User Profile

- `GET /api/v1/users/me`

## Authentication Header

Include the JWT token in requests:

```http
Authorization: Bearer <token>
```

## Sample Users

- Admin
    - Email: `admin@gmail.com`
    - Password: `admin123`

- User
    - Email: `rabindra@gmail.com`
    - Password: `rabindra`

## Booking Logic

- Slot conflict validation ensures no overlapping bookings for the same ground and time window.
- The booking algorithm supports:
    - `Day` slot bookings
    - `Night` slot bookings
    - `FullDay` bookings, which reserve the entire day and block conflicting day/night slots.
- Bookings are prevented for past dates and when slot availability conflicts with existing reservations.

## Response Model Simplification

- API responses use simplified DTO-based payloads rather than raw EF entities.
- Standard response shape includes `success`, `message`, and `data` where applicable.
- This keeps API output predictable and avoids circular JSON serialization issues.

## Notes

- The app runs EF Core migrations automatically on startup.
- JSON serialization is configured to ignore reference cycles for EF navigation properties.

## Troubleshooting

### Use the correct environment

Run the API in development mode to load `appsettings.Development.json`:

```bash
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS="http://0.0.0.0:5107" dotnet run --no-launch-profile
```

### Docker SQL connectivity

From inside Docker, `localhost` refers to the container itself. Use the database container hostname or network alias.

### Connection string precedence

`DB_CONNECTION_STRING` overrides the configured `ConnectionStrings:DefaultConnection` value.
