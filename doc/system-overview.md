# System Overview

Microservices architecture using .NET Aspire deployed to Azure Container Apps with shared infrastructure services.

# Folder Structure

- src/
  - Identity/
    - ProperTea.Identity.Api/
  - LandlordPortal/
    - ProperTea.Landlord.Bff/
  - Orchestration/
    - ProperTea.WorkflowOrchestrator/
  - Organization/
    - ProperTea.Organization.Api/
  - ProperTea.AppHost/
  - ProperTea.Gateway/
  - Shared/
    - ProperTea.ServiceDefaults/
  - UserManagement/
    - ProperTea.UserManagement.Api/

# Services
- ProperTea.Gateway: API Gateway for authentication and routing
- ProperTea.UserManagement.Api: User roles, permissions, and organization membership
- ProperTea.Identity.Api: Authentication and user credentials
- ProperTea.WorkflowOrchestrator: Multi-step business process orchestration
- ProperTea.Organization.Api: Organizations (multitenancy), profiles, settings, memberships
- ProperTea.Landlord.Bff: Backend-for-Frontend for Landlord Portal
- ProperTea.AppHost: Aspire AppHost
- ProperTea.ServiceDefaults: Shared infrastructure code

# Interservice Communication Strategy
- REST APIs with standardized error handling
- gRPC for high-performance internal service communication
- Circuit breakers and retries implemented via Polly

# Asynchronous Communication
- Azure Service Bus for reliable message delivery
- Event-driven architecture for process coordination
- Message schemas version-controlled in shared packages

# Workflow Implementation
- Azure Durable Functions for complex orchestrations
- Saga pattern with compensation actions for distributed transactions
- Event sourcing for critical business processes

# Technical Stack
- API Gateway: ASP.NET Core 8, YARP, Azure AD B2C integration
- Domain Services: .NET 9 with Aspire, Minimal API, EF Core 9, MediatR for CQRS
- Workflow Orchestrator: Azure Durable Functions, Azure Table Storage, Compensation patterns
- Shared Infrastructure: Application Insights, Azure Key Vault, Azure Container Registry
