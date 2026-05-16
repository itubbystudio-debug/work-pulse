# WorkPulse Backend

This solution defines the backend foundation for WorkPulse using .NET 10 Web API, Clean Architecture, CQRS with MediatR, SQL Server, EF Core, Dapper, and ClosedXML.

Frontend work in scope is standardized on Angular as the primary framework, PrimeNG as the required UI component library, and Sakai Template as the baseline UI template. Non-approved UI libraries require explicit approval, and mixed legacy frontend modules require a confirmed migration or exception approach before implementation.

## Architecture

- `WorkPulse.Domain`: domain primitives, entities, value objects, events, and domain exceptions. This project has no external infrastructure dependencies.
- `WorkPulse.Application`: CQRS requests and handlers, pipeline behaviors, DTOs, validation, and abstractions for persistence, SQL connections, reports, cache, current user, and time.
- `WorkPulse.Infrastructure`: EF Core SQL Server persistence, Dapper connection factory, ClosedXML report generation, and service implementations.
- `WorkPulse.Api`: controllers, middleware, API composition, Swagger/OpenAPI, and RFC 7807 problem responses.

Dependency direction is API -> Infrastructure -> Application -> Domain.
