using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.API.Handlers;
using OrderManagement.Application.Common.Behaviors;
using OrderManagement.Application.Common.Mappings;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Repositories.Orders;
using OrderManagement.Application.Services.Priorities;
using OrderManagement.Persistence;
using OrderManagement.Persistence.Services;
using OrderManagement.Persistence.Settings;
using Serilog;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(config =>
{
    config.ReadFrom.Configuration(builder.Configuration);
    config.Enrich.FromLogContext();
});

builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(o => o.UseNpgsqlConnection(builder.Configuration.GetSection(DataBaseSet.Configuration).Get<DataBaseSet>().ConnectionString)));
builder.Services.AddHangfireServer();

builder.Services.Configure<DataBaseSet>(builder.Configuration.GetSection(DataBaseSet.Configuration));
builder.Services.Configure<CurrencyApiSet>(builder.Configuration.GetSection(CurrencyApiSet.Configuration));

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetSection(DataBaseSet.Configuration).Get<DataBaseSet>().ConnectionString));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

builder.Services.AddScoped<IPrioritisationService, PrioritisationService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICurrencyApiService, CurrencyApiService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.Services.AddExceptionHandler<HandlerException>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "Documentation.xml");
    options.IncludeXmlComments(xmlPath);
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await Preparation.Initialize(context);
}

RecurringJob.AddOrUpdate<ICurrencyApiService>("OrderHandling", x => x.BuildingCurrencyConverter(
    builder.Configuration.GetSection(CurrencyApiSet.Configuration).Get<CurrencyApiSet>().URL,
    builder.Configuration.GetSection(CurrencyApiSet.Configuration).Get<CurrencyApiSet>().APIKey,
    builder.Configuration.GetSection(CurrencyApiSet.Configuration).Get<CurrencyApiSet>().BaseCurrency,
    builder.Configuration.GetSection(CurrencyApiSet.Configuration).Get<CurrencyApiSet>().Currencies,
    default), "0 */5 * ? * *");

app.Run();
