﻿using Microsoft.AspNetCore.Mvc;
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
            //public async Task<ActionResult<DataDto>> Get(int id, DataDto dataDto)
            //{
            //    var data = await _repository.GetLastDataAsync(id, HttpContext.RequestAborted);
            //    if (data == null) return NotFound(id);
            //    return Ok(dataDto(data));
            //}

        }
    }
}