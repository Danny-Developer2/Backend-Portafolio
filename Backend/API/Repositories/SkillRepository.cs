using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class SkillRepository
    {
        private readonly DataContext _context;

        public SkillRepository(DataContext context)
        {
            _context = context;
        }

        // Obtener todas las habilidades
        public async Task<List<AppSkill>> GetSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        // Obtener una habilidad por ID
        public async Task<AppSkill?> GetSkillByIdAsync(int id)
        {
            return await _context.Skills.FindAsync(id);
        }

        // Agregar una nueva habilidad
        public async Task<AppSkill> AddSkillAsync(AppSkill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill;  // Devolvemos la habilidad recién creada
        }

        // Actualizar una habilidad existente
        public async Task<bool> UpdateSkillAsync(AppSkill skill)
        {
            var existingSkill = await _context.Skills.FindAsync(skill.Id);
            if (existingSkill == null)
            {
                return false; // No se encontró la habilidad, no se actualiza
            }

            _context.Entry(existingSkill).CurrentValues.SetValues(skill);
            await _context.SaveChangesAsync();
            return true; // Actualización exitosa
        }

        // Eliminar una habilidad
        public async Task<bool> DeleteSkillAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return false; // No se encontró la habilidad, no se puede eliminar
            }

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }
    }
}
