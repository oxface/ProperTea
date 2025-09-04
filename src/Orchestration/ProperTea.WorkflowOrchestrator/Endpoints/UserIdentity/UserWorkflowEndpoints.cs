namespace ProperTea.WorkflowOrchestrator.Endpoints.UserIdentity;

public static class UserWorkflowEndpoints
{
    public static void MapUserWorkflowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflows/users")
            .WithTags("User Workflows")
            .RequireAuthorization();

        CreateUserWithIdentityEndpoint.Map(app);
    }
}