using ProperTea.Contracts.CQRS;
using ProperTea.Identity.Api.Application.Commands;
using ProperTea.Identity.Api.Domain.Identities;

namespace ProperTea.Identity.Api.Application.Handlers;

public class CreateIdentityCommandHandler : ICommandHandler<CreateIdentityCommand>
{
    private readonly IUserIdentityRepository _identityRepository;

    public CreateIdentityCommandHandler(IUserIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    public async Task HandleAsync(CreateIdentityCommand command, CancellationToken cancellationToken = default)
    {
        var existingIdentity = await _identityRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingIdentity != null)
        {
            throw new InvalidOperationException($"Identity with email '{command.Email}' already exists");
        }

        // In a real implementation, you would hash the password here
        var passwordHash = HashPassword(command.Password);
        
        var identity = UserIdentity.Create(command.UserId, command.Email, passwordHash);
        await _identityRepository.SaveAsync(identity, cancellationToken);
    }

    private static string HashPassword(string password)
    {
        // TODO: Implement proper password hashing (e.g., using BCrypt)
        // This is a placeholder - in real implementation use BCrypt, Argon2, etc.
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "_salt"));
    }
}

public class AuthenticateUserCommandHandler : ICommandHandler<AuthenticateUserCommand>
{
    private readonly IUserIdentityRepository _identityRepository;

    public AuthenticateUserCommandHandler(IUserIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    public async Task HandleAsync(AuthenticateUserCommand command, CancellationToken cancellationToken = default)
    {
        var identity = await _identityRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (identity == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var passwordHash = HashPassword(command.Password);
        if (!identity.VerifyPassword(passwordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        identity.UpdateLastLogin();
        await _identityRepository.SaveAsync(identity, cancellationToken);
    }

    private static string HashPassword(string password)
    {
        // TODO: Implement proper password hashing (same as CreateIdentityCommandHandler)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "_salt"));
    }
}
