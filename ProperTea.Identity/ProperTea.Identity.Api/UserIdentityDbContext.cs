using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProperTea.Identity.Api;

public class UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options)
    : IdentityDbContext<UserIdentity>(options);