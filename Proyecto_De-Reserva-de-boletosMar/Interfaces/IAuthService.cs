
using Microsoft.AspNetCore.Identity;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegistrarAsync(
            string nombreCompleto,
            string correo,
            string telefono,
            string password);

        Task<SignInResult> IniciarSesionAsync(
            string correo,
            string password,
            bool recordar);

        Task CerrarSesionAsync();
    }
}
