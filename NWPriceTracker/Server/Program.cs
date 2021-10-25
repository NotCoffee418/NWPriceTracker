// Apply database migrations if any
await MigrationHelper.ApplyMigrations();

// Start server
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddRouting();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// From: https://github.com/aspnet/Blazor/issues/1554
// Adds HttpContextAccessor
// Used to determine if a user is logged in
// and what their username is
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
// Required for HttpClient support in the Blazor Client project
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

builder.Services
    .AddAuthentication(options => 
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.RequireAuthenticatedSignIn = true;
    })
    .AddCookie()
    .AddDiscord(options =>
    {
        SettingsHelper sh = new();
        options.ClientId = sh.Get<string>("DiscordClientId");
        options.ClientSecret = sh.Get<string>("DiscordClientSecret");
        //options.Scope.Add("identity");
    });
// Authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Register other policies here

});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

// auth
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PriceHub>("/hub/price");
    endpoints.MapHub<SearchHub>("/hub/search");
    endpoints.MapHub<InstallerHub>("/hub/installer");
    endpoints.MapHub<SettingsHub>("/hub/settings");
    endpoints.MapHub<UserHub>("/hub/user");
    endpoints.MapDefaultControllerRoute();
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
