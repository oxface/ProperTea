# Company Management Flow

This flow describes how a user manages companies within their organization: creating, editing, or deleting companies.

```mermaid
sequenceDiagram
    participant User
    participant LandlordPortal
    participant LandlordBFF as Landlord BFF
    participant OrgService as Organization Service

    User->>LandlordPortal: Open company management view
    LandlordPortal->>LandlordBFF: Request company list
    LandlordBFF->>OrgService: Get list of companies
    OrgService-->>LandlordBFF: Return companies
    LandlordBFF-->>LandlordPortal: Return companies
    LandlordPortal->>User: Show company list
    User->>LandlordPortal: Create/Edit/Delete company
    LandlordPortal->>LandlordBFF: Send create/edit/delete request
    LandlordBFF->>OrgService: Process create/edit/delete
    OrgService-->>LandlordBFF: Confirm operation
    LandlordBFF-->>LandlordPortal: Confirm operation
    LandlordPortal->>User: Show updated company list
```

## Description
- User navigates to the company management view in the Landlord portal.
- The Landlord portal communicates with the Landlord BFF to retrieve and display the list of companies for the organization.
- User can create, edit, or delete companies.
- The Landlord BFF forwards requests to the Organization Service and returns the updated list to the portal.
