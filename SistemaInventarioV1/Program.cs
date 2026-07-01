using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioAccesoDatos.Repositorio;
using SistemaInventarioAccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.AccesoDatos.Data;
//using SistemaInventarioV1.Data;
using SistemaInventarioAccesoDatos.Data;
using SistemaInventarioUtilidades;
using Microsoft.AspNetCore.Identity.UI.Services; //se agrego para el tema dek Applicationuser

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddDefaultTokenProviders()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<SistemaInventarioAccesoDatos.Data.ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false) //despues regresarlo  a True
    .AddErrorDescriber<ErrorDescriber>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<SistemaInventarioV1.AccesoDatos.Data.ApplicationDbContext>();


builder.Services.Configure<IdentityOptions>(options => //reglas para password
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();//Se agrega AddRazorRuntimeCompilation , para que al refrezcar el navegador se visualice los cambios

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IEmailSender, EmailSender>();
//se agrega una sola vez y pueda seguirse usando las veces que sean.
builder.Services.AddScoped<IunidadTrabajo, UnidadTrabajo>();//se agrega para que se pueda agregar o llamar  en cualquier controlador


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
