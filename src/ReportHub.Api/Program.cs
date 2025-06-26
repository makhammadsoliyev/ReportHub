using ReportHub.Api.Extensions;
using ReportHub.Application;
using ReportHub.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration)
	=> configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddSwaggerGenWithUi();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwaggerUi();

await app.InitialiseDatabaseAsync();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandlerMiddleware();

app.MapControllers();

await app.RunAsync();
