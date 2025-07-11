using BackendEY.Data;
using BackendEY.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendEY.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> ListarProveedores()
        {
            return await _context.Proveedores
                .OrderByDescending(p => p.FechaEdicion)
                .ToListAsync();
        }

        public async Task<Proveedor?> ObtenerProveedor(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        public async Task<Proveedor> CrearProveedor(Proveedor proveedor)
        {
            proveedor.FechaEdicion = DateTime.UtcNow;
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task<bool> ActualizarProveedor(int id, Proveedor actualizado)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            proveedor.RazonSocial = actualizado.RazonSocial;
            proveedor.NombreComercial = actualizado.NombreComercial;
            proveedor.IdentificacionTributaria = actualizado.IdentificacionTributaria;
            proveedor.Telefono = actualizado.Telefono;
            proveedor.Email = actualizado.Email;
            proveedor.SitioWeb = actualizado.SitioWeb;
            proveedor.Direccion = actualizado.Direccion;
            proveedor.Pais = actualizado.Pais;
            proveedor.FacturacionAnual = actualizado.FacturacionAnual;
            proveedor.FechaEdicion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
