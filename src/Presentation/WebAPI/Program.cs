using Application.Handlers.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Authentication.Extensions;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(x =>
        x.UseSqlite(builder.Configuration["ConnectionStrings:SocialWebDb"]),
    builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddEndpoints();
builder.Services.AddCustomExceptionHandler();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.MigrateAsync();
}

app.UseCustomExceptionHandler();
app.UseFastEndpoints().UseAuthorization().UseSwaggerGen();

await app.RunAsync();