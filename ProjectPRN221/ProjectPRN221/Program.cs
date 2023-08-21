using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(otp => otp.IdleTimeout = TimeSpan.FromMinutes(5));

builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PRN221DB"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
