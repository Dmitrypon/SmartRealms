using Microsoft.AspNetCore.Mvc;
using SmartRealms.API.Repositories;

namespace SmartRealms.API.Controllers
{
    public class LightDevicesController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class DataController : ControllerBase
        {
            private readonly CommandDataRepository _repository;

            public DataController(CommandDataRepository repository)
            {
                _repository = repository;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<DataDto>> Get(int id)
            {
                var data = await _repository.GetLastDataAsync(id, HttpContext.RequestAborted);
                if (data == null) return NotFound(id);
                return Ok(new DataDto(data));
            }

        }

        public sealed class DataDto
        {
            private object data;

            public string Name { get; set; }
            public double Value { get; set; }
            public DateTimeOffset Registered { get; set; }
            public int DeviceId { get; set; }

            public DataDto() { }

            public DataDto(object data)
            {
                this.data = data;
            }            
        }
    }
}
