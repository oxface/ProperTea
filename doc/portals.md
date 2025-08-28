# Portals

| Portal     | Description                                                                 | Access Model                |
|------------|-----------------------------------------------------------------------------|-----------------------------|
| Landlord   | For landlord office users to manage properties, listings, applications, etc.| Authenticated, scoped       |
| Tenant     | For current/former tenants to view contracts, payments, etc.                | Authenticated, scoped       |
| Market     | For potential tenants to view vacancies and apply.                          | Public/unauthenticated view, authenticated for details/applications |
| Support    | For internal support and data operations.                                   | Authenticated, global, with organization opt-in for access          |

## Portal Details

### Landlord Portal
- Manage property listings, rental objects, applications, offers, and contracts.
- Configure listing periods, overbooking settings, and company/organization settings.

### Tenant Portal
- View current and past rental contracts, payment history, and submit maintenance requests.
- Manage personal profile and see application status.

### Market Portal
- Publicly browse available rental objects by location, filter by criteria.
- Unauthenticated users can view listings; authentication required for application or viewing sensitive details.

### Support Portal
- Internal tool for support staff to assist with data issues, migrations, and troubleshooting.
- Access to all organizations/companies, with opt-in security controls.

