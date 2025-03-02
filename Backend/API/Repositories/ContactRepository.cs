using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Repositories
{
    public class ContactRepository
    {
        private readonly DataContext _context;

        public ContactRepository(DataContext context)
        {
            _context = context;
        }

        // Obtener todos los contactos
        public async Task<List<AppConcactMessage>> GetContactsAsync()
        {
            return await _context.ContactMessages.ToListAsync();
        }

        // Obtener un contacto por ID
        public async Task<AppConcactMessage?> GetContactByIdAsync(int id)
        {
            return await _context.ContactMessages.FindAsync(id);
        }

        // Agregar un nuevo contacto
        public async Task<AppConcactMessage> AddContactAsync(AppConcactMessage contact)
        {
            _context.ContactMessages.Add(contact);
            await _context.SaveChangesAsync();
            return contact;  // Retorna el contacto recién creado
        }

        // Actualizar un contacto existente
        public async Task<bool> UpdateContactAsync(AppConcactMessage contact)
        {
            var existingContact = await _context.ContactMessages.FindAsync(contact.MessageId);
            if (existingContact == null)
            {
                return false; // No se encontró el contacto, no se actualiza
            }

            _context.Entry(existingContact).CurrentValues.SetValues(contact);
            await _context.SaveChangesAsync();
            return true; // Actualización exitosa
        }

        // Eliminar un contacto
        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _context.ContactMessages.FindAsync(id);
            if (contact == null)
            {
                return false; // No se encontró el contacto, no se puede eliminar
            }

            _context.ContactMessages.Remove(contact);
            await _context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }
    }
}
