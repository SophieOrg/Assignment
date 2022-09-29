using Microsoft.AspNetCore.Authentication.Cookies;
using FMS.Data.Services;
using FMS.Web;
using FMS.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

//Add Authentfication service using cookie scheme
builder.Services.AddCookieAuthentication();
       
// Add services to the container.
builder.Services.AddControllersWithViews();

//configure DI system
builder.Services.AddScoped<IRehomingService,RehomingServiceDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
} 
else {
    // in  development mode seed the database each time the application starts
    RehomingServiceSeeder.Seed(new RehomingServiceDb());
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Enable site Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
