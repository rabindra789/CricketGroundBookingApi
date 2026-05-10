# Cricket Ground Booking API

Backend API for a cricket ground booking platform built with ASP.NET Core Web API and SQL Server.

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- Ms SQL Server
- JWT Authentication
- Swagger

## Current Progress

### Completed
- Project setup
- Core domain entities
- Git setup
- Application DB context
- Entity relationships
- Database constraints
- Initial EF core migration
- SQL server integration
- Register API
- Login API
- Password hashing
- Swagger testing

### In Progress
- JWT authentication
- Protected endpoints

### Planned
- Ground APIs
- Booking engine
- Pricing engine
- Payment APIs
- Admin APIs

## Project Structure

```bash
CricketGroundBookingApi/
│
├── Controllers/
├── Data/
├── DTOs/
├── Entities/
├── Enums/
├── Interfaces/
├── Middleware/
├── Services/
├── Validators/
├── Program.cs
└── appsettings.json
```

## Sample Users
```
- Admin:
    - Email:admin@gmail.com
    - Password:admin123
- User:
    - Email:rabindrameher@gmail.com
    - Password:rabindra
```

## EF core circular fix
### Problem

While fetching entities with navigation properties (for example `Ground -> Slots -> Ground`), the APi threw this error:

```text
System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles. Path: $.data.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.Ground.Slots.
```

This happened because EF core navigation properties created circuler references during JSON serilization.

Example: 
Ground
  └── Slots
  └── Ground
  └── Slots

### Solution
Configured JSON serializer to ignore circular references in `Program.cs`.

```csharp
using System.Text.Json.Serialization;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =ReferenceHandler.IgnoreCycles;
    });
```

### References
- [Microsoft Docs: Handle circular references](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-preserve-references?pivots=dotnet-8-0)
- [JSON.Net Self referencing loop detected](https://stackoverflow.com/questions/13510204/json-net-self-referencing-loop-detected)
- [Entity Framework Core `HasConversion` property configuration not used when querying data with postgresql](https://stackoverflow.com/questions/59741856/entity-framework-core-hasconversion-property-configuration-not-used-when-query)