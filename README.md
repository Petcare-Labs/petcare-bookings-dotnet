# PetCare Bookings — .NET

A small production-style booking API for a fictional pet care platform.

The goal of this project is to demonstrate backend development with .NET, relational data modeling, application architecture, testing, and basic event-driven workflows.

### Project Overview

PetCare Bookings manages customers, pets, service providers, and bookings.

A customer can register pets and create bookings with available providers. The service is responsible for validating bookings and preventing invalid or conflicting requests.

---

### Planned Features

- [ ] Create and manage customers
- [ ] Create and manage pets
- [ ] Create and manage service providers
- [ ] Create, update, and cancel bookings
- [ ] Track booking status
- [ ] Validate booking requests
- [ ] Publish events when booking status changes
- [ ] Seed sample data for local development

### Example Booking Flow
```
Customer creates booking
        ↓
Booking is validated
        ↓
Booking is stored
        ↓
BookingCreated event is published
        ↓
Follow-up processing occurs
```

### Tech Stack
- C#
- .NET
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- FluentValidation
- xUnit
- Testcontainers
- Docker
- GitHub Actions

RabbitMQ may be added for asynchronous messaging.

### Possible Domain Model
```
Customer
 └── Pets

Provider
 └── Availability

Booking
 ├── Customer
 ├── Pet
 ├── Provider
 ├── Service Type
 ├── Start Time
 ├── End Time
 └── Status
 ```

### Initial API
```
GET    /api/customers
POST   /api/customers

GET    /api/pets/{id}
POST   /api/pets

GET    /api/providers
POST   /api/providers

GET    /api/bookings/{id}
POST   /api/bookings
PUT    /api/bookings/{id}
DELETE /api/bookings/{id}
```

### Project Goals

This project is intended to demonstrate:

- REST API design
- Clean and maintainable C# code
- Dependency injection
- Relational database design
- Validation
- Integration testing
- Event-driven application patterns
- Dockerized local development

### Running Locally

Eventually the application should be runnable with:

`docker compose up`

Additional setup instructions will be added as the project develops.