# Organization Management Flow

This flow describes how a user manages their organization, such as changing the name, logo, or other profile settings.

```mermaid
sequenceDiagram
    participant User
    participant LandlordPortal
    participant LandlordBFF as Landlord BFF
    participant OrgService as Organization Service

    User->>LandlordPortal: Open organization management view
    LandlordPortal->>LandlordBFF: Request organization profile
    LandlordBFF->>OrgService: Get organization profile
    OrgService-->>LandlordBFF: Return organization profile
    LandlordBFF-->>LandlordPortal: Return organization profile
    LandlordPortal->>User: Show organization profile
    User->>LandlordPortal: Edit organization details (name, logo, etc.)
    LandlordPortal->>LandlordBFF: Send update request
    LandlordBFF->>OrgService: Process update
    OrgService-->>LandlordBFF: Confirm update
    LandlordBFF-->>LandlordPortal: Confirm update
    LandlordPortal->>User: Show updated organization profile
```

## Description
- User navigates to the organization management view in the Landlord portal.
- The Landlord portal communicates with the Landlord BFF to retrieve and display the organization's profile.
- User can edit details such as name and logo.
- The Landlord BFF forwards update requests to the Organization Service and returns the updated profile to the portal.
