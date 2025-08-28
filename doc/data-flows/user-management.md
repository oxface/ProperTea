# User Management Flow

This flow describes how a user manages their own data and credentials in the system.

```mermaid
sequenceDiagram
    participant User
    participant LandlordPortal
    participant LandlordBFF as Landlord BFF
    participant UserService as User Management Service
    participant IdentityService

    User->>LandlordPortal: Open user profile view
    LandlordPortal->>LandlordBFF: Get user profile
    LandlordBFF->>UserService: Get user profile
    UserService-->>LandlordBFF: Return user profile
    LandlordBFF-->>LandlordPortal: Return user profile
    LandlordPortal->>User: Show user profile
    User->>LandlordPortal: Edit profile or change credentials
    alt Edit profile
        LandlordPortal->>LandlordBFF: Send profile update
        LandlordBFF->>UserService: Send profile update
        UserService-->>LandlordBFF: Confirm update
        LandlordBFF-->>LandlordPortal: Confirm update
    else Change credentials
        LandlordPortal->>LandlordBFF: Send credential update
        LandlordBFF->>IdentityService: Send credential update
        IdentityService-->>LandlordBFF: Confirm update
        LandlordBFF-->>LandlordPortal: Confirm update
    end
    LandlordPortal->>User: Show updated profile/confirmation
```

## Description
- User navigates to the user profile view in the Landlord portal.
- The Landlord portal communicates with the Landlord BFF to retrieve and display the user's profile.
- User can edit their profile data or change credentials (password, external login, etc.).
- Updates are processed by the User Management or Identity Service as appropriate, with all communication routed through the BFF.
