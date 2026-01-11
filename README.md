# NguyenCorpHR Web API

A modern HR Management REST API built with .NET 10.0, following Clean Architecture principles and CQRS pattern. This API provides comprehensive employee, department, position, and salary range management capabilities with enterprise-grade features including JWT authentication, API versioning, and advanced querying support.

## Features

- **Clean Architecture** - Clear separation of concerns with Domain, Application, Infrastructure, and Presentation layers
- **CQRS Pattern** - Command Query Responsibility Segregation using MediatR
- **Repository & Specification Patterns** - Flexible and maintainable data access layer
- **JWT Authentication** - Secure API endpoints with role-based authorization
- **API Versioning** - URL segment and header-based versioning support
- **Advanced Querying** - Pagination, filtering, sorting, and field selection (data shaping)
- **Fluent Validation** - Comprehensive request validation with detailed error messages
- **Swagger/OpenAPI** - Interactive API documentation
- **Structured Logging** - Serilog with file and console outputs
- **Execution Timing** - Built-in performance monitoring and timing headers
- **Comprehensive Testing** - Unit and integration tests across all layers

## Technology Stack

- **.NET 10.0** - Latest .NET framework with C# preview features
- **Entity Framework Core** - ORM for database operations
- **MediatR** - Mediator pattern implementation for CQRS
- **FluentValidation** - Model validation
- **AutoMapper** - Object-to-object mapping
- **Serilog** - Structured logging
- **NSwag** - OpenAPI/Swagger documentation
- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Fluent test assertions

## Architecture

The solution follows Clean Architecture with the following structure:

```
NguyenCorpHR/
├── NguyenCorpHR.Domain/              # Enterprise business rules and entities
├── NguyenCorpHR.Application/         # Application business rules (CQRS, DTOs, interfaces)
├── NguyenCorpHR.Infrastructure.Persistence/  # Data access (EF Core, repositories)
├── NguyenCorpHR.Infrastructure.Shared/       # Shared infrastructure services
├── NguyenCorpHR.WebApi/              # API controllers and middleware
└── Tests/
    ├── NguyenCorpHR.Application.Tests/
    ├── NguyenCorpHR.Infrastructure.Tests/
    └── NguyenCorpHR.WebApi.Tests/
```

### Core Domain Entities

- **Employee** - Employee information including personal details and position
- **Department** - Organizational departments
- **Position** - Job positions linked to departments
- **SalaryRange** - Salary range definitions for positions

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full version) or use in-memory database
- [Visual Studio 2025](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / [Rider](https://www.jetbrains.com/rider/) (optional)
- An STS (Security Token Service) for JWT token generation (or configure your own)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd NguyenCorpHR
```

### 2. Configure Database

Edit `NguyenCorpHR.WebApi/appsettings.json`:

**Option A: Use In-Memory Database** (Quick Start)
```json
{
  "UseInMemoryDatabase": true
}
```

**Option B: Use SQL Server**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NguyenCorpHRDb;Integrated Security=True;MultipleActiveResultSets=True"
  }
}
```

### 3. Configure Authentication

Update the STS (Security Token Service) settings in `appsettings.json`:

```json
{
  "Sts": {
    "ServerUrl": "https://your-sts-server.com",
    "Audience": "your-api-audience"
  }
}
```

### 4. Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
cd NguyenCorpHR.WebApi
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`

## Configuration

### appsettings.json Key Sections

#### Database Configuration
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  }
}
```

#### Authentication & Authorization
```json
{
  "Sts": {
    "ServerUrl": "https://localhost:44310",
    "Audience": "app.api.employeeprofile",
    "ValidIssuer": "optional-explicit-issuer"
  },
  "ApiRoles": {
    "EmployeeRole": "Employee",
    "ManagerRole": "Manager",
    "AdminRole": "HRAdmin"
  }
}
```

#### Execution Timing
```json
{
  "ExecutionTiming": {
    "Enabled": true,
    "IncludeHeader": true,
    "IncludeResultPayload": true,
    "LogTimings": false,
    "HeaderName": "x-execution-time-ms"
  }
}
```

#### CORS Configuration
```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200", "https://your-frontend.com"]
  }
}
```

## API Endpoints

### Employees
- `GET /api/v1/employees` - Get all employees (with filtering, sorting, pagination)
- `GET /api/v1/employees/{id}` - Get employee by ID
- `POST /api/v1/employees` - Create new employee
- `PUT /api/v1/employees/{id}` - Update employee
- `DELETE /api/v1/employees/{id}` - Delete employee
- `POST /api/v1/employees/paged` - Get paginated employees (DataTables support)

### Departments
- `GET /api/v1/departments` - Get all departments
- `GET /api/v1/departments/{id}` - Get department by ID
- `POST /api/v1/departments` - Create new department
- `PUT /api/v1/departments/{id}` - Update department
- `DELETE /api/v1/departments/{id}` - Delete department

### Positions
- `GET /api/v1/positions` - Get all positions
- `GET /api/v1/positions/{id}` - Get position by ID
- `POST /api/v1/positions` - Create new position
- `PUT /api/v1/positions/{id}` - Update position
- `DELETE /api/v1/positions/{id}` - Delete position

### Salary Ranges
- `GET /api/v1/salaryranges` - Get all salary ranges
- `GET /api/v1/salaryranges/{id}` - Get salary range by ID
- `POST /api/v1/salaryranges` - Create new salary range
- `PUT /api/v1/salaryranges/{id}` - Update salary range
- `DELETE /api/v1/salaryranges/{id}` - Delete salary range

### Query Parameters

**Filtering**
```
GET /api/v1/employees?search=john&positionId={guid}
```

**Sorting**
```
GET /api/v1/employees?orderBy=lastName desc,firstName asc
```

**Pagination**
```
GET /api/v1/employees?pageNumber=1&pageSize=10
```

**Field Selection (Data Shaping)**
```
GET /api/v1/employees?fields=id,firstName,lastName,email
```

## Authentication

The API uses JWT Bearer authentication. Include the token in the Authorization header:

```http
Authorization: Bearer {your-jwt-token}
```

### Role-Based Authorization

- **Employee Role** - Access to basic employee information
- **Manager Role** - Access to employee and department management
- **HRAdmin Role** - Full administrative access to all endpoints

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Tests by Project
```bash
# Application layer tests
dotnet test NguyenCorpHR.Application.Tests/NguyenCorpHR.Application.Tests.csproj

# Infrastructure layer tests
dotnet test NguyenCorpHR.Infrastructure.Tests/NguyenCorpHR.Infrastructure.Tests.csproj

# WebApi layer tests
dotnet test NguyenCorpHR.WebApi.Tests/NguyenCorpHR.WebApi.Tests.csproj
```

### Run Specific Test
```bash
dotnet test --filter "FullyQualifiedName~CreateEmployeeCommandHandlerTests"
```

### Code Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Development

### Adding a New Entity

1. **Create Domain Entity** in `NguyenCorpHR.Domain/Entities/`
2. **Add DbSet** to `ApplicationDbContext`
3. **Create Repository Interface** in `Application/Interfaces/Repositories/`
4. **Implement Repository** in `Infrastructure.Persistence/Repositories/`
5. **Create Commands/Queries** in `Application/Features/{Entity}/`
6. **Add Validators** using FluentValidation
7. **Configure Mappings** in `Application/Mappings/GeneralProfile.cs`
8. **Create Controller** in `WebApi/Controllers/v1/`
9. **Write Tests** for all layers

### Code Style

- Follow Clean Architecture principles
- Keep handlers focused and single-purpose
- Use FluentValidation for all command validation
- Return `Result<T>` from all handlers
- Write comprehensive unit tests for business logic

## Logging

Logs are written to:
- **Console** - All log levels
- **Logs/Error/** - Error and Fatal logs (retained for 7 days)
- **Logs/Info/** - Information logs (retained for 7 days)

Configure log levels in `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## Performance Monitoring

The API includes built-in execution timing:
- Response headers include `x-execution-time-ms`
- Result objects contain `ExecutionTimeMs` property
- Configure via `ExecutionTiming` section in appsettings.json

## Health Checks

Health check endpoint available at:
```
GET /health
```

## Swagger/OpenAPI

Interactive API documentation available at:
```
https://localhost:5001/swagger
```

Features:
- Try out API endpoints directly
- View request/response schemas
- JWT Bearer token authentication support

## Project Status

This is an active development project demonstrating Clean Architecture and modern .NET development practices.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

**Fuji Nguyen**
- GitHub: [@workcontrolgit](https://github.com/workcontrolgit)

## Acknowledgments

- Clean Architecture by Robert C. Martin
- CQRS and MediatR pattern
- .NET Community

---

For more detailed development guidance, see [CLAUDE.md](CLAUDE.md).
