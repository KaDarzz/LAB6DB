using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;
using Swashbuckle.AspNetCore.Annotations;   // Для аннотаций Swagger

namespace LAB6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypesAPIController : ControllerBase
    {
        private readonly HairdressingContext _context;

        public ServiceTypesAPIController(HairdressingContext context)
        {
            _context = context;
        }



        [HttpGet]
        [SwaggerOperation(Summary = "Получить список всех типов услуг", Description = "Возвращает все доступные типы услуг.")]
        [SwaggerResponse(200, "Возвращает список типов услуг", typeof(IEnumerable<ServiceType>))]
        [SwaggerResponse(404, "Не найдено типов услуг")]
        public async Task<ActionResult<IEnumerable<ServiceType>>> GetServiceTypes()
        {
            var serviceTypes = await _context.ServiceTypes.ToListAsync();

            if (serviceTypes == null || !serviceTypes.Any())
            {
                return NotFound();
            }

            return Ok(serviceTypes);
        }



        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить тип услуги по ID", Description = "Возвращает информацию о типе услуги по его ID.")]
        [SwaggerResponse(200, "Возвращает тип услуги", typeof(ServiceType))]
        [SwaggerResponse(404, "Не найден тип услуги с таким ID")]
        public async Task<ActionResult<ServiceType>> GetServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);

            if (serviceType == null)
            {
                return NotFound();
            }

            return Ok(serviceType);
        }



        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить информацию о типе услуги", Description = "Обновляет информацию о типе услуги по его ID.")]
        [SwaggerResponse(204, "Информация типа услуги успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Не найден тип услуги с таким ID")]
        public async Task<IActionResult> PutServiceType(int id, ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return BadRequest();
            }

            _context.Entry(serviceType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceTypeExists(id))
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
        [SwaggerOperation(Summary = "Создать новый тип услуги", Description = "Создает новый тип услуги в базе данных.")]
        [SwaggerResponse(201, "Новый тип услуги успешно создан", typeof(ServiceType))]
        public async Task<ActionResult<ServiceType>> PostServiceType(ServiceType serviceType)
        {
            _context.ServiceTypes.Add(serviceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceType", new { id = serviceType.Id }, serviceType);
        }



        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить тип услуги по ID", Description = "Удаляет тип услуги по его ID.")]
        [SwaggerResponse(204, "Тип услуги успешно удален")]
        [SwaggerResponse(404, "Не найден тип услуги с таким ID")]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return NotFound();
            }

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceTypeExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}
