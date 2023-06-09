
/*
   Title : Leave Management System with Timesheet
   Author : Kalaivani G
   Created At : 21-02-2023
   Updated At : 04-05-2023
   Reviewed By : 
   Reviewed At : 05-05-2023

*/

using Microsoft.EntityFrameworkCore;
using Leave.Data;
using Microsoft.AspNetCore.Identity;
using project.Areas.Identity.Data;
// using Microsoft.AspNetCore.Mvc.ViewEngines;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();

//Entity Framework Dbcontext

builder.Services.AddDbContext<LeaveDbContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });


//Scaffold Identity user DbContext

builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>();
    builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContextConnection")));   

// IServiceCollection serviceCollection = builder.Services.AddSingleton<ICompositeViewEngine, CompositeViewEngine>();

//Session

builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
