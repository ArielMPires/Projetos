using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexuSys.Components;
using NexuSys.Data;
using NexuSys.DTOs.Users;
using NexuSys.Entities;
using NexuSys.Helper;
using NexuSys.Interfaces;
using NexuSys.Services;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("Default")
        ))
        .EnableSensitiveDataLogging());

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<INFs,NFService>();
builder.Services.AddScoped<IUsers,UsersServices>();
builder.Services.AddScoped<IProducts,Productservice>();
builder.Services.AddScoped<IBudget, BudgetService>();
builder.Services.AddScoped<IFiles, FilesService>();
builder.Services.AddScoped<IService, OSService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IPurchase, PurchaseService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<PDFService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie( options =>
    {
        options.LoginPath = "/";
        options.Cookie.Name = "astec.auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(nav.BaseUri)
    };
});
builder.Services.AddCascadingAuthenticationState();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapPost("/internallogin", async (
    HttpContext context,
    IUsers service,
    [FromForm] int ID,
    [FromForm] string Password) =>
{
    var response = await service.Log_In(
        new Log_In
        {
            ID = ID,
            Password = Password
        });

    if (!response.Result)
        return Results.Redirect("/");

    var user = response.Data as Users;

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier,
                  user!.ID.ToString()),
        new Claim(ClaimTypes.Name,
                  user.Name)
    };

    var identity = new ClaimsIdentity(
        claims,
        CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal);

    return Results.Redirect("/ND06");
})
.DisableAntiforgery();





app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
