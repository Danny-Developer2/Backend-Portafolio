using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class ExperienceRepository
    {
        private readonly DataContext _context;

        public ExperienceRepository(DataContext context)
        {
            _context = context;
        }

        // Obtener todas las experiencias
        public async Task<List<AppExperience>> GetExperiencesAsync()
        {
            return await _context.Experiences.ToListAsync();
        }

        // Obtener una experiencia por ID
        public async Task<AppExperience?> GetExperienceByIdAsync(int id)
        {
            

            return await _context.Experiences.FindAsync(id); // Devuelve null si no encuentra la experiencia
        }


        // Agregar una nueva experiencia
        public async Task<AppExperience> AddExperienceAsync(AppExperience experience)
        {
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();
            return experience; // Devolver la experiencia creada
        }

        // Actualizar una experiencia existente
        public async Task<bool> UpdateExperienceAsync(AppExperience experience)
        {
            var existingExperience = await _context.Experiences.FindAsync(experience.Id);
            if (existingExperience == null)
            {
                return false; // No se encontró la experiencia, no se actualiza
            }

            _context.Entry(existingExperience).CurrentValues.SetValues(experience);
            await _context.SaveChangesAsync();
            return true; // Actualización exitosa
        }

        // Eliminar una experiencia
        public async Task<bool> DeleteExperienceAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return false; // No se encontró la experiencia, no se puede eliminar
            }

            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }
    }
}
