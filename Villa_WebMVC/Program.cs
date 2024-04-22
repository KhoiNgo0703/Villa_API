using Villa_WebMVC;
using Villa_WebMVC.Services;
using Villa_WebMVC.Services.IServices;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Villa_WebMVC.Extensions;

var builder = WebApplication.CreateBuilder(args);
//------------------------------------------------------------------------------
// Add services to the container.
builder.Services.AddControllersWithViews(u => u.Filters.Add(new AuthExceptionRedirection()));

///Razor package
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();// for hot reload

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//register for ihttp accessor 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//register for api message builder
builder.Services.AddSingleton<IApiMessageRequestBuilder, ApiMessageRequestBuilder>();

//register tokenProvider
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
//register baseService
builder.Services.AddScoped<IBaseService, BaseService>();
//register httpclient+independency injection VillaService
builder.Services.AddHttpClient<IVillaService,VillaService>();
builder.Services.AddScoped<IVillaService,VillaService>();
//register httpclient+independency injection VillaNumberService
builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
//register httpclient+independency injection AuthService
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//register for authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });


//to preserve token
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//------------------------------------------------------------------------------
//build app
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

//configure authentication 
app.UseAuthentication();

app.UseAuthorization();
//add for preserving token purpose
app.UseSession();
//
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
