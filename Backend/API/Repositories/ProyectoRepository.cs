using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class ProyectoRepository
    {
        private readonly DataContext _context;

        public ProyectoRepository(DataContext context)
        {
            _context = context;
        }

        // Obtener todos los proyectos
        public async Task<List<AppProyect>> GetProyectosAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        // Obtener un proyecto por ID
        public async Task<AppProyect?> GetProyectoByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        // Agregar un nuevo proyecto
        public async Task<AppProyect> AddProyectoAsync(AppProyect proyecto)
        {
            _context.Projects.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }

        // Actualizar un proyecto existente
        public async Task<bool> UpdateProyectoAsync(AppProyect proyecto)
        {
            var existingProyecto = await _context.Projects.FindAsync(proyecto.Id);
            if (existingProyecto == null)
            {
                return false; // No se encontró el proyecto, no se actualiza
            }

            _context.Entry(existingProyecto).CurrentValues.SetValues(proyecto);
            await _context.SaveChangesAsync();
            return true; // Actualización exitosa
        }

        // Eliminar un proyecto
        public async Task<bool> DeleteProyectoAsync(int id)
        {
            var proyecto = await _context.Projects.FindAsync(id);
            if (proyecto == null)
            {
                return false; // No se encontró el proyecto, no se puede eliminar
            }

            _context.Projects.Remove(proyecto);
            await _context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }
    }
}
