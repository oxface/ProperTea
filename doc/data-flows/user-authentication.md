# User Authentication Flow

This flow describes how a user authenticates to the system using credentials or an external provider via the Landlord portal. If the user is connected to multiple organizations, they can select which one to manage.

```mermaid
sequenceDiagram
    participant User
    participant LandlordPortal
    participant LandlordBFF as Landlord BFF
    participant IdentityService
    participant UserService

    User->>LandlordPortal: Open login page
    LandlordPortal->>LandlordBFF: Request authentication
    LandlordBFF->>IdentityService: Submit credentials or use external provider
    IdentityService-->>LandlordBFF: Return authentication token
    LandlordBFF-->>LandlordPortal: Return authentication token
    LandlordPortal->>LandlordBFF: Get organizations for user
    LandlordBFF->>UserService: Get organizations for user
    UserService-->>LandlordBFF: List of organizations
    LandlordBFF-->>LandlordPortal: List of organizations
    alt User has multiple organizations
        LandlordPortal->>User: Prompt to select organization
        User->>LandlordPortal: Select organization
    end
    LandlordPortal->>User: Show dashboard for selected organization
```

## Description
- User opens the login page in the Landlord portal.
- The Landlord portal communicates with the Landlord BFF for authentication and organization retrieval.
- User authenticates using credentials or an external provider.
- Upon successful authentication, the system retrieves the organizations the user is connected to.
- If the user is connected to multiple organizations, they are prompted to select one.
- The dashboard for the selected organization is displayed.
