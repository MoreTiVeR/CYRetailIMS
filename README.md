# CYRetailIMS
Ying Charoen Retail Inventory Management System

![Current API Version](https://img.shields.io/badge/api_version-3.0.4.2-brightgreen)
![Current WEB Version](https://img.shields.io/badge/web_version-3.0.6.2-brightgreen)

---

## Project Structure

```
CYRetailIMS/
├── src/
│   ├── Application/
│   ├── ComponentService.API/
│   ├── ComponentService.Web/
│   ├── Domain/
│   └── Infrastructure/
├── tests/
│   ├── Application.Test/
│   ├── Domain.Test/
│   └── Integration.Test/
├── .github/
│   └── copilot-instructions.md
├── CYRetailIMS.sln
└── README.md
```

---

## Architecture Diagram

```mermaid
graph TD
    A[ComponentService.Web - UI] -- HTTP --> B[ComponentService.API - REST API]
    B -- Application Services --> C[Application]
    C -- Domain Logic --> D[Domain]
    C -- Data Access --> E[Infrastructure]
    E -- DB/External --> F[Database or External Services]
```

---

## Description
CYRetailIMS is a modular .NET solution for retail inventory management, following clean architecture principles. It separates UI, API, business logic, domain, and infrastructure for maintainability and scalability.

---

## Getting Started
- .NET 7 SDK required
- See each project folder for specific setup instructions

---

## Contributing
- Follow the code style and architecture described in `.github/copilot-instructions.md`
- Add tests for new features in the `tests/` directory

---

## License
This project is licensed under the APPSBOXS License - see the LICENSE.md file for details