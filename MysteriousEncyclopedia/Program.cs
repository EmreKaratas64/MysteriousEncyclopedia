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
builder.Services.AddTransient<IComment, CommentRepository>();
builder.Services.AddTransient<IUser, UserRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
}).AddDapperStores(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("connection");
}).AddDefaultUI().AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.AccessDeniedPath = new PathString("/Home/AccessDenied/");
    options.LoginPath = "/Account/SignIn/";
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error404/", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
