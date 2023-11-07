using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRealms.API.DTO;
using SmartRealms.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SmartRealms.API.Model;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        private readonly IMapper _mapper;
        private object devices;

        public LightDeviceController(ILogger<LightDeviceController> logger, DevicesRepository devices, SchedulesRepository schedules, IMapper mapper)
        {
            // ILogger<LightDeviceController> logger;
            _logger = logger;
            _devices = devices;
            _schedules = schedules;
        }

        [HttpPost]
       // [ProducesResponseType(typeof(<Device>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<DeviceDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
        {
            var userId = UserId.Value;
            var result = await _service.Create(request.FrontSide, request.BackSide, userId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
        }

        private async Task<List<Device>> GetItemsByIdsAsync(int projectId, string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<Device>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _devices.GetItemsByIdsAsync(projectId, idsToSelect);

            return items;        
        }

        [HttpGet]
        [Route("{deviceId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(DeviceDTO), (int)HttpStatusCode.OK)]
        private async Task<ActionResult<DeviceDTO>> ItemByIdAsync(int deviceId)
        {
            if (deviceId <= 0)
            {
                return BadRequest();
            }

            var item = await _devices.GetAsync(deviceId);

            if (item != null)
            {
                return new DeviceDTO(item);
            }

            return NotFound();
        }

        [HttpPut]
        [Route("{deviceId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateDevicesAsync([FromRoute] int deviceId, [FromBody] DeviceDTO deviceToUpdate)
        {
            if (deviceId != deviceToUpdate.Id)
            {
                return UnprocessableEntity(new { error = "Invalid device" });
            }

            var device = await _devices.GetAsync(deviceToUpdate.Id.Value);

            if (device == null)
            {
                return NotFound(new { message = $"Item with id {deviceToUpdate.Id} not found." });
            }
           
            var data = device.Data;
           
            }
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteDeviceAsync(int deviceId)
        {
            var device = await _devices.GetAsync(deviceId);

            if (device == null)
            {
                return NotFound();
            }
            var serial = device.Serial;
            if (string.IsNullOrWhiteSpace(device.NodeSerial))
            {
                var children = await _devices.GetByNodeAsync(device.Serial);
                foreach (var child in children)
                {
                    await _devices.Delete(child);
                }
            }
            await _devices.Delete(device);
            
            return Ok(new { done = true });
        }

            private async Task<List<Device>> GetItemsByIdsAsync(int projectId, string ids)
            {
                var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

                if (!numIds.All(nid => nid.Ok))
                {
                    return new List<Device>();
                }

                var idsToSelect = numIds
                    .Select(id => id.Value);

                var items = await _devices.GetItemsByIdsAsync(projectId, idsToSelect);

                return items;
            }
        }

    }


