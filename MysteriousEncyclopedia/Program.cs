using Microsoft.AspNetCore.Identity;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Repositories.RepositoryClass;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Context>();
builder.Services.AddTransient<ITopic, TopicRepository>();
builder.Services.AddTransient<IMysteriousEvent, MysteriousEventRepository>();
builder.Services.AddTransient<IResource, ResourceRepository>();
builder.Services.AddTransient<IContact, ContactRepository>();
builder.Services.AddTransient<IRequest, RequestRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 3;
    //options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
}).AddDapperStores(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("connection");
}).AddDefaultUI().AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
    pattern: "{controller=Account}/{action=SignUp}/{id?}");

app.Run();
