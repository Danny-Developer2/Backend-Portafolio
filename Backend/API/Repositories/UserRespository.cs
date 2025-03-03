using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.DTO;
using API.Interfaces;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<AppUser>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Obtener un usuario por ID
        public async Task<AppUser> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }
            return user;
        }

        // Crear un nuevo usuario
        public async Task<AppUser> AddUser(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.PasswordHash))
            {
                throw new InvalidOperationException("La contraseña es obligatoria.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == userDTO.Email))
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            var user = new AppUser
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash),
                Role = userDTO.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Actualizar usuario
        public async Task<AppUser> UpdateUser(int id, UserDTO userDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            // Validar si el email pertenece a otro usuario
            if (await _context.Users.AnyAsync(u => u.Email == userDTO.Email && u.Id != id))
            {
                throw new InvalidOperationException("El correo electrónico ya está en uso por otro usuario.");
            }

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.Role = userDTO.Role;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrWhiteSpace(userDTO.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Eliminar usuario
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // ✅ Mejor que lanzar una excepción
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
