using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using System.Text;
using WolfInvoice.Configurations;
using WolfInvoice.Interfaces.IProviders;
using WolfInvoice.Providers.RequestUser;
using WolfInvoice.Interfaces.Services;
using WolfInvoice.Services;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Services.EntityService;
using FluentValidation.AspNetCore;
using FluentValidation;
using WolfInvoice.DTOs.Validation.User;

namespace WolfInvoice.Extensions;

/// <summary>
/// Provides dependency injection extension methods for configuring the application's services.
/// </summary>
public static class DI
{
    /// <summary>
    /// Adds Swagger documentation generation to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        _ = services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo { Title = "WolfInvoice", Version = "v1", });

            setup.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 2NG5Ff@t8ze^\""
                }
            );
            setup.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            );

            var filePath = Path.Combine(AppContext.BaseDirectory, "WolfInvoice.xml");
            setup.IncludeXmlComments(filePath);
        });

        return services;
    }

    /// <summary>
    /// Adds configuration objects to the service collection based on values from the application's configuration file.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddConfigs(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        /* Config Jwt  */
        var jwtConfig = new JwtConfig();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        /* Config BCrypt */
        var bcryptConfig = new BCryptConfig();
        configuration.GetSection("BCrypt").Bind(bcryptConfig);
        services.AddSingleton(bcryptConfig);

        /* Config  Company Information */
        var companyInfo = new CompanyInfoConfig();
        configuration.GetSection("CompanyInfo").Bind(companyInfo);
        services.AddSingleton(companyInfo);

        return services;
    }

    /// <summary>
    /// Adds authentication to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="configuration">for Jwt Token</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();

        var jwtConfig = new JwtConfig();
        configuration.GetSection("JWT").Bind(jwtConfig);

        services
            .AddAuthentication("Bearer")
            .AddJwtBearer(
                "Bearer",
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtConfig.Secret)
                        ),
                    };
                }
            );

        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Configures the Serilog logging for the application.
    /// </summary>
    public static void UseSerilog()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel
            .Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine} {Exception}"
            )
            .WriteTo.File("logs/wolf invoice.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    /// <summary>
    /// Extension method to adding validations to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <returns>The modified IServiceCollection instance.</returns>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        FluentValidationMvcExtensions.AddFluentValidation(services);
#pragma warning restore CS0618 // Type or member is obsolete
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

        return services;
    }

    /// <summary>
    /// Extension method to register services to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <returns>The modified IServiceCollection instance.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<CryptService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
