using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IMongoClient>(provider =>
{
    
    return new MongoClient("mongodb+srv://vishnupillai3413:vishnu123@node.odyl0yb.mongodb.net/test?retryWrites=true&w=majority");
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "register",
    pattern: "/register",
    defaults: new { controller = "Home", action = "register" });

app.MapControllerRoute(
    name: "dashboard",
    pattern: "/dashboard",
    defaults: new { controller = "Home", action = "Dashboard" });

app.MapControllerRoute(
    name: "logout",
    pattern: "/logout",
    defaults: new { controller = "Home", action = "Logout" });


app.Run();
