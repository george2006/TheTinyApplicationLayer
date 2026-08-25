using TheTinyApplicationLayer.Application.DependencyInjection;
using TheTinyApplicationLayer.Infrastructure.DependencyInjection;
using TheTinyApplicationLayer.Infrastructure.Persistence;
using TheTinyApplicationLayer.Web.Components;
using TheTinyApplicationLayer.Web.Users;
using TinyEvents;
using TinyEvents.Worker;
using TinyObservability.ApplicationMap;
using TinyObservability.ApplicationMap.TinyDispatcher;
using TinyObservability.ApplicationMap.TinyValidations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient("Application", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Application:BaseAddress"] ?? "http://localhost:5041");
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTinyApplicationMap(
    "TheTinyApplicationLayer",
    new Uri(builder.Configuration["TinyObservability:ApplicationMapAddress"] ?? "http://localhost:4317"),
    map => map
        .AddTinyDispatcher()
        .AddTinyValidations());
builder.Services.AddTinyEventsWorker(options =>
{
    options.BatchSize = 10;
    options.PollingInterval = TimeSpan.FromSeconds(2);
    options.ClaimTimeout = TimeSpan.FromMinutes(2);
    options.CleanupEnabled = true;
    options.ProcessedRetention = TimeSpan.FromHours(1);
    options.CleanupBatchSize = 1_000;
    options.CleanupInterval = TimeSpan.FromSeconds(1);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseTinyValidationProblemDetails();
app.UseAntiforgery();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

await app.Services.MigrateTinyEventsAsync();

app.MapUserEndpoints();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
