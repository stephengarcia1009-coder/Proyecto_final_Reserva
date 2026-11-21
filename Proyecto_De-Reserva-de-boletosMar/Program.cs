
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Repositories;
using Proyecto_De_Reserva_de_boletosMar.Services;
using Proyecto_De_Reserva_de_boletosMar.Seed;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    foreach (var permiso in Proyecto_De_Reserva_de_boletosMar.Constants.PermisosSistema.Todos)
    {
        options.AddPolicy(permiso, policy =>
        {
            policy.RequireAssertion(context =>
                context.User.IsInRole("Administrador") ||
                context.User.HasClaim("Permiso", permiso));
        });
    }
    options.AddPolicy("Viajes.Gestionar.Acceso", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.IsInRole("Administrador") ||
            context.User.HasClaim("Permiso", "Viajes.Gestionar"));
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<PangaDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccesoDenegado";
});

builder.Services.AddDbContext<PangaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionPanga")));

builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>));


builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<IRutaService, RutaService>();
builder.Services.AddScoped<IPangaService, PangaService>();
builder.Services.AddScoped<ITarifaService, TarifaService>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPermisoService, PermisoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IPagoService, PagoService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = services
        .GetRequiredService<UserManager<IdentityUser>>();

    await IdentitySeed.CrearRolesAsync(roleManager);

    await IdentitySeed.CrearAdministradorAsync(
        userManager,
        builder.Configuration);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/PaginaNoEncontrada");

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
