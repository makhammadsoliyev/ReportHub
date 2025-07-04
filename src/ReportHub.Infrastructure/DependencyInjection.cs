using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;
using ReportHub.Infrastructure.Countries;
using ReportHub.Infrastructure.Currencies;
using ReportHub.Infrastructure.Email;
using ReportHub.Infrastructure.Identity;
using ReportHub.Infrastructure.Organizations;
using ReportHub.Infrastructure.Pdf;
using ReportHub.Infrastructure.Persistence;
using ReportHub.Infrastructure.Persistence.Interceptors;
using ReportHub.Infrastructure.Persistence.KeyVault;
using ReportHub.Infrastructure.Persistence.MongoDb;
using ReportHub.Infrastructure.Persistence.Repositories;
using ReportHub.Infrastructure.Time;
using ReportHub.Infrastructure.Token;
using ReportHub.Infrastructure.Users;

namespace ReportHub.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfigurationBuilder configuration)
	{
		services.AddServices();
		services.AddJwtAuthentication(configuration);
		services.AddAuthorization();
		services.AddPersistence(configuration);

		return services;
	}

	private static void AddPersistence(this IServiceCollection services, IConfigurationBuilder configuration)
	{
		services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();

		services.AddOptions<KeyVaultOptions>().BindConfiguration(nameof(KeyVaultOptions));
		services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
		{
			var keyVaultOptions = serviceProvider.GetService<IOptions<KeyVaultOptions>>().Value;
			var connectionString = GetConnectionString(keyVaultOptions, configuration);
			var interceptors = serviceProvider.GetServices<ISaveChangesInterceptor>();

			options
				.UseNpgsql(connectionString)
				.AddInterceptors(interceptors)
				.UseSnakeCaseNamingConvention();
		});

		services.AddScoped<ApplicationDbContextInitializer>();

		services.AddOptions<MongoDbOptions>().BindConfiguration(nameof(MongoDbOptions));
		services.AddMongoDb(configuration);

		services.AddRepositories();
	}

	private static void AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<ILogRepository, LogRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IItemRepository, ItemRepository>();
		services.AddScoped<IPlanRepository, PlanRepository>();
		services.AddScoped<IInvoiceRepository, InvoiceRepository>();
		services.AddScoped<IPlanItemRepository, PlanItemRepository>();
		services.AddScoped<ICustomerRepository, CustomerRepository>();
		services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
		services.AddScoped<IOrganizationRepository, OrganizationRepository>();
		services.AddScoped<IOrganizationRoleRepository, OrganizationRoleRepository>();
		services.AddScoped<IOrganizationMemberRepository, OrganizationMemberRepository>();
	}

	private static void AddServices(this IServiceCollection services)
	{
		services.AddIdentityCore<User>(options =>
		{
			options.User.RequireUniqueEmail = true;
			options.Password.RequiredLength = 8;
			options.Password.RequireDigit = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireNonAlphanumeric = false;
		})
		.AddRoles<Role>()
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders();

		services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddOptions<JwtOptions>().BindConfiguration(nameof(JwtOptions));

		services.AddScoped<ITokenService, TokenService>();
		services.AddSingleton<IDateTimeService, DateTimeService>();

		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddScoped<ICurrentOrganizationService, CurrentOrganizationService>();

		services.AddScoped<IEmailService, EmailService>();
		services.AddOptions<SmtpEmailOptions>().BindConfiguration(nameof(SmtpEmailOptions));

		services.AddOptions<CountryOptions>().BindConfiguration(nameof(CountryOptions));
		services.AddHttpClient<ICountryService, CountryService>((sp, client) =>
		{
			var options = sp.GetRequiredService<IOptions<CountryOptions>>().Value;

			client.BaseAddress = new Uri(options.Url);
		});

		services.AddOptions<CurrencyOptions>().BindConfiguration(nameof(CurrencyOptions));
		services.AddHttpClient<ICurrencyService, CurrencyService>((sp, client) =>
		{
			var options = sp.GetRequiredService<IOptions<CurrencyOptions>>().Value;

			client.BaseAddress = new Uri(options.Url);
		});

		services.AddScoped<IPdfService, PdfService>();
	}

	private static void AddJwtAuthentication(this IServiceCollection services, IConfigurationBuilder configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			var jwtOptions = configuration.Build().GetSection(nameof(JwtOptions)).Get<JwtOptions>();

			options.Authority = jwtOptions.Issuer;
			options.Audience = jwtOptions.Audience;
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				RequireExpirationTime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtOptions.Issuer,
				ValidAudience = jwtOptions.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
			};
		});
	}

	private static string GetConnectionString(KeyVaultOptions options, IConfigurationBuilder configuration)
	{
		var credential = new ClientSecretCredential(options.DirectoryId, options.ClientId, options.ClientSecret);
		configuration.AddAzureKeyVault(options.KeyVaultUrl, options.ClientId, options.ClientSecret, new DefaultKeyVaultSecretManager());

		var client = new SecretClient(new Uri(options.KeyVaultUrl), credential);
		var connectionString = client.GetSecret("ProdConnection").Value.Value;

		return connectionString;
	}

	private static void AddMongoDb(this IServiceCollection services, IConfigurationBuilder configuration)
	{
		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
		ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);
		ConventionRegistry.Register("EnumStringConvention", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, _ => true);

		var options = configuration.Build().GetSection(nameof(MongoDbOptions)).Get<MongoDbOptions>();
		var mongoClient = new MongoClient(options.ConnectionString);

		services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase(options.DatabaseName));
		services.AddSingleton<MongoDbContext>();
	}
}
