var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddControllersWithViews();
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
app.UseAuthorization();
app.MapControllerRoute(
    name: "history",
    pattern: "history",
    defaults: new { controller = "Home", action = "History" });
app.MapControllerRoute(
    name: "barcode",
    pattern: "barcode",
    defaults: new { controller = "BarCode", action = "Index" });
app.MapControllerRoute(
    name: "samples",
    pattern: "samples",
    defaults: new { controller = "BarCode", action = "Samples" });
app.MapControllerRoute(
    name: "privacy",
    pattern: "privacy",
    defaults: new { controller = "Home", action = "Privacy" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
