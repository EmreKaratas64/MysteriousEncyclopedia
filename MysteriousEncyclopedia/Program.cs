using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
}).AddDapperStores(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("connection");
}).AddDefaultUI().AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.AccessDeniedPath = new PathString("/notallowed");
    options.LoginPath = "/login";
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/exception");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Account", action = "SignIn" }
    );

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Account", action = "SignUp" }
    );


app.MapControllerRoute(
    name: "register",
    pattern: "adminlogin",
    defaults: new { controller = "Account", action = "AdminLogin" }
    );


app.MapControllerRoute(
    name: "setting",
    pattern: "usersetting",
    defaults: new { controller = "Account", action = "Setting" }
    );

app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "Account", action = "SignOut" }
    );

app.MapControllerRoute(
    name: "mail",
    pattern: "mailverify",
    defaults: new { controller = "Account", action = "MailConfirm" }
    );

app.MapControllerRoute(
    name: "pass",
    pattern: "passwordrecovery",
    defaults: new { controller = "Account", action = "PasswordReset" }
    );

// PasswordRecover fonksiyonunun routing'i Controller da yapýldý
app.MapControllerRoute(
    name: "pass",
    pattern: "newpassword",
    defaults: new { controller = "Account", action = "PasswordRecover" }
    );



app.MapControllerRoute(
    name: "users",
    pattern: "showusers/{id?}",
    defaults: new { controller = "Account", action = "UserList" }
    );

app.MapControllerRoute(
    name: "users",
    pattern: "showuserroles/{id?}",
    defaults: new { controller = "Account", action = "UserRoles" }
    );


app.MapControllerRoute(
    name: "home",
    pattern: "home",
    defaults: new { controller = "Home", action = "HomePage" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "contactus",
    defaults: new { controller = "Home", action = "HomeContact" }
    );

app.MapControllerRoute(
    name: "events",
    pattern: "mysteriousevents",
    defaults: new { controller = "Home", action = "HomeMysteriousEventList" }
    );

app.MapControllerRoute(
    name: "eventsbytopic",
    pattern: "events/{topic?}",
    defaults: new { controller = "Home", action = "HomeMysteriousEventsByTopic" }
    );

app.MapControllerRoute(
    name: "eventdetails",
    pattern: "eventdetails/{id?}",
    defaults: new { controller = "Home", action = "HomeMysteriousEventDetail" }
    );

app.MapControllerRoute(
    name: "homeeventresources",
pattern: "resources",
    defaults: new { controller = "Home", action = "HomeResources" }
);


app.MapControllerRoute(
    name: "errorpage",
    pattern: "error/{code?}",
    defaults: new { controller = "Home", action = "ErrorPage" }
    );

app.MapControllerRoute(
    name: "error",
    pattern: "exception",
    defaults: new { controller = "Home", action = "Error" }
    );

app.MapControllerRoute(
    name: "accessdenied",
    pattern: "notallowed",
    defaults: new { controller = "Home", action = "AccessDenied" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "contacts",
    defaults: new { controller = "Contact", action = "ListContacts" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "removecont/{id?}",
    defaults: new { controller = "Contact", action = "DeleteContact" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "newrequest/{Id?}",
    defaults: new { controller = "Contact", action = "MakeRequest" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "requests",
    defaults: new { controller = "Contact", action = "RequestList" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "showrequest/{id?}",
    defaults: new { controller = "Contact", action = "RequestDetail" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "removereq/{Id?}",
    defaults: new { controller = "Contact", action = "DeleteRequest" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "requestapproval/{id?}",
    defaults: new { controller = "Contact", action = "ApproveRequest" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "requestcancel/{id?}",
    defaults: new { controller = "Contact", action = "CancelRequest" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "comments",
    defaults: new { controller = "Contact", action = "ListComments" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "showcomment/{id?}",
    defaults: new { controller = "Contact", action = "CommentDetail" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "remcomment/{id?}",
    defaults: new { controller = "Contact", action = "DeleteComment" }
    );

app.MapControllerRoute(
    name: "contact",
    pattern: "commentacceptence/{id?}",
    defaults: new { controller = "Contact", action = "AcceptComment" }
    );

app.MapControllerRoute(
    name: "mysteriousevent",
    pattern: "showmysteriousevents",
    defaults: new { controller = "MysteriousEvent", action = "MysteriousEventList" }
    );

app.MapControllerRoute(
    name: "mysteriousevent",
    pattern: "eventsources/{id?}",
    defaults: new { controller = "MysteriousEvent", action = "ResourcesByEventID" }
    );

app.MapControllerRoute(
    name: "mysteriousevent",
    pattern: "newresource/{id?}",
    defaults: new { controller = "MysteriousEvent", action = "AddResourceToEvent" }
    );

app.MapControllerRoute(
    name: "mysteriousevent",
    pattern: "newevent",
    defaults: new { controller = "MysteriousEvent", action = "MysteriousEventAdd" }
    );

app.MapControllerRoute(
    name: "mysteriousevent",
    pattern: "editevent/{id?}",
    defaults: new { controller = "MysteriousEvent", action = "MysteriousEventUpdate" }
    );

app.MapControllerRoute(
    name: "reference",
    pattern: "references",
    defaults: new { controller = "Reference", action = "ReferencesList" }
    );

app.MapControllerRoute(
    name: "reference",
    pattern: "newreference",
    defaults: new { controller = "Reference", action = "AddReference" }
    );

app.MapControllerRoute(
    name: "reference",
    pattern: "editref/{id?}",
    defaults: new { controller = "Reference", action = "UpdateReference" }
    );

app.MapControllerRoute(
    name: "topic",
    pattern: "eventtopics",
    defaults: new { controller = "Topic", action = "TopicList" }
    );

app.MapControllerRoute(
    name: "topic",
    pattern: "newtopic",
    defaults: new { controller = "Topic", action = "TopicAdd" }
    );

app.MapControllerRoute(
    name: "topic",
    pattern: "edittopic/{id?}",
    defaults: new { controller = "Topic", action = "TopicUpdate" }
    );

app.Run();
