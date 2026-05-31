# AI Guidelines

## Purpose

This document defines how the team should use AI tools such as GitHub Copilot, ChatGPT, Claude, or other AI coding assistants in this project.

AI should be used to support development, analysis, documentation, testing, and code review. AI must not replace developer responsibility, code ownership, or final technical decision-making.

---

## Allowed Use Cases

AI may be used for:

- Requirement analysis
- Task breakdown
- User story and acceptance criteria drafting
- Code suggestion
- Refactoring suggestion
- Unit test generation
- Code review assistance
- Documentation drafting
- Debugging support
- Architecture discussion
- CI/CD and deployment checklist preparation

---

## Not Allowed Use Cases

AI must not be used to:

- Generate or expose secrets
- Store passwords, tokens, API keys, or credentials
- Share production data
- Share customer personal data
- Copy sensitive business logic into public AI tools
- Make final security decisions without human review
- Merge code without developer review
- Replace required testing

---

## Sensitive Data Rules

Do not paste the following data into AI tools:

- Passwords
- API keys
- Access tokens
- Refresh tokens
- Connection strings
- Private certificates
- Production logs containing personal data
- Customer names, phone numbers, ID numbers, or payment data
- Internal IP addresses or infrastructure secrets
- Partner credentials
- Payment gateway secrets

Use mock data or sanitized examples instead.

---

## Developer Responsibility

Developers are responsible for:

- Reviewing all AI-generated code
- Verifying business logic
- Running tests
- Checking security impact
- Checking performance impact
- Checking database impact
- Checking backward compatibility
- Ensuring code follows project standards

AI-generated code must be treated as a suggestion, not final code.

---

## Code Review Rules

When AI-generated code is used, reviewers should check:

- Correctness
- Maintainability
- Security
- Test coverage
- Error handling
- Logging safety
- Performance
- Database query efficiency
- API compatibility
- Deployment impact

---

## Testing Requirements

AI-generated code must be tested before merge.

Required tests may include:

- Unit tests
- Integration tests
- API tests
- Regression tests
- Manual verification
- Payment callback duplicate tests
- External API failure tests

Critical business logic must not be merged without test coverage.

---

## Architecture Rules

AI suggestions must follow the project architecture.

General rules:

- Controllers must stay thin
- Business logic must not be placed in Controllers or Views
- Application layer handles use cases
- Domain layer handles business rules
- Infrastructure layer handles database, external APIs, files, queues, and payment gateway clients
- Dependencies must follow Clean Architecture direction

---

## Pull Request Rules

Pull requests containing AI-assisted changes should include:

- Summary of changes
- Business reason
- Files changed
- Test evidence
- Risk assessment
- Deployment impact
- Rollback consideration

---

## Prompt Management

The source prompt for generating Copilot instructions is stored at:

```text
docs/ai/copilot-instructions-prompt.md