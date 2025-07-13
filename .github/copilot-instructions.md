# Copilot Instructions for CYRetailIMS

## Project Overview

CYRetailIMS is a modular .NET solution for a retail inventory management system. It follows clean architecture principles, separating concerns into Application, Domain, Infrastructure, API, and Web layers. The project is designed for maintainability, scalability, and testability.

---

## Project Structure

- `src/Application`: Application services, business logic, and service orchestration
- `src/Domain`: Core domain entities, events, and domain services
- `src/Infrastructure`: Data access, repositories, and external service integrations
- `src/ComponentService.API`: RESTful API layer (controllers, endpoints)
- `src/ComponentService.Web`: ASP.NET Core MVC web application (UI, controllers, views)
- `tests/`: Unit and integration tests for all layers

---

## C# Code Style Guide

- **SDK Version:** .NET 7.0
- **Language Version:** C# 9.0+ (.NET 7)
- **Naming:**
  - Classes, Methods, Properties: `PascalCase`
  - Private fields: `_camelCase`
  - Interfaces: Prefix with `I` (e.g., `IRepository`)
  - Constants: `ALL_CAPS_WITH_UNDERSCORES`
- **File Organization:** One class per file, filename matches class name
- **Namespaces:** Reflect folder structure and layer
- **Dependency Injection:** Use constructor injection for all dependencies
- **Async:** Use `async`/`await` for I/O-bound operations
- **Error Handling:** Use try/catch for external calls, throw custom exceptions for business logic errors
- **DTOs:** Use data annotations for validation
- **Unit Tests:** Place in `tests/` with clear Arrange-Act-Assert structure
- **Comments:** Use XML documentation for public APIs and methods
- **Formatting:** 4 spaces for indentation, no tabs

---

## Copilot Usage Recommendations

- Follow the above naming and organization conventions
- Prefer dependency injection for all services and repositories
- Use DTOs for API input/output models
- Register new services in `ConfigureService.cs` in the appropriate layer
- Place configuration in `appsettings.json` and access via `IConfiguration`
- For new features, add corresponding unit/integration tests in `tests/`

---

## Additional Notes

- All code should be compatible with .NET 5/6
- Avoid business logic in controllers; keep it in Application/Domain layers
- Use repository pattern for data access
- Use ViewModels for passing data to MVC views

---

_This file provides guidance for GitHub Copilot and contributors to generate code consistent with project standards._
