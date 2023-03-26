using Core.Entities;
using Data;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.HelperServices.Implementations;
using Service.HelperServices.Interfaces;
using Service.Services;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using Service.Validators;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionStrings = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(connectionStrings);
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
    opt.Password.RequiredUniqueChars = 0;
    opt.Password.RequireUppercase = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SliderCreateValidator)));
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ILocalService, LocalStorage>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<LayoutService>();

builder.Services.AddDataLayerServices();
builder.Services.AddServiceLayerServices();

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


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "areas",
        "{area:exists}/{controller=account}/{action=login}/{id?}/{slug?}"
        );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
