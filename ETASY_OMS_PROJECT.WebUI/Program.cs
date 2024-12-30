using ETASY_OMS_PROJECT.WebUI.Business.Abstracts;
using ETASY_OMS_PROJECT.WebUI.Business.Concretes;
using ETASY_OMS_PROJECT.WebUI.DAL.Abstracts;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes;
using ETASY_OMS_PROJECT.WebUI.DAL.Concretes.Data;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Abstract;
using ETASY_OMS_PROJECT.WebUI.DAL.Repos.Concrete;
using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<IPasswordService, PasswordManager>();
builder.Services.AddScoped<IAccountDal, AccountDal>();
builder.Services.AddScoped<IProductDal, ProductDal>();

builder.Services.AddScoped<IGenericRepository<EntityBase>, GenericRepository<EntityBase>>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(ops =>
    {
        ops.Cookie.Name = "ETASY_OMS_Cookie_Auth";
        ops.LoginPath = "/Account/Login";
        ops.LogoutPath = "/Account/Logout";
        ops.AccessDeniedPath = "/Account/AccessDenied";
        ops.ExpireTimeSpan = TimeSpan.FromDays(1);
        ops.SlidingExpiration = true;
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
