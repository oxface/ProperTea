# Global Error Handling

This folder contains shared error handling infrastructure that can be used across all ProperTea services.

## Components

### GlobalExceptionHandler
- Handles all unhandled exceptions
- Maps exceptions to appropriate HTTP status codes
- Provides structured logging with correlation IDs
- Returns RFC 7807 compliant ProblemDetails responses

### StatusCodeHelpers
- Provides consistent status code titles and details
- Creates standardized ProblemDetails for status code responses

### ErrorHandlingExtensions
- Extension methods for easy integration into services
- Single-line setup for comprehensive error handling

## Usage

### In any service Program.cs:

```csharp
// Add error handling services
builder.Services.AddGlobalErrorHandling("YourServiceName");

// Configure error handling middleware (early in pipeline)
app.UseGlobalErrorHandling("YourServiceName");
```

### Supported Exception Mappings

- `UnauthorizedAccessException` → 401 Unauthorized
- `ArgumentException` → 400 Bad Request  
- `InvalidOperationException` → 400 Bad Request
- `TimeoutException` → 408 Request Timeout
- `HttpRequestException` → 502 Bad Gateway
- All others → 500 Internal Server Error

### Response Format

All errors return consistent JSON structure:

```json
{
  "type": "https://httpstatuses.io/400",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request was invalid or malformed.",
  "instance": "/api/organizations",
  "correlationId": "abc123-def456-ghi789",
  "timestamp": "2025-08-27T10:30:00Z",
  "service": "YourServiceName"
}
```

### Benefits

- ✅ Consistent error responses across all services
- ✅ RFC 7807 compliant ProblemDetails
- ✅ Correlation ID tracking for distributed tracing
- ✅ Structured logging with full context
- ✅ Single-line integration into any service
