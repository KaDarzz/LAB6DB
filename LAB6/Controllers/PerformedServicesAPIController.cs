using Lab6.Data;
using Lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;   // Для аннотаций Swagger

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformedServicesAPIController : ControllerBase
    {
        private readonly HairdressingContext _context;

        public PerformedServicesAPIController(HairdressingContext context)
        {
            _context = context;
        }

        // GET: api/PerformedServices
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список выполненных услуг", Description = "Возвращает все выполненные услуги, включая информацию о клиенте, услуге и сотруднике.")]
        [SwaggerResponse(200, "Успешный запрос", typeof(IEnumerable<PerformedService>))]
        [SwaggerResponse(404, "Услуги не найдены")]
        public async Task<ActionResult<IEnumerable<PerformedService>>> GetPerformedServices()
        {
            var services = await _context.PerformedServices
                .Include(p => p.Client)
                .Include(p => p.Service)
                .Include(p => p.Employee)
                .Take(20)
                .ToListAsync();

            if (services == null || !services.Any())
            {
                return NotFound();
            }

            return Ok(services);
        }

        // GET: api/PerformedServices/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить выполненную услугу по ID", Description = "Возвращает информацию о конкретной выполненной услуге по её ID.")]
        [SwaggerResponse(200, "Услуга найдена", typeof(PerformedService))]
        [SwaggerResponse(404, "Услуга не найдена")]
        public async Task<ActionResult<PerformedService>> GetPerformedService(int id)
        {
            var performedService = await _context.PerformedServices
                .Include(p => p.Client)
                .Include(p => p.Service)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (performedService == null)
            {
                return NotFound();
            }

            return Ok(performedService);
        }

        // PUT: api/PerformedServices/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить выполненную услугу", Description = "Обновляет информацию о выполненной услуге по её ID.")]
        [SwaggerResponse(204, "Услуга успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Услуга не найдена")]
        public async Task<IActionResult> PutPerformedService(int id, PerformedService performedService)
        {
            if (id != performedService.Id)
            {
                return BadRequest();
            }

            _context.Entry(performedService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerformedServiceExists(id))
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

        // POST: api/PerformedServices
        [HttpPost]
        [SwaggerOperation(Summary = "Создать новую выполненную услугу", Description = "Создает новую выполненную услугу в базе данных.")]
        [SwaggerResponse(201, "Услуга успешно создана", typeof(PerformedService))]
        [SwaggerResponse(400, "Некорректные данные")]
        public async Task<ActionResult<PerformedService>> PostPerformedService(PerformedService performedService)
        {
            _context.PerformedServices.Add(performedService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerformedService", new { id = performedService.Id }, performedService);
        }

        // DELETE: api/PerformedServices/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить выполненную услугу", Description = "Удаляет выполненную услугу по её ID.")]
        [SwaggerResponse(204, "Услуга успешно удалена")]
        [SwaggerResponse(404, "Услуга не найдена")]
        public async Task<IActionResult> DeletePerformedService(int id)
        {
            var performedService = await _context.PerformedServices
                .Include(p => p.Client)
                .Include(p => p.Service)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (performedService == null)
            {
                return NotFound();
            }

            _context.PerformedServices.Remove(performedService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerformedServiceExists(int id)
        {
            return _context.PerformedServices.Any(e => e.Id == id);
        }
    }
}
