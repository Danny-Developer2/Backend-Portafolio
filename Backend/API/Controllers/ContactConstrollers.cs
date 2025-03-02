using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Errors;  // Asegúrate de incluir el espacio de nombres para ApiException

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly DataContext _context;

        public ContactsController(DataContext context)
        {
            _context = context;
        }

        // Método para crear un nuevo contacto
        [HttpPost]
        public async Task<ActionResult<AppConcactMessage>> CreateContact(ContactDTO newContact)
        {
            try
            {
                var contact = new AppConcactMessage
                {
                    Name = newContact.Name,
                    Email = newContact.Email,
                    Phone = newContact.Phone,
                    Message = newContact.Message
                };

                _context.ContactMessages.Add(contact);
                await _context.SaveChangesAsync();

                // Retorna la URL del contacto creado
                return CreatedAtAction(nameof(GetContactById), new { id = contact.MessageId }, contact);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al crear el contacto: {ex.Message}"); // Lanza una ApiException
            }
        }

        // Método para obtener un contacto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AppConcactMessage>> GetContactById(int id)
        {
            try
            {
                var contact = await _context.ContactMessages.FindAsync(id);
                if (contact == null)
                {
                    throw new ApiException(404, "El contacto no fue encontrado."); // Lanza una ApiException si no se encuentra el contacto
                }

                return contact; // Si se encuentra, retorna el contacto
            }
            catch (Exception ex)
            {
                throw new ApiException(500, $"Error al obtener el contacto: {ex.Message}"); // Lanza una ApiException en caso de error inesperado
            }
        }
    }
}
