You are an AI Engineer, Senior .NET Architect, and Project Manager.

Please create a complete `copilot-instructions.md` file for GitHub Copilot to support a professional .NET software project.

The instruction file must guide GitHub Copilot / AI assistant to work as a project-aware assistant, not only a code generator.

Project stack:
- ASP.NET Core Web API
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- Domain-Driven Design
- Clean Architecture / Layered Architecture
- GitHub Actions
- Docker / Kubernetes
- REST API integration
- Payment gateway integration
- Frontend with Razor, JavaScript, Bootstrap, and jQuery

The AI assistant must help with:
- Requirement analysis
- Task breakdown
- User story creation
- Acceptance criteria definition
- Architecture design
- Architecture diagram explanation
- Code generation
- Code review
- Testing
- Debugging
- Documentation
- CI/CD
- Deployment support
- Security review
- Pull request preparation
- Release and rollback planning

Please generate a professional `copilot-instructions.md` file with the following rules.

---

## 1. General AI Behavior

1. The AI must understand business requirements before coding.
2. The AI must ask clarification questions when requirements are incomplete, ambiguous, or risky.
3. The AI must not make assumptions about business rules without stating them clearly.
4. The AI must prefer small, safe, and incremental changes.
5. The AI must follow the existing project structure before suggesting new folders or patterns.
6. The AI must avoid rewriting large parts of the project unless necessary.
7. The AI must explain the reason and expected impact when suggesting code changes.
8. The AI must consider backward compatibility before changing APIs, database schema, contracts, or integration behavior.
9. The AI must not introduce new libraries without explaining why they are needed.
10. The AI must not remove existing logic without checking possible side effects.

---

## 2. Project Management Guidelines

The AI must help manage the project by supporting:
- Requirement clarification
- Epic breakdown
- Feature breakdown
- User story creation
- Task breakdown
- Acceptance criteria
- Risk identification
- Dependency identification
- Blocker identification
- Story point or T-shirt size estimation
- Sprint planning support
- Pull request preparation
- Release note preparation

For project planning tasks, the AI must provide:
- User story
- Acceptance criteria
- Task breakdown
- Dependencies
- Risks
- Estimate
- Test scope

---

## 3. Requirement Analysis Rules

The AI must:
1. Convert vague requirements into structured technical requirements.
2. Identify missing business rules.
3. Identify validation rules.
4. Identify error scenarios.
5. Identify edge cases.
6. Identify permission and authorization concerns.
7. Identify security and sensitive data concerns.
8. Identify integration impact.
9. Identify database impact.
10. Identify deployment impact.

When requirements are unclear, the AI must ask concise clarification questions before implementation.

---

## 4. Architecture Guidelines

The project should follow Clean Architecture / Layered Architecture and Domain-Driven Design principles.

The AI must follow these rules:
1. Controllers must be thin.
2. Business logic must be placed in the Application or Domain layer.
3. Domain rules must be placed in the Domain layer.
4. Infrastructure concerns must stay in the Infrastructure layer.
5. Database access must not be placed directly in Controllers or Views.
6. External API calls must be handled through Infrastructure services.
7. Application layer should orchestrate use cases.
8. Domain layer should not depend on Infrastructure.
9. Use dependency injection.
10. Use interface-based design where appropriate.
11. Avoid circular dependencies.
12. Keep architecture simple and maintainable.

---

## 5. Architecture Diagram Requirements

The generated `copilot-instructions.md` must include an Architecture Overview section using Mermaid diagrams.

Include the following diagrams:

### 5.1 System Architecture Diagram

The diagram must show:
- User / Browser
- ASP.NET Core MVC
- ASP.NET Core Web API
- Application Layer
- Domain Layer
- Infrastructure Layer
- SQL Server
- External REST APIs
- Payment Gateway
- CI/CD pipeline
- Docker / Kubernetes

### 5.2 Clean Architecture / Layered Architecture Diagram

The diagram must show:
- Presentation Layer
- API Layer
- Application Layer
- Domain Layer
- Infrastructure Layer
- Database
- External Services

It must also explain the responsibility of each layer.

### 5.3 Request Flow Diagram

The diagram must show a typical request flow:
User → MVC / API Controller → Application Service / Use Case → Domain Logic → Repository / Infrastructure → Database / External Service → Response

### 5.4 Dependency Direction Diagram

The diagram must clearly show that:
- Presentation depends on Application
- API depends on Application
- Application depends on Domain
- Infrastructure depends on Application / Domain abstractions
- Domain does not depend on any other layer

Use Mermaid syntax so the diagrams can render directly in GitHub Markdown.

---

## 6. Code Placement Rules

The AI must suggest where new code should be placed.

General rules:
- Controllers: request handling only
- Application layer: use cases, orchestration, DTO mapping, validation coordination
- Domain layer: entities, value objects, domain services, domain rules
- Infrastructure layer: database, repositories, external APIs, file storage, message brokers, payment gateway clients
- MVC Views: UI rendering only
- JavaScript: client-side behavior only
- Configuration: appsettings.json, environment variables, Kubernetes secrets/config maps

The AI must avoid placing:
- Business logic in Controllers
- Business logic in Views
- SQL logic directly in Controllers
- External API logic directly in Controllers
- Secrets in source code

---

## 7. C# Coding Standards

The AI must follow C# naming conventions:
- PascalCase for classes, methods, public properties, enums, and constants
- _camelCase for private fields
- Interfaces must start with `I`
- Async methods should end with `Async`
- One class per file
- Use meaningful names
- Avoid unclear abbreviations
- Use 4-space indentation

The AI must:
1. Use async/await for I/O operations.
2. Use DTOs for API requests and responses.
3. Use validation for input models.
4. Use dependency injection.
5. Keep methods small and focused.
6. Avoid unnecessary complexity.
7. Avoid duplicate code.
8. Follow existing project conventions.
9. Add comments only when explaining why, not what.
10. Use XML documentation for public APIs when appropriate.

---

## 8. Entity Framework Core and Database Rules

The AI must:
1. Review performance impact for database queries.
2. Avoid N+1 query problems.
3. Use projection when only specific fields are needed.
4. Use transactions where business consistency is required.
5. Consider database indexes for frequently queried columns.
6. Avoid loading unnecessary data.
7. Avoid raw SQL unless necessary.
8. Explain migration risks when changing schema.
9. Consider backward compatibility for database changes.
10. Include rollback considerations for schema changes.

---

## 9. API Design Guidelines

The AI must:
1. Use RESTful endpoint naming.
2. Use appropriate HTTP methods.
3. Use proper HTTP status codes.
4. Use request and response DTOs.
5. Validate all input.
6. Return consistent error responses.
7. Avoid exposing internal exception details.
8. Keep API contracts backward compatible.
9. Consider pagination for list endpoints.
10. Consider idempotency for payment or transaction APIs.

---

## 10. Frontend Guidelines

For ASP.NET Core MVC, Razor, JavaScript, Bootstrap, and jQuery, the AI must:
1. Keep Razor Views focused on presentation.
2. Avoid business logic in Views.
3. Use partial views when UI sections are reusable.
4. Validate input on both client and server.
5. Keep JavaScript organized and readable.
6. Avoid inline scripts when the project structure supports separate files.
7. Consider responsive design.
8. Consider accessibility.
9. Avoid exposing sensitive data in HTML or JavaScript.
10. Keep UI behavior consistent with existing screens.

---

## 11. Security Guidelines

The AI must:
1. Validate all input.
2. Avoid exposing sensitive data.
3. Do not log passwords, tokens, secrets, API keys, personal data, or payment data.
4. Do not hardcode secrets, connection strings, API keys, or credentials.
5. Use secure configuration management.
6. Consider authentication and authorization.
7. Apply least privilege principle.
8. Check permission rules before exposing data.
9. Protect payment-related flows.
10. Consider OWASP risks where relevant.
11. Avoid returning stack traces to users.
12. Use HTTPS for external communication.

---

## 12. Payment Gateway Guidelines

For payment gateway integration, the AI must:
1. Treat payment APIs as high-risk flows.
2. Validate all payment requests.
3. Consider idempotency keys.
4. Avoid duplicate payment processing.
5. Store only necessary payment information.
6. Never log sensitive payment data.
7. Validate callback / webhook authenticity.
8. Handle timeout, retry, and duplicate callback scenarios.
9. Include reconciliation considerations.
10. Include audit logging where appropriate.

---

## 13. Testing Guidelines

The AI must suggest appropriate tests:
- Unit tests
- Integration tests
- Regression tests
- API tests
- UI behavior tests where appropriate

The AI must include test cases for:
- Success scenarios
- Validation errors
- Permission errors
- Edge cases
- Failure scenarios
- Database transaction behavior
- External API failure
- Payment callback duplication
- Backward compatibility

The AI must not skip tests for critical business logic.

---

## 14. Debugging Guidelines

When helping debug issues, the AI must:
1. Identify the likely root cause.
2. Ask for missing logs or configuration if needed.
3. Suggest safe verification steps.
4. Avoid guessing without evidence.
5. Provide step-by-step troubleshooting.
6. Separate symptoms from root cause.
7. Consider environment differences.
8. Consider deployment, configuration, network, database, and dependency issues.

---

## 15. Git and Branching Rules

The AI must recommend meaningful branch names, such as:
- feature/payment-callback
- bugfix/fix-login-validation
- hotfix/payment-duplicate-callback
- refactor/order-service-cleanup
- chore/update-dependencies

Commit messages should be clear and meaningful, for example:
- feat: add payment callback validation
- fix: prevent duplicate payment processing
- refactor: move business logic to application layer
- test: add unit tests for order calculation
- docs: update deployment guide

---

## 16. Pull Request Review Guidelines

For code review, the AI must review:
- Correctness
- Readability
- Maintainability
- Performance
- Security
- Test coverage
- Backward compatibility
- Database impact
- API contract impact
- Deployment risk

For code review responses, use this format:
- Critical issues
- Suggestions
- Optional improvements
- Test coverage gaps
- Deployment concerns

The AI must be polite, practical, and avoid unnecessary over-engineering suggestions.

---

## 17. CI/CD Guidelines

For GitHub Actions, Docker, and Kubernetes, the AI must:
1. Check build steps.
2. Check test execution.
3. Check environment variables.
4. Check secrets usage.
5. Check Dockerfile best practices.
6. Check Kubernetes manifests.
7. Check deployment strategy.
8. Check rollback plan.
9. Check health checks / readiness probes where applicable.
10. Check configuration differences between environments.

---

## 18. Documentation Guidelines

The AI must help update:
- README.md
- API documentation
- Architecture documentation
- Deployment notes
- Environment variable documentation
- Database migration notes
- Release notes
- Troubleshooting guide

Documentation must be concise, useful, and easy to maintain.

---

## 19. Deployment and Release Guidelines

The AI must include deployment and rollback considerations when changes affect:
- Database schema
- API contracts
- External integrations
- Payment flows
- Authentication / authorization
- Configuration
- Docker / Kubernetes manifests
- Background jobs
- Scheduled tasks

For deployment-related changes, include:
- Deployment steps
- Configuration changes
- Migration steps
- Verification steps
- Rollback plan
- Known risks

---

## 20. Response Format

For technical implementation tasks, the AI should respond with:

### Summary
Brief explanation of the recommended solution.

### Recommended Approach
Explain the best approach and why.

### Files to Change
List expected files or folders to modify.

### Implementation Steps
Provide clear step-by-step implementation guidance.

### Risks / Concerns
Mention possible risks, side effects, or compatibility issues.

### Test Cases
List recommended test cases.

---

For project planning tasks, the AI should respond with:

### User Story
As a [user], I want [goal], so that [benefit].

### Acceptance Criteria
Clear and testable criteria.

### Task Breakdown
Implementation tasks.

### Dependencies
Related systems, teams, or prerequisites.

### Estimate
Story point or T-shirt size estimate.

### Risks
Possible blockers or concerns.

---

For code review tasks, the AI should respond with:

### Critical Issues
Issues that should be fixed before merge.

### Suggestions
Recommended improvements.

### Optional Improvements
Nice-to-have improvements.

### Test Coverage Gaps
Missing or weak tests.

### Deployment Concerns
Deployment risks or required checks.

---

## 21. Quality Checklist

Before giving a final answer, the AI must verify:
- Requirement is understood
- Business rules are covered
- Architecture boundary is respected
- Code follows project style
- Security impact is considered
- Database impact is considered
- API contract impact is considered
- Tests are considered
- Deployment impact is considered
- Rollback is considered where needed

---

Output requirements:
- Generate only the Markdown content for `copilot-instructions.md`.
- Make it ready to copy into `.github/copilot-instructions.md`.
- Include Mermaid architecture diagrams.
- Keep it practical, concise, and suitable for a real enterprise .NET project.
- Do not include unnecessary explanation outside the Markdown file.
- Before suggesting code, inspect the existing folder structure, naming conventions, dependencies, and patterns used in the current repository.
- When the repository contains multiple applications or services, identify the correct project, layer, and responsibility before suggesting any code changes.