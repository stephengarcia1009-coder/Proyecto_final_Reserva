
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Data
{
    public class PangaDbContext : IdentityDbContext
    {
        public PangaDbContext(DbContextOptions<PangaDbContext> options)
            : base(options)
        {
        }
        public DbSet<Destino> Destinos { get; set; }

        public DbSet<Ruta> Rutas { get; set; }

        public DbSet<Panga> Pangas { get; set; }

        public DbSet<Tarifa> Tarifas { get; set; }

        public DbSet<EstadoViaje> EstadosViaje { get; set; }

        public DbSet<Viaje> Viajes { get; set; }

        public DbSet<EstadoReserva> EstadosReserva { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<DetalleReserva> DetallesReserva { get; set; }

        public DbSet<MetodoPago> MetodosPago { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<DetallePago> DetallesPago { get; set; }

        public DbSet<DetalleTarifa> DetallesTarifa { get; set; }

        public DbSet<DetalleViaje> DetallesViaje { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ruta>()
                .HasOne(r => r.Origen)
                .WithMany()
                .HasForeignKey(r => r.OrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ruta>()
                .HasOne(r => r.Destino)
                .WithMany()
                .HasForeignKey(r => r.DestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarifa>()
    .HasOne(t => t.Ruta)
    .WithMany()
    .HasForeignKey(t => t.RutaId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarifa>()
                .HasIndex(t => t.RutaId)
                .IsUnique();

            modelBuilder.Entity<Viaje>()
    .HasOne(v => v.Ruta)
    .WithMany()
    .HasForeignKey(v => v.RutaId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.Panga)
                .WithMany()
                .HasForeignKey(v => v.PangaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.EstadoViaje)
                .WithMany()
                .HasForeignKey(v => v.EstadoViajeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
    .HasOne(c => c.Usuario)
    .WithMany()
    .HasForeignKey(c => c.UsuarioId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.UsuarioId)
                .IsUnique();

            modelBuilder.Entity<Reserva>()
    .HasOne(r => r.Cliente)
    .WithMany()
    .HasForeignKey(r => r.ClienteId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Viaje)
                .WithMany()
                .HasForeignKey(r => r.ViajeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.EstadoReserva)
                .WithMany()
                .HasForeignKey(r => r.EstadoReservaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasIndex(r => r.CodigoReserva)
                .IsUnique();

            modelBuilder.Entity<DetalleReserva>()
    .HasOne(d => d.Reserva)
    .WithMany()
    .HasForeignKey(d => d.ReservaId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
    .HasOne(p => p.Reserva)
    .WithOne()
    .HasForeignKey<Pago>(p => p.ReservaId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.MetodoPago)
                .WithMany()
                .HasForeignKey(p => p.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetallePago>()
    .HasOne(d => d.Pago)
    .WithMany()
    .HasForeignKey(d => d.PagoId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleTarifa>()
    .HasOne(d => d.Tarifa)
    .WithMany()
    .HasForeignKey(d => d.TarifaId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleViaje>()
    .HasOne(d => d.Viaje)
    .WithMany()
    .HasForeignKey(d => d.ViajeId)
    .OnDelete(DeleteBehavior.Restrict);

        }
    }   


}
