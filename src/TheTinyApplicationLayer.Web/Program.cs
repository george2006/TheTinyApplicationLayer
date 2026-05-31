using TheTinyApplicationLayer.Application.DependencyInjection;
using TheTinyApplicationLayer.Infrastructure.DependencyInjection;
using TheTinyApplicationLayer.Infrastructure.Persistence;
using TheTinyApplicationLayer.Web.Components;
using TheTinyApplicationLayer.Web.Users;
using TinyEvents.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTinyEventsWorker(options =>
{
    options.BatchSize = 10;
    options.PollingInterval = TimeSpan.FromSeconds(2);
    options.ClaimTimeout = TimeSpan.FromMinutes(2);
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

app.MapUserEndpoints();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
