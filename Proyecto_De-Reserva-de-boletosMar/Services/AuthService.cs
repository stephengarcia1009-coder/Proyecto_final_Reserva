
using Microsoft.AspNetCore.Identity;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly PangaDbContext _context;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            PangaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IdentityResult> RegistrarAsync(
            string nombreCompleto,
            string correo,
            string telefono,
            string password)
        {
            var usuario = new IdentityUser
            {
                UserName = correo,
                Email = correo
            };

            var resultado = await _userManager.CreateAsync(
                usuario, password);

            if (!resultado.Succeeded)
                return resultado;

            // Asignar automáticamente el rol Cliente
            var resultadoRol = await _userManager.AddToRoleAsync(
                usuario, "Cliente");

            if (!resultadoRol.Succeeded)
            {
                await _userManager.DeleteAsync(usuario);
                return resultadoRol;
            }

            try
            {
                var cliente = new Cliente
                {
                    UsuarioId = usuario.Id,
                    NombreCompleto = nombreCompleto,
                    Correo = correo,
                    Telefono = telefono,
                    FechaRegistro = DateTime.Now
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return resultado;
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }
        }

        public async Task<SignInResult> IniciarSesionAsync(
            string correo,
            string password,
            bool recordar)
        {
            return await _signInManager.PasswordSignInAsync(
                correo,
                password,
                recordar,
                lockoutOnFailure: false);
        }

        public async Task CerrarSesionAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
