using ExampleModernBlazorApp.Services;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//	.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllersWithViews()
	.AddMicrosoftIdentityUI();
//builder.Services.AddAuthorization(options =>
//	options.FallbackPolicy = options.DefaultPolicy);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
	.AddMicrosoftIdentityConsentHandler();
builder.Services.AddSingleton<ExampleService>();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
	_ = app.UseExceptionHandler("/Error");
	_ = app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
