# Overbooking/Application/Contract Flow

```mermaid
sequenceDiagram
    participant Landlord
    participant LandlordPortal
    participant LandlordBFF as Landlord BFF
    participant ListingPortal
    participant ListingBFF as Listing BFF
    participant Applicant
    participant ApplicationService
    participant OfferService
    participant RentalContractService
    participant VacanciesService

    Landlord->>LandlordPortal: Publish rental object (set listing period)
    LandlordPortal->>LandlordBFF: Send publish request
    LandlordBFF->>ApplicationService: Publish rental object
    Applicant->>ListingPortal: View listing, create application
    ListingPortal->>ListingBFF: Submit application (may include co-tenants)
    ListingBFF->>ApplicationService: Submit application
    loop Listing Period
        Applicant->>ListingPortal: More applications submitted
        ListingPortal->>ListingBFF: Submit application
        ListingBFF->>ApplicationService: Submit application
    end
    alt End of listing period or manual trigger
        ApplicationService->>OfferService: Select top applications, send offers
        OfferService->>ListingBFF: Notify offer (email/portal)
        ListingBFF->>Applicant: Notify offer
        Applicant->>ListingBFF: Accept/decline offer
        ListingBFF->>OfferService: Accept/decline offer
        OfferService->>LandlordBFF: Notify accepted offers
        LandlordBFF->>LandlordPortal: Notify accepted offers
        LandlordPortal->>Landlord: Show accepted offers
        Landlord->>LandlordPortal: Finalize contract for selected offer
        LandlordPortal->>LandlordBFF: Finalize contract
        LandlordBFF->>RentalContractService: Finalize contract
        RentalContractService->>VacanciesService: Update vacancy status
    else Simple flow
        Landlord->>LandlordPortal: Review applications manually
        LandlordPortal->>LandlordBFF: Get applications
        LandlordBFF->>ApplicationService: Get applications
        Landlord->>LandlordPortal: Create contract for chosen application
        LandlordPortal->>LandlordBFF: Create contract
        LandlordBFF->>RentalContractService: Create contract
        RentalContractService->>VacanciesService: Update vacancy status
    end
```

## Notes
- Overbooking is allowed during the listing/application phase, but only one contract can be finalized for a given period.
- The process is configurable per company or rental object.
- The workflow for offers, acceptances, and contract creation is orchestrated and auditable.
- Double-booking is prevented at the contract finalization step.
- All portal/backend communication is routed through the appropriate BFF.
