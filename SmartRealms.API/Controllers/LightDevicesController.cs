using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRealms.API.DTO;
using SmartRealms.API.Repositories;
using System.Diagnostics.Contracts;

namespace SmartRealms.API.Controllers
{
        // [Route]
        [ApiController]
        [Authorize]
        public class LightDeviceController : ControllerBase
        {
        private readonly ILogger<LightDeviceController> _logger;
        private readonly DevicesRepository _devices;
        private readonly SchedulesRepository _schedules;

        public LightDeviceController(ILogger<LightDeviceController> logger, DevicesRepository devices, SchedulesRepository schedules)
        {
            // ILogger<LightDeviceController> logger;
            _logger = logger;
            _devices = devices;
            _schedules = schedules;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTO.DeviceDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult Get()
        //{
            
        //}


    }

    }  
}
