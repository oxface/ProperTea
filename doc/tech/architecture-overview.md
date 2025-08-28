# Architecture Overview

This document provides a high-level technical overview of the ProperTea platform architecture. It includes:

- System Context Diagram
- Project Structure
- Network Flow and Gateway Placement

Each diagram is accompanied by a textual explanation. All diagrams are maintained in Mermaid format for easy updates and integration with documentation tools.

---

## 1. System Context Diagram

See [system-context.mmd](system-context.mmd) for the Mermaid source.

```mermaid
%% System Context Diagram
flowchart LR
    subgraph Portals
        LandlordPortal["Landlord Portal"]
        TenantPortal["Tenant Portal"]
        ListingPortal["Listing Portal"]
        SupportPortal["Support Portal"]
    end
    subgraph BFFs
        ProperTea.Landlord.Bff["ProperTea.Landlord.Bff"]
        ProperTea.Tenant.Bff["ProperTea.Tenant.Bff"]
        ProperTea.Listing.Bff["ProperTea.Listing.Bff"]
        ProperTea.Support.Bff["ProperTea.Support.Bff"]
    end
    ProperTea.Gateway["ProperTea.Gateway (API Gateway, YARP)"]
    subgraph CoreServices
        ProperTea.UserManagement.Api["ProperTea.UserManagement.Api"]
        ProperTea.Identity.Api["ProperTea.Identity.Api"]
        ProperTea.WorkflowOrchestrator["ProperTea.WorkflowOrchestrator"]
        ProperTea.Organization.Api["ProperTea.Organization.Api"]
        ProperTea.Property.Api["ProperTea.Property.Api"]
        ProperTea.RentalObject.Api["ProperTea.RentalObject.Api"]
        ProperTea.RentalContract.Api["ProperTea.RentalContract.Api"]
        ProperTea.Vacancies.Api["ProperTea.Vacancies.Api"]
        ProperTea.Tenant.Api["ProperTea.Tenant.Api"]
        ProperTea.Application.Api["ProperTea.Application.Api"]
    end
    subgraph Shared
        ProperTea.ServiceDefaults["ProperTea.ServiceDefaults"]
    end
    LandlordPortal-->|REST/gRPC|ProperTea.Landlord.Bff
    TenantPortal-->|REST/gRPC|ProperTea.Tenant.Bff
    ListingPortal-->|REST/gRPC|ProperTea.Listing.Bff
    SupportPortal-->|REST/gRPC|ProperTea.Support.Bff
    ProperTea.Landlord.Bff-->|REST/gRPC|ProperTea.Gateway
    ProperTea.Tenant.Bff-->|REST/gRPC|ProperTea.Gateway
    ProperTea.Listing.Bff-->|REST/gRPC|ProperTea.Gateway
    ProperTea.Support.Bff-->|REST/gRPC|ProperTea.Gateway
    ProperTea.Gateway-->|REST/gRPC|ProperTea.UserManagement.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Identity.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.WorkflowOrchestrator
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Organization.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Property.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.RentalObject.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.RentalContract.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Vacancies.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Tenant.Api
    ProperTea.Gateway-->|REST/gRPC|ProperTea.Application.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Organization.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.UserManagement.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Identity.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Property.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.RentalObject.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.RentalContract.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Vacancies.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Tenant.Api
    ProperTea.WorkflowOrchestrator-->|Events/REST|ProperTea.Application.Api
```

### Explanation
- **Portals**: User-facing web/mobile apps for different user roles.
- **BFFs**: Backend-for-Frontend services, one per portal, aggregate and shape data for their portal.
- **API Gateway**: Single entry point for all requests from BFFs, handles routing, authentication, and cross-cutting concerns. BFFs do not call services directly, but always go through the Gateway.
- **Core Services**: Microservices implementing business logic and workflows.
- **Shared**: Shared libraries and defaults used across services.

---

## 2. Network Flow and Gateway Placement

### Development/Local
- User → Portal (FE) → BFF → API Gateway (ProperTea.Gateway) → Microservices

### Production/Cloud
- User → Edge Gateway (e.g., Azure Front Door, Cloudflare, NGINX) → Portal (FE) or BFF
- BFF → API Gateway (ProperTea.Gateway)
- API Gateway → Microservices

**Edge Gateway** provides:
- SSL termination
- DDoS protection
- Web Application Firewall (WAF)
- Global load balancing
- Routing to the correct BFF/Portal

**Note:** The API Gateway is not exposed directly to the public internet; all requests should go through the edge gateway in production.

---

## 3. Project Structure

The solution is organized as follows:

```text
src/
  Identity/
    ProperTea.Identity.Api/
  LandlordPortal/
    ProperTea.Landlord.Bff/
  Orchestration/
    ProperTea.WorkflowOrchestrator/
  Organization/
    ProperTea.Organization.Api/
  ProperTea.AppHost/
  ProperTea.Gateway/
  Shared/
    ProperTea.ServiceDefaults/
  UserManagement/
    ProperTea.UserManagement.Api/
```

- Each domain service is in its own folder, with a clear separation of API/BFF projects.
- Orchestration (Durable Functions) is separated as ProperTea.WorkflowOrchestrator.
- BFFs (e.g., ProperTea.Landlord.Bff) are grouped with their respective portal context.
- Shared code (ProperTea.ServiceDefaults) is in a Shared folder.
- AppHost and Gateway are at the root of src, which is standard for Aspire-based solutions.

> As new services or portals are added, follow this structure for consistency and maintainability.
