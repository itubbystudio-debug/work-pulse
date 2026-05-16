using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using WorkPulse.Api.Extensions;
using WorkPulse.Api.Middleware;
using WorkPulse.Api.Security;
using WorkPulse.Application;
using WorkPulse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication(AdminBearerAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, AdminBearerAuthenticationHandler>(
        AdminBearerAuthenticationHandler.SchemeName,
        _ => { });
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
