using InscriptionApp.Api.Adapters;
using InscriptionApp.Api.Data;
using InscriptionApp.Api.Decorators;
using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Factories;
using InscriptionApp.Api.Filters;
using InscriptionApp.Api.Mappers;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Options;
using InscriptionApp.Api.Proxies;
using InscriptionApp.Api.Repositories.Implementations;
using InscriptionApp.Api.Repositories.Interfaces;
using InscriptionApp.Api.Services.Implementations;
using InscriptionApp.Api.Services.Interfaces;
using InscriptionApp.Api.Strategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using InscriptionApp.Api.Events;
using InscriptionApp.Api.Events.Handlers;
using InscriptionApp.Api.Commands;
using InscriptionApp.Api.Commands.Handlers;
using InscriptionApp.Api.Mediator;
using InscriptionApp.Api.Factories.Kits;

var builder = WebApplication.CreateBuilder(args);

// Controllers + filtre global d'exceptions
builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

// Base de données
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories (Scoped : dépendent du DbContext)
builder.Services.AddScoped<IVendeurRepository, VendeurRepository>();
builder.Services.AddScoped<IAcheteurRepository, AcheteurRepository>();

// Email : SINGLETON
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));

// Sources possibles d'envoi
builder.Services.AddSingleton<ExternalMailClient>();
builder.Services.AddSingleton<EmailSender>();

// Chaîne : Logging -> RateLimit (Proxy) -> Retry -> expéditeur réel (SMTP ou Adapter)
builder.Services.AddSingleton<IEmailSender>(sp =>
{
    var opt = sp.GetRequiredService<IOptions<SmtpOptions>>().Value;

    IEmailSender real = opt.Provider.Equals("External", StringComparison.OrdinalIgnoreCase)
        ? new ExternalMailAdapter(sp.GetRequiredService<ExternalMailClient>())
        : sp.GetRequiredService<EmailSender>();

    var retry = new RetryEmailSender(
        real,
        sp.GetRequiredService<ILogger<RetryEmailSender>>(),
        opt.MaxRetryAttempts,
        TimeSpan.FromMilliseconds(500));

    var limited = new RateLimitedEmailSenderProxy(
        retry,
        sp.GetRequiredService<ILogger<RateLimitedEmailSenderProxy>>(),
        opt.MaxEmailsPerMinute);

    return new LoggingEmailSender(limited, sp.GetRequiredService<ILogger<LoggingEmailSender>>());
});

// Strategy
builder.Services.AddSingleton<INotificationStrategy, EmailNotificationStrategy>();
builder.Services.AddSingleton<INotificationStrategy, SmsNotificationStrategy>();
builder.Services.AddSingleton<NotificationContext>();

// Mappers et factories : sans état => Singleton
builder.Services.AddSingleton<IMapper<Vendeur, VendeurCreateRequest, VendeurResponse>, VendeurMapper>();
builder.Services.AddSingleton<IMapper<Acheteur, AcheteurCreateRequest, AcheteurResponse>, AcheteurMapper>();
builder.Services.AddSingleton<ConfirmationMessageFactory<Vendeur>, VendeurMessageFactory>();
builder.Services.AddSingleton<ConfirmationMessageFactory<Acheteur>, AcheteurMessageFactory>();

// Services (Scoped)
builder.Services.AddScoped<IVendeurService, VendeurService>();
builder.Services.AddScoped<IAcheteurService, AcheteurService>();

// Observer : le publieur et les abonnés
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEventHandler<UtilisateurInscritEvent<Vendeur>>, EnvoyerConfirmationHandler<Vendeur>>();
builder.Services.AddScoped<IEventHandler<UtilisateurInscritEvent<Vendeur>>, JournaliserInscriptionHandler<Vendeur>>();
builder.Services.AddScoped<IEventHandler<UtilisateurInscritEvent<Acheteur>>, EnvoyerConfirmationHandler<Acheteur>>();
builder.Services.AddScoped<IEventHandler<UtilisateurInscritEvent<Acheteur>>, JournaliserInscriptionHandler<Acheteur>>();

// Command + Mediator + Facade
builder.Services.AddScoped<IMediator, CommandMediator>();
builder.Services.AddScoped<ICommandHandler<InscrireVendeurCommand, VendeurResponse>, InscrireVendeurCommandHandler>();
builder.Services.AddScoped<ICommandHandler<InscrireAcheteurCommand, AcheteurResponse>, InscrireAcheteurCommandHandler>();
builder.Services.AddScoped<IInscriptionFacade, InscriptionFacade>();

// Abstract Factory : les kits par langue
builder.Services.AddSingleton<INotificationKitFactory, KitFrancaisFactory>();
builder.Services.AddSingleton<INotificationKitFactory, KitAnglaisFactory>();
builder.Services.AddSingleton<NotificationKitProvider>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();