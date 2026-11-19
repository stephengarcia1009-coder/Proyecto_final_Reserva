
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
    }
}
