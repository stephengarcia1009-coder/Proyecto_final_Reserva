
namespace Proyecto_De_Reserva_de_boletosMar.Constants
{
    public static class PermisosSistema
    {
        public const string UsuariosVer = "Usuarios.Ver";
        public const string UsuariosGestionar = "Usuarios.Gestionar";

        public const string ReservasVer = "Reservas.Ver";
        public const string ReservasGestionar = "Reservas.Gestionar";

        public const string ViajesVer = "Viajes.Ver";
        public const string ViajesGestionar = "Viajes.Gestionar";

        public const string CatalogosGestionar = "Catalogos.Gestionar";

        public const string PermisosAsignar = "Permisos.Asignar";
        public const string PermisosRetirar = "Permisos.Retirar";

        public static readonly IReadOnlyList<string> Todos =
            new List<string>
            {
                UsuariosVer,
                UsuariosGestionar,
                ReservasVer,
                ReservasGestionar,
                ViajesVer,
                ViajesGestionar,
                CatalogosGestionar,
                PermisosAsignar,
                PermisosRetirar
            }.AsReadOnly();

        public static bool EsValido(string permiso)
        {
            return !string.IsNullOrWhiteSpace(permiso)
                && Todos.Contains(permiso.Trim(),
                    StringComparer.OrdinalIgnoreCase);
        }
    }
}
