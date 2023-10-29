using Microsoft.AspNetCore.Mvc;
using SmartRealms.API.Repositories;

namespace SmartRealms.API.Controllers
{
    public partial class LightDevicesController
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
            public async Task<ActionResult<DeviceDto>> Get(int id, DeviceDto dataDto)
            {
                var data = await _repository.GetLastDataAsync(id, HttpContext.RequestAborted);
                if (data == null) return NotFound(id);
                return Ok(DeviceDto);
             }

            private object? DeviceDto(object data)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class DeviceDto
    {
    }
}
