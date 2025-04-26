using master.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ? Add HTTP Client
builder.Services.AddHttpClient();

// ? Add MVC controllers with views
builder.Services.AddControllersWithViews();

// ? Add Session service
builder.Services.AddSession();

// ? Add Database Context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

var app = builder.Build();

// ? Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ? Enable Session (must come BEFORE Authorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
