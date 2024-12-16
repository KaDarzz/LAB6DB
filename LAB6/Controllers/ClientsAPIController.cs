using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;
using Swashbuckle.AspNetCore.Annotations;   // Для аннотаций Swagger

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsAPIController : ControllerBase
    {
        private readonly HairdressingContext _context;

        public ClientsAPIController(HairdressingContext context)
        {
            _context = context;
        }



        [HttpGet]
        [SwaggerOperation(Summary = "Получить список всех клиентов", Description = "Возвращает все клиенты, зарегистрированные в системе.")]
        [SwaggerResponse(200, "Возвращает список клиентов", typeof(IEnumerable<Client>))]
        [SwaggerResponse(404, "Клиенты не найдены")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();

            if (clients == null || !clients.Any())
            {
                return NotFound();
            }

            return Ok(clients);
        }



        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить клиента по ID", Description = "Возвращает информацию о клиенте по его ID.")]
        [SwaggerResponse(200, "Возвращает клиента", typeof(Client))]
        [SwaggerResponse(404, "Не найден клиент с таким ID")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }



        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить информацию о клиенте", Description = "Обновляет информацию о клиенте по его ID.")]
        [SwaggerResponse(204, "Информация клиента успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Не найден клиент с таким ID")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Создать нового клиента", Description = "Создает нового клиента в системе.")]
        [SwaggerResponse(201, "Новый клиент успешно создан", typeof(Client))]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить клиента по ID", Description = "Удаляет клиента по его ID.")]
        [SwaggerResponse(204, "Клиент успешно удален")]
        [SwaggerResponse(404, "Не найден клиент с таким ID")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
