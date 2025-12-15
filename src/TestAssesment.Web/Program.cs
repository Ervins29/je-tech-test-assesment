using MudBlazor.Services;
using TestAssesment.Data.DataAccess.Extensions;
using TestAssesment.Data.Services.Extensions;
using TestAssesment.Integrations.Omdb.Extensions;
using TestAssesment.Web.Components;
using TestAssesment.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddDataAccess(configuration);
builder.Services.AddDataServices();

builder.Services.AddTransient<SearchService>();

builder.Services.AddOmdbApi(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();