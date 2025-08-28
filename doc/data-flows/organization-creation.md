# Organization Creation Flow

This flow describes how an unauthenticated user creates a new Organization and specifies the first administrator. The system handles both new and existing users. The process is orchestrated by the Workflow Orchestrator (Durable Functions).

```mermaid
sequenceDiagram
    participant User as Unauthenticated User
    participant OrgPortal as Landlord Portal
    participant LandlordBFF as Landlord BFF
    participant Orchestrator as Workflow Orchestrator
    participant OrgService as Organization Service
    participant UserService as User Management
    participant IdentityService as Identity Service

    User->>OrgPortal: Open organization creation page
    User->>OrgPortal: Submit organization details and first admin info
    OrgPortal->>LandlordBFF: Send organization creation request
    LandlordBFF->>Orchestrator: Start organization creation workflow (org + admin info)
    Orchestrator->>OrgService: Create organization (pending)
    Orchestrator->>UserService: Check if admin user exists
    alt Admin user does not exist
        Orchestrator->>UserService: Create SystemUser
        Orchestrator->>IdentityService: Create Identity (credentials)
        Orchestrator->>OrgService: Link new user as admin to organization
    else Admin user exists
        Orchestrator->>OrgService: Link existing user as admin to organization
    end
    Orchestrator->>OrgService: Activate organization
    Orchestrator->>LandlordBFF: Return organization and admin info
    LandlordBFF->>OrgPortal: Return organization and admin info
    OrgPortal->>User: Show confirmation / next steps
```

## Description
- Unauthenticated user opens the organization creation page in the Landlord portal.
- User submits organization details and first admin user info (email, name, credentials).
- The Landlord portal calls the Landlord BFF, which calls the Workflow Orchestrator to start the organization creation process.
- The Orchestrator creates the organization in a pending state, checks if the admin user exists, and creates the user and identity if needed.
- The Orchestrator links the user as admin to the new organization and activates the organization.
- Confirmation is returned through the BFF and shown to the user; future enhancements may include sending an invitation/confirmation email.
