using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ProperTea.Identity.Api;
using ProperTea.Identity.Api.Endpoints;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("propertea-identity-db")));
builder.Services.AddIdentity<UserIdentity, IdentityRole>()
    .AddEntityFrameworkStores<UserIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();

app.UseHttpsRedirection();

app.MapUserIdentityEndpoints();

app.Run();