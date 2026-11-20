
namespace Proyecto_De_Reserva_de_boletosMar.ViewModels
{
    public class UsuariosGestionViewModel
    {
        public List<UsuarioGestionItemViewModel> Usuarios { get; set; } = new();

        public List<string> RolesDisponibles { get; set; } = new();
    }

    public class UsuarioGestionItemViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string? Email { get; set; }

        public List<string> Roles { get; set; } = new();

        public List<string> Permisos { get; set; } = new();
    }
}
