using AutoMapper;
using lcapis.Entities;
using lcapis.Helpers;
using lcapis.Models.LCServers;
using lcapis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/servers/[controller]")]
    public class LCServersController : ControllerBase
    {
        private IServerService _serverService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LCServersController(
            IServerService serverService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _serverService = serverService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateServerModel model)
        {
            LCServer server = _mapper.Map<LCServer>(model);
            _serverService.Create(server, long.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinServerModel model)
        {
            try
            {
                _serverService.Join(model.InviteCode, long.Parse(User.Identity.Name));
                return Ok();
            } catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
