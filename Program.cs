using FluentValidation;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Session;
using xPlugUniAdmissionManager;
using xPlugUniAdmissionManager.Assets.AppSession.Core;

var builder = WebApplication.CreateBuilder(args);

//builder.ConfigureServiceOptions();

var assembly = typeof(Program).Assembly;
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddXPCaching();

// Add services to the container.
builder.Services.AddControllersWithViews()
.AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null)
.AddRazorOptions(opt =>
{
    opt.ViewLocationExpanders.Add(new ViewLocationExpander());

    //Area Locations
    opt.AreaViewLocationFormats.Clear();
    opt.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
    opt.AreaViewLocationFormats.Union(AppViewConfig.CustomSharedDirectories());

});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IStartSession, InitSession>();
builder.Services.AddSingleton<ISessionStore, DistributedSessionStoreWithStart>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseRewriter();
app.UseSession();
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
