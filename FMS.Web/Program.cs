using Microsoft.AspNetCore.Authentication.Cookies;
using FMS.Data.Services;
using FMS.Web;
using FMS.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

//Add Authentfication service using cookie scheme
builder.Services.AddCookieAuthentication();
       
//Add services to the container.
//enables MVC
builder.Services.AddControllersWithViews();

//configure DI system (container provides a specific instance of a service defined by the specified interface)
//when an instance of IRehomingService is requested, provide the RehomingServiceDb instance
//created here by the DI system
//if we want to change the instance our controllers use we just change it in once place
//reduces coupling between controllers and the service implementation
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
//allows static resources to be served by the web application (HTML pages, css)
app.UseStaticFiles();

app.UseRouting();

//Enable site Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
