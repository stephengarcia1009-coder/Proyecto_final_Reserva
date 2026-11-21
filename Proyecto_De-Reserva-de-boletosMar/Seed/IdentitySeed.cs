

using Microsoft.AspNetCore.Identity;

namespace Proyecto_De_Reserva_de_boletosMar.Seed
{
    public static class IdentitySeed
    {
        public static async Task CrearRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Administrador", "Cliente" };

            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }

        public static async Task CrearAdministradorAsync(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            var correo = configuration["AdministradorInicial:Correo"];
            var password = configuration["AdministradorInicial:Password"];

            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "Faltan las credenciales del administrador inicial.");
            }

            var usuario = await userManager.FindByEmailAsync(correo);

            if (usuario == null)
            {
                usuario = new IdentityUser
                {
                    UserName = correo,
                    Email = correo,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(
                    usuario, password);

                if (!resultado.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo crear el administrador: " +
                        string.Join(", ", resultado.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(usuario, "Administrador"))
            {
                var resultadoRol = await userManager.AddToRoleAsync(
                    usuario, "Administrador");

                if (!resultadoRol.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo asignar el rol Administrador.");
                }
            }
        }
    }
}
