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
using SmartRealms.API.Procedure;
using Microsoft.Azure.Cosmos;
using TechTalk.SpecFlow.Assist;

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
        public async Task<IActionResult> Create([FromBody] DeviceAttachment request)
        {
            var userId = LocationId.Value;
            var result = await _service.Create(request.ProjectId, request.Id, userId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Procedure.GetDeviceResponse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var userId = LocationId.Value;
            var cachCards = await _cache.GetData<Device[]?>($"{Id}-cards");
            if (cachCards is not null)
            {
                return Ok(_mapper.Map<Device[], Procedure.GetDeviceResponse[]>(cachCards));
            }

            var cards = await _service.Get(userId);
            await _cache.SetData<Device[]?>($"{locationId}-cards", cards, DateTime.Now.AddMinutes(5));

            return Ok(_mapper.Map<Device[], Procedure.GetDeviceResponse[]>(cards));
        }

        
        [HttpDelete("{deviceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int cardId)
        {
            var userId = LocationId.Value;
            var result = await _service.Delete(cardId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
        }

        
        [HttpPut("{deviceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int deviceId, [FromBody]  device)
        {
            var userId = LocationId.Value;
            var result = await _service.Update(deviceId, device., device);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{deviceId}-cards");

            return Ok(result.Value);
        }

    }


