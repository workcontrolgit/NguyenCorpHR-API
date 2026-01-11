# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NguyenCorpHR is an HR management API built using Clean Architecture principles with .NET 10.0. The solution follows CQRS pattern using MediatR, implements repository pattern with specification pattern, and provides a RESTful API for managing employees, departments, positions, and salary ranges.

## Solution Structure

The solution follows Clean Architecture with clear separation of concerns:

```
/src/Core/
  - NguyenCorpHR.Domain          # Domain entities, enums
  - NguyenCorpHR.Application     # Business logic, CQRS handlers, DTOs, specifications

/src/Infrastructure/
  - NguyenCorpHR.Infrastructure.Persistence  # EF Core, repositories, DbContext
  - NguyenCorpHR.Infrastructure.Shared       # Shared services (email, datetime)

/src/Presentation/
  - NguyenCorpHR.WebApi         # Controllers, middleware, filters

/tests/
  - NguyenCorpHR.Application.Tests
  - NguyenCorpHR.Infrastructure.Tests
  - NguyenCorpHR.WebApi.Tests
```

## Build and Run Commands

### Build the solution
```bash
dotnet build
```

### Run the Web API (from WebApi directory)
```bash
cd NguyenCorpHR.WebApi
dotnet run
```

### Run all tests
```bash
dotnet test
```

### Run tests for a specific project
```bash
dotnet test NguyenCorpHR.Application.Tests/NguyenCorpHR.Application.Tests.csproj
```

### Run a specific test
```bash
dotnet test --filter "FullyQualifiedName~CreateEmployeeCommandHandlerTests"
```

### Restore packages
```bash
dotnet restore
```

## Architecture Patterns

### CQRS with MediatR

All business logic is organized using the CQRS pattern:
- **Commands**: Located in `Application/Features/{Entity}/Commands/{CommandName}/`
  - Each command has its own file with the command class and handler in the same file
  - Example: `CreateEmployeeCommand` and `CreateEmployeeCommandHandler` in the same file
- **Queries**: Located in `Application/Features/{Entity}/Queries/{QueryName}/`
  - Each query has its own file with the query class and handler in the same file
  - Example: `GetEmployeesQuery` and `GetEmployeesQueryHandler` in the same file

Controllers use the Mediator pattern to send commands/queries. All controllers inherit from `BaseApiController` which provides access to `IMediator` through the `Mediator` property.

### Specification Pattern

The application uses the Specification pattern for complex query logic:
- Base implementation: `Application/Specifications/BaseSpecification.cs`
- Entity-specific specifications: `Application/Specifications/{Entity}/{Entity}Specifications.cs`
- Specifications are evaluated by `SpecificationEvaluator` in the Infrastructure.Persistence layer
- Supports criteria, includes, pagination, and ordering

### Repository Pattern

- Generic repository: `IGenericRepositoryAsync<T>` provides basic CRUD operations
- Entity-specific repositories extend the generic repository for specialized queries
- All repositories are registered automatically using Scrutor in `ServiceRegistration.AddPersistenceInfrastructure()`

### Result Pattern

The application uses a custom Result pattern for operation outcomes:
- Located in `Application/Common/Results/Result.cs`
- `Result` for operations without return values
- `Result<T>` for operations returning data
- Contains `IsSuccess`, `IsFailure`, `Message`, `Errors`, and `ExecutionTimeMs` properties
- Use `Result.Success()` or `Result.Failure()` factory methods

### DTO Organization

DTOs are organized following the plan in `Application/Documentation/DTO-Structure.md`:
- Feature-specific DTOs live in `Features/{FeatureName}/DTOs/`
- Commands and Queries may compose DTOs from the shared folder
- Example: `Features/Positions/DTOs/PositionSummaryDto.cs`

## Key Technologies

- **.NET 10.0** with C# preview features
- **Entity Framework Core** for data access
- **MediatR** for CQRS implementation
- **FluentValidation** for validation (integrated via `ValidationBehavior` pipeline)
- **AutoMapper** for object mapping
- **Serilog** for logging
- **NSwag** for OpenAPI/Swagger documentation
- **JWT Bearer Authentication** via external STS (Security Token Service)
- **xUnit, Moq, FluentAssertions** for testing

## Database Configuration

The application supports both SQL Server and in-memory database:
- Set `UseInMemoryDatabase: true` in appsettings.json for in-memory mode
- SQL Server connection string: `ConnectionStrings:DefaultConnection`
- Database is auto-created and seeded in Development environment via `DbInitializer.SeedData()`

## Authentication and Authorization

- JWT Bearer tokens issued by external STS at `Sts:ServerUrl`
- Three role-based policies defined in `AuthorizationConsts`:
  - `AdminPolicy`: HRAdmin only
  - `ManagerPolicy`: Manager and HRAdmin
  - `EmployeePolicy`: Employee, Manager, and HRAdmin
- Roles are configured in appsettings.json under `ApiRoles`

## API Versioning

- URL segment versioning: `/api/v{version}/{controller}`
- Default version: 1.0
- Controllers use `[ApiVersion("1.0")]` attribute

## Middleware and Filters

- `ErrorHandlerMiddleware`: Global error handling
- `RequestTimingMiddleware`: Request timing tracking
- `ExecutionTimeResultFilter`: Adds execution time to Result objects and response headers
- Execution timing can be configured via `ExecutionTiming` section in appsettings.json

## Data Shaping

The application supports data shaping via `IDataShapeHelper<T>`:
- Registered for each entity type (Employee, Department, Position, SalaryRange)
- Allows clients to specify which fields to return
- Implemented in `Application/Helpers/DataShapeHelper.cs`

## Testing Approach

- **Application.Tests**: Test MediatR command/query handlers using mocked repositories
- **Infrastructure.Tests**: Test repository implementations with in-memory DbContext
- **WebApi.Tests**: Test controllers and API endpoints
- All test projects use xUnit, Moq, and FluentAssertions

## Important Development Notes

### Adding a New Feature

1. Define domain entity in `Domain/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create repository interface in `Application/Interfaces/Repositories/`
4. Implement repository in `Infrastructure.Persistence/Repositories/`
5. Create CQRS commands/queries in `Application/Features/{Entity}/`
6. Add validators using FluentValidation
7. Configure AutoMapper mappings in `Application/Mappings/GeneralProfile.cs`
8. Create controller in `WebApi/Controllers/v{version}/`
9. Add tests for each layer

### Validation

- FluentValidation validators are automatically discovered and registered
- `ValidationBehavior<,>` pipeline behavior validates requests before handlers execute
- Validation failures throw `ValidationException` which is handled by `ErrorHandlerMiddleware`

### Service Registration

- Application layer: `ServiceExtensions.AddApplicationLayer()`
- Persistence: `ServiceRegistration.AddPersistenceInfrastructure()`
- Shared infrastructure: registered via `AddSharedInfrastructure()`
- All use Scrutor for assembly scanning and automatic registration

### Logging

- Serilog configured to log to Console and File
- Separate log files for different levels: `Logs/Error/` and `Logs/Info/`
- Request logging via `UseSerilogRequestLogging()` middleware

## Configuration Sections

Key appsettings.json sections:
- `UseInMemoryDatabase`: Toggle between SQL Server and in-memory database
- `ConnectionStrings:DefaultConnection`: SQL Server connection string
- `Sts:ServerUrl` and `Sts:Audience`: JWT authentication configuration
- `ApiRoles`: Role name mappings
- `ExecutionTiming`: Execution time tracking configuration
- `Serilog`: Logging configuration
- `MailSettings`: SMTP configuration for email service
