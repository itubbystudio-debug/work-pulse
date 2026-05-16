using Microsoft.AspNetCore.Authentication;
using WorkPulse.Api.Extensions;
using WorkPulse.Api.Authentication;
using WorkPulse.Api.Middleware;
using WorkPulse.Application;
using WorkPulse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services
    .AddAuthentication(HeaderAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>(
        HeaderAuthenticationDefaults.AuthenticationScheme,
        options => { });
builder.Services.AddAuthorization();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiOpenApi();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
