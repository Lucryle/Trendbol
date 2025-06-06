# Trendbol E-Commerce Platform

> **ÖNEMLİ NOT:** Bu proje geliştirme sürecinde SQL Server LocalDB kullanılarak tasarlanmıştır. Ancak, Docker container'ları genellikle Linux tabanlı olduğundan LocalDB ile uyumlu değildir. Bu nedenle, proje Docker ortamında çalıştırılmak istendiğinde veritabanı bağlantı dizisinin (connection string) uygun bir şekilde güncellenmesi gerekmektedir. Proje başlangıcında LocalDB tercih ettiğimiz için, son aşamada Docker’a geçişte bu sorunla karşılaşılmıştır. Zaman kısıtı ve projenin genel işleyişini etkilememesi nedeniyle bu yapı değiştirilmemiş; ancak Docker image başarıyla oluşturulmuş ve çalıştırılabilir durumdadır.

## Overview

# E-Commerce Platform
This project is part of our **Software Development Design and Practice** course and is currently in progress with continuous updates being made each week.

Update: The project is finished 👍

## Project Overview
A general marketplace for e-commerce, allowing users to buy and sell products securely. Features include user authentication, order placement, transaction tracking, order tracking, and email notifications.

## Weekly Progress

### ✅ Week 1: Project Setup & GitHub Initialization
**Tasks Completed:**
- Created a GitHub repository for version control.
- Set up Git on local machines and connected it to GitHub.
- Established a branch workflow for collaborative development.
- Added an initial `README.md` file to document project progress.

### ✅ Week 2: Object-Oriented Design & UML Modeling
**Tasks Completed:**
- Identified key project entities: `User`, `Product`, `Order`.
- Designed a UML class diagram using draw.io.
- Defined class structures in C# (User.cs, Product.cs, Order.cs).
- Pushed UML diagram and class skeletons to GitHub.

### ✅ Week 3: Implementing Business Logic (OOP & SOLID)
**Tasks Completed:**
- Implemented Repository Pattern for data access.
- Created Service Layer for business logic.
- Applied SOLID principles throughout the codebase.
- Set up Dependency Injection for better maintainability.
- Created interfaces for repositories and services.

### ✅ Week 4: APIs & Database Integration
**Tasks Completed:**
- Integrated Entity Framework Core for database operations.
- Created API Controllers for all entities.
- Implemented CRUD operations for all endpoints.
- Set up database migrations and initial seed data.

### ✅ Week 5: Implementing Design Patterns
**Tasks Completed:**
- Implemented Factory Pattern for product creation.
- Improved code organization and maintainability.

### ✅ Week 6: Git Collaboration & Pull Requests Reviews
**Tasks Completed:**
- Collaborated on Git, reviewed pull requests.
- Enhanced team collaboration and code quality.

### ✅ Week 7: Authentication & Security
**Tasks Completed:**
- JWT (JSON Web Token) authentication system added.
- Email verification system added.
- Only necessary endpoints shown in Swagger.
- Code cleanup and optimizations performed.

### ✅ Week 8: Unit Testing & Test-Driven Development
**Tasks Completed:**
- Basic unit tests were written for UserService, ProductService and OrderService.
- Tests were developed using xUnit and Moq.
- Test project was created under `UnitTests` folder in the main directory.
- All tests were run successfully and basic business logic was secured.

### ✅ Week 9: CI/CD & Automated Deployment
**Tasks Completed:**
- Set up GitHub Actions workflow for automated CI/CD
- Implemented Docker containerization
- Created Dockerfile for the application
- Configured Docker Hub integration
- Successfully built and pushed Docker image
- Added automated testing in the pipeline
- Implemented build and push stages
- Added Docker login and verification steps
- Configured environment variables for Docker
- Set up automated deployment process

### ✅ Week 10: Code Review & Refactoring
**Tasks Completed:**
- Code quality and readability improvements made
- Unnecessary code duplications removed

### ✅ Week 11: Containerization
**Tasks Completed:**
- Docker containerization implemented
- Docker Hub integration completed
- Dockerfile created and tested

### ✅ Week 12: Final Debugging & Documentation
**Tasks Completed:**
- Final debugging of all features and components
- Comprehensive documentation updates
- Performance testing and optimization
- Security review and enhancements
- Final code cleanup and refactoring
- Project documentation completion
- User guide and API documentation updates
- Final testing and validation

## CI/CD Pipeline

Our project uses GitHub Actions for automated build, test, and deployment processes. The pipeline includes the following steps:

1. **Build**: Project is compiled and dependencies are checked
2. **Test**: Unit tests are executed
3. **Publish**: Application is published
4. **Docker**: Docker image is created and pushed to Docker Hub

### Pipeline Triggers

The pipeline automatically runs when:
- Code is pushed to `main` branch
- Code is pushed to `backend` branch
- Pull request is opened to `main` branch
- Pull request is opened to `backend` branch

### Docker Image Usage

```bash
docker pull lucryle/trendbol-api:latest

docker run -p 5000:80 lucryle/trendbol-api:latest
```

## Project Structure
```
Trendbol/
├── .github/                    # GitHub Actions workflows
│   └── workflows/
│       └── main.yml         # CI/CD pipeline configuration
│
├── TrendbolAPI/               # Main project (backend)
│   ├── Controllers/          # API endpoints
│   │   ├── AuthController.cs
│   │   ├── ProductController.cs
│   │   ├── UserController.cs
│   │   └── OrderController.cs
│   │
│   ├── Models/               # Data models
│   │   ├── Auth/            # Authentication models (Was lazy to implement DTO's)
│   │   │   ├── EmailRequest.cs           # Email request model 
│   │   │   ├── ConfirmEmailRequest.cs    # Email confirmation model
│   │   │   ├── VerifyRegisterRequest.cs  # Registration verification model
│   │   │   ├── LoginRequest.cs           # Login request model
│   │   │   └── RegisterRequest.cs        # Registration request model
│   │   │
│   │   └── Entities/        # Database entities
│   │       ├── Product.cs    # Product entity
│   │       ├── User.cs      # User entity
│   │       └── Order.cs     # Order entity
│   │
│   ├── Services/            # Business logic layer
│   │   ├── Implementations/
│   │   │   ├── EmailService.cs
│   │   │   ├── JwtService.cs
│   │   │   ├── OrderService.cs
│   │   │   ├── ProductService.cs
│   │   │   └── UserService.cs
│   │   └── Interfaces/
│   │       ├── IEmailService.cs
│   │       ├── IJwtService.cs
│   │       ├── IOrderService.cs
│   │       ├── IProductService.cs
│   │       └── IUserService.cs
│   │
│   ├── Repositories/        # Data access layer
│   │   ├── Implementations/
│   │   │   ├── OrderRepository.cs
│   │   │   ├── ProductRepository.cs
│   │   │   └── UserRepository.cs
│   │   └── Interfaces/
│   │       ├── IOrderRepository.cs
│   │       ├── IProductRepository.cs
│   │       └── IUserRepository.cs
│   │
│   ├── Data/               # Database configuration
│   │   └── TrendbolContext.cs
│   │
│   ├── Factories/          # Factory pattern implementation
│   │   └── ProductFactory.cs
│   │
│   ├── Migrations/         # Entity Framework migrations
│   │
│   ├── Properties/         # Project properties
│   │   └── launchSettings.json
│   │
│   ├── Program.cs         # Application entry point
│   ├── appsettings.json   # Application configuration
│   ├── appsettings.Development.json  # Development configuration
│   ├── appsettings.Example.json      # Example configuration
│   └── TrendbolAPI.csproj # Project file
│
├── UnitTests/              # Unit test project
│   ├── OrderServiceTests.cs    # Order service unit tests
│   ├── ProductServiceTests.cs  # Product service unit tests
│   ├── UserServiceTests.cs     # User service unit tests
│   └── UnitTests.csproj        # Test project file
│
├── docs/                  # Project documentation
│   └── Software Development Design and Practice Project Proposal.pdf  # Project proposal document
│
├── .gitignore            # Git ignore file
├── Dockerfile            # Docker configuration
└── README.md            # Project documentation

## Technologies Used
- .NET 7.0
- Entity Framework Core
- SQL Server Express LocalDB (Development)
- Swagger/OpenAPI
- JWT Authentication (Json Web Token)
- Docker
- GitHub Actions
- xUnit (Test framework)

## Hassas Bilgilerin Ayarlanması

Projeyi çalıştırmak için aşağıdaki hassas bilgileri ayarlamanız gerekmektedir:

1. User Secrets'ı etkinleştir:
```bash
dotnet user-secrets init --project TrendbolAPI
```

2. Gerekli secrets'ları ayarla:
```bash
dotnet user-secrets set "EmailSettings:SmtpUsername" "YOUR_EMAIL" --project TrendbolAPI
dotnet user-secrets set "EmailSettings:SmtpPassword" "YOUR_APP_PASSWORD" --project TrendbolAPI
dotnet user-secrets set "EmailSettings:FromEmail" "YOUR_EMAIL" --project TrendbolAPI
dotnet user-secrets set "JwtSettings:SecretKey" "YOUR_JWT_SECRET_KEY" --project TrendbolAPI
```

Not: Gmail kullanıyorsanız, "Uygulama Şifreleri" oluşturmanız gerekecektir. Bunun için:
1. Google Hesabınıza gidin
2. Güvenlik > 2 Adımlı Doğrulama'yı açın
3. Uygulama Şifreleri'ne gidin
4. Yeni bir uygulama şifresi oluşturun

Bu şifre SmtpPassword yerine geçer.
