# Organization Creation Flow - Implementation Guide

## Overview
This document provides a complete implementation guide for the organization creation flow in the ProperTea platform, following Domain-Driven Design (DDD) principles with .NET 9, Azure Cosmos DB, and Azure Durable Functions.

## Architecture Components

### 1. Shared Infrastructure
- **ProperTea.Contracts**: Shared DTOs, events, and CQRS abstractions
- **ProperTea.Infrastructure**: Custom CQRS implementation, domain building blocks, and Cosmos DB repository base classes

### 2. Domain Services
Each service follows a layered DDD architecture:

#### Organization Service (ProperTea.Organization.Api)
- **Domain Layer**: Organization aggregate root with domain events
- **Application Layer**: Commands, queries, and handlers for organization management
- **Infrastructure Layer**: Cosmos DB repository implementation
- **API Layer**: REST controllers for organization operations

#### UserManagement Service (ProperTea.UserManagement.Api)
- **Domain Layer**: SystemUser aggregate with organization memberships
- **Application Layer**: User creation and organization membership management
- **Infrastructure Layer**: Cosmos DB repository for user data
- **API Layer**: REST controllers for user operations

#### Identity Service (ProperTea.Identity.Api)
- **Domain Layer**: UserIdentity aggregate for authentication
- **Application Layer**: Identity creation and authentication commands
- **Infrastructure Layer**: Cosmos DB repository for credentials
- **API Layer**: REST controllers for identity operations

### 3. Workflow Orchestration
#### Workflow Orchestrator (ProperTea.WorkflowOrchestrator)
- **Azure Durable Functions** for orchestrating the organization creation process
- **OrganizationCreationOrchestrator**: Main orchestrator function coordinating the entire flow
- **Activities**: Individual activity functions calling microservices through the API Gateway

### 4. Backend for Frontend
#### Landlord BFF (ProperTea.Landlord.Bff)
- Entry point for Landlord Portal requests
- Coordinates with Workflow Orchestrator for complex operations
- Provides portal-specific data aggregation and shaping

## Data Flow Implementation

The organization creation flow is implemented as follows:

1. **User Request**: Unauthenticated user submits organization creation request via Landlord Portal
2. **BFF Processing**: Landlord BFF receives request and forwards to Workflow Orchestrator
3. **Orchestration**: Durable Functions orchestrator coordinates:
   - Check if admin user exists (UserManagement Service)
   - Create user if needed (UserManagement Service)
   - Create identity if needed (Identity Service)
   - Create organization (Organization Service)
   - Add user as admin to organization (UserManagement Service)
   - Activate organization (Organization Service)
4. **Response**: Orchestration status and result returned through BFF to portal

## Technical Stack

### Database Strategy
- **Azure Cosmos DB** for all domain data with eventual consistency
- **Shared infrastructure** but separate containers per service:
  - Organizations container for Organization Service
  - Users container for UserManagement Service  
  - Identities container for Identity Service
- **Azure Table Storage** for Workflow Orchestrator state management

### Event-Driven Architecture
- **Domain Events**: Published immediately within transactions
- **Integration Events**: Published via Azure Service Bus (ready for implementation)
- **Custom CQRS**: No external frameworks like MediatR - custom implementation in shared infrastructure

### Infrastructure
- **.NET 9** with preview language features
- **Aspire** for local development (ready for AppHost configuration)
- **Azure emulators** for local development
- **YARP-based API Gateway** (ready for configuration)

## Project Structure

```
src/
├── Shared/
│   ├── ProperTea.Contracts/           # Shared DTOs, events, CQRS abstractions
│   └── ProperTea.Infrastructure/      # CQRS implementation, domain building blocks
├── Organization/
│   └── ProperTea.Organization.Api/    # Organization domain service
├── UserManagement/
│   └── ProperTea.UserManagement.Api/  # User management domain service
├── Identity/
│   └── ProperTea.Identity.Api/        # Identity domain service
├── LandlordPortal/
│   └── ProperTea.Landlord.Bff/        # Landlord portal BFF
└── Orchestration/
    └── ProperTea.WorkflowOrchestrator/ # Durable Functions orchestrator
```

## Next Steps

### 1. Immediate Tasks
- Configure Aspire AppHost to register all services
- Configure API Gateway (YARP) routing rules
- Set up Azure Cosmos DB emulator containers
- Add integration tests using emulators

### 2. Service Discovery & Configuration
- Update AppHost to register services with proper URLs
- Configure service-to-service communication through Gateway
- Set up Azure Service Bus emulator for integration events

### 3. Testing Strategy
- Unit tests for domain logic and application handlers
- Integration tests using Azure emulators
- End-to-end tests for the complete organization creation flow

### 4. Future Enhancements
- Add proper password hashing (BCrypt/Argon2)
- Implement email notifications
- Add data validation and error handling improvements
- Implement integration events for service decoupling

## Configuration Requirements

### Local Development
- Cosmos DB Emulator running on default port (8081)
- Azure Storage Emulator for Durable Functions
- All services configured through Aspire AppHost

### Azure Deployment
- Azure Cosmos DB with containers: Organizations, Users, Identities
- Azure Functions for Workflow Orchestrator
- Azure Container Apps for domain services
- Azure Service Bus for integration events

This implementation provides a solid foundation for the ProperTea platform with proper DDD architecture, CQRS patterns, and microservices design following your specifications.
